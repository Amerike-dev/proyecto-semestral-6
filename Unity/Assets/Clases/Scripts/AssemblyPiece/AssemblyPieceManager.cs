/*

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AssemblyPieceManager : MonoBehaviour
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
    public bool defaultGridSnap = false;   // OFF para evitar bloqueos por redondeo
    public float defaultCellSize = 0.5f;

    [Header("Input (New Input System)")]
    [Tooltip("Player/Move (Vector2)")]
    public InputActionReference moveAction;
    [Tooltip("Player/ToggleAssemblyMode (Button)")]
    public InputActionReference toggleModeAction;
    [Tooltip("Player/PlacePiece (Button)")]
    public InputActionReference dropAction;

    [Header("UI/Debug")]
    public ManipulationMode startMode = ManipulationMode.Rotation;

    int _spawnIndex = 0;
    PieceManipulator _current;

    void OnEnable()
    {
        if (moveAction != null) moveAction.action.Enable();
        if (toggleModeAction != null) toggleModeAction.action.Enable();
        if (dropAction != null) dropAction.action.Enable();
    }

    void OnDisable()
    {
        if (moveAction != null) moveAction.action.Disable();
        if (toggleModeAction != null) toggleModeAction.action.Disable();
        if (dropAction != null) dropAction.action.Disable();
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

        if (toggleModeAction != null && toggleModeAction.action.triggered)
        {
            var newMode = _current.mode == ManipulationMode.Rotation
                ? ManipulationMode.Translation
                : ManipulationMode.Rotation;

            _current.SetMode(newMode);
        }

        Vector2 arrows = Vector2.zero;
        if (moveAction != null)
            arrows = moveAction.action.ReadValue<Vector2>();

        _current.HandleArrows(new Vector2(arrows.x, arrows.y), Time.deltaTime);
        _current.Tick(Time.deltaTime);

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

        Vector3 pos; Quaternion rot = Quaternion.identity;
        if (spawnPoint != null)
        {
            pos = spawnPoint.position;
            rot = spawnPoint.rotation;
        }
        else
        {
            var b = buildingZone.WorldBounds;
            pos = new Vector3(b.center.x, b.min.y, b.center.z);
        }

        var go = Instantiate(prefab, pos, rot);
        var manip = go.GetComponent<PieceManipulator>();
        if (manip == null) manip = go.AddComponent<PieceManipulator>();

        manip.moveSpeed = defaultMoveSpeed;
        manip.rotationSpeedDegPerSec = defaultRotSpeed;
        manip.hoverHeight = defaultHover;
        manip.enableGridSnap = defaultGridSnap;
        manip.cellSize = defaultCellSize;
        manip.mode = startMode;

        manip.Activate(buildingZone, pos, rot);
        manip.OnPlaced += HandlePlaced;

        _current = manip;
    }

    void HandlePlaced(PieceManipulator placed)
    {
        placed.OnPlaced -= HandlePlaced;
        _current = null;
        SpawnNextPiece();
    }
}
*/

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AssemblyPieceManager : MonoBehaviour
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
    public ManipulationMode startMode = ManipulationMode.Rotation;

    int _spawnIndex = 0;
    PieceManipulator _current;

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
            var newMode = _current.mode == ManipulationMode.Rotation
                ? ManipulationMode.Translation
                : ManipulationMode.Rotation;
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
        var manip = go.GetComponent<PieceManipulator>();
        if (manip == null) manip = go.AddComponent<PieceManipulator>();

        // Defaults
        manip.moveSpeed = defaultMoveSpeed;
        manip.rotationSpeedDegPerSec = defaultRotSpeed;
        manip.hoverHeight = defaultHover;
        manip.enableGridSnap = defaultGridSnap;
        manip.cellSize = defaultCellSize;
        manip.IsRotationFixed = defaultIsRotationFixed;   // << aquí
        manip.mode = startMode;

        manip.Activate(buildingZone, pos, rot);
        manip.OnPlaced += HandlePlaced;
        _current = manip;
    }

    void HandlePlaced(PieceManipulator placed)
    {
        placed.OnPlaced -= HandlePlaced;
        _current = null;
        SpawnNextPiece();
    }
}
