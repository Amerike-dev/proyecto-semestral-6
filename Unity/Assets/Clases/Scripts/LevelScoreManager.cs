using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelScoreManager : MonoBehaviour
{
    public static LevelScoreManager Instance;

    [Header("Resultados del nivel actual")]
    [Tooltip("Lista con todas las calificaciones (A, B, C, D, E, S) generadas durante el nivel.")]
    public List<string> calificaciones = new List<string>();

    [Tooltip("Cantidad total de estrellas obtenidas (0 a 3).")]
    public int estrellasFinales = 0;
    public TextMeshProUGUI NdeEstrellasText;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }
    //Guarda un nuevo rank obtenido durante el nivel.
    public void RegistrarRank(string rank)
    {
        calificaciones.Add(rank);
        Debug.Log($"[LevelScoreManager] Se registró calificación: {rank}");
    }

    //Evalúa todas las calificaciones registradas y calcula una calificación final de 0 a 3.
    public void EvaluarNivelFinal()  // Llamar al finalizar el nivel
    {
        if (calificaciones.Count == 0)
        {
            estrellasFinales = 0;
            Debug.LogWarning("[LevelScoreManager] No hay calificaciones para evaluar.");
            return;
        }
        float totalValor = 0f;
        foreach (string rank in calificaciones)
            totalValor += RankToValue(rank);
        float promedio = totalValor / calificaciones.Count;
        estrellasFinales = CalcularEstrellas(promedio);
        Debug.Log($"[LevelScoreManager] Promedio: {promedio:F2} -> {estrellasFinales} estrellas");
        NdeEstrellasText.text = estrellasFinales.ToString();
    }

    //Convierte las letras en valores numéricos para evaluar el promedio.
    private int RankToValue(string rank)
    {
        switch (rank)
        {
            case "S": return 6;
            case "A": return 5;
            case "B": return 4;
            case "C": return 3;
            case "D": return 2;
            case "E": return 1;
            default: return 0;
        }
    }

    //Determina la cantidad de estrellas según el promedio de calificaciones.
    private int CalcularEstrellas(float promedio)
    {
        if (promedio >= 5.5f) return 3;
        if (promedio >= 4f) return 2;
        if (promedio >= 2f) return 1;
        return 0;
    }
    
    // Limpia los datos al reiniciar o comenzar un nuevo nivel.
    public void ResetearNivel()
    {
        calificaciones.Clear();
        estrellasFinales = 0;
    }
}
