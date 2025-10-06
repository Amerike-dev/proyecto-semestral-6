# Clase Language

## Descripción

La clase `Language` es un **singleton estático** que gestiona los textos multilenguaje de las escenas.  
Devuelve diccionarios con los textos a utilizar en UI y HUD.

## Lenguajes soportados

- `esp` → Español
- `ing` → Inglés

## Funciones actuales

- `SetLanguage(SupportedLanguage lang)` → Cambia el idioma actual del juego.
- `GetCurrentLanguage()` → Devuelve el idioma actual.
- `GetMainMenuText(SupportedLanguage lang)` → Retorna un diccionario con los textos del menú principal.

### Ejemplo de salida

```json
{
  "language": "esp",
  "title": "Iniciar juego",
  "buttons": {
    "exit": "Salir",
    "start": "Iniciar"
  }
}
```

### Prueba unitaria

![Preba unitaria](image.png)
