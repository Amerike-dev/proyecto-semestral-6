using UnityEngine;
using System.Collections;

public class LevelTimerManager : MonoBehaviour
{
    public static LevelTimerManager Instance { get; private set; }

    [Header("Tiempos (segundos)")]
    public float countdownTime = 3f;    
    public float levelTime = 180f;      

    private bool gameActive = false;
    private float currentTime;
    private LevelTimerUI timerUI;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        timerUI = FindAnyObjectByType<LevelTimerUI>();
        //StartCoroutine(CountdownSequence());
    }

    private IEnumerator CountdownSequence()
    {
        float counter = countdownTime;

        Time.timeScale = 0f;
        gameActive = false;

        while (counter > 0)
        {
            timerUI.UpdateCountdown(Mathf.CeilToInt(counter));
            yield return new WaitForSecondsRealtime(1f);
            counter--;
        }

        timerUI.ShowGoSignal();

        yield return new WaitForSecondsRealtime(0.8f);

        Time.timeScale = 1f;
        gameActive = true;
        StartCoroutine(LevelTimerRoutine());
    }


    public IEnumerator LevelTimerRoutine()
    {
        currentTime = levelTime;
        while (currentTime > 0)
        {
            timerUI.UpdateLevelTimer(currentTime);
            currentTime -= Time.deltaTime;
            yield return null;
        }

        currentTime = 0;
        timerUI.UpdateLevelTimer(0);
        EndLevel();
    }

    public IEnumerator StartCountdownExternally()
    {
        yield return CountdownSequence();
    }

    private void EndLevel()
    {
        gameActive = false;
    }

    public bool IsGameActive() => gameActive;
}

