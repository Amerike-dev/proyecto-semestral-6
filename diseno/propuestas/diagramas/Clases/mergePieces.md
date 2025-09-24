```mermaid
---
title: FusionManager y FusionZone
---
classDiagram
    class FusionZone {
        +List<GameObject> pieces
        +void OnTriggerEnter(Collider other)
        +void OnTriggerExit(Collider other)
    }

    class FusionManager {
        -FusionZone areaDeFusion
        -InputActionReference mergeAction
        +void OnEnable()
        +void OnDisable()
        +void OnFusionar(InputAction.CallbackContext context)
        -void FusionarPiezas()
        -Vector3 CalcularCentro(List<GameObject> piezas)
    }

    FusionManager --> FusionZone : accede a piezas dentro
