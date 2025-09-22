# 📘 Documento Técnico de Diseño: Gob's at Work

## 🎮 Introducción

**Título:** Gob's at Work  
**Género:** Party Game, Cooperativo, Top-Down  
**Plataformas:** Nintendo Switch, Steam Deck  
**Versión:** 1.0  
**Licencia:** Pietaria  

**Resumen:**  
Gob's at Work es un juego cooperativo local para hasta 4 jugadores, donde los jugadores encarnan goblins que deben construir estanterías siguiendo planos específicos, bajo presión de tiempo y con recursos limitados. El juego se desarrolla en entornos industriales subterráneos y combina mecánicas de construcción, trabajo en equipo, y resolución de retos en una experiencia caótica pero divertida.

---

## 🧍‍♂️ Personajes

### Información General

- **Nombre:** No tienen nombre individual  
- **Alias:** Gob  
- **Especie:** Goblins  
- **Edad:** Indefinida  
- **Rol:** Jugadores activos en la cooperativa de construcción  

### Jugabilidad

- **Habilidades básicas:**  
  - Moverse libremente por el mapa  
  - Agarrar, cargar y colocar objetos  
  - Craftear y modificar piezas  
  - Desechar objetos erróneos  
  - Vender estanterías completadas  

- **Limitaciones:**  
  - Solo pueden cargar un objeto a la vez  
  - Colisiones entre personajes afectan la eficiencia  
  - No pueden modificar planos, solo ejecutarlos  

- **Estilo de juego:**  
  - Vista elevada (top-down)  
  - Multijugador local cooperativo  
  - Requiere comunicación constante entre jugadores  

### Diseño Visual

