using UnityEngine;
using UnityEngine.UI;

public class GameStateDemo : MonoBehaviour
{
    public Text outputText;
    public Button updateScoreButton;
    public Button updateTimeButton;
    public Button updatePercentageButton;
    public Button updateUnlockablesButton;

    void Start()
    {
        updateScoreButton.onClick.AddListener(TestUpdateLevelScore);
        updateTimeButton.onClick.AddListener(TestUpdateTimePlayed);
        updatePercentageButton.onClick.AddListener(TestUpdateGamePercentage);
        updateUnlockablesButton.onClick.AddListener(TestUpdateUnlockables);

        UpdateOutputText();
    }

    void TestUpdateLevelScore()
    {
        int randomScore = Random.Range(100, 1000);
        int randomStars = Random.Range(1, 4);
        string randomLevel = Random.Range(1, 6).ToString();

        GameState.Instance.UpdateLevelScore(randomLevel, randomScore, randomStars);
        UpdateOutputText();
    }

    void TestUpdateTimePlayed()
    {
        float randomTime = Random.Range(60, 300);
        GameState.Instance.UpdateTimePlayed(randomTime);
        UpdateOutputText();
    }

    void TestUpdateGamePercentage()
    {
        GameState.Instance.UpdateGamePercentage();
        UpdateOutputText();
    }

    void TestUpdateUnlockables()
    {
        string[] characters = { "Character2", "Character3", "Character4", "Character5" };
        string randomCharacter = characters[Random.Range(0, characters.Length)];

        GameState.Instance.UpdateUnlockables(randomCharacter);
        UpdateOutputText();
    }

    void UpdateOutputText()
    {
        outputText.text = $"Estado del Juego:\n" +
                         $"Niveles Desbloqueados: {GameState.Instance.GetUnlockedLevels()}\n" +
                         $"Tiempo Jugado: {GameState.Instance.GetTotalPlayTime()} segundos\n" +
                         $"Porcentaje Completado: {GameState.Instance.GetGameCompletion()}%\n" +
                         $"Personajes Desbloqueados: {string.Join(", ", GameState.Instance.GetUnlockedCharacters())}\n\n" +
                         $"Puntuaciones por Nivel:\n";

        for (int i = 1; i <= 5; i++)
        {
            outputText.text += $"Nivel {i}: {GameState.Instance.GetLevelScore(i.ToString())} pts, " +
                              $"{GameState.Instance.GetLevelStars(i.ToString())} estrellas\n";
        }
    }
}