```mermaid
classDiagram
    class BuildZone {
        - int id
        - int capacity
        - List<object> objects
        - bool isComplete
        + bool CanAccept()
        + bool Add(object obj)
        + bool Remove(object obj)
        + int Count()
        + bool CanFuse()
        + object FuseAll()
        + void Clear()
    }

    class BuildZoneController {
        - BuildZone zone
        - Collider zoneCollider
        - bool autoFuse
        - UnityEvent<GameObject> onFuse
        + void Start()
        + void OnTriggerEnter(Collider other)
        + void TryFuse()
    }

    BuildZoneController --> BuildZone : usa