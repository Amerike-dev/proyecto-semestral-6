```mermaid
---
title: Propuesta de Arquitectura – Estado actual y crecimiento
---
flowchart TB

%% ====== ESTILOS ======
classDef planned stroke-dasharray: 6 4,opacity:0.75
classDef core fill:#0000,stroke:#888,color:#ddd
classDef mech fill:#0000,stroke:#888,color:#ddd
classDef comm fill:#0000,stroke:#888,color:#ddd
classDef data fill:#0000,stroke:#888,color:#ddd

%% ====== CORE (actual) ======
subgraph Core[Core del Juego]
  GM[GameManager\n• estados de juego\n• score / flujo de nivel]
  SCN[SceneController\n• cambio de escenas\n• salir del juego]
end
class GM,SCN core

%% ====== JUGADOR (actual) ======
subgraph PlayerDomain[Dominio Jugador]
  PC[PlayerController\n• movimiento/acciones\n• input del jugador]
  PD[Player\n• id, nombre]
end

PC --> PD

%% ====== MECÁNICAS (actual) ======
subgraph Mechanics[Mecánicas de Ensamblaje]
  APM[AssemblyPieceManager\n• ciclo de vida de pieza\n• input actions (New Input System)]
  PMAN[PieceManipulator\n• mover/rotar/soltar\n• OnPlaced event]
  BZA[BuildingZoneArea\n• límites de construcción]
end
class APM,PMAN,BZA mech

APM -->|instancia/gestiona| PMAN
APM -->|usa| BZA
PMAN -->|ClampInside| BZA

%% ====== COMUNICACIÓN (actual) ======
subgraph Comm[Comunicación]
  EV[C# Events / Delegados\n• suscripción a eventos\n• OnPlaced, etc.]
end
class EV comm

GM -. emite/escucha .- EV
APM -. usa .- EV
PMAN -. dispara .- EV
PC -. puede suscribirse .- EV

%% ====== CONFIG & DATOS ======
subgraph Config[Configuración]
  INP[(InputActionReference\n(New Input System))]
  %% futuros ScriptableObjects para reglas
  GCFG[(GameConfig)]:::planned
  PCFG[(PieceConfig/SpawnTable)]:::planned
end

APM --> INP

subgraph Data[Datos / Persistencia]
  SLOTS[(Saves / Slots)]:::planned
end
class SLOTS planned

%% ====== FUTURO (planeado) ======
IM[InputManager]:::planned
SLM[SaveLoadManager]:::planned
PFUSE[PieceFusionSystem]:::planned
PMOVE[PieceMovementSystem]:::planned
PSCORE[PieceScoringSystem]:::planned
PSPAWN[PieceSpawner]:::planned
BUS[EventBus\n(Observer central)]:::planned

%% Conexiones propuestas (futuras)
GM -. coordina .- PSPAWN
GM -. coordina .- PFUSE
GM -. coordina .- PMOVE
GM -. coordina .- PSCORE

PSPAWN -. crea .- PMAN
PFUSE -. opera .- PMAN
PMOVE -. opera .- PMAN
PSCORE -. consulta .- PMAN

BUS -. reemplaza/agrupa .- EV
SLM -. persiste .- SLOTS
IM -. normaliza entradas .- PC
IM -. entrega acciones .- APM

%% Relaciones Core
SCN -->|ChangeScene| GM