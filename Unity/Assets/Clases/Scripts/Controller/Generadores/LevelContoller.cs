using UnityEngine;

public class LevelContoller : MonoBehaviour
{
    public GameObject prefab;
    public Transform spawnPoint;
    void Start()
    {
        Instantiate(prefab, spawnPoint.position, Quaternion.identity);
    }

}
