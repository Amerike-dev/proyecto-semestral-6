using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu Instance { get; private set; }
    public GameObject pauseCanvas;
    public bool isPaused = false;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            gameObject.SetActive(false);
            return;
        }
        Instance = this;
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        pauseCanvas.SetActive(isPaused);
        Debug.Log("Canvas activo: " + pauseCanvas.activeSelf);
        Time.timeScale = isPaused ? 0 : 1;
    }
}
