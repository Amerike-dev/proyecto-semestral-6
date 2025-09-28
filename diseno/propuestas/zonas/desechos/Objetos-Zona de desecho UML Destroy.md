```mermaid
    ---
    title: Objetos-Zona de desecho (Destroy)
    ---
classDiagram
    class TrashZone {
        - Vector3 position
        - float radius
        - List<GameObject> discardedPieces
        + void DiscardSinglePiece(GameObject piece)
        + void DiscardMergedPiece(GameObject mergedPiece)
        + void ClearDestroyedPieces()
    }

    class TrashController {
        - TrashZone trashZone
        - Collider zoneCollider
        + void Start()
        + void OnTriggerEnter(Collider other)
        + void DestroyPiece(GameObject piece)
    }

    TrashController ..> TrashZone : uses
```