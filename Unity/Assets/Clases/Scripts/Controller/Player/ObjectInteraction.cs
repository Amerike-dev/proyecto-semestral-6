using UnityEngine;

public class ObjectInteraction : MonoBehaviour
{
    public GameObject handPoint;
    private GameObject pickedObject = null;

    // Llamado desde PlayerController cuando el jugador presiona el botón de interactuar
    public void TryInteract(PlayerController player)
    {
        // Si ya tengo un objeto en mano → lo suelto
        if (pickedObject != null)
        {
            DropObject(player);
            return;
        }

        // Si no tengo nada en mano → busco algo cercano
        Collider[] colliders = Physics.OverlapSphere(transform.position, 1f); // radio de detección
        foreach (var col in colliders)
        {
            IInteractable interactable = col.GetComponent<IInteractable>();
            if (interactable != null)
            {
                interactable.Interact(player); // Lógica del objeto
                PickUpObject(col.gameObject, player); // Lo recojo
                return;
            }
        }
    }

    private void PickUpObject(GameObject obj, PlayerController player)
    {
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = false;
            rb.isKinematic = true;
        }

        obj.transform.position = handPoint.transform.position;
        obj.transform.SetParent(handPoint.transform);
        pickedObject = obj;

        player.CanJump = false;
    }

    private void DropObject(PlayerController player)
    {
        Rigidbody rb = pickedObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = true;
            rb.isKinematic = false;
        }

        pickedObject.transform.SetParent(null);
        pickedObject = null;

        player.CanJump = true;
    }
}
