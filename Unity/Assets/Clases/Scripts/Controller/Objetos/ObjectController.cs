// Este script debe ser asignado a cada ladrillo u objeto que se pueda usar en la construcción.
//la variable objectID define el tipo de ladrillo 

using UnityEngine;

public class ObjectController : MonoBehaviour
{
    [Header("ID del objeto / tipo de ladrillo")]
    public int objectID; // 0 = madera, 1 = metal, 2 = roca, etc.
}
