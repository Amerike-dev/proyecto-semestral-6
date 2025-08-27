using UnityEngine;

// Define los límites jugables del nivel y un punto de respawn.
// Se puede configurar con un BoxCollider2D o con valores manuales.

[RequireComponent(typeof(BoxCollider2D))]
public class LevelBounds : MonoBehaviour
{
    [Header("Respawn Settings")]
    public Transform respawnPoint;

    private BoxCollider boundsCollider;

    private void Awake()
    {
        boundsCollider = GetComponent<BoxCollider>();
        boundsCollider.isTrigger = true;
    }

    public bool IsInsideBounds(Vector2 position)
    {
        return boundsCollider.bounds.Contains(position);
    }

    // Devuelve un punto de respawn.
    public Vector3 GetRespawnPoint()
    {
        if (respawnPoint != null)
            return respawnPoint.position;

        Debug.LogWarning("No se asignó un respawnPoint en LevelBounds. Se usará el centro del mapa.");
        return boundsCollider.bounds.center;
    }
}

