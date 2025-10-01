// PlayerController.cs
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    [Header("Jugador")]
    [SerializeField] private int playerID = 1;
    [SerializeField] private string playerName = "Player";
    public Player PlayerData { get; private set; }

    [Header("Movimiento")]
    public float moveSpeed = 5f;
    public float jumpHeight = 1.5f;
    public float gravity = -9.81f;

    [Header("Rotación (Look)")]
    public float lookSensitivity = 200f;
    public Transform cameraRoot;

    [Header("Sonido")]
    public AudioSource AudioSource;
    public AudioClip JumpSound;

    // --- Dispositivo que asigna PlayerManager ---
    [HideInInspector] public Gamepad gamepad;

    // Para ObjectInteraction
    public bool CanJump { get; set; } = true;

    // Internos de movimiento
    CharacterController controller;
    PlayerInput playerInput;
    Vector2 moveInput;       // Move (Vector2)
    Vector2 lookInput;       // Look (Vector2)
    float verticalVel;       // gravedad acumulada
    bool jumpQueued;         // se activa desde OnJump
    bool devicePaired;       // si ya pareamos el gamepad asignado

    // NUEVO: reference al ObjectInteraction del jugador
    [HideInInspector] public ObjectInteraction objectInteraction;

    // NUEVO: booleano que confirma si la última interacción fue ejecutada
    public bool InteractionConfirmed { get; private set; } = false;

    void Awake()
    {
        AudioSource = GetComponent<AudioSource>();
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        PlayerData = new Player(playerID, playerName);

        // Asegura que sólo el map "Player" esté activo
        var asset = playerInput.actions;
        foreach (var map in asset.actionMaps) map.Disable();
        asset.FindActionMap("Player", throwIfNotFound: true).Enable();

        // Validar que exista la acción "Drop" en el mapa activo
        var dropAction = asset.FindAction("Drop", throwIfNotFound: false);
        if (dropAction == null)
        {
            Debug.LogWarning("[PlayerController] No se encontró la acción 'Drop' en el Action Map 'Player'. OnDrop no se llamará.");
        }

        // Evita que cambie de scheme solo si no quieres autoswitch (opcional)
        playerInput.neverAutoSwitchControlSchemes = false;

        // Obtener ObjectInteraction (debe existir porque pusimos RequireComponent)
        objectInteraction = GetComponent<ObjectInteraction>();
        if (objectInteraction == null)
            Debug.LogError("[PlayerController] falta ObjectInteraction en " + name);
    }

    void Update()
    {
        TryPairAssignedGamepad();

        //Rotación
        float yawDelta = lookInput.x * lookSensitivity * Time.deltaTime;
        (cameraRoot ? cameraRoot : transform).Rotate(0f, yawDelta, 0f);

        //Salto (grounding y gravedad)
        if (controller.isGrounded && verticalVel < 0f)
            verticalVel = -2f;  // pegado al piso

        if (jumpQueued && controller.isGrounded && CanJump)
        {
            if (AudioSource != null && JumpSound != null)
                AudioSource.PlayOneShot(JumpSound);
            verticalVel = Mathf.Sqrt(jumpHeight * -2f * gravity); // v = sqrt(2 g h)
            jumpQueued = false;
        }

        verticalVel += gravity * Time.deltaTime;

        //Movimiento
        Transform basis = cameraRoot ? cameraRoot : transform;
        Vector3 fwd = basis.forward; fwd.y = 0f; fwd.Normalize();
        Vector3 right = basis.right; right.y = 0f; right.Normalize();
        Vector3 planar = (right * moveInput.x + fwd * moveInput.y) * moveSpeed;

        //Aplicar con CharacterController
        Vector3 velocity = planar + Vector3.up * verticalVel;
        controller.Move(velocity * Time.deltaTime);
    }

    // ----------------- Callbacks del PlayerInput -----------------
    public void OnMove(InputValue value) { moveInput = value.Get<Vector2>(); }
    public void OnLook(InputValue value) { lookInput = value.Get<Vector2>(); }
    public void OnJump(InputValue value)
    {
            Debug.Log("OnJump fue llamado");

        if (value.Get<float>() > 0.5f) jumpQueued = true;
    }

    // MAPEAR una acción llamada EXACTAMENTE "interact" -> método OnInterat será invocado
  public void OnInteract(InputValue value)
{
    Debug.Log("OnInteract fue llamado");
    if (value.Get<float>() > 0.5f)
    {
        InteractionConfirmed = false;
        if (objectInteraction != null)
        {
            bool result = objectInteraction.TryInteract(this);
            InteractionConfirmed = result;
            Debug.Log("Interacción realizada: " + result);
        }
    }
}


    // Si quieres acciones separadas para drop / throw, crea dos acciones llamadas "Drop" y "Throw"
    public void OnDrop(InputValue value)
    {
        Debug.Log($"OnDrop input: {value.Get<float>()}");
        if (value.Get<float>() > 0.5f && objectInteraction != null)
        {
            bool result = objectInteraction.TryDrop(this);
            InteractionConfirmed = result;
        }
    }

    public void OnThrow(InputValue value)
    {
        if (value.Get<float>() > 0.5f && objectInteraction != null)
        {
            bool result = objectInteraction.TryThrow(this);
            InteractionConfirmed = result;
        }
    }

    // Intento de emparejar gamepad si se asignó desde PlayerManager
    void TryPairAssignedGamepad()
    {
        if (devicePaired) return;
        if (gamepad == null) return;
        if (playerInput == null) return;

        try
        {
            playerInput.SwitchCurrentControlScheme("Gamepad", gamepad);
        }
        catch
        {
            Debug.LogWarning("No pude cambiar al scheme Gamepad");
        }

        devicePaired = true;
        Debug.Log($"[Player {PlayerData.PlayerID}] usando gamepad: {gamepad.displayName}");
    }

    // Util: saber si estoy sosteniendo algo
    public bool HasPiece()
    {
        return objectInteraction != null && objectInteraction.PickedObject != null;
    }
}
