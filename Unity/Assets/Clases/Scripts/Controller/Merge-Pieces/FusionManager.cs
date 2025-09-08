using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class FusionManager : MonoBehaviour
{
    public FusionZone areaDeFusion;
    public InputActionReference mergeAction;

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
        FusionarPiezas();
    }

    private void FusionarPiezas()
    {
       

        if (areaDeFusion.pieces.Count == 0) return;

        GameObject grupoFusionado = new GameObject("GrupoFusionado");
        grupoFusionado.transform.position = CalcularCentro(areaDeFusion.pieces);

        foreach (GameObject pieza in areaDeFusion.pieces)
        {
            pieza.transform.SetParent(grupoFusionado.transform);
        }

        Rigidbody rb = grupoFusionado.AddComponent<Rigidbody>();
        rb.useGravity = false;
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
