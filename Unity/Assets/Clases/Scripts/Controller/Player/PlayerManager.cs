using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class PlayerManager : MonoBehaviour
{
    public GameObject[] modelos;

    private Dictionary<Gamepad, GameObject> jugadoresActivos = new Dictionary<Gamepad, GameObject>();
    // Límite de jugadores a spawnear, leído desde el menú (Player.SelectedPlayersCount)
    private int maxPlayersToSpawn = 1;

    void OnEnable()
    {
        InputSystem.onDeviceChange += OnDeviceChange;
    }

    void OnDisable()
    {
        InputSystem.onDeviceChange -= OnDeviceChange;
    }

    void Start()
    {
        // Limitar por la cantidad seleccionada y por la cantidad de modelos disponibles
        maxPlayersToSpawn = Mathf.Min(Mathf.Max(Player.SelectedPlayersCount, 1), modelos.Length);
        // Spawnear inmediatamente al entrar a la escena
        TrySpawnMissingPlayers();
    }

    void Update()
    {
        // Completar jugadores faltantes si entran gamepads nuevos
        TrySpawnMissingPlayers();
    }

    private void TrySpawnMissingPlayers()
    {
        foreach (var gamepad in Gamepad.all)
        {
            if (jugadoresActivos.Count >= maxPlayersToSpawn) break;
            if (!jugadoresActivos.ContainsKey(gamepad))
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
        controller.AssignDevice(gamepad);

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
                        // Intentar llenar el cupo con otro gamepad disponible
                        TrySpawnMissingPlayers();
                    }
                    break;

                case InputDeviceChange.Reconnected:
                    Debug.Log($"Gamepad reconectado: {gamepad.displayName}");
                    // Si hay cupo disponible, crear jugador para este gamepad
                    TrySpawnMissingPlayers();
                    break;
            }
        }
    }
}
