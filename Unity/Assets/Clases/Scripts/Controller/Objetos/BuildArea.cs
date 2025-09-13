// Este script debe ser asignado a un GameObject con un Collider configurado como Trigger.

// Este script se encarga de detectar los ladrillos que entran en el área de construcción y contar cuántos hay de cada tipo.
// Cada ladrillo debe tener un ObjectController con su ID correspondiente.

using UnityEngine;
using System.Collections.Generic;

public class BuildArea : MonoBehaviour
{
    private Dictionary<int, int> objectCounts = new Dictionary<int, int>();

    [System.Serializable]
    public class CountData
    {
        public int objectID;
        public int count;
    }

    [Header("Debug en Inspector")]
    public List<CountData> debugList = new List<CountData>();

    private void OnTriggerEnter(Collider other)
    {
        ObjectController obj = other.GetComponent<ObjectController>();
        if (obj != null)
        {
            int id = obj.objectID;

            if (!objectCounts.ContainsKey(id))
                objectCounts[id] = 0;

            objectCounts[id]++;

            UpdateDebugList();

            Debug.Log($"[BuildArea] Ladrillo ID={id} detectado. Total ahora: {objectCounts[id]}");

            // Desactivamos el ladrillo después de registrarlo
            other.gameObject.SetActive(false);
        }
    }

    private void UpdateDebugList()
    {
        debugList.Clear();
        foreach (var kvp in objectCounts)
        {
            debugList.Add(new CountData { objectID = kvp.Key, count = kvp.Value });
        }
    }

    public Dictionary<int, int> GetObjectCounts()
    {
        return new Dictionary<int, int>(objectCounts);
    }
}
