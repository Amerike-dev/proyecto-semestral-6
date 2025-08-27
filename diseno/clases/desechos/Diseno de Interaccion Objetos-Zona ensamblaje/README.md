# BuildZone System

## Objetivo

El sistema **BuildZone** gestiona un área delimitada donde los objetos pueden interactuar bajo ciertas reglas.  
Su propósito principal es permitir que una zona en Unity:

- Almacene objetos hasta un límite definido (capacidad).  
- Controle la entrada y salida de objetos.  
- Detecte si está llena o vacía.  
- Ejecute fusiones de objetos (por ejemplo, combinar piezas en un objeto final).  
- Notifique eventos a otros sistemas cuando ocurra una fusión.  

### Casos de uso
- Construcción de estructuras (juntar bloques para crear un edificio).  
- Almacenamiento de ítems en zonas específicas (cofres, estaciones).  
- Puzzles donde al fusionar piezas se genera un nuevo objeto.  

---

## Componentes principales

### BuildZone (lógica central)

Clase principal que administra la zona de almacenamiento y define las reglas del sistema.

#### Atributos
- `id : int` → Identificador único de la zona (permite distinguir entre múltiples zonas en la escena).  
- `capacity : int` → Número máximo de objetos permitidos en la zona.  
- `objects : List<object>` → Colección de objetos actualmente almacenados.  
- `isComplete : bool` → Indica si la zona alcanzó su capacidad máxima.  

#### Métodos
- `CanAccept() : bool` → Retorna `true` si aún hay espacio en la zona.  
- `Add(obj : object) : bool` → Intenta añadir un objeto; devuelve `true` si se logró.  
- `Remove(obj : object) : bool` → Elimina un objeto de la zona.  
- `Count() : int` → Retorna cuántos objetos hay actualmente.  
- `CanFuse() : bool` → Verifica si las condiciones para fusionar se cumplen (ejemplo: al menos 2 objetos compatibles).  
- `FuseAll() : object` → Fusiona todos los objetos almacenados y retorna el objeto resultante.  
- `Clear() : void` → Vacía la zona por completo.  

---

### BuildZoneController (integración con Unity)

Se encarga de conectar la lógica de **BuildZone** con el motor Unity y los objetos de la escena.

#### Atributos
- `zone : BuildZone` → Referencia a la zona lógica asociada.  
- `zoneCollider : Collider` → Área física de colisión donde los objetos pueden entrar.  
- `autoFuse : bool` → Define si la fusión ocurre automáticamente al completar la zona.  
- `onFuse : UnityEvent<GameObject>` → Evento que se dispara cuando se produce una fusión.  

#### Métodos
- `Start() : void` → Inicializa el controlador y prepara referencias.  
- `OnTriggerEnter(other : Collider) : void` → Detecta cuándo un objeto entra en la zona.  
- `TryFuse() : void` → Lanza manualmente el proceso de fusión (si `autoFuse` está en `false`).  

#### Referencia visual

![Diagrama de Clases](/proyecto-semestral-6/diseno/clases/desechos/Diseno%20de%20Interaccion%20Objetos-Zona%20ensamblaje/Zona%20de%20Desechos.png)
