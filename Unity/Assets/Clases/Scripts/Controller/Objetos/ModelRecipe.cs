// Este script debe ser asignado a un GameObject que representa un modelo que se puede construir.

// Este script define los requisitos para construir un modelo específico.
// Cada requisito especifica un ID de objeto y la cantidad necesaria de ese objeto.

using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class RecipeRequirement
{
    public int objectID;
    public int requiredAmount;
}

public class ModelRecipe : MonoBehaviour
{
    [Header("Requisitos del modelo")]
    public List<RecipeRequirement> requirements = new List<RecipeRequirement>();
}
