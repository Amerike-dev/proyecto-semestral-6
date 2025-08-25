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
                GameObject brick = bricksList[i];

                // Reiniciar transform
                brick.transform.position = position;
                brick.transform.rotation = Quaternion.identity;

                // Reiniciar Rigidbody (si tiene)
                Rigidbody rb = brick.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.linearVelocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                }
                brick.SetActive(true);
                return brick;
            }
        }
        return null; // No hay objetos libres en el pool
    }

}
