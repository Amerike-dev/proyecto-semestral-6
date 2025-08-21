using UnityEngine;
using System.Collections.Generic;

public class SpawnerGeneral
{
    public GameObject brickPrefab;
    
    // Lista de objetos que se pueden reutilizar (Object Pooling)
    public List<GameObject> bricksList;

    public GameObject AskForObject(Vector3 position)
    {
        for (int i = 0; i < bricksList.Count; i++)
        {
            if (!bricksList[i].activeInHierarchy)
            {
                bricksList[i].transform.position = position;
                bricksList[i].SetActive(true);
                return bricksList[i];
            }
        }

        // Si no hay objetos libres en el pool, regresa null
        return null;
    }
}
