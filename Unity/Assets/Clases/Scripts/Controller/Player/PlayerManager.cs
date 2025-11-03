using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class PlayerManager : MonoBehaviour
{
    public GameObject modeloVisualPrefab; // solo modelo, sin PlayerInput
    public Transform[] posicionesSpawn;

    private Dictionary<Gamepad, GameObject> jugadoresVisuales = new Dictionary<Gamepad, GameObject>();
    private int maxPlayersToSpawn = 4;

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
        maxPlayersToSpawn = Mathf.Min(4, posicionesSpawn.Length);
        TrySpawnVisuales();
    }

    void Update()
    {
        TrySpawnVisuales();
    }

    private void TrySpawnVisuales()
    {
        foreach (var gamepad in Gamepad.all)
        {
            if (jugadoresVisuales.Count >= maxPlayersToSpawn) break;
            if (!jugadoresVisuales.ContainsKey(gamepad))
            {
                CrearVisual(gamepad, jugadoresVisuales.Count);
            }
        }
    }

    void CrearVisual(Gamepad gamepad, int index)
    {
        Vector3 posicion = posicionesSpawn[index].position;
        GameObject visual = Instantiate(modeloVisualPrefab, posicion, Quaternion.identity);

        jugadoresVisuales.Add(gamepad, visual);
        JugadorPersistente.Instancia?.RegistrarJugador(gamepad, index);
    }

    void OnDeviceChange(InputDevice device, InputDeviceChange change)
    {
        if (device is Gamepad gamepad)
        {
            switch (change)
            {
                case InputDeviceChange.Disconnected:
                    if (jugadoresVisuales.ContainsKey(gamepad))
                    {
                        Destroy(jugadoresVisuales[gamepad]);
                        jugadoresVisuales.Remove(gamepad);
                        TrySpawnVisuales();
                    }
                    break;

                case InputDeviceChange.Reconnected:
                    TrySpawnVisuales();
                    break;
            }
        }
    }
}
