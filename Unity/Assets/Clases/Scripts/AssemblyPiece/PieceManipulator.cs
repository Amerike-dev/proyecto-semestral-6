/*
using System;
using System.Collections;
using UnityEngine;

public enum ManipulationMode { Rotation, Translation }

[RequireComponent(typeof(Rigidbody))]
public class PieceManipulator : MonoBehaviour
{
    [Header("Speeds")]
    public float moveSpeed = 6f;
    public float rotationSpeedDegPerSec = 120f;

    [Header("Smoothing")]
    [Tooltip("Tiempo de suavizado (mayor = más suave)")]
    public float positionSmoothTime = 0.07f;

    [Header("Placement / Hover")]
    [Tooltip("Altura a la que flota la pieza mientras la manipulas")]
    public float hoverHeight = 0.2f;

    [Header("Grid (opcional)")]
    public bool enableGridSnap = false;   // default en OFF para que sea simple y fluido
    public float cellSize = 0.5f;

    [NonSerialized] public ManipulationMode mode = ManipulationMode.Rotation;

    Rigidbody _rb;
    Collider[] _colliders;

    BuildingZoneArea _zone;

    // Destino final
    Vector3 _targetPos;
    Quaternion _targetRot;
    Vector3 _posVelRef;

    // Acumulador crudo para traslación
    Vector3 _rawTargetPos;

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
            // 1) Acumular en crudo (sin snap)
            Vector3 delta = new Vector3(arrows.x, 0f, arrows.y) * moveSpeed * deltaTime;
            _rawTargetPos += delta;

            // 2) Calcular destino visible aplicando snap y clamp
            Vector3 dest = _rawTargetPos;

            if (enableGridSnap && cellSize > 0.0001f)
            {
                dest.x = Mathf.Round(dest.x / cellSize) * cellSize;
                dest.z = Mathf.Round(dest.z / cellSize) * cellSize;
            }

            dest = _zone.ClampInside(dest, GetHalfExtentsXZ()); // clampa XZ
            dest.y = _targetPos.y; // mantener la altura de hover/spawn

            _targetPos = dest;
        }
        else // Rotation
        {
            float yaw = arrows.x * rotationSpeedDegPerSec * deltaTime;
            float pitch = -arrows.y * rotationSpeedDegPerSec * deltaTime; // arriba = +X
            _targetRot = Quaternion.Euler(pitch, yaw, 0f) * _targetRot;
        }
    }

    public void Tick(float deltaTime)
    {
        if (!_active) return;

        transform.position = Vector3.SmoothDamp(transform.position, _targetPos, ref _posVelRef, positionSmoothTime);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, _targetRot, rotationSpeedDegPerSec * deltaTime);
    }

    
    public void SetMode(ManipulationMode newMode)
    {
        mode = newMode;
        _targetPos = transform.position;
        _rawTargetPos = transform.position;  // importante para que traslade al instante
        _targetRot = transform.rotation;
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

    Vector3 GetHalfExtentsXZ()
    {
        var renderers = GetComponentsInChildren<Renderer>();
        if (renderers.Length > 0)
        {
            Bounds b = new Bounds(renderers[0].bounds.center, Vector3.zero);
            foreach (var r in renderers) b.Encapsulate(r.bounds);
            return new Vector3(b.extents.x, 0f, b.extents.z);
        }

        var colliders = GetComponentsInChildren<Collider>();
        if (colliders.Length > 0)
        {
            Bounds b = new Bounds(colliders[0].bounds.center, Vector3.zero);
            foreach (var c in colliders) b.Encapsulate(c.bounds);
            return new Vector3(b.extents.x, 0f, b.extents.z);
        }

        return new Vector3(0.25f, 0f, 0.25f);
    }
}
*/

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
    [Tooltip("Tiempo de suavizado (mayor = más suave)")]
    public float positionSmoothTime = 0.07f;

    [Header("Placement / Hover")]
    [Tooltip("Altura a la que flota la pieza mientras la manipulas")]
    public float hoverHeight = 0.2f;

    [Header("Grid (opcional)")]
    public bool enableGridSnap = true;
    public float cellSize = 0.5f;

    [Header("Rotation Mode")]
    [Tooltip("Si true, rotará en pasos de 90°; si false, rotación libre suave")]
    public bool IsRotationFixed = false;          
    public float snapAngle = 90f;                 // usado sólo si IsRotationFixed = true
    public float snapRepeatDelay = 1f;//0.25f;         // repetición al mantener tecla (modo fijo)

    [NonSerialized] public ManipulationMode mode = ManipulationMode.Rotation;

    Rigidbody _rb;
    Collider[] _colliders;

    BuildingZoneArea _zone;

    // Destino interpolado
    Vector3 _targetPos;
    Quaternion _targetRot;
    Vector3 _posVelRef;

    // Acumulador crudo para traslación
    Vector3 _rawTargetPos;

    // Estado para rotación fija
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

    /// <summary>
    /// arrows.x = izquierda(-1)/derecha(1), arrows.y = abajo(-1)/arriba(1)
    /// </summary>
    public void HandleArrows(Vector2 arrows, float deltaTime)
    {
        if (!_active || _dropping) return;

        if (mode == ManipulationMode.Translation)
        {
            // 1) Acumular en crudo (XZ)
            Vector3 delta = new Vector3(arrows.x, 0f, arrows.y) * moveSpeed * deltaTime;
            _rawTargetPos += delta;

            // 2) Destino visible (snap + clamp)
            Vector3 dest = _rawTargetPos;

            if (enableGridSnap && cellSize > 0.0001f)
            {
                dest.x = Mathf.Round(dest.x / cellSize) * cellSize;
                dest.z = Mathf.Round(dest.z / cellSize) * cellSize;
            }

            dest = _zone.ClampInside(dest, GetHalfExtentsWorldXZ());

            // Clamp Y usando extents de la pieza
            var b = _zone.WorldBounds;
            float halfY = GetHalfExtentsWorld().y;
            float yMin = b.min.y + halfY;
            float yMax = b.max.y - halfY;
            dest.y = Mathf.Clamp(dest.y, yMin, yMax);

            _targetPos = dest;
        }
        else // Rotation
        {
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

    /// <summary>Entrada vertical (Space=+1, Ctrl=-1) sólo en modo Traslación.</summary>
    public void HandleVertical(float yInput, float deltaTime)
    {
        if (!_active || _dropping || mode != ManipulationMode.Translation) return;
        if (Mathf.Approximately(yInput, 0f)) return;

        _rawTargetPos.y += yInput * moveSpeed * deltaTime;
        // El clamp de Y se aplica en HandleArrows (centralizamos lógica).
    }

    public void Tick(float deltaTime)
    {
        if (!_active) return;

        transform.position = Vector3.SmoothDamp(transform.position, _targetPos, ref _posVelRef, positionSmoothTime);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, _targetRot, rotationSpeedDegPerSec * deltaTime);
    }

    public void SetMode(ManipulationMode newMode)
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
}

