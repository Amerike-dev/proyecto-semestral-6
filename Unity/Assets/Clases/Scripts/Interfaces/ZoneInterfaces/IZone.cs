using UnityEngine;

namespace BuildSystem
{
    public interface IZone
    {
        //Cantidad de objetos registrados actualmente en la zona.
        int Count { get; }

        //Notifica que un objeto/pieza entra a la zona.
        void Accept(GameObject piece);

        //Notifica que un objeto/pieza sale o se retira de la zona.
        void Remove(GameObject piece);

        //Limpia referencias nulas.
        void ClearDestroyedPieces();
    }
}

