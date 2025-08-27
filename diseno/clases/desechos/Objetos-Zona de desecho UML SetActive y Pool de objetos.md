```mermaid
    ---
    title: Objetos-Zona de desecho (SetActive y Pool de objetos)
    ---
classDiagram
    class TrashZone {
        - Vector3 position
        - float radius
        - List<GameObject> discardedPieces
        - Queue<GameObject> objectPool
        + void DiscardSinglePiece(GameObject piece)
        + void DiscardMergedPiece(GameObject mergedPiece)
        + void ClearDestroyedPieces()
        + void ReturnToPool(GameObject piece)
        + GameObject GetFromPool()
    }

    class TrashController {
        - TrashZone trashZone
        - Collider zoneCollider
        + void Start()
        + void OnTriggerEnter(Collider other)
        + void DeactivatePiece(GameObject piece)
    }

    TrashController ..> TrashZone : uses
```