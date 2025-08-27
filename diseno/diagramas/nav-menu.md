# 📑 Diseño de Navegación – Goblins At Work

Este documento describe los **casos de navegación**  del videojuego **Diagramas de**

---

## 🎮 Caso 1: Inicio del Juego

```mermaid
sequenceDiagram
    actor Jugador
    Jugador ->> Juego: Inicia aplicación
    Juego ->> Animacion: Reproducir intro
    Animacion ->> MenuPrincipal: Mostrar menú principal

```


## 🎮 Caso 2: Menu Principal
```mermaid
sequenceDiagram
    actor Jugador
    Jugador ->> MenuPrincipal: Navegar opciones
    MenuPrincipal ->> AgregarJugadores: Seleccionar "Unirse"
    MenuPrincipal ->> SeleccionNivel: Seleccionar "Jugar"
    MenuPrincipal ->> Configuracion: Seleccionar "Opciones"
    MenuPrincipal ->> Creditos: Seleccionar "Créditos"
```
## 🎮 Caso 3: Seleccion de Nivel
```mermaid
sequenceDiagram
Jugador ->> SeleccionNivel: Ingresar a selección de nivel
    SeleccionNivel ->> Juego: Desplegar mundos disponibles
    Juego ->> Jugador: Mostrar niveles con requisitos y estrellas
```
## 🎮 Caso 4: Seleccion de Jugadores
```mermaid
sequenceDiagram
Jugador ->> SeleccionJugadores: Unirse a slot disponible
    SeleccionJugadores ->> Jugador: Mostrar goblin con color único
    Jugador ->> SeleccionJugadores: Confirmar "Listo"
    SeleccionJugadores ->> Juego: Activar botón "Iniciar partida"
```
## 🎮 Caso 5: Partida en Curso
```mermaid
sequenceDiagram
Jugador ->> Juego: Inicia nivel seleccionado
    Juego ->> PantallaCarga: Mostrar tips y animación
    PantallaCarga ->> Nivel: Cargar mapa y objetivos
    Nivel ->> Jugador: Mostrar diseños de estantes
    Jugador ->> Nivel: Interactuar con objetos (agarrar, saltar, emoji)
    Nivel ->> SistemaPuntuacion: Calcular estrellas según desempeño
```
## 🎮 Caso 6: Partida en Curso
```mermaid
sequenceDiagram
Nivel ->> SistemaPuntuacion: Enviar progreso y tiempo
    SistemaPuntuacion ->> Jugador: Mostrar resultado (1 a 3 estrellas)
    Jugador ->> MenuPrincipal: Elegir repetir, cambiar nivel o salir
```
## 🎮 Caso 7:Configuracion
```mermaid
sequenceDiagram
Jugador ->> Configuracion: Abrir opciones
    Configuracion ->> Jugador: Mostrar ajustes (sonido, controles, idioma)
    Jugador ->> Configuracion: Guardar cambios
```
## 🎮 Caso 8:Creditos
```mermaid
sequenceDiagram
Jugador ->> Creditos: Abrir créditos
    Creditos ->> Jugador: Mostrar equipo de desarrollo
```
