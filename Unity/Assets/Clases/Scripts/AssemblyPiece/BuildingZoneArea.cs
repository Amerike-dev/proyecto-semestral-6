using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(BoxCollider))]
public class BuildingZoneArea : MonoBehaviour
{
    [Tooltip("Color del gizmo del area de construccion.")]
    public Color gizmoColor = new Color(0f, 0.6f, 1f, 0.15f);

    [Header("Grid (visual y snap)")]
    [Tooltip("Tamaño de celda de la cuadrícula (úsalo también en las piezas).")]
    public float gridCellSize = 1f;

    [Tooltip("Dibujar la cuadrícula sobre el piso del área.")]
    public bool showGrid = true;

    [Tooltip("Color de líneas de la cuadrícula (Gizmos).")]
    public Color gridLineColor = new Color(1f, 1f, 1f, 0.2f);

    [Tooltip("Color del highlight de la celda objetivo (malla runtime).")]
    public Color highlightColor = new Color(1f, 0.9f, 0.2f, 0.35f);

    BoxCollider _box;
    GameObject _highlightGO;
    Material _highlightMat;

    void EnsureHighlight()
    {
        if (_highlightGO != null) return;

        _highlightGO = GameObject.CreatePrimitive(PrimitiveType.Quad);
        _highlightGO.name = "[GridCellHighlight]";
        _highlightGO.transform.SetParent(transform, worldPositionStays: false);
        _highlightGO.transform.localRotation = Quaternion.Euler(90f, 0f, 0f); // quad acostado (XZ)
        _highlightGO.GetComponent<Collider>().enabled = false;

        // material sin iluminación
        var mr = _highlightGO.GetComponent<MeshRenderer>();
        _highlightMat = new Material(Shader.Find("Unlit/Color"));
        _highlightMat.color = highlightColor;
        mr.sharedMaterial = _highlightMat;

        _highlightGO.SetActive(false);
    }

    //Snapea XZ al centro de la celda, alineado al borde mínimo del área.
    public Vector3 SnapToGrid(Vector3 worldPos)
    {
        var b = WorldBounds;
        float cell = Mathf.Max(0.0001f, gridCellSize);

        // Índices de celda desde el min
        float ix = Mathf.Floor((worldPos.x - b.min.x) / cell) + 0.5f;
        float iz = Mathf.Floor((worldPos.z - b.min.z) / cell) + 0.5f;

        float x = b.min.x + ix * cell;
        float z = b.min.z + iz * cell;

        return new Vector3(x, worldPos.y, z);
    }

    //Muestra el highlight en el centro de celda dado (XZ). Y se coloca sobre el piso.
    public void ShowHighlightAt(Vector3 snappedXZCenter)
    {
        EnsureHighlight();

        var b = WorldBounds;
        // altura apenas sobre el piso del área
        float y = b.min.y + 0.01f;
        _highlightGO.transform.position = new Vector3(snappedXZCenter.x, y, snappedXZCenter.z);
        _highlightGO.transform.localScale = new Vector3(gridCellSize, gridCellSize, 1f);
        _highlightGO.SetActive(true);
    }

    //Oculta el highlight
    public void HideHighlight()
    {
        if (_highlightGO != null) _highlightGO.SetActive(false);
    }


    public Bounds WorldBounds
    {
        get
        {
            EnsureBox();
            var b = _box.bounds;
            return b;
        }
    }

    void EnsureBox()
    {
        if (_box == null) _box = GetComponent<BoxCollider>();
    }

    public Vector3 ClampInside(Vector3 position, Vector3 halfExtents)
    {
        var b = WorldBounds;
        float x = Mathf.Clamp(position.x, b.min.x + halfExtents.x, b.max.x - halfExtents.x);
        float z = Mathf.Clamp(position.z, b.min.z + halfExtents.z, b.max.z - halfExtents.z);
        return new Vector3(x, position.y, z);
    }

    void OnDrawGizmos()
    {
        EnsureBox();
        if (_box == null) return;

        Gizmos.color = gizmoColor;
        Gizmos.matrix = Matrix4x4.identity;
        Gizmos.DrawCube(_box.bounds.center, _box.bounds.size);

        Gizmos.color = Color.cyan;
        var b = _box.bounds;
        Vector3 floorCenter = new Vector3(b.center.x, b.min.y, b.center.z);
        Vector3 floorSize = new Vector3(b.size.x, 0.01f, b.size.z);
        Gizmos.DrawCube(floorCenter, floorSize);

        if (showGrid && gridCellSize > 0.0001f)
        {
            Gizmos.color = gridLineColor;
            float cell = gridCellSize;

            // líneas paralelas al eje X
            for (float z = b.min.z; z <= b.max.z + 0.0001f; z += cell)
                Gizmos.DrawLine(new Vector3(b.min.x, b.min.y + 0.02f, z), new Vector3(b.max.x, b.min.y + 0.02f, z));

            // líneas paralelas al eje Z
            for (float x = b.min.x; x <= b.max.x + 0.0001f; x += cell)
                Gizmos.DrawLine(new Vector3(x, b.min.y + 0.02f, b.min.z), new Vector3(x, b.min.y + 0.02f, b.max.z));
        }

    }
}