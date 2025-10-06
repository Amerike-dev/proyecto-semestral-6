using UnityEngine;

public class SceneStarter : MonoBehaviour
{
    public PlayerController initialPlayer; 
    public GameObject brickPrefab;

    void Start()
    {
        if (initialPlayer == null || brickPrefab == null) return;

        GameObject b = Instantiate(brickPrefab, initialPlayer.transform.position + Vector3.up * 1f, Quaternion.identity);
        var oi = initialPlayer.GetComponent<ObjectInteraction>();
        if (oi != null)
        {
            oi.ForcePickup(b, initialPlayer);
        }
    }
}
