using UnityEngine;
using System;

public class GeneralSpawner : MonoBehaviour
{
    public GameObject Spawn(GameObject prefab, Vector3 position)
    {
        if (prefab == null)
            throw new ArgumentNullException(nameof(prefab));

        GameObject obj = Instantiate(prefab, position, Quaternion.identity);
        return obj;
    }
}