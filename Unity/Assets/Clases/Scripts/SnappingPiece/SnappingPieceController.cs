using System;
using System.Collections;
using UnityEngine;

public enum ManipulationModeSnap { Rotation, Translation }

[RequireComponent(typeof(Rigidbody))]
public class SnappingPieceController : MonoBehaviour
{
    [Header("Speeds")]
    public float moveSpeed = 6f;
    public float rotationSpeedDegPerSec = 240f;

    [Header("Smoothing")]
    [Tooltip("Tiempo de suavizado (mayor = mas suave)")]
    public float positionSmoothTime = 0.07f;

    [Header("Placement / Hover")]
    [Tooltip("Altura a la que flota la pieza mientras la manipulas")]
    public float hoverHeight = 0.2f;

    [Header("Grid (opcional)")]
    public bool enableGridSnap = true;
    public float cellSize = 0.5f;

    [Header("Rotation Mode")]
    [Tooltip("Si true, rotara en pasos de 90 grados; si false, rotacion libre suave")]
    public bool IsRotationFixed = false;
    public float snapAngle = 90f;
    public float snapRepeatDelay = 1f;

    [NonSerialized] public ManipulationModeSnap mode = ManipulationModeSnap.Rotation;

    Rigidbody _rb;
    Collider[] _colliders;

    BuildingZoneArea _zone;

    Vector3 _targetPos;
    Quaternion _targetRot;
    Vector3 _posVelRef;

    Vector3 _rawTargetPos;

    Vector2Int _lastSnapDir = Vector2Int.zero;
    float _snapCooldown = 0f;

    bool _active = false;
    bool _dropping = false;

