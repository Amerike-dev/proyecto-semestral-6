using UnityEngine;

// Manager central para manejar respawns.
public class RespawnManager : MonoBehaviour
{
    public static RespawnManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
    }

    public void RespawnPlayer(GameObject player, Vector3 respawnPoint)
    {
        var rb = player.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.position = respawnPoint;
            rb.linearVelocity = Vector2.zero;
        }
        else
        {
            player.transform.position = respawnPoint;
        }
    }
}
