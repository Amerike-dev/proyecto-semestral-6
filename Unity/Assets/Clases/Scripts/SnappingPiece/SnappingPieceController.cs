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

    [Header("Placement / Hover")]
    public float hoverHeight = 0.2f;

    [Header("Grid")]
    public bool enableGridSnap = true;
    public float cellSize = 1f;

    [Header("Rotation Mode")]
    public bool IsRotationFixed = false;
    public float snapAngle = 90f;
    public float snapRepeatDelay = 0.25f;

    [Header("References")]
    public GridController gridVisual;

    [Header("Snapping Boundaries")]
    public Transform boundaryObject;


    [NonSerialized] public ManipulationModeSnap mode = ManipulationModeSnap.Translation;

    [NonSerialized] public SnapAssemblyPieceManager manager;
    [NonSerialized] public BuildingZoneArea buildingZone;

    Rigidbody _rb;
    Collider[] _colliders;

    Vector3 _targetPos;
    Quaternion _targetRot;
    Vector3 _posVelRef;
    Vector3 _rawTargetPos;


    bool _active = false;
    bool _dropping = false;

    public event Action<SnappingPieceController> OnPlaced;

    float inputCooldown = 0.15f; 
    float inputTimer = 0f;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _colliders = GetComponentsInChildren<Collider>(includeInactive: true);
        SetPreDropPhysics();
    }

    public void Activate(BuildingZoneArea zone, Vector3 spawnPos, Quaternion spawnRot)
    {
        buildingZone = zone;
        _active = true;
        _dropping = false;

        transform.SetPositionAndRotation(spawnPos, spawnRot);

        _targetRot = spawnRot;

        SetPreDropPhysics();

        float yBase = zone.WorldBounds.max.y + hoverHeight;
        _targetPos = new Vector3(spawnPos.x, yBase, spawnPos.z);
        _rawTargetPos = _targetPos;
        transform.position = _targetPos;

    }

    public void HandleArrows(Vector2 arrows, float deltaTime)
    {
        if (!_active || _dropping) return;

        inputTimer -= deltaTime;
        if (inputTimer > 0f) return;

        if (mode == ManipulationModeSnap.Translation)
        {
            int dx = arrows.x > 0.5f ? 1 : (arrows.x < -0.5f ? -1 : 0);
            int dz = arrows.y > 0.5f ? 1 : (arrows.y < -0.5f ? -1 : 0);

            if (dx != 0 || dz != 0)
            {
                inputTimer = inputCooldown;
                Vector3 step = new Vector3(dx * cellSize, 0f, dz * cellSize);
                Vector3 candidate = _rawTargetPos + step;

                Bounds b;
                if (boundaryObject != null)
                {
                    Renderer r = boundaryObject.GetComponent<Renderer>();
                    if (r != null)
                    {
                        b = r.bounds;
                    }
                    else
                    {
                        b = buildingZone.WorldBounds;
                    }
                }
                else
                {
                    b = buildingZone.WorldBounds;
                }

                float gridOriginX = b.min.x;
                float gridOriginZ = b.min.z;

                if (enableGridSnap && cellSize > 0.0001f)
                {
                    candidate.x = Mathf.Round((candidate.x - gridOriginX) / cellSize) * cellSize + gridOriginX;
                    candidate.z = Mathf.Round((candidate.z - gridOriginZ) / cellSize) * cellSize + gridOriginZ;
                }

                candidate.x = Mathf.Clamp(candidate.x, b.min.x, b.max.x);
                candidate.z = Mathf.Clamp(candidate.z, b.min.z, b.max.z);
                candidate.y = b.max.y + hoverHeight;

                if (candidate != _targetPos)
                {
                    _targetPos = candidate;
                    _rawTargetPos = _targetPos; 
                }
            }
        }
    }

    void SetPreDropPhysics()
    {
        _rb.isKinematic = true;
        _rb.useGravity = false;
        foreach (var c in _colliders) c.isTrigger = true;
    }

    void SetPostDropPhysics()
    {
        _rb.isKinematic = false;
        _rb.useGravity = true;
        foreach (var c in _colliders) c.isTrigger = false;
    }

    public void Tick(float deltaTime)
    {
        if (!_active) return;
        transform.position = Vector3.SmoothDamp(transform.position, _targetPos, ref _posVelRef, 0.07f);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, _targetRot, rotationSpeedDegPerSec * deltaTime);
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
}