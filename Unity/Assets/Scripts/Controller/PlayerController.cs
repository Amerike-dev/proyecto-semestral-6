using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Configuración del Jugador")]
    [SerializeField] private int playerID = 1;
    [SerializeField] private string playerName = "Player";

    // Esto referencia al objeto Player
    public Player PlayerData { get; private set; }

    private void Awake()
    {
        InitializePlayer();
    }

    private void InitializePlayer()
    {
        // Esto crea una nueva instancia del jugador con los datos configurados.
        PlayerData = new Player(playerID, playerName);
        
        Debug.Log($"Jugador inicializado: {PlayerData.PlayerName} (ID: {PlayerData.PlayerID})");
    }


}

