using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameStateUI : MonoBehaviour
{
    [Header("UI Inputs")]
    public TMP_InputField playerName;
    public TMP_InputField unlockedLevelsInput;
    public TMP_InputField playTimeInput;
    public TMP_InputField completionInput;
    public TMP_InputField charactersInput;

    [Header("UI Output")]
    public TextMeshProUGUI playerNameText;
    public TextMeshProUGUI outputText;

    [Header("Bot�n de actualizacion")]
    public Button updateButton;

    void Start()
    {
        if (playerNameText == null)
            playerNameText = GameObject.Find("PlayerNameText")?.GetComponent<TextMeshProUGUI>();
        if (unlockedLevelsInput == null)
            unlockedLevelsInput = GameObject.Find("UnlockedLevelsInput")?.GetComponent<TMP_InputField>();
        if (playTimeInput == null)
            playTimeInput = GameObject.Find("PlayTimeInput")?.GetComponent<TMP_InputField>();
        if (completionInput == null)
            completionInput = GameObject.Find("CompletionInput")?.GetComponent<TMP_InputField>();
        if (charactersInput == null)
            charactersInput = GameObject.Find("CharactersInput")?.GetComponent<TMP_InputField>();

        if (outputText == null)
            outputText = GameObject.Find("OutputText")?.GetComponent<TextMeshProUGUI>();

        if (updateButton == null)
            updateButton = GameObject.Find("UpdateButton")?.GetComponent<Button>();

        if (updateButton != null)
            updateButton.onClick.AddListener(OnUpdateButtonClick);

        RefreshUI();
    }

    public void OnUpdateButtonClick()
    {
        var gs = GameState.Instance;

        if (int.TryParse(unlockedLevelsInput.text, out int unlocked))
        {
            gs.SetUnlockedLevels(unlocked);
        }

        if (float.TryParse(playTimeInput.text, out float playTime))
        {
            gs.SetTotalPlayTime(playTime);
        }

        if (float.TryParse(completionInput.text, out float completion))
        {
            gs.SetGameCompletion(completion);
        }

        if (!string.IsNullOrEmpty(charactersInput.text))
        {
            gs.SetUnlockedCharacters(new List<string> { charactersInput.text });
        }

        RefreshUI();
    }

    void RefreshUI()
    {
        var gs = GameState.Instance;

        if (unlockedLevelsInput != null) unlockedLevelsInput.text = gs.GetUnlockedLevels().ToString();
        if (playTimeInput != null) playTimeInput.text = gs.GetTotalPlayTime().ToString("F2");
        if (completionInput != null) completionInput.text = gs.GetGameCompletion().ToString("F1");
        if (charactersInput != null) charactersInput.text = string.Join(",", gs.GetUnlockedCharacters());

        if (outputText != null)
        {
            outputText.text =
                $"Niveles desbloqueados: {gs.GetUnlockedLevels()}\n" +
                $"Tiempo total jugado: {gs.GetTotalPlayTime():F2}\n" +
                $"Progreso total: {gs.GetGameCompletion():F1}%\n" +
                $"Personajes desbloqueados: {string.Join(",", gs.GetUnlockedCharacters())}";
        }
    }
}