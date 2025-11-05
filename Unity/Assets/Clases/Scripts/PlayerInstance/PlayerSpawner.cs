using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject jugadorPrefab;
    public Transform[] posicionesSpawn;

    void Start()
    {
        var persistente = JugadorPersistente.Instancia;
        if (persistente == null) return;

        for (int i = 0; i < persistente.gamepads.Count; i++)
        {
            Vector3 posicion = posicionesSpawn[i].position;
            GameObject jugador = Instantiate(jugadorPrefab, posicion, Quaternion.identity);

            var playerInput = jugador.GetComponent<PlayerInput>();
            if (playerInput != null)
            {
                playerInput.SwitchCurrentControlScheme(persistente.gamepads[i]);
                playerInput.ActivateInput();
            }
        }
    }
}
