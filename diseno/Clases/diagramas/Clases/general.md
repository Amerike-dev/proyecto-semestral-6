```mermaid
---
title: Player y PlayerController
---
classDiagram
    class Player {
        +int PlayerID
        +string PlayerName
        +Player(int playerId, string playerName = "")
    }

    class PlayerController {
        -int playerID
        -string playerName
        -float moveSpeed
        -float jumpHeight
        -float gravity
        -CharacterController controller
        -PlayerInput playerInput
        -Vector2 moveInput
        -float verticalVel
        +bool CanJump
        +Player PlayerData
        +void Awake()
        +void Update()
        +void OnMove(InputAction.CallbackContext ctx)
        +void OnJump(InputAction.CallbackContext ctx)
        -void InitializePlayer()
    }

    PlayerController --> Player : usa
