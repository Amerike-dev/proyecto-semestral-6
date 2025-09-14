using System.Collections.Generic;
using UnityEngine;

namespace BuildSystem
{
    public class BuildZone : IZone
    {
        public int Id { get; private set; }
        public int Capacity { get; private set; } 
        public bool IsComplete { get; private set; }

        private readonly List<GameObject> objects = new List<GameObject>();

        public BuildZone(int id, int capacity)
        {
            Id = id;
            Capacity = Mathf.Max(1, capacity);
            IsComplete = false;
        }

        public int Count => objects.Count;

        public void Accept(GameObject piece)
        {
            if (piece == null) return;
            ClearDestroyedPieces();
            if (IsComplete) return;
            if (objects.Contains(piece)) return;
            if (objects.Count >= Capacity) return; 
            objects.Add(piece);
        }

        public void Remove(GameObject piece)
        {
            if (piece == null) return;
            objects.Remove(piece);
            ClearDestroyedPieces();
        }

        public void ClearDestroyedPieces()
        {
            objects.RemoveAll(p => p == null);
        }

        public bool CanFuse()
        {
            ClearDestroyedPieces();
            return !IsComplete && objects.Count > 0;
        }

        public GameObject FuseAll()
        {
            if (!CanFuse()) return null;


            Vector3 center = Vector3.zero;
            int validCount = 0;
            foreach (var o in objects)
            {
                if (o == null) continue;
                center += o.transform.position;
                validCount++;
            }
            if (validCount > 0) center /= validCount;

            var fused = new GameObject($"FusedPiece_{Id}");
            fused.transform.position = center;
            fused.transform.rotation = Quaternion.identity;

            // Reparentar todas las piezas a la nueva raíz
            foreach (var o in objects)
            {
                if (o == null) continue;
                o.transform.SetParent(fused.transform, worldPositionStays: true);
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
