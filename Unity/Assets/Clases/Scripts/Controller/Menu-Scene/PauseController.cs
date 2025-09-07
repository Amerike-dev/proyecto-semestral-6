using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
public class PauseController : MonoBehaviour
{
   public GameObject pausePanel;

    private bool isPaused = false;

    void Update()
    {
        // Detecta tecla ESC en teclado
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Pause();
        }

        // Detecta botón START en gamepad
        if (Gamepad.current != null && Gamepad.current.startButton.wasPressedThisFrame)
        {
            Pause();
        }
    }

    private void Pause()
    {
        if (isPaused)
            ResumeGame();
        else
            PauseGame();
    }

    // Pausar
    public void PauseGame()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    // Continuar
    public void ResumeGame()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    // Salir
    public void QuitGame()
    {
        Debug.Log("Saliendo del juego...");
        // Application.Quit();
    }

    // Reiniciar
    public void RestartScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        pausePanel.SetActive(false);
    }
}
