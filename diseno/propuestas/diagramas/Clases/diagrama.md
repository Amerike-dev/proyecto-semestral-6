```mermaid
---
title: Propuestas de Arquitectura
---
classDiagram
direction LR

%% =====================================
%% ====== CORE ========================
%% =====================================
class GameManager {
  +StartGame()
  +EndGame()
  +AddScore(points:int)
}

class SceneController {
  +ChangeSceneByIndex(index:int)
  +QuitGame()
}

SceneController --> GameManager : controla estado

class TimerController {
  +startertime: float
  +SceneIndex: int
  -RemainingTime: float
  +Start()
  +Update()
}

SceneController --> TimerController : usa para cambiar escena por tiempo

%% =====================================
%% ====== PLAYER ======================
%% =====================================
class Player {
  +id: int
  +name: string
}

class PlayerController {
  +player: Player
  +moveSpeed: float
  +jumpHeight: float
  +lookSensitivity: float
  +cameraRoot: Transform
  +CanJump: bool
  +HasPiece(): bool
  +OnMove(value:InputValue)
  +OnLook(value:InputValue)
  +OnJump(value:InputValue)
  +OnInteract(value:InputValue)
  +OnDrop(value:InputValue)
  +OnThrow(value:InputValue)
}

PlayerController --> Player : posee
PlayerController --> GameManager : se comunica con
PlayerController --> ObjectInteraction : usa

%% =====================================
%% ====== MECÁNICAS ====================
%% =====================================
class AssemblyPieceManager {
  +Setup()
  +OnPlaced()
}

class PieceManipulator {
  +Place()
  +Rotate()
  +Drop()
}

class BuildingZoneArea {
  +ClampInside(obj)
}

AssemblyPieceManager --> PieceManipulator : crea/controla
AssemblyPieceManager --> BuildingZoneArea : usa
PieceManipulator --> BuildingZoneArea : limita posición

%% =====================================
%% ====== OBJETOS E INTERACCIÓN =======
%% =====================================
class ObjectInteraction {
  +handPoint: Transform
  +interactionRadius: float
  +throwDistanceMultiplier: float
  +throwSphereCastRadius: float
  +throwForce: float
  +throwUpwardBoost: float
  +interactableMask: LayerMask
  +playerMask: LayerMask
  -pickedObject: GameObject
  -owner: PlayerController
  +PickedObject: GameObject
  +TryInteract(player:PlayerController): bool
  +ForcePickup(obj:GameObject, player:PlayerController)
  +ForceDrop()
  +TryDrop(player:PlayerController): bool
  +TryThrow(player:PlayerController): bool
  -PickUpObject(obj:GameObject, player:PlayerController)
  -DropObject(player:PlayerController)
  -ThrowObject(player:PlayerController)
  +OnDrawGizmosSelected()
}

class BrickBehavior {
  +IsHeld: bool
  +CurrentHolder: PlayerController
  +Interact(player:PlayerController)
  +OnPickedUp(player:PlayerController)
  +OnDropped()
}

class ObjectController {
  +objectID: int
}

ObjectInteraction --> BrickBehavior : detecta/interactúa con
ObjectInteraction --> ObjectController : puede manipular
ObjectInteraction --> ThrownPiece : agrega componente al lanzar
BrickBehavior --> PlayerController : referencia al jugador que la sostiene

class ThrownPiece {
  -thrower: PlayerController
  -canBeCaught: bool
  +Initialize(player:PlayerController)
  -EnableCatch()
  -OnCollisionEnter(collision:Collision)
}

ThrownPiece --> PlayerController : referencia a

%% =====================================
%% ====== SISTEMA DE FUSIÓN ============
%% =====================================
class FusionManager {
  +areaDeFusion: FusionZone
  +mergeAction: InputActionReference
  +OnEnable()
  +OnDisable()
  -OnFusionar(context:InputAction.CallbackContext)
  -FusionarPiezas()
  -CalcularCentro(piezas:List<GameObject>): Vector3
}

class FusionZone {
  +pieces: List<GameObject>
}

FusionManager --> FusionZone : contiene piezas
FusionManager --> InputActionReference : usa para input

%% =====================================
%% ====== SISTEMA DE TRASLADO ==========
%% =====================================
class SpawnZone {
  -objectsInZone: HashSet<GameObject>
  +IsEmpty: bool
  +OnTriggerEnter(collider:Collider)
  +OnTriggerExit(collider:Collider)
}

class Conveyor {
  +Speed: float
  +Direction: Vector3
  +ApplyConveyorEffect(rb:Rigidbody)
  +SetSpeed(speed:float)
  +InvertDirection()
}

class ConveyorController {
  +spawnZone: SpawnZone
  +baseSpawnZoneLength: float
  +initialSpeed: float
  +initialDirection: Vector3
  +decelRate: float
  +accelRate: float
  -conveyor: Conveyor
  -reversing: bool
  -reverseAction: InputAction
  +Awake()
  +Update()
  +OnEnable()
  +OnDisable()
  +OnTriggerStay(collider:Collider)
  -OnReverse(context:InputAction.CallbackContext)
  -ChangeDirectionSmoothly(): IEnumerator
}

ConveyorController --> Conveyor : controla
ConveyorController --> SpawnZone : contiene
ConveyorController --> PlayerInput : maneja entrada de usuario

%% =====================================
%% ====== ESCENAS ======================
%% =====================================
class SceneIndex {
  <<enum>>
  MENU
  LEVEL_1
  LEVEL_2
  GAME_OVER
}

SceneController --> SceneIndex : usa