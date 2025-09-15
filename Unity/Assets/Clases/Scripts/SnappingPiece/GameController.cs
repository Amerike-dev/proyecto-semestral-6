using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }

    [Header("Prefabs")]
    public GameObject piecePrefab;

    private SnappingPieceController activePiece;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        SpawnPiece();
    }

    public void SpawnPiece()
    {
        if (piecePrefab == null)
        {
            Debug.LogError("No se asignó prefab de pieza en GameController.");
            return;
        }

        GameObject obj = Instantiate(piecePrefab);
        activePiece = obj.GetComponent<SnappingPieceController>();
    }

    public void OnPieceDropped(SnappingPieceController piece)
    {
        //Debug.Log("Pieza soltada en celda: " + piece.GetCurrentCell());
        SpawnPiece();
    }
}
