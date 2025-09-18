# Propuesta: Sistema de TrashZone en Unity

## Descripción
El sistema de **TrashZone** permite a los goblins desechar piezas (únicas o fusionadas) en una zona específica del mapa.  
Consta de dos clases principales:

- **TrashZone** (modelo lógico, no hereda de nada).
- **TrashController** (controlador Unity que hereda de `MonoBehaviour`).

La idea es separar la lógica (TrashZone) de la implementación en Unity (TrashController).

---

## Clases

### 🔹 TrashZone
Clase que representa la **zona de desechos** de manera lógica.

**Campos:**
- `position : Vector3` → Ubicación de la zona.  
- `radius : float` → Radio de acción.  
- `discardedPieces : List<GameObject>` → Lista de piezas que fueron desechadas.  

**Métodos:**
- `DiscardSinglePiece(piece : GameObject) : void` → Desecha una pieza única.  
- `DiscardMergedPiece(mergedPiece : GameObject) : void` → Desecha una pieza compuesta.  
- `ClearDestroyedPieces() : void` → Limpia referencias de piezas ya destruidas.  

---

### 🔹 TrashController (MonoBehaviour)
Clase que se adjunta a un **GameObject con un Collider** en Unity.  
Se encarga de detectar objetos que entran en la zona y llamar a la lógica de `TrashZone`.

**Campos:**
- `trashZone : TrashZone` → Zona de desecho asociada.  
- `zoneCollider : Collider` → Collider del área de trigger en Unity.  

**Métodos:**
- `Start() : void` → Inicializa la zona.  
- `OnTriggerEnter(other : Collider) : void` → Detecta si un objeto entra en la zona.  
- `DestroyPiece(piece : GameObject) : void` → Llama a `Destroy()` para eliminar físicamente el objeto en Unity.  

---

## Consideraciones sobre el uso de Destroy vs SetActive(false)

### 🔹 `Destroy(GameObject)`
- **Qué hace:** Elimina permanentemente el objeto de la memoria después del frame actual.  
- **Tiempo:** La destrucción real ocurre al final del frame, por lo que no es inmediata.  
- **Impacto en rendimiento:**  
  - Requiere más procesamiento porque Unity libera memoria y referencias.  
  - Si hay muchas instancias destruyéndose por frame (cientos o miles), puede provocar caídas de FPS.  
- **Cuándo usarlo:**  
  - Si el número de piezas simultáneas es bajo (<100 activas en escena).  
  - Si no necesitas reutilizar esas piezas.  

---

### 🔹 `SetActive(false)` con Object Pooling
- **Qué hace:** Desactiva el objeto, pero permanece en memoria y puede reactivarse sin costos de reinstanciación.  
- **Tiempo:** Instantáneo y muy barato en comparación con `Destroy`.  
- **Impacto en rendimiento:**  
  - Optimiza muchísimo en juegos donde se crean y destruyen objetos constantemente.  
  - Evita la sobrecarga de `Instantiate` y `Destroy`.  
- **Cuándo usarlo:**  
  - Si esperas que los jugadores estén desechando piezas de forma muy frecuente.  
  - En niveles avanzados con gran cantidad de piezas activas.  

---

## Recomendación para este proyecto
- Dado que se menciona que en promedio habrá **menos de 100 piezas activas** en simultáneo, **`Destroy()` puede ser suficiente** sin afectar de forma crítica el rendimiento.  
- Si en pruebas de estrés o en niveles más complejos el conteo de piezas aumenta (200–500 en escena), será recomendable implementar un **sistema de Pooling** donde los objetos se desactiven (`SetActive(false)`) y se reutilicen.  

---

# Propuesta: Sistema de TrashZone con Object Pooling en Unity

## Descripción
El sistema de **TrashZone** permite a los goblins desechar piezas (únicas o fusionadas) en una zona específica del mapa.  
A diferencia de la primera propuesta (que usaba `Destroy()`), ahora se implementa un **Object Pooling** que desactiva (`SetActive(false)`) las piezas y las reutiliza cuando se necesiten de nuevo.  

Esto reduce la sobrecarga de `Instantiate` y `Destroy`, optimizando el rendimiento en escenarios con muchas piezas.

---

## Clases

### 🔹 TrashZone
Clase que representa la **zona de desechos** de manera lógica.

**Campos:**
- `position : Vector3` → Ubicación de la zona.  
- `radius : float` → Radio de acción.  
- `discardedPieces : List<GameObject>` → Lista de piezas que han pasado por la zona.  
- `objectPool : Queue<GameObject>` → Cola que administra las piezas inactivas listas para reutilizarse.  

**Métodos:**
- `DiscardSinglePiece(piece : GameObject) : void` → Desecha una pieza única.  
- `DiscardMergedPiece(mergedPiece : GameObject) : void` → Desecha una pieza compuesta.  
- `ClearDestroyedPieces() : void` → Limpia referencias innecesarias.  
- `ReturnToPool(piece : GameObject) : void` → Desactiva una pieza y la guarda en el pool.  
- `GetFromPool() : GameObject` → Obtiene una pieza previamente desactivada para reutilizarla.  

---

### 🔹 TrashController (MonoBehaviour)
Clase que se adjunta a un **GameObject con un Collider** en Unity.  
Se encarga de detectar objetos que entran en la zona y delega a `TrashZone` la lógica de desecho.

**Campos:**
- `trashZone : TrashZone` → Zona de desecho asociada.  
- `zoneCollider : Collider` → Collider del área de trigger en Unity.  

**Métodos:**
- `Start() : void` → Inicializa la zona.  
- `OnTriggerEnter(other : Collider) : void` → Detecta si un objeto entra en la zona.  
- `DeactivatePiece(piece : GameObject) : void` → Llama a `ReturnToPool()` para desactivar la pieza en lugar de destruirla.  

---

## Ventajas de esta propuesta
- ✅ **Optimización**: Reduce la sobrecarga de CPU y GC al evitar llamadas frecuentes a `Destroy()` y `Instantiate()`.  
- ✅ **Escalabilidad**: Funciona bien incluso si en niveles avanzados se manejan cientos de piezas.  
- ✅ **Reutilización**: Las piezas se guardan desactivadas y se pueden volver a activar cuando se necesiten.  
- ⚠️ **Complejidad adicional**: Se debe manejar correctamente el pool para evitar fugas o referencias inválidas.  

---

## Recomendación para este proyecto
- Si el número de piezas simultáneas es **bajo (<100)**, el uso de `Destroy()` sigue siendo viable.  
- Si se espera que los jugadores desechen muchas piezas (200–500 o más), **este sistema de Pooling será más eficiente y estable en FPS**.  
