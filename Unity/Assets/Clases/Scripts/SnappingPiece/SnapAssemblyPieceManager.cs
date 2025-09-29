using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class SnapAssemblyPieceManager : MonoBehaviour
{
    [Header("Scene References")]
    public BuildingZoneArea buildingZone;
    public Transform spawnPoint;

    [Header("Pieces")]
    public List<GameObject> piecePrefabs = new List<GameObject>();
    public bool randomOrder = true;

    [Header("Active Piece Defaults")]
    public float defaultMoveSpeed = 6f;
    public float defaultRotSpeed = 120f;
    public float defaultHover = 0.5f;
    public bool defaultGridSnap = true;

    [Range(0.5f, 5f)]
    public float defaultCellSize = 5f;

    public bool defaultIsRotationFixed = false;

    [Header("Input (New Input System)")]
    public InputActionReference moveAction;
    public InputActionReference dropAction;

    [Header("UI/Debug")]
    public ManipulationModeSnap startMode = ManipulationModeSnap.Translation;
    public TextMeshProUGUI coordText;

    int _spawnIndex = 0;
    SnappingPieceController _current;

    void OnEnable()
    {
        moveAction?.action.Enable();
        dropAction?.action.Enable();
    }

    void OnDisable()
    {
        moveAction?.action.Disable();
        dropAction?.action.Disable();
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

        Vector2 arrows = moveAction != null ? moveAction.action.ReadValue<Vector2>() : Vector2.zero;
        if (arrows != Vector2.zero)
            _current.HandleArrows(arrows, Time.deltaTime);

        _current.Tick(Time.deltaTime);

        if (dropAction != null && dropAction.action.triggered)
        {
            _current.BeginDrop();
        }

        Vector3 pos = _current.transform.position;
        string coords = $"Coords: ({pos.x:F2}, {pos.y:F2}, {pos.z:F2})";

        if (coordText != null)
            coordText.text = coords;

        Debug.Log(coords);
    }

    void SpawnNextPiece()
    {
        if (piecePrefabs.Count == 0) return;

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
            pos = new Vector3(b.center.x, b.min.y + defaultHover, b.center.z);
        }

        var go = Instantiate(prefab, pos, rot);
        var manip = go.GetComponent<SnappingPieceController>();
        manip.cellSize = defaultCellSize;

        if (manip == null) manip = go.AddComponent<SnappingPieceController>();

        manip.cellSize = defaultCellSize;
        manip.manager = this;
        manip.buildingZone = buildingZone;
        manip.moveSpeed = defaultMoveSpeed;
        manip.rotationSpeedDegPerSec = defaultRotSpeed;
        manip.IsRotationFixed = defaultIsRotationFixed;

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