using UnityEngine;
using TMPro;

public class InputPlayerName : MonoBehaviour
{
    public TMP_InputField nameInputField;
    public TextMeshProUGUI displayText;
    public void SavePlayerName()
    {
        string playerName = nameInputField.text.Trim();

        if (string.IsNullOrEmpty(playerName))
        {
            Debug.LogWarning("El nombre del jugador está vacío.");
            return;
        }

        GameState.Instance.UpdatePlayerName(playerName);
        Debug.Log($"Nombre del jugador guardado: {playerName}");
        displayText.text = $"{playerName}";
    }

    private void Start()
    {
        string savedName = GameState.Instance.GetPlayerName();
        if (!string.IsNullOrEmpty(savedName))
        {
            nameInputField.text = savedName;
            displayText.text = $"{savedName}";
        }
    }
}
