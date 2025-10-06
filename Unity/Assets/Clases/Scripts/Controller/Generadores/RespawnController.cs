using UnityEngine;

public class RespawnContoller : MonoBehaviour
{
    public GameObject prefab;
    public Transform spawnPoint;
    void Start()
    {
        Instantiate(prefab, spawnPoint.position, Quaternion.identity);
    }

}
