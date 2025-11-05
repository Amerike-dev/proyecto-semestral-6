using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class FusionManager : MonoBehaviour
{
    [Header("Referencias principales")]
    public FusionZone areaDeFusion;
    public InputActionReference mergeAction;

    [Header("UI (asigna manualmente en el Inspector)")]
    [Tooltip("Arrastra aquí el objeto de texto que quieres usar para mostrar el estado y la calificación.")]
    public Text uiText;

    private float startTime;
    private bool isFusionActive = false;

    private void Start()
    {
        // Limpiar el texto al inicio
        if (uiText != null)
            uiText.text = "";

        startTime = Time.time;
    }

    private void OnEnable()
    {
        mergeAction.action.performed += OnFusionar;
        mergeAction.action.Enable();
    }

    private void OnDisable()
    {
        mergeAction.action.performed -= OnFusionar;
        mergeAction.action.Disable();
    }

    private void OnFusionar(InputAction.CallbackContext context)
    {
        if (!isFusionActive)
        {
            StartCoroutine(FusionarPiezasCoroutine());
        }
    }

    private IEnumerator FusionarPiezasCoroutine()
    {
        if (areaDeFusion.pieces.Count == 0 || uiText == null) yield break;

        isFusionActive = true;

        // Mostrar loader
        uiText.text = "Evaluando piezas...";
        yield return new WaitForSeconds(1f);

        // Crear grupo fusionado
        GameObject grupoFusionado = new GameObject("GrupoFusionado");
        grupoFusionado.transform.position = CalcularCentro(areaDeFusion.pieces);

        foreach (GameObject pieza in areaDeFusion.pieces)
        {
            pieza.transform.SetParent(grupoFusionado.transform);
        }

        // Calcular calificación
        float totalTime = Time.time - startTime;
        string rank = CalcularRango(totalTime);

        // Mostrar calificación
        uiText.text = "Calificación: " + rank;
        yield return new WaitForSeconds(3f);

        // Eliminar el objeto fusionado
        Destroy(grupoFusionado);
        areaDeFusion.pieces.Clear();

        // Limpiar texto
        uiText.text = "";
        startTime = Time.time;
        isFusionActive = false;

        // Enviar calificación al LevelScoreManager
        LevelScoreManager.Instance.RegistrarRank(rank);
    }

    private string CalcularRango(float time)
    {
        if (time <= 5f) return "S";
        if (time <= 10f) return "A";
        if (time <= 15f) return "B";
        if (time <= 20f) return "C";
        if (time <= 25f) return "D";
        return "E";
    }

    private Vector3 CalcularCentro(List<GameObject> piezas)
    {
        Vector3 centro = Vector3.zero;
        foreach (GameObject pieza in piezas)
        {
            centro += pieza.transform.position;
        }
        return centro / piezas.Count;
    }
}
