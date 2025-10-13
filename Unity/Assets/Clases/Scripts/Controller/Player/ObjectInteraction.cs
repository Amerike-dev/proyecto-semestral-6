using UnityEngine;
using System.Collections;

public class ObjectInteraction : MonoBehaviour
{
    [Header("Referencias")]
    public Transform handPoint;

    [Header("Rangos y máscaras")]
    public float interactionRadius = 1f;
    public float throwDistanceMultiplier = 2f;
    public float throwSphereCastRadius = 0.5f;
    public float throwForce = 8f;
    public float throwUpwardBoost = 2f;
    public LayerMask interactableMask = ~0;
    public LayerMask playerMask = ~0;

    GameObject pickedObject = null;
    PlayerController owner = null;

    public GameObject PickedObject => pickedObject;

    void Awake()
    {
        owner = GetComponent<PlayerController>();
        if (handPoint == null)
            Debug.LogWarning($"[ObjectInteraction] handPoint no asignado en {name}");
    }

    public bool TryInteract(PlayerController player)
    {
        if (pickedObject != null)
        {
            DropObject(player);
            return true;
        }

        Collider[] cols = Physics.OverlapSphere(transform.position, interactionRadius, interactableMask);
        foreach (var col in cols)
        {
            var brick = col.GetComponent<BrickBehavior>();
            if (brick != null)
            {
                PickUpObject(brick.gameObject, player);
                return true;
            }

            var interactable = col.GetComponent<IInteractable>();
            if (interactable != null)
            {
                interactable.Interact(player);
                return true;
            }
        }

        return false;
    }

    // Forzar que un jugador recoja esta pieza (externo: por ejemplo cuando una pieza fue lanzada y choca)
    // Este método separa la pieza de quien la tenga y la pone en la mano del 'player'
    public void ForcePickup(GameObject obj, PlayerController player)
    {
        if (obj == null || player == null) return;
        var brickComp = obj.GetComponent<BrickBehavior>();
        if (brickComp != null && brickComp.IsHeld && brickComp.CurrentHolder != null)
        {
            var prevHolder = brickComp.CurrentHolder;
            var prevOI = prevHolder.GetComponent<ObjectInteraction>();
            if (prevOI != null)
            {
                prevOI.ForceDrop();
            }
        }

        PickUpObject(obj, player);
    }

    public void ForceDrop()
    {
        if (pickedObject == null) return;
        PlayerController p = owner != null ? owner : GetComponent<PlayerController>();
        DropObject(p);
    }

    public bool TryDrop(PlayerController player)
    {
        if (pickedObject != null)
        {
            DropObject(player);
            return true;
        }
        return false;
    }
    public bool TryThrow(PlayerController player)
    {
        if (pickedObject == null) return false;
        ThrowObject(player);
        return true;
    }

    private void PickUpObject(GameObject obj, PlayerController player)
    {
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.useGravity = false;
            rb.isKinematic = true;
        }

        if (handPoint != null)
        {
            obj.transform.position = handPoint.position;
            obj.transform.rotation = handPoint.rotation;
            obj.transform.SetParent(handPoint, true);
        }

        pickedObject = obj;
        owner = player;

        var brick = obj.GetComponent<BrickBehavior>();
        if (brick != null) brick.OnPickedUp(player);

        if (player != null) player.CanJump = false;
    }

    private void DropObject(PlayerController player)
    {
        if (pickedObject == null) return;

        Rigidbody rb = pickedObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = true;
            rb.isKinematic = false;
        }

        pickedObject.transform.SetParent(null);

        var brick = pickedObject.GetComponent<BrickBehavior>();
        if (brick != null) brick.OnDropped();

        pickedObject = null;

        if (player != null) player.CanJump = true;

        owner = null;
    }

    private void ThrowObject(PlayerController player)
    {
        if (pickedObject == null) return;

        GameObject obj = pickedObject;
        Rigidbody rb = obj.GetComponent<Rigidbody>();

        obj.transform.SetParent(null);
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }

        var brickComp = obj.GetComponent<BrickBehavior>();
        if (brickComp != null) brickComp.OnDropped();

        Transform basis = player.cameraRoot ? player.cameraRoot : player.transform;
        Vector3 dir = basis.forward;
        dir.y = 0f;
        dir.Normalize();

        float throwDistance = interactionRadius * throwDistanceMultiplier;
        Vector3 origin = handPoint != null ? handPoint.position : player.transform.position + Vector3.up * 1f;

        if (rb != null)
        {
            Vector3 vel = dir * throwForce + Vector3.up * throwUpwardBoost;
            rb.linearVelocity = vel;
        }

        pickedObject = null;
        if (player != null) player.CanJump = true;
        owner = null;

        // === NUEVO: Añadir detección física real para recibir ===
        var thrownPiece = obj.GetComponent<ThrownPiece>();
        if (thrownPiece == null)
            thrownPiece = obj.AddComponent<ThrownPiece>();

        thrownPiece.Initialize(player);
    }


    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);

        if (handPoint != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(handPoint.position, 0.15f);
        }
    }
}
