using System.Collections.Generic;
using UnityEngine;

namespace BuildSystem
{
    // Clase de dominio (no es MonoBehaviour)
    public class BuildZone
    {
        public int Id { get; private set; }
        public int Capacity { get; private set; }

        private readonly List<GameObject> objects = new List<GameObject>();
        public bool IsComplete { get; private set; }

        public BuildZone(int id, int capacity)
        {
            Id = id;
            Capacity = Mathf.Max(0, capacity);
            IsComplete = false;
        }

        public bool CanAccept() => objects.Count < Capacity;

        public bool Add(GameObject obj)
        {
            if (obj == null) return false;
            if (!CanAccept()) return false;
            objects.Add(obj);
            return true;
        }

        public bool Remove(GameObject obj) => obj != null && objects.Remove(obj);

        public int Count() => objects.Count;

        // Regla simple: se puede fusionar si hay 2 o más piezas.
        public bool CanFuse() => objects.Count >= 2;

        // Devuelve un GameObject "fusionado", vacía la zona y marca IsComplete.
        public GameObject FuseAll()
        {
            if (!CanFuse()) return null;

            var fused = new GameObject("Fused");
            foreach (var o in objects)
            {
                if (o != null) o.transform.SetParent(fused.transform, worldPositionStays: true);
            }

            objects.Clear();
            IsComplete = true;
            return fused;
        }

        public void Clear()
        {
            objects.Clear();
            IsComplete = false;
        }
    }
}