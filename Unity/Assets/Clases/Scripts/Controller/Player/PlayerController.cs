using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users; // <--- NUEVO: para InputUser

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
    Vector2 moveInput;       
    Vector2 lookInput;       
    float verticalVel;      
    bool jumpQueued;         
    bool controlsBound;       // <--- NUEVO: indica si ya emparejamos dispositivos
    InputUser inputUser;      // <--- NUEVO: usuario del Input System para este Player

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

        // Asegurar que el asset de acciones esté asignado (cargado desde Resources si falta)
        if (playerInput.actions == null)
        {
            var asset = Resources.Load<InputActionAsset>("InputSystem_Actions");
            if (asset != null)
                playerInput.actions = ScriptableObject.Instantiate(asset); // instancia propia por jugador
            else
                Debug.LogError("[PlayerController] No se pudo cargar InputSystem_Actions desde Resources.");
        }

        // Asegura que sólo el map "Player" esté activo
        var assetRef = playerInput.actions;
        foreach (var map in assetRef.actionMaps) map.Disable();
        assetRef.FindActionMap("Player", throwIfNotFound: true).Enable();

        // Validar que exista la acción "Drop" en el mapa activo
        var dropAction = assetRef.FindAction("Drop", throwIfNotFound: false);
        if (dropAction == null)
        {
            Debug.LogWarning("[PlayerController] No se encontró la acción 'Drop' en el Action Map 'Player'. OnDrop no se llamará.");
        }

        // Bloquear autoswitch de schemes para que este jugador solo escuche su(s) dispositivo(s)
        playerInput.neverAutoSwitchControlSchemes = true;

        // Obtener ObjectInteraction (debe existir porque pusimos RequireComponent)
        objectInteraction = GetComponent<ObjectInteraction>();
        if (objectInteraction == null)
            Debug.LogError("[PlayerController] falta ObjectInteraction en " + name);
    }

    void Update()
    {
        // Emparejamiento ahora se hace explícitamente vía AssignDevice() desde PlayerManager

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

    // Reemplazo de TryPairAssignedGamepad: empareja/desempareja el dispositivo de este jugador
    public void AssignDevice(Gamepad newGamepad)
    {
        gamepad = newGamepad;

        // Tomar el usuario de este PlayerInput y limpiar emparejamientos previos
        inputUser = playerInput.user;
        if (inputUser.valid)
            inputUser.UnpairDevices();

        if (gamepad != null)
        {
            InputUser.PerformPairingWithDevice(gamepad, inputUser);
            playerInput.SwitchCurrentControlScheme("Gamepad", gamepad);
            controlsBound = true;
            Debug.Log($"[Player {PlayerData.PlayerID}] emparejado a gamepad: {gamepad.displayName}");
        }
        else
        {
            controlsBound = false;
            Debug.Log($"[Player {PlayerData.PlayerID}] sin gamepad asignado.");
        }
    }

    void OnDestroy()
    {
        // Liberar dispositivos cuando el jugador se destruye
        if (inputUser.valid)
            inputUser.UnpairDevices();
    }

    // Util: saber si estoy sosteniendo algo
    public bool HasPiece()
    {
        return objectInteraction != null && objectInteraction.PickedObject != null;
    }
}
