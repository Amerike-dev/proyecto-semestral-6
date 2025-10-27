using UnityEngine;
using System;

public class StatsReader : MonoBehaviour
{
    public static StatsReader Instance { get; private set; }

    // Claves PlayerPrefs
    private const string KEY_PLAYTIME_SECONDS = "sr.playtimeSeconds";
    private const string KEY_COINS = "sr.coins";
    private const string KEY_LEVELS_LOCKED = "sr.levelsLocked";
    private const string KEY_LEVELS_UNLOCKED = "sr.levelsUnlocked";

    // Cache en memoria
    private float _totalSeconds;   // acumulado guardado
    private float _sessionSeconds; // acumulado de la sesión actual (no guardado aún)
    private int _coins;
    private int _levelsLocked;
    private int _levelsUnlocked;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadFromPrefs();
    }

    private void Update()
    {
        // Contar tiempo real
        _sessionSeconds += Time.unscaledDeltaTime;
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause) { CommitSessionAndSave(); }
    }

    private void OnApplicationQuit()
    {
        CommitSessionAndSave();
    }

    private void CommitSessionAndSave()
    {
        if (_sessionSeconds > 0f)
        {
            _totalSeconds += _sessionSeconds;
            _sessionSeconds = 0f;
        }
        SaveToPrefs();
    }

    private void LoadFromPrefs()
    {
        _totalSeconds = PlayerPrefs.GetFloat(KEY_PLAYTIME_SECONDS, 0f);
        _coins = PlayerPrefs.GetInt(KEY_COINS, 0);
        _levelsLocked = PlayerPrefs.GetInt(KEY_LEVELS_LOCKED, 0);
        _levelsUnlocked = PlayerPrefs.GetInt(KEY_LEVELS_UNLOCKED, 0);
    }

    private void SaveToPrefs()
    {
        PlayerPrefs.SetFloat(KEY_PLAYTIME_SECONDS, _totalSeconds);
        PlayerPrefs.SetInt(KEY_COINS, _coins);
        PlayerPrefs.SetInt(KEY_LEVELS_LOCKED, _levelsLocked);
        PlayerPrefs.SetInt(KEY_LEVELS_UNLOCKED, _levelsUnlocked);
        PlayerPrefs.Save();
    }

    // Getters
    public float GetPlaytimeSeconds() => _totalSeconds + _sessionSeconds; // total
    public string GetPlaytimeHMS()
    {
        var total = TimeSpan.FromSeconds(GetPlaytimeSeconds());
        return $"{(int)total.TotalHours:D2}:{total.Minutes:D2}:{total.Seconds:D2}";
    }
    public int GetCoins() => _coins;
    public int GetLevelsLocked() => _levelsLocked;
    public int GetLevelsUnlocked() => _levelsUnlocked;

    // Setters - Utilizar estos Setters desde otros scripts con "StatsReader.Instance.X"
    public void SetCoins(int value) { _coins = Mathf.Max(0, value); SaveToPrefs(); }
    public void AddCoins(int delta) { SetCoins(_coins + delta); }
    public void SetLevelsLocked(int value) { _levelsLocked = Mathf.Max(0, value); SaveToPrefs(); }
    public void SetLevelsUnlocked(int v) { _levelsUnlocked = Mathf.Max(0, v); SaveToPrefs(); }
}

