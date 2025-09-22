// Brick.cs
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Brick : MonoBehaviour, IInteractable
{
    Rigidbody rb;
    // Indica si actualmente está siendo sostenida
    public bool IsHeld { get; private set; } = false;
    // Referencia al jugador que la sostiene (si aplica)
    public PlayerController CurrentHolder { get; private set; }

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Método que se llama si un jugador "interactúa" con la pieza (desde OverlapSphere)
    public void Interact(PlayerController player)
    {
        var oi = player.GetComponent<ObjectInteraction>();
        if (oi != null) oi.ForcePickup(gameObject, player);
    }

    // Llamado cuando la pieza fue recogida
    public void OnPickedUp(PlayerController player)
    {
        IsHeld = true;
        CurrentHolder = player;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.useGravity = false;
        rb.isKinematic = true;
    }

    // Llamado cuando la pieza fue soltada (por drop o por impacto)
    public void OnDropped()
    {
        IsHeld = false;
        CurrentHolder = null;
        rb.isKinematic = false;
        rb.useGravity = true;
    }
}
