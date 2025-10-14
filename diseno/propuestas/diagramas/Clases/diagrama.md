```mermaid
---
title: Propuestas de Arquitectura
---
classDiagram
direction LR

%% ====== CORE (actual) ======
class GameManager {
  +StartGame()
  +EndGame()
  +AddScore(points:int)
}
class SceneController {
  +ChangeSceneByIndex(index:int)
  +QuitGame()
}
SceneController --> GameManager : changes state

%% ====== PLAYER (actual) ======
class Player {
  +id:int
  +name:string
}
class PlayerController {
  +player: Player
  +Move(dir:Vector2)
  +Interact()
  +Drop()
  +Throw()
}
PlayerController --> Player : has

%% ====== MECANICAS (actual) ======
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
AssemblyPieceManager --> PieceManipulator : creates/controls
AssemblyPieceManager --> BuildingZoneArea : uses
PieceManipulator --> BuildingZoneArea : clamps

%% ====== COMUNICACION (actual) ======
class CSharpEvents
GameManager ..> CSharpEvents : emits/listens
AssemblyPieceManager ..> CSharpEvents : uses
PieceManipulator ..> CSharpEvents : fires
PlayerController ..> CSharpEvents : subscribes

%% ====== CONFIG (actual) ======
class InputActionReference
AssemblyPieceManager --> InputActionReference : uses

%% ====== FUTURO (planeado) ======
class InputManager
class SaveLoadManager
class PieceSpawner
class PieceMovementSystem
class PieceFusionSystem
class PieceScoringSystem
class EventBus

GameManager ..> PieceSpawner
GameManager ..> PieceMovementSystem
GameManager ..> PieceFusionSystem
GameManager ..> PieceScoringSystem

PieceSpawner ..> PieceManipulator
PieceMovementSystem ..> PieceManipulator
PieceFusionSystem ..> PieceManipulator
PieceScoringSystem ..> PieceManipulator

EventBus ..> GameManager
EventBus ..> AssemblyPieceManager
EventBus ..> PlayerController
EventBus ..> PieceManipulator
