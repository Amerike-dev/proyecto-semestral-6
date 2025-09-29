```mermaid
---
title: Diagrama de Clases Spawner General
---
classDiagram
    class SpawnerGeneral {
        +GameObject brickPrefab
        +List<GameObject> bricksList
        +GameObject AskForObject(Vector3 position)
    }