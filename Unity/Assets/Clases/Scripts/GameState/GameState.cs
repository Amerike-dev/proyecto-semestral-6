using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System;

public class GameState
{
    private static GameState _instance;
    public static GameState Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameState();
                _instance.Initialize();
            }
            return _instance;
        }
    }

    [System.Serializable]
    private class GameData
    {
        public string playerName ;
        public int unlockedLevels = 1;
        public float totalPlayTime = 0.0f;
        public float gameCompletion = 0.0f;
        public Dictionary<string, int> levelStars = new Dictionary<string, int>();
        public Dictionary<string, int> levelScores = new Dictionary<string, int>();
        public List<string> unlockedCharacters = new List<string>();
    }

    private GameData _gameData;
    private string _dataPath;
    private string _logPath;

    private void Initialize()
    {
        _dataPath = Path.Combine(Application.dataPath, "DB/gameState.json");
        _logPath = Path.Combine(Application.dataPath, "DB/gameLog.txt");

        Directory.CreateDirectory(Path.GetDirectoryName(_dataPath));
        Directory.CreateDirectory(Path.GetDirectoryName(_logPath));

        LoadGameState();
    }

    // Para tests
    public void InitializeForTests(string dataPath, string logPath)
    {
        _dataPath = dataPath;
        _logPath = logPath;

        Directory.CreateDirectory(Path.GetDirectoryName(_dataPath));
        Directory.CreateDirectory(Path.GetDirectoryName(_logPath));

        LoadGameState();
    }

    private void LoadGameState()
    {
        if (File.Exists(_dataPath))
        {
            string json = File.ReadAllText(_dataPath);
            _gameData = JsonUtility.FromJson<GameData>(json);
        }
        else
        {
            _gameData = new GameData();
            _gameData.unlockedCharacters.Add("Character1");
            SaveGameState();
            Log("Archivo de guardado creado por primera vez");
        }
    }

    private void SaveGameState()
    {
        string json = JsonUtility.ToJson(_gameData, true);
        File.WriteAllText(_dataPath, json);
    }

    private void Log(string message)
    {
        string timestamp = DateTime.Now.ToString("dd-MM-yyyy:HH:mm:ss");
        string logMessage = $"{timestamp} - {message}\n";
        File.AppendAllText(_logPath, logMessage);
    }

    // =============================
    // Métodos Set (para UI y pruebas)
    // =============================
    public void SetPlayerName(string name)
    {
        _gameData.playerName = name;
        SaveGameState();
    }
    public void SetUnlockedLevels(int levels)
    {
        _gameData.unlockedLevels = levels;
        SaveGameState();
    }

    public void SetTotalPlayTime(float time)
    {
        _gameData.totalPlayTime = time;
        SaveGameState();
    }

    public void SetGameCompletion(float completion)
    {
        _gameData.gameCompletion = completion;
        SaveGameState();
    }

    public void SetLevelStars(Dictionary<string, int> stars)
    {
        _gameData.levelStars = stars;
        SaveGameState();
    }

    public void SetLevelScores(Dictionary<string, int> scores)
    {
        _gameData.levelScores = scores;
        SaveGameState();
    }

    public void SetUnlockedCharacters(List<string> chars)
    {
        _gameData.unlockedCharacters = chars;
        SaveGameState();
    }

    // =============================
    // Métodos nuevos para GameStateDemo
    // =============================
    public void UpdatePlayerName(string name)
    {
        _gameData.playerName = name;
        SaveGameState();
        Log($"Nombre del jugador actualizado a: {name}");
    }
    public void UpdateLevelScore(string level, int score, int stars)
    {
        _gameData.levelScores[level] = score;
        _gameData.levelStars[level] = stars;
        SaveGameState();
        Log($"Nivel {level} actualizado con {score} puntos y {stars} estrellas.");
    }

    public void UpdateTimePlayed(float time)
    {
        _gameData.totalPlayTime += time;
        SaveGameState();
        Log($"Tiempo jugado aumentado en {time} segundos. Total: {_gameData.totalPlayTime}");
    }

    public void UpdateGamePercentage()
    {
        // Ejemplo: calcular % en base a niveles desbloqueados (ajusta a tu lógica real)
        _gameData.gameCompletion = (_gameData.unlockedLevels / 5f) * 100f;
        SaveGameState();
        Log($"Porcentaje de juego actualizado: {_gameData.gameCompletion}%");
    }

    public void UpdateUnlockables(string character)
    {
        if (!_gameData.unlockedCharacters.Contains(character))
        {
            _gameData.unlockedCharacters.Add(character);
            SaveGameState();
            Log($"Personaje desbloqueado: {character}");
        }
    }
    public void ResetGameData()
    {
        _gameData = new GameData();
        _gameData.unlockedCharacters.Add("Character1"); // personaje inicial
        SaveGameState();
        Log("Juego reiniciado manualmente");
    }

    public int GetLevelScore(string level) =>
        _gameData.levelScores.ContainsKey(level) ? _gameData.levelScores[level] : 0;

    public int GetLevelStars(string level) =>
        _gameData.levelStars.ContainsKey(level) ? _gameData.levelStars[level] : 0;

    // =============================
    // Métodos Get (para UI y lógica)
    // =============================
    public string GetPlayerName() => _gameData.playerName;
    public int GetUnlockedLevels() => _gameData.unlockedLevels;
    public float GetTotalPlayTime() => _gameData.totalPlayTime;
    public float GetGameCompletion() => _gameData.gameCompletion;
    public Dictionary<string, int> GetLevelStars() => _gameData.levelStars;
    public Dictionary<string, int> GetLevelScores() => _gameData.levelScores;
    public List<string> GetUnlockedCharacters() => _gameData.unlockedCharacters;
}
