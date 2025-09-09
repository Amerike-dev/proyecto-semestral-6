using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    // EVENTOS

    // Flujo de navegación
    public event Action OnGameStart;
    public event Action OnMenuShown;
    public event Action OnLevelSelected;
    public event Action OnPlayersSelected;

    // Flujo de partida
    public event Action OnLevelLoading;
    public event Action OnLevelStarted;
    public event Action<GameObject> OnObjectInteracted; // obj = ladrillo, madera, metal
    public event Action<string> OnDesignAssigned;       // id del diseño
    public event Action<string> OnDesignCompleted;      // id del diseño
    public event Action<GameObject> OnTrashUsed;        // objeto que se desactivó

    // Tiempo y puntuación
    public event Action<float> OnTimeTick; // tiempo restante
    public event Action<int> OnScoreUpdated;
    public event Action<int> OnStarAchieved; // 1,2,3

    // Fin de partida
    public event Action OnLevelCompleted;
    public event Action OnResultsShown;
    public event Action OnReturnToMenu;

    //  Estado del Juego
    public enum GameState
    {
        None,
        Intro,
        Menu,
        PlayerSelection,
        LevelSelection,
        Loading,
        Playing,
        Completed,
        Results
    }

    public GameState CurrentState { get; private set; } = GameState.None;
    private float levelTimer;
    private int score;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    //  MÉTODOS PÚBLICOS

    public void StartGame()
    {
        CurrentState = GameState.Intro;
        OnGameStart?.Invoke();
    }

    public void ShowMenu()
    {
        CurrentState = GameState.Menu;
        OnMenuShown?.Invoke();
    }

    public void SelectLevel()
    {
        CurrentState = GameState.LevelSelection;
        OnLevelSelected?.Invoke();
    }

    public void SelectPlayers()
    {
        CurrentState = GameState.PlayerSelection;
        OnPlayersSelected?.Invoke();
    }

    public void LoadLevel()
    {
        CurrentState = GameState.Loading;
        OnLevelLoading?.Invoke();
    }

    public void StartLevel(float duration)
    {
        CurrentState = GameState.Playing;
        levelTimer = duration;
        score = 0;
        OnLevelStarted?.Invoke();
    }

    public void RegisterObjectInteraction(GameObject obj)
    {
        OnObjectInteracted?.Invoke(obj);
    }

    public void RegisterTrash(GameObject obj)
    {
        OnTrashUsed?.Invoke(obj);
    }

    public void AssignDesign(string designId)
    {
        OnDesignAssigned?.Invoke(designId);
    }

    public void CompleteDesign(string designId)
    {
        OnDesignCompleted?.Invoke(designId);
        AddScore(100); // ejemplo: cada diseño = 100 pts
    }

    public void AddScore(int points)
    {
        score += points;
        OnScoreUpdated?.Invoke(score);

        if (score >= 100 && score < 200) OnStarAchieved?.Invoke(1);
        if (score >= 200 && score < 300) OnStarAchieved?.Invoke(2);
        if (score >= 300) OnStarAchieved?.Invoke(3);
    }

    private void Update()
    {
        if (CurrentState == GameState.Playing)
        {
            levelTimer -= Time.deltaTime;
            OnTimeTick?.Invoke(levelTimer);

            if (levelTimer <= 0)
            {
                EndLevel();
            }
        }
    }

    public void EndLevel()
    {
        CurrentState = GameState.Completed;
        OnLevelCompleted?.Invoke();
    }

    public void ShowResults()
    {
        CurrentState = GameState.Results;
        OnResultsShown?.Invoke();
    }

    public void ReturnToMenu()
    {
        CurrentState = GameState.Menu;
        OnReturnToMenu?.Invoke();
    }
}
