<<<<<<< Updated upstream
using UnityEngine;
=======
ï»¿using UnityEngine;
>>>>>>> Stashed changes

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class Brick : MonoBehaviour
{
    public enum BrickState
    {
        Raw,
        Cut,
        Painted,
        CutAndPainted
    }

    public BrickState CurrentState { get; private set; } = BrickState.Raw;

    [Header("Colores del ladrillo")]
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Color paintedColor = Color.red;
    private Color originalColor;

    [Header("Estados del ladrillo")]
    public bool isCut = false;
    public bool isPainted = false;
    public bool isReseted = false;

    private void Start()
    {
        if (meshRenderer == null)
        {
            meshRenderer = GetComponent<MeshRenderer>();
        }

        originalColor = meshRenderer.material.color;
    }

    private void Update()
    {
        // se corta el ladrillo
        if (isCut)
        {
            Cut();
            isCut = false;
        }

        // se pinta el ladrillo
        if (isPainted)
        {
            Paint();
            isPainted = false;
        }

        // se reinicia todo
        if (isReseted)
        {
            ResetBrick();
            isReseted = false;
        }
    }

    public void Cut()
    {
        if (CurrentState == BrickState.Raw)
        {
            CurrentState = BrickState.Cut;
<<<<<<< Updated upstream
            Debug.Log("Se cortó el ladrillo");
=======
            Debug.Log("Se cortï¿½ el ladrillo");
>>>>>>> Stashed changes
        }
        else if (CurrentState == BrickState.Painted)
        {
            CurrentState = BrickState.CutAndPainted;
<<<<<<< Updated upstream
            Debug.Log("se cortó el ladrillo despues de que se pintó");
=======
            Debug.Log("se cortï¿½ el ladrillo despues de que se pintï¿½");
>>>>>>> Stashed changes
        }
    }

    public void Paint()
    {
        if (CurrentState == BrickState.Raw)
        {
            CurrentState = BrickState.Painted;
            ApplyPaintColor();
<<<<<<< Updated upstream
            Debug.Log("se pintó el ladrillo");
=======
            Debug.Log("se pintï¿½ el ladrillo");
>>>>>>> Stashed changes
        }
        else if (CurrentState == BrickState.Cut)
        {
            CurrentState = BrickState.CutAndPainted;
            ApplyPaintColor();
<<<<<<< Updated upstream
            Debug.Log("se pintó el ladrillo despues de que se haya cortado");
=======
            Debug.Log("se pintï¿½ el ladrillo despues de que se haya cortado");
>>>>>>> Stashed changes
        }
    }

    private void ApplyPaintColor()
    {
        if (meshRenderer != null)
        {
            meshRenderer.material.color = paintedColor;
        }
    }

    public void ResetBrick()
    {
        CurrentState = BrickState.Raw;
        if (meshRenderer != null)
        {
            meshRenderer.material.color = originalColor;
        }
    }
<<<<<<< Updated upstream
}
=======
}
>>>>>>> Stashed changes
