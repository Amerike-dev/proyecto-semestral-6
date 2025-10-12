// BrickBehavior.cs
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BrickBehavior : MonoBehaviour, IInteractable
{
    private Rigidbody rb;
    public Brick Model { get; private set; }

    public bool IsHeld => Model.IsHeld;
    public PlayerController CurrentHolder => Model.CurrentHolder;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        Model = new Brick();
    }

    public void Interact(PlayerController player)
    {
        var oi = player.GetComponent<ObjectInteraction>();
        if (oi != null) oi.ForcePickup(gameObject, player);
    }

    public void OnPickedUp(PlayerController player)
    {

        Model.PickUp(player);

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        rb.useGravity = false;
        rb.isKinematic = true;
    }

    public void OnDropped()
    {
        Model.Drop();

        rb.isKinematic = false;
        rb.useGravity = true;
    }
}
