using UnityEngine;

[ExecuteAlways]
public class GridController : MonoBehaviour
{
    [Header("References")]
    public Transform plane;
    public SnapAssemblyPieceManager manager;

    [Header("Grid Display")]
    public Color gridColor = Color.green;
    public float lineWidth = 0.05f;

    [Header("Display Modes")]
    public bool showInScene = true;
    public bool showInGame = false;
    public Material lineMaterial;

    private float cellSize = 1f;
    private int gridWidth;
    private int gridHeight;
    private bool needsUpdate = false;

    private void Start()
    {
        if (showInGame && plane != null && manager != null)
        {
            UpdateGridSize();
            DrawGridInGame();
        }
    }

    private void Update()
    {
        if (needsUpdate)
        {
            if (showInGame && plane != null && manager != null)
            {
                UpdateGridSize();
                DrawGridInGame();
            }
            needsUpdate = false;
        }
    }

    public void UpdateGridSize()
    {
        if (plane == null || manager == null) return;

        cellSize = manager.defaultCellSize;

        Renderer r = plane.GetComponent<Renderer>();
        if (r != null)
        {
            Vector3 size = r.bounds.size;
            gridWidth = Mathf.RoundToInt(size.x / cellSize);
            gridHeight = Mathf.RoundToInt(size.z / cellSize);
        }
    }

    private void OnDrawGizmos()
    {
        if (!showInScene || plane == null || manager == null) return;

        UpdateGridSize();
        Gizmos.color = gridColor;

        Vector3 origin = plane.position;
        origin.x -= (gridWidth * cellSize) / 2f;
        origin.z -= (gridHeight * cellSize) / 2f;

        for (int x = 0; x <= gridWidth; x++)
        {
            Vector3 start = origin + new Vector3(x * cellSize, 0, 0);
            Vector3 end = origin + new Vector3(x * cellSize, 0, gridHeight * cellSize);
            Gizmos.DrawLine(start, end);
        }

        for (int z = 0; z <= gridHeight; z++)
        {
            Vector3 start = origin + new Vector3(0, 0, z * cellSize);
            Vector3 end = origin + new Vector3(gridWidth * cellSize, 0, z * cellSize);
            Gizmos.DrawLine(start, end);
        }
    }

    private void DrawGridInGame()
    {
        Vector3 origin = plane.position;
        origin.x -= (gridWidth * cellSize) / 2f;
        origin.z -= (gridHeight * cellSize) / 2f;

        for (int x = 0; x <= gridWidth; x++)
        {
            CreateLine(
                origin + new Vector3(x * cellSize, 0, 0),
                origin + new Vector3(x * cellSize, 0, gridHeight * cellSize)
            );
        }

        for (int z = 0; z <= gridHeight; z++)
        {
            CreateLine(
                origin + new Vector3(0, 0, z * cellSize),
                origin + new Vector3(gridWidth * cellSize, 0, z * cellSize)
            );
        }
    }

    private void CreateLine(Vector3 start, Vector3 end)
    {
        GameObject lineObj = new GameObject("GridLine");
        lineObj.transform.parent = transform;

        LineRenderer lr = lineObj.AddComponent<LineRenderer>();
        lr.material = lineMaterial != null
            ? lineMaterial
            : new Material(Shader.Find("Sprites/Default"));

        lr.startColor = gridColor;
        lr.endColor = gridColor;
        lr.startWidth = lineWidth;
        lr.endWidth = lineWidth;
        lr.positionCount = 2;
        lr.useWorldSpace = true;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
    }
}