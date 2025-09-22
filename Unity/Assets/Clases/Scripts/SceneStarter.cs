// SceneStarter.cs
using UnityEngine;

public class SceneStarter : MonoBehaviour
{
    public PlayerController initialPlayer; // asignar en inspector
    public GameObject brickPrefab;         // asignar prefab Brick

    void Start()
    {
        if (initialPlayer == null || brickPrefab == null) return;

        // Instancia y fuerza que el jugador la recoja
        GameObject b = Instantiate(brickPrefab, initialPlayer.transform.position + Vector3.up * 1f, Quaternion.identity);
        var oi = initialPlayer.GetComponent<ObjectInteraction>();
        if (oi != null)
        {
            oi.ForcePickup(b, initialPlayer);
        }
    }
}
