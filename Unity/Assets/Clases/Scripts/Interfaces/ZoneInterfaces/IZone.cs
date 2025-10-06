using UnityEngine;

namespace BuildSystem
{
    public interface IZone
    {
        int Count { get; }

        void Accept(GameObject piece);

        void Remove(GameObject piece);

        void ClearDestroyedPieces();
    }
}