- **Vestimenta:** Overol de trabajo y sombrero de copa  
- **Elemento icónico:** Antorcha en la mano derecha  
- **Color personalizado:** Cada jugador tiene un color único  
- **Referencia visual:**  
  ![GoblinA](/documentacion//img/GoblinA.jpg)

### Requisitos Técnicos

- **Animaciones necesarias:**  
  - Idle  
  - Caminar  
  - Saltar  
  - Agarrar y cargar objetos  
  - Colocar y craftear objetos  

- **Estados del personaje:**  
  - Idle  
  - Moviendo  
  - Saltando  
  - Cargando  
  - Colocando  
  - Crafteando  

- **Interacciones con el entorno:**  
  - Pueden interactuar con estaciones de trabajo, contenedores de materiales, planos y zonas de venta  

---

## 🗺️ Niveles y Progresión

El juego cuenta con **27 niveles**, distribuidos en **3 mundos**, cada uno con **3 biomas**. Cada bioma contiene **3 niveles**. La dificultad escala progresivamente, introduciendo nuevas mecánicas en cada etapa.

| Mundo | Bioma | Nivel | Mecánica Principal |
|-------|-------|-------|--------------------|
| 1. Talleres de Entrenamiento | Bosque Goblin | 1 | Ensamblado básico (tutorial) |
|       |       | 2 | Materiales distintos (madera/ladrillo) |
|       |       | 3 | Planos rotados/espejados |
|       | Cueva de Madera | 4 | Piezas de distintos tamaños |
|       |       | 5 | Objetos pesados (requieren 2 jugadores) |
|       |       | 6 | Obstáculos fijos en el mapa |
|       | Aldea Goblin | 7 | Dos planos simultáneos |
|       |       | 8 | Zona de trabajo limitada |
|       |       | 9 | NPCs goblins estorbando |
| 2. Retos Avanzados | Desierto de Chatarra | 10 | Interferencias visuales (espejismos) |
|       |       | 11 | Arena y piezas frágiles |
|       |       | 12 | Calor destruye piezas en el suelo |
|       | Volcán Mecánico | 13 | Lava bloquea caminos temporalmente |
|       |       | 14 | Géiseres de fuego expulsan objetos |
|       |       | 15 | Herramientas con usos limitados |
|       | Montaña Helada | 16 | Suelo resbaladizo |
|       |       | 17 | Plataformas de hielo que se derriten |
|       |       | 18 | Puertas congeladas (requieren 2 jugadores) |
| 3. El Caos del Tesoro | Fábrica Dorada | 19 | Cintas transportadoras rápidas |
|       |       | 20 | Botones dobles para desbloquear zonas |
|       |       | 21 | Maquinaria bloquea caminos |
|       | Sombras del Tesoro | 22 | Oscuridad parcial |
|       |       | 23 | Linterna portátil (sacrifica movilidad) |
|       |       | 24 | Obstáculos invisibles |
|       | Forja de Vapor | 25 | Autómatas ladrones de materiales |
|       |       | 26 | Plataformas móviles |
|       |       | 27 | Taller del Caos Final (todas las mecánicas combinadas) |

> Para más detalles sobre el sistema de dificultad progresiva, consulta [Complejidad.md](/diseno/Secuencias/Mecanicas/Complejidad.md)

---

## ⚙️ Mecánicas del Juego

Gob's at Work se basa en una combinación de mecánicas cooperativas, físicas y de resolución de retos. Estas se introducen progresivamente a lo largo de los 27 niveles, agrupadas en las siguientes categorías:

### 🧱 Mecánicas de Construcción
- **Ensamblado básico**: unir piezas según planos (Nivel 1)
- **Materiales múltiples**: madera, ladrillo, metal (Niveles 2, 10+)
- **Planos rotados/espejados**: interpretación espacial (Nivel 3)
- **Tamaño de piezas**: piezas grandes y pequeñas (Nivel 4)
- **Crafteo en estaciones**: modificar piezas antes de colocarlas (desde Mundo 2)

### 🤝 Mecánicas Cooperativas
- **Carga compartida**: piezas pesadas requieren 2 jugadores (Nivel 5, 27)
- **Botones dobles**: zonas bloqueadas requieren sincronización (Nivel 20, 18)
- **Puertas y plataformas**: activación simultánea (Nivel 18)
- **Reparto de roles**: ensamblador, recolector, soporte (emergente desde Nivel 7)

### 🌍 Mecánicas Ambientales
- **Obstáculos fijos**: rutas bloqueadas (Nivel 6)
- **NPCs estorbando**: goblins que interfieren (Nivel 9)
- **Terreno resbaladizo**: hielo y aceite (Niveles 16, 27)
- **Oscuridad parcial**: foco fijo o linterna (Niveles 22–23)
- **Plataformas móviles**: terreno cambiante (Nivel 26)

### 🔥 Mecánicas de Riesgo y Tiempo
- **Pago decreciente**: el valor baja con el tiempo (todos los niveles)
- **Piezas frágiles o destructibles**: por calor, caída o abandono (Niveles 11–12)
- **Interferencias visuales**: espejismos y humo (Niveles 10, 27)
- **Enemigos activos**: autómatas ladrones, géiseres, lava (Niveles 13–15, 25, 27)

### 🛠️ Mecánicas de Herramientas y Validación
- **Herramientas con usos limitados**: martillos, sierras (Nivel 15)
- **Validación automática**: verificación de estantería en zona de entrega
- **Errores penalizados**: piezas mal colocadas reducen puntuación

### ⭐ Sistema de Puntuación
- **Estrellas (1–3)**: según tiempo, precisión y eficiencia
- **Bonificación por plano extra**: niveles con múltiples estantes (Nivel 7)
- **Penalización por errores**: piezas mal colocadas, materiales incorrectos
  
---

## 📈 Progreso

- **Medición del progreso:**  
  - Número de diseños completados  
  - Estrellas obtenidas por nivel  
  - Tiempo promedio de construcción  

- **Desbloqueo de contenido:**  
  - Nuevos niveles y biomas se desbloquean al completar los anteriores  
  - Logros por desempeño desbloquean cosméticos o efectos visuales  

- **Recompensas:**  
  - Estrellas acumuladas desbloquean mundos  
  - Logros por eficiencia, velocidad y precisión  
  - Posibilidad de repetir niveles para mejorar puntuación  

- **Ejemplo de progreso:**  
  - Un jugador que completa el nivel 1-3 con 3 estrellas desbloquea el bioma Cueva del Mundo 2  
  - Si lo completa con solo 1 estrella, puede avanzar pero no desbloquea logros visuales  

---

## 🖥️ Interfaces

- **Pantalla de carga:**  
  - Opacidad de 0.3  
  - Barra de progreso que refleja generación de mapa por piezas  
  - Loader visual animado  

- **Selección de nivel:**  
  - Menú visual con mundos, biomas y niveles  
  - Indicadores de estrellas obtenidas  

- **HUD en juego:**  
  - Tiempo restante  
  - Materiales disponibles  
  - Puntuación actual  
  - Plano visible en pantalla  

- **Pantalla de resultados:**  
  - Estrellas obtenidas  
  - Tiempo final  
  - Pago recibido  
  - Botón para repetir o avanzar  

---

