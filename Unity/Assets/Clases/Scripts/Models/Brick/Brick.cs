// Brick.cs
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Brick : MonoBehaviour, IInteractable
{
    Rigidbody rb;
    public bool IsHeld { get; private set; } = false;
    public PlayerController CurrentHolder { get; private set; }

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Interact(PlayerController player)
    {
        var oi = player.GetComponent<ObjectInteraction>();
        if (oi != null) oi.ForcePickup(gameObject, player);
    }
    public void OnPickedUp(PlayerController player)
    {
        IsHeld = true;
        CurrentHolder = player;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.useGravity = false;
        rb.isKinematic = true;
    }
    public void OnDropped()
    {
        IsHeld = false;
        CurrentHolder = null;
        rb.isKinematic = false;
        rb.useGravity = true;
    }
}
