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

    // Internos
    CharacterController controller;
    PlayerInput playerInput;
    Vector2 moveInput;       // Move (Vector2)
    Vector2 lookInput;       // Look (Vector2)
    float verticalVel;       // gravedad acumulada
    bool devicePaired;       // si ya pareamos el gamepad asignado

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

        // Evita que cambie de scheme solo si no quieres autoswitch (opcional)
        playerInput.neverAutoSwitchControlSchemes = false;
    }

    void Update()
    {
        TryPairAssignedGamepad();

        //Rotación
        float yawDelta = lookInput.x * lookSensitivity * Time.deltaTime;
        (cameraRoot ? cameraRoot : transform).Rotate(0f, yawDelta, 0f);

        // 🔹 SALTO
        if (controller.isGrounded && verticalVel < 0f)
            verticalVel = -2f;  // pegado al piso

        // Usamos WasPressedThisFrame para detectar SOLO la pulsación inicial
        if (playerInput.actions["Jump"].WasPressedThisFrame() && controller.isGrounded && CanJump)
        {
            AudioSource.PlayOneShot(JumpSound);
            verticalVel = Mathf.Sqrt(jumpHeight * -2f * gravity);
            Debug.Log("Salto");
        }

        // Aplicar gravedad
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

    //Callbacks del PlayerInput
    public void OnMove(InputValue value) { moveInput = value.Get<Vector2>(); }
    public void OnLook(InputValue value) { lookInput = value.Get<Vector2>(); }

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
}
