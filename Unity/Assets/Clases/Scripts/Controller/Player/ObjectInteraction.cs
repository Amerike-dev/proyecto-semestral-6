using UnityEngine;
using System.Collections;

public class ObjectInteraction : MonoBehaviour
{
    [Header("Referencias")]
    public Transform handPoint; // punto donde se coloca la pieza en la mano (asignar en inspector)

    [Header("Rangos y máscaras")]
    public float interactionRadius = 1f;            // radio para encontrar piezas cercanas
    public float throwDistanceMultiplier = 2f;     // "2 veces la distancia" -> throwDistance = interactionRadius * multiplier
    public float throwSphereCastRadius = 0.5f;     // grosor de la comprobación en la ruta del lanzamiento
    public float throwForce = 8f;                  // fuerza base aplicada al lanzar
    public float throwUpwardBoost = 2f;            // empujito vertical para arco
    public LayerMask interactableMask = ~0;        // capa(s) donde están las piezas
    public LayerMask playerMask = ~0;              // capa(s) donde están los jugadores (para detectar en la ruta)

    // Interno
    GameObject pickedObject = null; // la pieza que este jugador sostiene (si hay)
    PlayerController owner = null;  // jugador dueño (this player)

    // Getter público (para que otras clases consulten si el jugador tiene algo)
    public GameObject PickedObject => pickedObject;

    void Awake()
    {
        owner = GetComponent<PlayerController>();
        if (handPoint == null)
            Debug.LogWarning($"[ObjectInteraction] handPoint no asignado en {name}");
    }

    // Intenta interactuar (toggle pick/drop). Devuelve true si se ejecutó alguna acción.
    public bool TryInteract(PlayerController player)
    {
        // Si ya sostengo algo -> lo suelto
        if (pickedObject != null)
        {
            DropObject(player);
            return true;
        }

        // Si no tengo nada -> busco una pieza cercana (OverlapSphere)
        Collider[] cols = Physics.OverlapSphere(transform.position, interactionRadius, interactableMask);
        foreach (var col in cols)
        {
            // Preferimos Brick, pero sirve cualquier IInteractable
            var brick = col.GetComponent<Brick>();
            if (brick != null)
            {
                PickUpObject(brick.gameObject, player);
                return true;
            }

            var interactable = col.GetComponent<IInteractable>();
            if (interactable != null)
            {
                interactable.Interact(player);
                // suponer que la interacción se encargará de PickUp si aplica
                return true;
            }
        }

        // nada encontrado
        return false;
    }

    // Forzar que un jugador recoja esta pieza (externo: por ejemplo cuando una pieza fue lanzada y choca)
    // Este método separa la pieza de quien la tenga y la pone en la mano del 'player'
    public void ForcePickup(GameObject obj, PlayerController player)
    {
        if (obj == null || player == null) return;

        // Si la pieza está siendo sostenida por otro, forzamos el drop de ese otro
        var brickComp = obj.GetComponent<Brick>();
        if (brickComp != null && brickComp.IsHeld && brickComp.CurrentHolder != null)
        {
            var prevHolder = brickComp.CurrentHolder;
            var prevOI = prevHolder.GetComponent<ObjectInteraction>();
            if (prevOI != null)
            {
                prevOI.ForceDrop(); // fuerza que el anterior la suelte
            }
        }

        // Ahora la recogemos
        PickUpObject(obj, player);
    }

    // Forzar soltar la pieza que estamos sosteniendo (sin player explícito)
    public void ForceDrop()
    {
        if (pickedObject == null) return;
        // Usamos owner si existe, sino buscamos quien tenga el script
        PlayerController p = owner != null ? owner : GetComponent<PlayerController>();
        DropObject(p);
    }

    // Intenta soltar activamente. True si hay algo que soltar.
    public bool TryDrop(PlayerController player)
    {
        if (pickedObject != null)
        {
            DropObject(player);
            return true;
        }
        return false;
    }

