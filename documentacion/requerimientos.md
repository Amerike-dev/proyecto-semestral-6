## Requerimientos
Los requerimientos son la definicion de lo que el proyecto necesita, se pueden definir como `funcionales` y no `funcionales`.

### Requerimientos funcionales
Definen la funcionalidad que se espera tener, es decir, si se busca que un juego tenga:
- multijugador
- generacion procedural
- microtransacciones
- pantalla de carga
- puntuacion

Como tal, definen lo tangible.

### Requerimientos no funcionales
Describen el negocio o lo que se requiere del juego en funcion del objetivo, por ejemplo:

- Localizarse en las principales consolas al mismo tiempo de salida.
- Obtener al menos 100 usuarios jugando en linea.

Generalmente, estos requerimientos tienden a traducirse en requerimientos funcionales a un nivel mas pequeño.

## Requerimiento del proyecto
Se hara un videojuego cooperativo local con una tematica party.

### Referencias
- Overcooked
- Ultimate Chiken Horse
- Unrailed
- Good Job

# Gob's at Work
El videojuego trata de un goblins que intentan construir estantes donde guardaran sus monedas de oro, cuando las tengan.

- Es un multijugador top down cooperativo para 4 personas.
- Los personajes pueden agarrar objetos, saltar, y dar emojis.
- Algunos objetos tienen acciones.
- Los personajes chocan entre ellos.

Para poder armar los estantes, reciben un diseño que deben poder replicar.

- Los diseños pueden mostrarse volteados (rotados)
- Los goblins contaran con: ladrillos | madera | metal segun el diseño y el nivel
- Tienen un tiempo especifico para completar el estante

La partida tiene una duracion especifica, con un sistema de puntuacion de 3 estrellas
- La primer estrella se hace si se logra completar todos los diseños.
- La segunda estrella se hace si se logra hacer en menos de cierto tiempo (definido por nivel).
- La tercer estrella se hace si se logra una puntuacion alta.

Los clientes son goblins con mucho dinero, y que no pueden desperdiciar su tiempo, por lo que por cada cierto segundos decidiran pagar menos por perdida de tiempo.
