using System.Collections.Generic;
using UnityEngine;

public class FusionZone : MonoBehaviour
{
    public List<GameObject> pieces = new List<GameObject>();

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Piece") && !pieces.Contains(other.gameObject))
        {
            pieces.Add(other.gameObject);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Piece") && pieces.Contains(other.gameObject))
        {
            pieces.Remove(other.gameObject);
        }
    }
}
