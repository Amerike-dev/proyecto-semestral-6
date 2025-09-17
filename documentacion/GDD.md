# Documento de Diseño de Videojuego (GDD)

## Introducción

**Título:** Gob's at Work

**Género:** Party, Cooperativo, Top Down

**Plataforma:** Nintendo Switch, Steam Deck

*Justificación:* Estas plataformas fueron seleccionadas por su enfoque en juegos multijugador locales y portabilidad, ideales para experiencias party cooperativas. Nintendo Switch y Steam Deck permiten jugar en grupo fácilmente y tienen una base de usuarios que disfruta este tipo de juegos.

**Versión:** 1.0

**Sinopsis:**
Gob's at Work es un videojuego cooperativo local para hasta 4 jugadores, donde los participantes toman el rol de goblins que deben construir estantes para guardar monedas de oro, siguiendo diseños específicos bajo presión de tiempo y con recursos limitados. El juego combina trabajo en equipo, resolución de retos y diversión en una temática party.

**Alcance:**
Desarrollar un juego multijugador local con mecánicas de construcción, interacción entre personajes y objetos, sistema de puntuación por estrellas y niveles con diferentes retos. El objetivo es lograr una experiencia divertida y desafiante para grupos de amigos.

**Categoría:** Entretenimiento

**Licencia:** Pietaria

## Guion
### Visión General
Juego cooperativo de construcción rápida donde una cooperativa de goblins fabrica estanterías especializadas para clientes impacientes. El valor del pago desciende en tiempo real, forzando coordinación y trabajo en equipo.

### Mundo y Contexto
Talleres subterráneos y entornos que cada vez van requiriendo mayor dificultad y menor margen d eerror por parte de los jugadores. Ambientación industrial.

### Premisa Narrativa
Cada contrato es un pedido urgente con planos que pueden llegar con distintos grados de dificultad o requisitos como tipo de material o diseño. La reputación se construye entregando diseños exactos antes de que el pago sea menor  no púedan conseguir todas las estrellas de un nivel.

### Objetivo del Jugador
Completar el diseño con exactitud, sin cometer tatos errores y en el menor tiempo posible para maximizar estrellas y mantener mejores pagos.

### Bucle de Juego
1. Mostrar plano.
2. Lectura y reparto de tareas (quién trae qué / quién ensambla / quién despeja).
3. Transporte de materiales (madera, ladrillo, metal).
4. Colocación y corrección  de piezas erróneas.
5. Validación automática del estante y cálculo de pago + estrellas.
6. Paso al siguiente plano / nivel / mundo.

### Escalado de Dificultad
- Básico:Planos simple, un solo material.
- Intermedio: Diseños mas complejos y uso de otros materiales como ladrillo.
- Avanzado: Diseños variandos entre sencillo y complejo e uso de 3 diferentes tipos de materiales (madera,ladrillo y metal)

### Tono y Estilo
Caótico pero divertido. Fallos generan humor entre frustacion y diversion al mismo tiempo.

### Diferenciadores / Gancho
- Construcción detallada (no recetas abstractas).
- Pago decreciente crea urgencia.
- Variación de planos y materiales mantiene rejugabilidad.
- Claridad: Pasar de solucionar a optimizar rutas y tiempos para lograr la mejor puntuacion posible.

## Mecánicas
- Multijugador local cooperativo para hasta 4 personas.
- Construcción de estantes siguiendo diseños que pueden estar rotados.
- Uso de materiales: ladrillos, madera, metal según el nivel.
- Personajes pueden agarrar objetos, saltar y dar emojis.
- Interacción con objetos que pueden tener acciones específicas.
- Colisiones entre personajes.
- Tiempo limitado para completar cada estante.
- Sistema de puntuación de 3 estrellas basado en desempeño y tiempo.

## Interfaces
- Pantalla de carga.
- Interfaz de selección de nivel y diseño.
- HUD con tiempo restante, materiales disponibles y puntuación.
- Pantalla de resultados al finalizar el nivel.
- Menús intuitivos y visuales, adaptados a la temática de goblins y construcción.

## Niveles
- Niveles con diferentes diseños de estantes y retos.
- Variedad en materiales y dificultad según el avance.
- Cada nivel tiene un tiempo específico para completarse.

## Progreso del juego
- El progreso se mide por la cantidad de diseños completados y estrellas obtenidas.
- Desbloqueo de nuevos niveles y retos al avanzar.
- Sistema de recompensas y logros por desempeño.

## Personajes
### Información General

- **Nombre completo:**   No tienen
- **Alias o apodo:**  Gob
- **Edad:**  Indefinida
- **Raza/Especie:**  Goblins
- **Rol en la historia:** Personajes jugables 

---

### Jugabilidad

- **Habilidades básicas:**  
  - Moverse 
  - Agarrar objetos  
  - Modificar objetos 
  - Craftear objetos  
  - Desechar objetos
  - Vender objetos 
- **Estilo de juego:** Multijugador y con vista elevada   
- **limitaciones:**  El jugador solo podrá llevar un objeto a la vez

---

### Diseño Visual

- **Vestimenta:**  Usa un overol y un sombrero de copa
- **Elementos icónicos:** Su vestimenta y la antorcha que tiene en la mano derecha 
- **Referencias visuales:**  
![GoblinA](./img/GoblinA.jpg)
- **Cambios visuales a lo largo del juego:**  Cada jugador tendrá su propio color al entrar al juego

---

### Requisitos Técnicos

- **Animaciones necesarias:**  
  - Idle  
  - Caminar  
  - Saltar
  - Agarrar objetos
  - Cargar objetos  
  - Colocar objetos  
- **Estados del personaje:**  
  - Idle  
  - Moviendo  
  - Saltando
  - Cargando objetos  
  - Colocando objetos  
  - Crafteando objetos 
- **Interacciones con entorno:**  Puede agarrar, colocar, modificar y craftear objetos para venderlos


