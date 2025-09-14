using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SnapAssemblyPieceManager : MonoBehaviour
{
    [Header("Scene References")]
    public BuildingZoneArea buildingZone;
    [Tooltip("Punto base de aparición (si está vacío, se usa el centro del BuildingZone).")]
    public Transform spawnPoint;

    [Header("Pieces")]
    public List<GameObject> piecePrefabs = new List<GameObject>();
    public bool randomOrder = true;

    [Header("Active Piece Defaults")]
    public float defaultMoveSpeed = 6f;
    public float defaultRotSpeed = 120f;
    public float defaultHover = 0.2f;
    public bool defaultGridSnap = false;
    public float defaultCellSize = 0.5f;
    [Tooltip("Si true, la rotación será por pasos de 90°")]
    public bool defaultIsRotationFixed = false;

    [Header("Input (New Input System)")]
    [Tooltip("Player/Move (Vector2)")]
    public InputActionReference moveAction;
    [Tooltip("Player/ToggleAssemblyMode (Button) - T")]
    public InputActionReference toggleModeAction;
    [Tooltip("Player/PlacePiece (Button) - Enter")]
    public InputActionReference dropAction;
    [Tooltip("Player/Raise (Button) - Space")]
    public InputActionReference raiseAction;
    [Tooltip("Player/Lower (Button) - Ctrl")]
    public InputActionReference lowerAction;

    [Header("UI/Debug")]
    public ManipulationModeSnap startMode = ManipulationModeSnap.Rotation;

    int _spawnIndex = 0;
    SnappingPieceController _current;

    void OnEnable()
    {
        moveAction?.action.Enable();
        toggleModeAction?.action.Enable();
        dropAction?.action.Enable();
        raiseAction?.action.Enable();
        lowerAction?.action.Enable();
    }

    void OnDisable()
    {
        moveAction?.action.Disable();
        toggleModeAction?.action.Disable();
        dropAction?.action.Disable();
        raiseAction?.action.Disable();
        lowerAction?.action.Disable();
    }

    void Start()
    {
        if (buildingZone == null)
        {
            Debug.LogError("AssemblyPieceManager: Asigna un BuildingZoneArea.");
            enabled = false; return;
        }
        SpawnNextPiece();
    }

    void Update()
    {
        if (_current == null) return;

        // T: Rotación <-> Traslación
        if (toggleModeAction != null && toggleModeAction.action.triggered)
        {
            var newMode = _current.mode == ManipulationModeSnap.Rotation
                ? ManipulationModeSnap.Translation
                : ManipulationModeSnap.Rotation;
            _current.SetMode(newMode);
        }

        // Vector2 de flechas/WASD
        Vector2 arrows = moveAction != null ? moveAction.action.ReadValue<Vector2>() : Vector2.zero;

        // Vertical (Space/Ctrl) sólo en Traslación
        float yInput = 0f;
        if (raiseAction != null && raiseAction.action.IsPressed()) yInput += 1f;
        if (lowerAction != null && lowerAction.action.IsPressed()) yInput -= 1f;

        _current.HandleArrows(new Vector2(arrows.x, arrows.y), Time.deltaTime);
        _current.HandleVertical(yInput, Time.deltaTime);
        _current.Tick(Time.deltaTime);

        // Enter: soltar
        if (dropAction != null && dropAction.action.triggered)
            _current.BeginDrop();
    }

    void SpawnNextPiece()
    {
        if (piecePrefabs.Count == 0)
        {
            Debug.LogError("AssemblyPieceManager: Agrega prefabs de piezas.");
            return;
        }

        GameObject prefab = randomOrder
            ? piecePrefabs[Random.Range(0, piecePrefabs.Count)]
            : piecePrefabs[_spawnIndex++ % piecePrefabs.Count];

        // Punto de aparición
        Vector3 pos; Quaternion rot = Quaternion.identity;
        if (spawnPoint != null) { pos = spawnPoint.position; rot = spawnPoint.rotation; }
        else
        {
            var b = buildingZone.WorldBounds;
            pos = new Vector3(b.center.x, b.min.y, b.center.z);
        }

        var go = Instantiate(prefab, pos, rot);
        var manip = go.GetComponent<SnappingPieceController>();
        if (manip == null) manip = go.AddComponent<SnappingPieceController>();

        // Defaults
        manip.moveSpeed = defaultMoveSpeed;
        manip.rotationSpeedDegPerSec = defaultRotSpeed;
        manip.hoverHeight = defaultHover;
        manip.enableGridSnap = defaultGridSnap;
        manip.cellSize = defaultCellSize;
        manip.IsRotationFixed = defaultIsRotationFixed;
        manip.mode = startMode;

        manip.Activate(buildingZone, pos, rot);
        manip.OnPlaced += HandlePlaced;
        _current = manip;
    }

    void HandlePlaced(SnappingPieceController placed)
    {
        placed.OnPlaced -= HandlePlaced;
        _current = null;
        SpawnNextPiece();
    }
}
