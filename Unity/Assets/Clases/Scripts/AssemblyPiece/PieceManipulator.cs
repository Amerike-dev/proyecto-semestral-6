using System;
using System.Collections;
using UnityEngine;

public enum ManipulationMode { Rotation, Translation }

[RequireComponent(typeof(Rigidbody))]
public class PieceManipulator : MonoBehaviour
{
    [Header("Speeds")]
    public float moveSpeed = 6f;
    public float rotationSpeedDegPerSec = 240f;

    [Header("Smoothing")]
    public float positionSmoothTime = 0.07f;

    [Header("Placement / Hover")]
    public float hoverHeight = 0.2f;

    [Header("Grid (opcional)")]
    public bool enableGridSnap = true;
    public float cellSize = 0.5f;

    [Header("Rotation Mode")]
    public bool IsRotationFixed = false;          
    public float snapAngle = 90f;
    public float snapRepeatDelay = 1f;

    [Header("Drop")]
    public bool tetrisDrop = true;
    public float dropFallSpeed = 10f;

    [NonSerialized] public ManipulationMode mode = ManipulationMode.Rotation;

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

    public event Action<PieceManipulator> OnPlaced;

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

        if (enableGridSnap)
            _zone.ShowHighlightAt(_zone.SnapToGrid(_targetPos));
        else
            _zone.HideHighlight();
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

        if (mode == ManipulationMode.Translation)
        {

            Vector3 delta = new Vector3(arrows.x, 0f, arrows.y) * moveSpeed * deltaTime;
            _rawTargetPos += delta;

            Vector3 dest = _rawTargetPos;

            if (enableGridSnap && cellSize > 0.0001f)
            {
                dest = _zone.SnapToGrid(dest);
            }

            dest = _zone.ClampInside(dest, GetHalfExtentsWorldXZ());

            var b = _zone.WorldBounds;
            float halfY = GetHalfExtentsWorld().y;
            float yMin = b.min.y + halfY;
            float yMax = b.max.y - halfY;
            dest.y = Mathf.Clamp(dest.y, yMin, yMax);

            _targetPos = dest;

            // Highlight de celda
            if (enableGridSnap)
                _zone.ShowHighlightAt(new Vector3(dest.x, 0f, dest.z));
            else
                _zone.HideHighlight();

        }
        else // Rotation
        {
            if (enableGridSnap)
                _zone.ShowHighlightAt(_zone.SnapToGrid(transform.position));
            else
                _zone.HideHighlight();

            if (!IsRotationFixed)
            {
                float yaw = arrows.x * rotationSpeedDegPerSec * deltaTime;
                float pitch = -arrows.y * rotationSpeedDegPerSec * deltaTime; // arriba = +X
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
                    if (dx != 0) step = Quaternion.Euler(0f, dx * snapAngle, 0f) * step;   // yaw
                    if (dy != 0) step = Quaternion.Euler(-dy * snapAngle, 0f, 0f) * step;   // pitch

                    _targetRot = step * _targetRot;
                    _lastSnapDir = dir;
                    _snapCooldown = snapRepeatDelay;
                }
            }
        }
    }
    public void HandleVertical(float yInput, float deltaTime)
    {
        if (!_active || _dropping || mode != ManipulationMode.Translation) return;
        if (Mathf.Approximately(yInput, 0f)) return;

        _rawTargetPos.y += yInput * moveSpeed * deltaTime;
    }

    public void Tick(float deltaTime)
    {
        if (!_active) return;

        transform.position = Vector3.SmoothDamp(transform.position, _targetPos, ref _posVelRef, positionSmoothTime);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, _targetRot, rotationSpeedDegPerSec * deltaTime);
    }

    public void SetMode(ManipulationMode newMode)
    {
        if (mode == ManipulationMode.Rotation)
            FinalizeRotation();

        mode = newMode;

        _targetPos = transform.position;
        _rawTargetPos = transform.position;
        _targetRot = transform.rotation;
        _lastSnapDir = Vector2Int.zero;
        _snapCooldown = 0f;

        if (enableGridSnap)
            _zone.ShowHighlightAt(_zone.SnapToGrid(transform.position));
        else
            _zone.HideHighlight();
    }


    public void BeginDrop()
    {
        if (!_active || _dropping) return;

        FinalizeRotation();

        _zone.HideHighlight();
        _dropping = true;
        _active = false;

        if (tetrisDrop)
        {
            StopAllCoroutines();
            StartCoroutine(DropStraightCoroutine());
        }
        else
        {
            SetPostDropPhysics();
            StopAllCoroutines();
            StartCoroutine(WaitUntilRestCoroutine());
        }
    }

    IEnumerator DropStraightCoroutine()
    {
        if (_zone == null)
        {
            SetPostDropPhysics();
            yield return StartCoroutine(WaitUntilRestCoroutine());
            yield break;
        }

        // 1) Calcula Y de aterrizaje
        var b = _zone.WorldBounds;
        float halfY = GetHalfExtentsWorld().y;
        float landingY = b.min.y + halfY;

        // 2) Asegura XZ actuales
        Vector3 startPos = transform.position;
        Vector3 finalPos = startPos;
        finalPos.y = landingY;

        // 3) Congela completamente mientras cae
        if (_rb != null)
        {
            _rb.isKinematic = true;
            _rb.useGravity = false;
        }
        if (_colliders != null) foreach (var c in _colliders) c.isTrigger = true;

        // 4) Animación de caída recta
        while (transform.position.y > landingY + 0.0001f)
        {
            float step = dropFallSpeed * Time.deltaTime;
            float newY = Mathf.MoveTowards(transform.position.y, landingY, step);
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
            yield return null;
        }
        transform.position = finalPos; // aterriza exacto

        // 5) Fijar la pieza en escena
        if (_colliders != null) foreach (var c in _colliders) c.isTrigger = false;
        if (_rb != null)
        {
            _rb.isKinematic = true;   // se queda inmóvil
            _rb.useGravity = false;   // sin gravedad para que no se mueva
        }

        // 6) Notifica que se colocó y spawnea la siguiente pieza
        OnPlaced?.Invoke(this);
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

    // Extents en mundo (considera rotación/escala)
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

    void FinalizeRotation()
    {
        if (!IsRotationFixed)
        {
            transform.rotation = _targetRot;
        }
        else
        {
            Vector3 e = transform.rotation.eulerAngles;
            e.x = Mathf.Round(e.x / snapAngle) * snapAngle;
            e.y = Mathf.Round(e.y / snapAngle) * snapAngle;
            e.z = Mathf.Round(e.z / snapAngle) * snapAngle;
            var snapped = Quaternion.Euler(e);
            transform.rotation = snapped;
            _targetRot = snapped;
        }
    }

}

