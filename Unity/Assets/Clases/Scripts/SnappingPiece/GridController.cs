using UnityEngine;

[ExecuteAlways]
public class GridController : MonoBehaviour
{
    [Header("Referencia a la pieza")]
    public SnappingPieceController piece;

    [Header("Ajustes del Grid")]
    public float cellSize = 1f;
    public float yOffset = 0.01f;   // Para que no se superponga al suelo

    [Header("Opcional")]
    public bool followRotation = false;

    Renderer _renderer;

    void Awake()
    {
        _renderer = GetComponent<Renderer>();
    }

    void Update()
    {
        if (piece == null) return;

        Vector3Int cell = piece.CurrentGridCell;

        // Convertir de celda a coordenadas del mundo
        Vector3 worldPos = new Vector3(
            cell.x * cellSize,
            cell.y * cellSize,
            cell.z * cellSize
        );

        // Ajustar para quedar justo sobre el piso
        worldPos.y += yOffset;

        transform.position = worldPos;

        if (followRotation)
            transform.rotation = piece.transform.rotation;
        else
            transform.rotation = Quaternion.identity;

        transform.localScale = new Vector3(cellSize, 1f, cellSize);
    }

    public void SetColor(Color c)
    {
        if (_renderer != null)
        {
            _renderer.material.color = c;
        }
    }
}
