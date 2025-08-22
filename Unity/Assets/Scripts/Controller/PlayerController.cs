using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    [Header("Configuración del Jugador")]
    [SerializeField] private int playerID = 1;
    [SerializeField] private string playerName = "Player";

    [Header("Movimiento")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpHeight = 2.2f;
    [SerializeField] private float gravity = -9.81f * 2f;

    CharacterController controller;
    PlayerInput playerInput;
    Vector2 moveInput;
    float verticalVel;


    // Esto referencia al objeto Player
    public Player PlayerData { get; private set; }


    void Awake()
    {
        InitializePlayer();

        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();

        // <<< asignacion de dispositivos
        InputDevicePolicy.Assign(playerInput);
    }

    void Update()
    {
        var dt = Time.deltaTime;

        // Permitir que el dueno del teclado adopte un pad libre (si presiona un boton)
        InputDevicePolicy.TryAdoptFreePadIfKeyboardOwner(playerInput);

        Vector3 forward = Vector3.forward, right = Vector3.right;
        
        Vector3 moveXZ = (right * moveInput.x + forward * moveInput.y) * moveSpeed;

        if (controller.isGrounded && verticalVel < 0f) verticalVel = -2f;
        verticalVel += gravity * dt;

        controller.Move(new Vector3(moveXZ.x, verticalVel, moveXZ.z) * dt);
    }

    public void OnMove(InputAction.CallbackContext ctx) => moveInput = ctx.ReadValue<Vector2>();
    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && controller.isGrounded)
            verticalVel = Mathf.Sqrt(jumpHeight * -2f * gravity);
    }

    private void InitializePlayer()
    {
        // Esto crea una nueva instancia del jugador con los datos configurados.
        PlayerData = new Player(playerID, playerName);

        Debug.Log($"Jugador inicializado: {PlayerData.PlayerName} (ID: {PlayerData.PlayerID})");
    }
}