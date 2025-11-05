using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class JugadorPersistente : MonoBehaviour
{
    public static JugadorPersistente Instancia;

    public List<Gamepad> gamepads = new List<Gamepad>();
    public List<int> indices = new List<int>();

    void Awake()
    {
        if (Instancia != null)
        {
            gameObject.SetActive(false);
            return;
        }

        Instancia = this;
        DontDestroyOnLoad(gameObject);
    }

    public void RegistrarJugador(Gamepad gamepad, int index)
    {
        if (!gamepads.Contains(gamepad))
        {
            gamepads.Add(gamepad);
            indices.Add(index);
        }
    }

    public void Limpiar()
    {
        gamepads.Clear();
        indices.Clear();
    }
}
