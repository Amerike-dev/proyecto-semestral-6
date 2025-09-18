# 📑 Propuesta de `GameManager` – Goblins At Work

El `GameManager` es el **núcleo de control** del videojuego. Se encarga de:  
- Mantener y gestionar el **estado global del juego**.  
- Disparar **eventos** que otros sistemas (UI, objetos, audio, niveles) escuchan.  
- Orquestar la **navegación** entre pantallas, niveles y resultados.  
- Integrar la lógica ya implementada con **objetos, trashzone, pool de objetos y puntuación**.  

---

##  Estados principales

El juego se compone de los siguientes estados controlados por el `GameManager`:

1. **Intro** – Se muestra la animación inicial.  
2. **Menu** – Pantalla principal, navegación hacia jugar, opciones, créditos.  
3. **PlayerSelection** – Selección y confirmación de jugadores.  
4. **LevelSelection** – Elección del nivel a jugar.  
5. **Loading** – Pantalla de carga con tips.  
6. **Playing** – Partida en curso (jugadores interactúan, arman estantes, usan objetos).  
7. **Completed** – El nivel terminó (por tiempo o por completar objetivos).  
8. **Results** – Se muestran puntuaciones y estrellas.  

---

##  Eventos gestionados por el `GameManager`

###  Flujo de navegación
- **OnGameStart** → El juego inicia (intro).  
- **OnMenuShown** → Se muestra el menú principal.  
- **OnLevelSelected** → Un nivel fue elegido.  
- **OnPlayersSelected** → Los jugadores confirman que están listos.  

###  Flujo de partida
- **OnLevelLoading** → Comienza la carga del nivel.  
- **OnLevelStarted** → Nivel iniciado, partida activa.  
- **OnObjectInteracted (GameObject)** → Un jugador toma o suelta un objeto.  
- **OnDesignAssigned (string)** → Se asigna un diseño de estante.  
- **OnDesignCompleted (string)** → Se completa un diseño correctamente.  
- **OnTrashUsed (GameObject)** → Un objeto se eliminó en la `TrashZone` (pool → `SetActive(false)`).  

###  Tiempo y puntuación
- **OnTimeTick (float)** → Actualización del temporizador.  
- **OnScoreUpdated (int)** → Se modificó la puntuación.  
- **OnStarAchieved (int)** → Se ganó una estrella (1, 2 o 3).  

###  Fin de partida
- **OnLevelCompleted** → El tiempo terminó o todos los diseños fueron completados.  
- **OnResultsShown** → Pantalla de resultados activa.  
- **OnReturnToMenu** → Jugador decide volver al menú principal.  

---

##  Integración con sistemas existentes

- **ObjectInteraction**  
  Cuando el jugador toma/suelta un objeto:  
  ```csharp
  GameManager.Instance.RegisterObjectInteraction(obj);

## Diagrama de Estados

```mermaid
---
title: GameManager
---

stateDiagram-v2
    [*] --> Intro
    Intro --> Menu : OnGameStart
    Menu --> PlayerSelection : OnPlayersSelected
    Menu --> LevelSelection : OnLevelSelected
    LevelSelection --> Loading : OnLevelLoading
    Loading --> Playing : OnLevelStarted
    Playing --> Completed : OnLevelCompleted
    Completed --> Results : OnResultsShown
    Results --> Menu : OnReturnToMenu