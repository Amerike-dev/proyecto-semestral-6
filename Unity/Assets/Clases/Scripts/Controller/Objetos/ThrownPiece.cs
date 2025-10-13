using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ThrownPiece : MonoBehaviour
{
    private PlayerController thrower;
    private bool canBeCaught = false;

    public void Initialize(PlayerController player)
    {
        thrower = player;
        canBeCaught = false;
        // Pequeño delay para no atraparla de inmediato al lanzarla
        Invoke(nameof(EnableCatch), 0.3f);
    }

    private void EnableCatch()
    {
        canBeCaught = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!canBeCaught) return;

        var hitPlayer = collision.collider.GetComponentInParent<PlayerController>();
        if (hitPlayer != null && hitPlayer != thrower)
        {
            var hitInteraction = hitPlayer.GetComponent<ObjectInteraction>();
            if (hitInteraction != null && hitInteraction.PickedObject == null)
            {
                // Entregarle la pieza al jugador que fue golpeado
                hitInteraction.ForcePickup(gameObject, hitPlayer);
                Debug.Log($"[{hitPlayer.name}] recibió la pieza lanzada por [{thrower.name}]");
            }

            // Detener movimiento físico
            var rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;
                rb.useGravity = false;
            }
        }
    }
}
