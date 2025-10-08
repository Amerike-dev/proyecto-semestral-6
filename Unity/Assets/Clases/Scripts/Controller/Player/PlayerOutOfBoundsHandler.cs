using UnityEngine;
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

        if (!levelBounds.IsInsideBounds(transform.position))
        {
            StartCoroutine(RespawnPlayer());
        }
    }

    private System.Collections.IEnumerator RespawnPlayer()
    {
        yield return new WaitForSeconds(respawnDelay);

        Vector3 safePos = levelBounds.GetRespawnPoint();
        rb.position = safePos;
        rb.linearVelocity = Vector3.zero;

        Debug.Log($"Jugador {gameObject.name} respawneado en {safePos}");
    }
}

