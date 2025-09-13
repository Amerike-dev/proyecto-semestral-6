//Este script va en un objeto vacio en la escena y luego tienes que asignar 
//cual es el ModelRecipe y cual es el BuildArea que quieres evaluar
//Luego asignas a un boton la funcion EvaluateAndPrint para que veas el resultado en consola

//Este escript evalua el area de construccion y compara los objetos colocados con los requeridos en la receta
//Devuelve un porcentaje de cuan bien se hizo la construccion

using UnityEngine;
using System.Collections.Generic;

public class ModelEvaluator : MonoBehaviour
{
    [Header("Asignar desde Inspector")]
    public ModelRecipe recipe;
    public BuildArea buildArea;

    public float Evaluate()
    {
        if (recipe == null || buildArea == null)
        {
            Debug.LogWarning("ModelEvaluator: Faltan referencias en el Inspector");
            return 0f;
        }

        Dictionary<int, int> placed = buildArea.GetObjectCounts();

        int totalRequired = 0;
        int totalCorrect = 0;

        foreach (var req in recipe.requirements)
        {
            totalRequired += req.requiredAmount;

            if (placed.ContainsKey(req.objectID))
            {
                int correct = Mathf.Min(req.requiredAmount, placed[req.objectID]);
                totalCorrect += correct;
            }
        }

        if (totalRequired == 0) return 0f;
        return (float)totalCorrect / totalRequired * 100f;
    }
    public void EvaluateAndPrint()
    {
        float result = Evaluate();
        Debug.Log($"Resultado de la calificación: {result}%");
    }
}
