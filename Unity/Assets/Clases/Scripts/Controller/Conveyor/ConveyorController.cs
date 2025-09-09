using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(PlayerInput))]
public class ConveyorController : MonoBehaviour
{
    [SerializeField] private float initialSpeed = 2f;
    [SerializeField] private Vector3 initialDirection = Vector3.right;
    [SerializeField] private float decelRate = 2f; 
    [SerializeField] private float accelRate = 2f; 

    private Conveyor conveyor;
    private bool reversing = false;

    private InputAction reverseAction;

    private void Awake()
    {
        Collider col = GetComponent<Collider>();
        if (col == null) col = gameObject.AddComponent<BoxCollider>();
        col.isTrigger = true;

        conveyor = new Conveyor(col, initialSpeed, initialDirection);

        // Obtener el PlayerInput y buscar la acción "Reverse"
        var playerInput = GetComponent<PlayerInput>();
        reverseAction = playerInput.actions["Reverse"];
    }

    private void OnEnable()
    {
        reverseAction.performed += OnReverse;
        reverseAction.Enable();
    }

    private void OnDisable()
    {
        reverseAction.performed -= OnReverse;
        reverseAction.Disable();
    }

    private void OnTriggerStay(Collider other)
    {
        Rigidbody rb = other.attachedRigidbody;
        if (rb != null)
        {
            conveyor.ApplyConveyorEffect(rb);
        }
    }

    private void OnReverse(InputAction.CallbackContext context)
    {
        if (!reversing)
        {
            StartCoroutine(ChangeDirectionSmoothly());
        }
    }

    private IEnumerator ChangeDirectionSmoothly()
    {
        reversing = true;

        // 1) Frenar hasta 0
        while (conveyor.Speed > 0.001f)
        {
            float newSpeed = Mathf.MoveTowards(conveyor.Speed, 0f, decelRate * Time.deltaTime);
            conveyor.SetSpeed(newSpeed);
            yield return null;
        }
        conveyor.SetSpeed(0f);

        // 2) Cambiar dirección
        conveyor.InvertDirection();

        // 3) Acelerar hasta la velocidad inicial
        while (conveyor.Speed < initialSpeed - 0.001f)
        {
            float newSpeed = Mathf.MoveTowards(conveyor.Speed, initialSpeed, accelRate * Time.deltaTime);
            conveyor.SetSpeed(newSpeed);
            yield return null;
        }
        conveyor.SetSpeed(initialSpeed);
        reversing = false;
    }
}
