using System.Collections.Generic;
using UnityEngine;

namespace BuildSystem
{

    public class TrashZone : IZone
    {
        public Vector3 position { get; private set; }
        public float radius { get; private set; }

        private readonly List<GameObject> discardedPieces = new List<GameObject>();

        public TrashZone(Vector3 position, float radius)
        {
            this.position = position;
            this.radius = Mathf.Max(0f, radius);
        }


        public int Count => discardedPieces.Count;

        public void Accept(GameObject piece)
        {
            if (piece == null) return;
            if (!discardedPieces.Contains(piece))
                discardedPieces.Add(piece);

            Object.Destroy(piece);
        }

        public void Remove(GameObject piece)
        {
            if (piece != null)
                discardedPieces.Remove(piece);

            ClearDestroyedPieces();
        }

        public void ClearDestroyedPieces()
        {
            discardedPieces.RemoveAll(p => p == null);
        }

        public void DiscardSinglePiece(GameObject piece) => Accept(piece);

        public void DiscardMergedPiece(GameObject mergedPiece) => Accept(mergedPiece);
    }
}
