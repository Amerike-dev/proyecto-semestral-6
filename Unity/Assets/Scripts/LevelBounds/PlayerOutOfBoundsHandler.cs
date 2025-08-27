using UnityEngine;

//Script unido al Player

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerOutOfBoundsHandler : MonoBehaviour
{
    private Rigidbody rb;
    private LevelBounds levelBounds;

    [Header("Respawn")]
    public float respawnDelay = 0.5f;

    private void Start()
    {
        rb = GetComponent < Rigidbody>();
        levelBounds = FindAnyObjectByType<LevelBounds>();

        if (levelBounds == null)
            Debug.LogError("No hay un level bounds");
    }

    private void Update()
    {
        if (levelBounds == null) return;

        // Verificar si el jugador está fuera de los límites
        if (!levelBounds.IsInsideBounds(transform.position))
        {
            StartCoroutine(RespawnPlayer());
        }
    }

    private System.Collections.IEnumerator RespawnPlayer()
    {
        yield return new WaitForSeconds(respawnDelay);

        // Reposicionar al jugador en el SpawnPoint
        Vector3 safePos = levelBounds.GetRespawnPoint();
        rb.position = safePos;
        rb.linearVelocity = Vector2.zero;

        Debug.Log($"Jugador {gameObject.name} respawneado en {safePos}");
    }
}