    // Intenta lanzar la pieza. True si había una pieza y la lanzó.
    public bool TryThrow(PlayerController player)
    {
        if (pickedObject == null) return false;
        ThrowObject(player);
        return true;
    }

    // --- IMPLEMENTACIONES PRIVADAS ---
    private void PickUpObject(GameObject obj, PlayerController player)
    {
        // Ajustes físicos
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.useGravity = false;
            rb.isKinematic = true;
        }

        // Mover a la mano y parentear
        if (handPoint != null)
        {
            obj.transform.position = handPoint.position;
            obj.transform.rotation = handPoint.rotation;
            obj.transform.SetParent(handPoint, true);
        }

        // recordar
        pickedObject = obj;
        owner = player;

        // Si hay componente Brick, avisarle
        var brick = obj.GetComponent<Brick>();
        if (brick != null) brick.OnPickedUp(player);

        // El jugador no puede saltar mientras sostiene
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

        // separarlo del parent
        pickedObject.transform.SetParent(null);

        // avisar
        var brick = pickedObject.GetComponent<Brick>();
        if (brick != null) brick.OnDropped();

        pickedObject = null;

        // permitir salto
        if (player != null) player.CanJump = true;

        owner = null;
    }

    private void ThrowObject(PlayerController player)
    {
        if (pickedObject == null) return;

        // Desparentar y activar físicas
        GameObject obj = pickedObject;
        Rigidbody rb = obj.GetComponent<Rigidbody>();

        // liberar el objeto de la mano (físicas activas)
        obj.transform.SetParent(null);
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }

        // avisar que se soltó (propietario ya no lo sostiene)
        var brickComp = obj.GetComponent<Brick>();
        if (brickComp != null) brickComp.OnDropped();

        // calcular dirección de lanzamiento (hacia delante del jugador)
        Transform basis = player.cameraRoot ? player.cameraRoot : player.transform;
        Vector3 dir = basis.forward;
        dir.y = 0f;
        dir.Normalize();

        // calcular distancia objetivo según tu requerimiento: 2 * interactionRadius por defecto
        float throwDistance = interactionRadius * throwDistanceMultiplier;
        Vector3 origin = handPoint != null ? handPoint.position : player.transform.position + Vector3.up * 1f;
        Vector3 targetPoint = origin + dir * throwDistance;

        // aplicar velocidad / impulso
        if (rb != null)
        {
            // simple: velocidad inicial para ir hacia adelante con un pequeño arco
            Vector3 vel = dir * throwForce + Vector3.up * throwUpwardBoost;
            rb.linearVelocity = vel;
        }

        // ya no hay objeto sostenido por este jugador
        pickedObject = null;
        if (player != null) player.CanJump = true;
        owner = null;

        // Ahora comprobamos si en la ruta hay un jugador
        RaycastHit hit;
        // usamos SphereCast para "grosor" en la ruta
        if (Physics.SphereCast(origin, throwSphereCastRadius, dir, out hit, throwDistance, playerMask))
        {
            var hitPlayer = hit.collider.GetComponentInParent<PlayerController>();
            if (hitPlayer != null && hitPlayer != player)
            {
                // Si el jugador impactado no tiene pieza -> se la damos
                var hitOI = hitPlayer.GetComponent<ObjectInteraction>();
                if (hitOI != null)
                {
                    if (hitOI.PickedObject == null)
                    {
                        // Forzamos que el jugador la recoja (la pieza actual es 'obj')
                        hitOI.ForcePickup(obj, hitPlayer);
                        return;
                    }
                    else
                    {
                        // El jugador ya tenía una pieza: ambas se sueltan al suelo
                        hitOI.ForceDrop(); // suelta la suya
                        // la pieza lanzada ya está suelta y con física; podemos añadir un pequeño "empujón" por impacto
                        if (rb != null)
                        {
                            rb.AddForce(dir * (throwForce * 0.5f), ForceMode.Impulse);
                        }
                        return;
                    }
                }
            }
        }

        // Si no impactó a nadie -> la pieza queda volando/caerá por física normalmente.
    }

    // Visualización en editor (gizmo) para debug
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
