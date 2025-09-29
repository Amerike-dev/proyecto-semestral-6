using UnityEngine;

public class GameManagerTester : MonoBehaviour
{
    void Start()
    {
        var gm = GameManager.Instance;

        gm.OnGameStart += () => Debug.Log("OnGameStart instanciado con éxito");
        gm.OnMenuShown += () => Debug.Log("OnMenuShown instanciado con éxito");
        gm.OnLevelStarted += () => Debug.Log("OnLevelStarted instanciado con éxito");
        gm.OnPlayersSelected += () => Debug.Log("OnPlayerSelected instanciado con éxito");
        gm.OnScoreUpdated += (score) => Debug.Log($"Score updated: {score}");
        gm.OnStarAchieved += (stars) => Debug.Log($"Stars achieved: {stars}");


        gm.StartGame();
        gm.ShowMenu();
        gm.SelectPlayers();
        gm.SelectLevel();
        gm.LoadLevel();
        gm.StartLevel(5f);
        gm.AddScore(150); 
        gm.CompleteDesign("design_1");
        gm.EndLevel();
        gm.ShowResults();
        gm.ReturnToMenu();
    }
}

