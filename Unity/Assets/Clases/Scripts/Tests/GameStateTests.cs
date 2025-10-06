using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameStateTests
{
    string tempDataPath;
    string tempLogPath;

    [SetUp]
    public void Setup()
    {
        string tempDir = Path.Combine(Application.persistentDataPath, "GameStateTests");
        if (Directory.Exists(tempDir))
            Directory.Delete(tempDir, true);
        Directory.CreateDirectory(tempDir);

        tempDataPath = Path.Combine(tempDir, "gameState.json");
        tempLogPath = Path.Combine(tempDir, "gameLog.txt");

        var gs = GameState.Instance;
        gs.InitializeForTests(tempDataPath, tempLogPath);
    }

    [Test]
    public void TestSetAndGetUnlockedLevels()
    {
        var gs = GameState.Instance;
        gs.SetUnlockedLevels(5);

        Assert.AreEqual(5, gs.GetUnlockedLevels());
    }

    [Test]
    public void TestSetAndGetTotalPlayTime()
    {
        var gs = GameState.Instance;
        gs.SetTotalPlayTime(120.5f);

        Assert.AreEqual(120.5f, gs.GetTotalPlayTime(), 0.001f);
    }

    [Test]
    public void TestSetAndGetCompletion()
    {
        var gs = GameState.Instance;
        gs.SetGameCompletion(75.3f);

        Assert.AreEqual(75.3f, gs.GetGameCompletion(), 0.001f);
    }

    [Test]
    public void TestSetAndGetLevelStars()
    {
        var gs = GameState.Instance;
        var stars = new Dictionary<string, int>
        {
            { "level1", 3 },
            { "level2", 2 }
        };
        gs.SetLevelStars(stars);

        var result = gs.GetLevelStars();
        Assert.AreEqual(3, result["level1"]);
        Assert.AreEqual(2, result["level2"]);
    }

    [Test]
    public void TestSetAndGetLevelScores()
    {
        var gs = GameState.Instance;
        var scores = new Dictionary<string, int>
        {
            { "level1", 500 },
            { "level2", 800 }
        };
        gs.SetLevelScores(scores);

        var result = gs.GetLevelScores();
        Assert.AreEqual(500, result["level1"]);
        Assert.AreEqual(800, result["level2"]);
    }

    [Test]
    public void TestSetAndGetUnlockedCharacters()
    {
        var gs = GameState.Instance;
        var chars = new List<string> { "Character1", "Character2" };
        gs.SetUnlockedCharacters(chars);

        var result = gs.GetUnlockedCharacters();
        Assert.Contains("Character1", result);
        Assert.Contains("Character2", result);
    }

    [Test]
    public void TestFileCreatedOnSave()
    {
        var gs = GameState.Instance;
        gs.SetUnlockedLevels(2);

        Assert.IsTrue(File.Exists(tempDataPath), "El archivo JSON no fue creado");
        Assert.IsTrue(File.Exists(tempLogPath), "El archivo de log no fue creado");
    }
}