    public event Action<SnappingPieceController> OnPlaced;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _colliders = GetComponentsInChildren<Collider>(includeInactive: true);
        SetPreDropPhysics();
    }

    public void Activate(BuildingZoneArea zone, Vector3 spawnPos, Quaternion spawnRot)
    {
        _zone = zone;
        _active = true;
        _dropping = false;

        transform.SetPositionAndRotation(spawnPos, spawnRot);

        _targetPos = spawnPos;
        _rawTargetPos = spawnPos;
        _targetRot = spawnRot;

        SetPreDropPhysics();

        float minY = _zone.WorldBounds.min.y + hoverHeight;
        _targetPos.y = Mathf.Max(spawnPos.y, minY);
        _rawTargetPos.y = _targetPos.y;
        transform.position = _targetPos;
    }

    void SetPreDropPhysics()
    {
        if (_rb != null)
        {
            _rb.isKinematic = true;
            _rb.useGravity = false;
        }
        if (_colliders != null)
        {
            foreach (var c in _colliders) c.isTrigger = true;
        }
    }

    void SetPostDropPhysics()
    {
        if (_rb != null)
        {
            _rb.isKinematic = false;
            _rb.useGravity = true;
        }
        if (_colliders != null)
        {
            foreach (var c in _colliders) c.isTrigger = false;
        }
    }

    public void HandleArrows(Vector2 arrows, float deltaTime)
    {
        if (!_active || _dropping) return;

        if (mode == ManipulationModeSnap.Translation)
        {
            int dx = arrows.x > 0.5f ? 1 : (arrows.x < -0.5f ? -1 : 0);
            int dz = arrows.y > 0.5f ? 1 : (arrows.y < -0.5f ? -1 : 0);

            if (dx != 0 || dz != 0)
            {
                Vector3 step = new Vector3(dx * cellSize, 0f, dz * cellSize);
                _rawTargetPos += step;
            }

            Vector3 dest = _rawTargetPos;

            if (enableGridSnap && cellSize > 0.0001f)
            {
                dest.x = Mathf.Round(dest.x / cellSize) * cellSize;
                dest.z = Mathf.Round(dest.z / cellSize) * cellSize;
            }

            dest = _zone.ClampInside(dest, GetHalfExtentsWorldXZ());

            var b = _zone.WorldBounds;
            float halfY = GetHalfExtentsWorld().y;
            float yMin = b.min.y + halfY;
            float yMax = b.max.y - halfY;
            dest.y = Mathf.Clamp(dest.y, yMin, yMax);

            _targetPos = dest;
        }
        else 
        {
            if (!IsRotationFixed)
            {
                float yaw = arrows.x * rotationSpeedDegPerSec * deltaTime;
                float pitch = -arrows.y * rotationSpeedDegPerSec * deltaTime; 
                _targetRot = Quaternion.Euler(pitch, yaw, 0f) * _targetRot;
            }
            else
            {
                int dx = arrows.x > 0.5f ? 1 : (arrows.x < -0.5f ? -1 : 0);
                int dy = arrows.y > 0.5f ? 1 : (arrows.y < -0.5f ? -1 : 0);
                Vector2Int dir = new Vector2Int(dx, dy);

                if (_snapCooldown > 0f) _snapCooldown -= deltaTime;

                if (dir == Vector2Int.zero)
                {
                    _lastSnapDir = Vector2Int.zero;
                    _snapCooldown = 0f;
                }
                else if (_snapCooldown <= 0f || dir != _lastSnapDir)
                {
                    Quaternion step = Quaternion.identity;
                    if (dx != 0) step = Quaternion.Euler(0f, dx * snapAngle, 0f) * step;  
                    if (dy != 0) step = Quaternion.Euler(-dy * snapAngle, 0f, 0f) * step; 

                    _targetRot = step * _targetRot;
                    _lastSnapDir = dir;
                    _snapCooldown = snapRepeatDelay;
                }
            }
        }
    }

    public void HandleVertical(float yInput, float deltaTime)
    {
        if (!_active || _dropping || mode != ManipulationModeSnap.Translation) return;
        if (Mathf.Approximately(yInput, 0f)) return;

        _rawTargetPos.y += yInput * moveSpeed * deltaTime;
    }

    public void Tick(float deltaTime)
    {
        if (!_active) return;

        transform.position = Vector3.SmoothDamp(transform.position, _targetPos, ref _posVelRef, positionSmoothTime);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, _targetRot, rotationSpeedDegPerSec * deltaTime);
    }

    public void SetMode(ManipulationModeSnap newMode)
    {
        mode = newMode;
        _targetPos = transform.position;
        _rawTargetPos = transform.position;
        _targetRot = transform.rotation;
        _lastSnapDir = Vector2Int.zero;
        _snapCooldown = 0f;
    }

    public void BeginDrop()
    {
        if (!_active || _dropping) return;
        _dropping = true;
        _active = false;
        SetPostDropPhysics();
        StartCoroutine(WaitUntilRestCoroutine());
    }

    IEnumerator WaitUntilRestCoroutine()
    {
        float stableTimer = 0f;
        const float stableNeeded = 0.35f;
        const float velThreshold = 0.05f;

        while (true)
        {
            if (_rb == null) break;
            if (_rb.linearVelocity.sqrMagnitude < velThreshold * velThreshold)
            {
                stableTimer += Time.fixedDeltaTime;
                if (stableTimer >= stableNeeded) break;
            }
            else stableTimer = 0f;

            yield return new WaitForFixedUpdate();
        }

        OnPlaced?.Invoke(this);
    }

    public Vector3Int CurrentGridCell
    {
        get
        {
            return new Vector3Int(
                Mathf.RoundToInt(_targetPos.x / cellSize),
                Mathf.RoundToInt(_targetPos.y / cellSize),
                Mathf.RoundToInt(_targetPos.z / cellSize));
        }
    }

    Vector3 GetHalfExtentsWorld()
    {
        var renderers = GetComponentsInChildren<Renderer>();
        if (renderers.Length > 0)
        {
            Bounds b = new Bounds(renderers[0].bounds.center, Vector3.zero);
            foreach (var r in renderers) b.Encapsulate(r.bounds);
            return b.extents;
        }

        var colliders = GetComponentsInChildren<Collider>();
        if (colliders.Length > 0)
        {
            Bounds b = new Bounds(colliders[0].bounds.center, Vector3.zero);
            foreach (var c in colliders) b.Encapsulate(c.bounds);
            return b.extents;
        }

        return new Vector3(0.25f, 0.25f, 0.25f);
    }

    Vector3 GetHalfExtentsWorldXZ()
    {
        var e = GetHalfExtentsWorld();
        return new Vector3(e.x, 0f, e.z);
    }
}
