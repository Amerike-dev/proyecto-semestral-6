using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class PlayerManager : MonoBehaviour
{
    public GameObject[] modelos;

    private Dictionary<Gamepad, GameObject> jugadoresActivos = new Dictionary<Gamepad, GameObject>();

    void OnEnable()
    {
        InputSystem.onDeviceChange += OnDeviceChange;
    }

    void OnDisable()
    {
        InputSystem.onDeviceChange -= OnDeviceChange;
    }

    void Update()
    {
        foreach (var gamepad in Gamepad.all)
        {
            if (!jugadoresActivos.ContainsKey(gamepad) && jugadoresActivos.Count < modelos.Length)
            {
                CrearJugador(gamepad, jugadoresActivos.Count);
            }
        }
    }

    void CrearJugador(Gamepad gamepad, int index)
    {
        Vector3 posicion = new Vector3(index * 3, 0, 0);
        GameObject modelo = Instantiate(modelos[index], posicion, Quaternion.identity);
        var controller = modelo.AddComponent<PlayerController>();
        controller.gamepad = gamepad;

        jugadoresActivos.Add(gamepad, modelo);
    }

    void OnDeviceChange(InputDevice device, InputDeviceChange change)
    {
        if (device is Gamepad gamepad)
        {
            switch (change)
            {
                case InputDeviceChange.Disconnected:
                    if (jugadoresActivos.ContainsKey(gamepad))
                    {
                        Destroy(jugadoresActivos[gamepad]);
                        jugadoresActivos.Remove(gamepad);
                        Debug.Log($"Jugador con {gamepad.displayName} desconectado.");
                    }
                    break;

                case InputDeviceChange.Reconnected:
                    Debug.Log($"Gamepad reconectado: {gamepad.displayName}");
                    // Puedes decidir si quieres reinstanciar al jugador aquí
                    break;
            }
        }
    }
}
