using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
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

    public Vector3 GetRespawnPoint()
    {
        if (respawnPoint != null)
            return respawnPoint.position;

        Debug.LogWarning("No se asigno un respawnPoint en LevelBounds. Se usara el centro del mapa.");
        return boundsCollider.bounds.center;
    }
}

