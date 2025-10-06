using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(PlayerInput))]
public class ConveyorController : MonoBehaviour
{
    [Header("Zona de Spawn")]
    [SerializeField] private SpawnZone spawnZone;
    [SerializeField] private float baseSpawnZoneLength = 2f;
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

        var playerInput = GetComponent<PlayerInput>();
        reverseAction = playerInput.actions["Reverse"];

        if (spawnZone == null)
        {
            spawnZone = GetComponentInChildren<SpawnZone>();
        }
    }
    private void Update()
    {
        // Actualizar tamaño de la zona de spawn según la velocidad
        if (spawnZone != null && conveyor != null)
        {
            BoxCollider zoneCollider = spawnZone.GetComponent<BoxCollider>();
            if (zoneCollider != null)
            {
                float length = baseSpawnZoneLength * (conveyor.Speed / initialSpeed);
                length = Mathf.Max(baseSpawnZoneLength, length); // Nunca menor al base
                Vector3 size = zoneCollider.size;
                size.z = length;
                zoneCollider.size = size;
            }
        }
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

        while (conveyor.Speed > 0.001f)
        {
            float newSpeed = Mathf.MoveTowards(conveyor.Speed, 0f, decelRate * Time.deltaTime);
            conveyor.SetSpeed(newSpeed);
            yield return null;
        }
        conveyor.SetSpeed(0f);

        conveyor.InvertDirection();

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
