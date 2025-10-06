/* Con el Object Pooling solo se tiene que desactivar 
el ladrillo para que se regrese en automatico*/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnerController : MonoBehaviour
{
    [Header("Zona de Spawn")]
    [SerializeField] private SpawnZone spawnZone;

    [Header("Prefab a Instanciar")]
    public GameObject brickPrefab;

    [Header("Cantidad Inicial en Pool")]
    public int poolSize = 3;

    [Header("Tiempo entre Spawns (segundos)")]
    public float spawnInterval = 2f;

    private SpawnerGeneral spawner;

    void Awake()
    {
        spawner = new SpawnerGeneral
        {
            brickPrefab = brickPrefab,
            bricksList = new List<GameObject>()
        };

        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(brickPrefab, transform.position, Quaternion.identity);
            obj.SetActive(false);
            spawner.bricksList.Add(obj);
        }
    }

    void Start()
    {
        StartCoroutine(AutoSpawn());
    }
    
    public GameObject SpawnObject()
    {
        if (spawnZone == null || spawnZone.IsEmpty)
        {
            return spawner.AskForObject(transform.position);
        }
        return null;
    }

    private IEnumerator AutoSpawn()
    {
        while (true)
        {
            SpawnObject();
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
