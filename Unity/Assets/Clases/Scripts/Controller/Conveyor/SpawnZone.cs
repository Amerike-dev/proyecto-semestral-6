using UnityEngine;
using System.Collections.Generic;

public class SpawnZone : MonoBehaviour
{
    private HashSet<GameObject> objectsInZone = new HashSet<GameObject>();

    public bool IsEmpty => objectsInZone.Count == 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Brick"))
        {
            objectsInZone.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Brick"))
        {
            objectsInZone.Remove(other.gameObject);
        }
    }
}
