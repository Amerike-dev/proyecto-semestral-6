using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseController : MonoBehaviour
{
    public GameObject pausePanel;
    public GameObject settingsPanel;
    public GameObject confirmPanel;

    private bool isPaused = false;

    // Referencia a la acción de pausa
    private InputAction pauseAction;

    void OnEnable()
    {
        // Crea una instancia temporal si no usas PlayerInput
        var inputActions = new InputActionMap("UI");
        pauseAction = inputActions.AddAction("Pause", binding: "<Keyboard>/escape");
        pauseAction.AddBinding("<Gamepad>/start");

        pauseAction.performed += ctx => TogglePause();
        pauseAction.Enable();
    }

    void OnDisable()
    {
        pauseAction.Disable();
    }

    private void TogglePause()
    {
        if (isPaused)
            ResumeGame();
        else
            PauseGame();
    }

    public void PauseGame()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void QuitGame()
    {
        confirmPanel.SetActive(true);
    }

    public void ConfirmQuitGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
        pausePanel.SetActive(false);
    }

    public void CancelQuitGame()
    {
        confirmPanel.SetActive(false);
    }

    public void RestartScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        pausePanel.SetActive(false);
    }

    public void Settings()
    {
        settingsPanel.SetActive(true);
    }

    public void BackFromSettings()
    {
        settingsPanel.SetActive(false);
    }
}
