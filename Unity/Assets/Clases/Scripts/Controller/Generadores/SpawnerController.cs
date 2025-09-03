/* Con el Object Pooling solo se tiene que desactivar 
el ladrillo para que se regrese en automatico*/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnerController : MonoBehaviour
{
    [Header("Prefab a Instanciar")]
    public GameObject brickPrefab;

    [Header("Cantidad Inicial en Pool")]
    public int poolSize = 3;

    [Header("Tiempo entre Spawns (segundos)")]
    public float spawnInterval = 2f;

    private SpawnerGeneral spawner;

    void Awake()
    {
        // Inicializa el spawner y su lista
        spawner = new SpawnerGeneral
        {
            brickPrefab = brickPrefab,
            bricksList = new List<GameObject>()
        };

        // Pre-cargar objetos en el pool
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(brickPrefab, transform.position, Quaternion.identity);
            obj.SetActive(false);
            spawner.bricksList.Add(obj);
        }
    }

    void Start()
    {
        // Comienza el spawn automático
        StartCoroutine(AutoSpawn());
    }
    
    //Pide un objeto del pool en la posición del spawner
    public GameObject SpawnObject()
    {
        return spawner.AskForObject(transform.position);
    }

    // Corutina que genera objetos automáticamente
    private IEnumerator AutoSpawn()
    {
        while (true)
        {
            SpawnObject();
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
