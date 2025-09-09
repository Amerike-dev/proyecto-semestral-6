using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.IO;
using System.Collections;

public class GameStateTests
{
    private string testDataPath;
    private string testLogPath;

    [SetUp]
    public void SetUp()
    {
        testDataPath = Path.Combine(Application.dataPath, "DB/testGameState.json");
        testLogPath = Path.Combine(Application.dataPath, "DB/testGameLog.txt");

        if (File.Exists(testDataPath)) File.Delete(testDataPath);
        if (File.Exists(testLogPath)) File.Delete(testLogPath);

        GameState.Instance.InitializeForTests(testDataPath, testLogPath);
    }

    [TearDown]
    public void TearDown()
    {
        if (File.Exists(testDataPath)) File.Delete(testDataPath);
        if (File.Exists(testLogPath)) File.Delete(testLogPath);
    }

    [Test]
    public void GameState_UpdateLevelScore_UpdatesScoreWhenHigher()
    {
        string level = "1";
        int initialScore = 400;
        int higherScore = 800;

        GameState.Instance.UpdateLevelScore(level, initialScore);
        GameState.Instance.UpdateLevelScore(level, higherScore);

        Assert.AreEqual(higherScore, GameState.Instance.GetLevelScore(level));
    }

    [Test]
    public void GameState_UpdateLevelScore_DoesNotUpdateScoreWhenLower()
    {
        string level = "1";
        int initialScore = 800;
        int lowerScore = 400;

        GameState.Instance.UpdateLevelScore(level, initialScore);
        GameState.Instance.UpdateLevelScore(level, lowerScore);

        Assert.AreEqual(initialScore, GameState.Instance.GetLevelScore(level));
    }

    [Test]
    public void GameState_UpdateLevelScore_UpdatesStarsWhenHigher()
    {
        string level = "1";
        int initialStars = 2;
        int higherStars = 3;

        GameState.Instance.UpdateLevelScore(level, 500, initialStars);
        GameState.Instance.UpdateLevelScore(level, 600, higherStars);

        Assert.AreEqual(higherStars, GameState.Instance.GetLevelStars(level));
    }

    [Test]
    public void GameState_UpdateTimePlayed_AddsTimeCorrectly()
    {
        float initialTime = GameState.Instance.GetTotalPlayTime();
        float additionalTime = 120.5f;

        GameState.Instance.UpdateTimePlayed(additionalTime);

        Assert.AreEqual(initialTime + additionalTime, GameState.Instance.GetTotalPlayTime(), 0.01f);
    }

    [Test]
    public void GameState_UpdateGamePercentage_CalculatesCorrectly()
    {
        GameState.Instance.UpdateLevelScore("1", 1000, 3);
        GameState.Instance.UpdateLevelScore("2", 800, 2);

        GameState.Instance.UpdateGamePercentage();
        float expectedPercentage = 19f;
        float actualPercentage = GameState.Instance.GetGameCompletion();

        Assert.GreaterOrEqual(actualPercentage, expectedPercentage - 1f);
        Assert.LessOrEqual(actualPercentage, expectedPercentage + 1f);
    }

    [Test]
    public void GameState_UpdateUnlockables_UnlocksNewLevels()
    {
        int initialUnlocked = GameState.Instance.GetUnlockedLevels();
        GameState.Instance.UpdateLevelScore((initialUnlocked + 1).ToString(), 1000);
        GameState.Instance.UpdateUnlockables();

        Assert.AreEqual(initialUnlocked + 1, GameState.Instance.GetUnlockedLevels());
    }

    [Test]
    public void GameState_UpdateUnlockables_UnlocksNewCharacters()
    {
        string newCharacter = "Character2";
        GameState.Instance.UpdateUnlockables(newCharacter);

        Assert.Contains(newCharacter, GameState.Instance.GetUnlockedCharacters());
    }

    [Test]
    public void GameState_Logging_CreatesLogEntries()
    {
        long initialLogSize = File.Exists(testLogPath) ? new FileInfo(testLogPath).Length : 0;
        GameState.Instance.UpdateLevelScore("1", 500);
        Assert.Greater(new FileInfo(testLogPath).Length, initialLogSize);
    }

    [Test]
    public void GameState_Singleton_ReturnsSameInstance()
    {
        var instance1 = GameState.Instance;
        var instance2 = GameState.Instance;
        Assert.AreSame(instance1, instance2);
    }
}