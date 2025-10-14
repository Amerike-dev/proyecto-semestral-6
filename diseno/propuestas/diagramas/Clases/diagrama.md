flowchart TB

classDef planned stroke-dasharray: 6 4

subgraph Core
  GM[GameManager]
  SCN[SceneController]
end

subgraph PlayerDomain
  PC[PlayerController]
  PD[Player]
end
PC --> PD

subgraph Mechanics
  APM[AssemblyPieceManager]
  PMAN[PieceManipulator]
  BZA[BuildingZoneArea]
end
APM --> PMAN
APM --> BZA
PMAN --> BZA

subgraph Comm
  EV[Events Delegates]
end
GM -.-> EV
APM -.-> EV
PMAN -.-> EV
PC -.-> EV

subgraph Config
  INP[InputActionReference]
  GCFG[GameConfig]:::planned
  PCFG[PieceConfig or SpawnTable]:::planned
end
APM --> INP

subgraph Data
  SLOTS[Saves Slots]:::planned
end

IM[InputManager]:::planned
SLM[SaveLoadManager]:::planned
PFUSE[PieceFusionSystem]:::planned
PMOVE[PieceMovementSystem]:::planned
PSCORE[PieceScoringSystem]:::planned
PSPAWN[PieceSpawner]:::planned
BUS[EventBus]:::planned

GM -.-> PSPAWN
GM -.-> PFUSE
GM -.-> PMOVE
GM -.-> PSCORE

PSPAWN -.-> PMAN
PFUSE -.-> PMAN
PMOVE -.-> PMAN
PSCORE -.-> PMAN

BUS -.-> EV
SLM -.-> SLOTS
IM -.-> PC
IM -.-> APM

SCN --> GM