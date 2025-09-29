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

    // M�todos p�blicos para actualizar el estado del juego
    public void UpdateLevelScore(string level, int score, int stars = 0)
    {
        string levelKey = $"level{level}";
        
        if (!_gameData.levelScores.ContainsKey(levelKey) || _gameData.levelScores[levelKey] < score)
        {
            _gameData.levelScores[levelKey] = score;
            Log($"Se actualiza el score del nivel {level} a {score}");
        }

        if (stars > 0 && (!_gameData.levelStars.ContainsKey(levelKey) || _gameData.levelStars[levelKey] < stars))
        {
            _gameData.levelStars[levelKey] = stars;
            Log($"Se actualizaron las estrellas del nivel {level} a {stars}");
        }

        SaveGameState();
    }

    public void UpdateTimePlayed(float additionalTime)
    {
        _gameData.totalPlayTime += additionalTime;
        Log($"Se agregaron {additionalTime} segundos al tiempo jugado. Total: {_gameData.totalPlayTime}");
        SaveGameState();
    }

    public void UpdateGamePercentage()
    {
        int totalLevels = 10;

        int completedLevels = 0;
        foreach (var levelScore in _gameData.levelScores)
        {
            if (levelScore.Value > 0) completedLevels++;
        }

        float levelCompletion = (completedLevels / (float)totalLevels) * 0.7f;

        float starsCompletion = 0f;
        if (_gameData.levelStars.Count > 0)
        {
            int totalStars = 0;
            int maxPossibleStars = totalLevels * 3;

            foreach (var stars in _gameData.levelStars.Values)
            {
                totalStars += stars;
            }

            starsCompletion = (totalStars / (float)maxPossibleStars) * 0.3f;
        }

        _gameData.gameCompletion = (levelCompletion + starsCompletion) * 100f;
        Log($"Porcentaje de juego actualizado a: {_gameData.gameCompletion}%");
        SaveGameState();
    }

    public void UpdateUnlockables(string characterName = null)
    {
        if (characterName != null && !_gameData.unlockedCharacters.Contains(characterName))
        {
            _gameData.unlockedCharacters.Add(characterName);
            Log($"Personaje desbloqueado: {characterName}");
        }
        
        int maxUnlocked = 1;
        foreach (var level in _gameData.levelScores.Keys)
        {
            int levelNum = int.Parse(level.Replace("level", ""));
            if (levelNum > maxUnlocked && _gameData.levelScores[level] > 0)
            {
                maxUnlocked = levelNum;
            }
        }
        
        if (maxUnlocked > _gameData.unlockedLevels)
        {
            _gameData.unlockedLevels = maxUnlocked;
            Log($"Nivel {maxUnlocked} desbloqueado");
        }
        
        SaveGameState();
    }

    // Metodos para obtener datos
    public int GetUnlockedLevels() => _gameData.unlockedLevels;
    public float GetTotalPlayTime() => _gameData.totalPlayTime;
    public float GetGameCompletion() => _gameData.gameCompletion;
    public int GetLevelScore(string level) => 
        _gameData.levelScores.ContainsKey($"level{level}") ? _gameData.levelScores[$"level{level}"] : 0;
    public int GetLevelStars(string level) => 
        _gameData.levelStars.ContainsKey($"level{level}") ? _gameData.levelStars[$"level{level}"] : 0;
    public List<string> GetUnlockedCharacters() => _gameData.unlockedCharacters;
}