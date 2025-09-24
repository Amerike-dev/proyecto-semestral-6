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

    class BuildingZoneArea {
        -BoxCollider _box
        +Color gizmoColor
        +Bounds WorldBounds
        +Vector3 ClampInside(Vector3 position, Vector3 halfExtents)
        +void OnDrawGizmos()
    }

    class PieceManipulator {
        %% Public config
        +float moveSpeed
        +float rotationSpeedDegPerSec
        +float positionSmoothTime
        +float hoverHeight
        +bool enableGridSnap
        +float cellSize
        +bool IsRotationFixed
        +float snapAngle
        +float snapRepeatDelay
        +ManipulationMode mode
        %% Internals
        -Rigidbody _rb
        -Collider[] _colliders
        -BuildingZoneArea _zone
        -Vector3 _targetPos
        -Vector3 _rawTargetPos
        -Quaternion _targetRot
        -Vector3 _posVelRef
        -Vector2Int _lastSnapDir
        -float _snapCooldown
        -bool _active
        -bool _dropping
        %% API
        +void Activate(BuildingZoneArea zone, Vector3 spawnPos, Quaternion spawnRot)
        +void HandleArrows(Vector2 arrows, float dt)
        +void HandleVertical(float yInput, float dt)
        +void Tick(float dt)
        +void SetMode(ManipulationMode newMode)
        +void BeginDrop()
        +event Action<PieceManipulator> OnPlaced
    }

    class AssemblyPieceManager {
        %% Scene refs
        +BuildingZoneArea buildingZone
        +Transform spawnPoint
        %% Prefabs
        +List~GameObject~ piecePrefabs
        +bool randomOrder
        %% Defaults
        +float defaultMoveSpeed
        +float defaultRotSpeed
        +float defaultHover
        +bool defaultGridSnap
        +float defaultCellSize
        +bool defaultIsRotationFixed
        %% Input (New Input System)
        +InputActionReference moveAction
        +InputActionReference toggleModeAction
        +InputActionReference dropAction
        +InputActionReference raiseAction
        +InputActionReference lowerAction
        %% State/UI
        +ManipulationMode startMode
        -int _spawnIndex
        -PieceManipulator _current
        %% Lifecycle
        +void OnEnable()
        +void OnDisable()
        +void Start()
        +void Update()
        -void SpawnNextPiece()
        -void HandlePlaced(PieceManipulator placed)
    }

    %% Relaciones entre nuevas clases
    AssemblyPieceManager --> BuildingZoneArea : usa
    AssemblyPieceManager --> PieceManipulator : instancia/gestiona
    PieceManipulator --> BuildingZoneArea : consulta límites (ClampInside)

    %% Enums de apoyo
    class ManipulationMode {
        <<enumeration>>
        Rotation
        Translation
    }