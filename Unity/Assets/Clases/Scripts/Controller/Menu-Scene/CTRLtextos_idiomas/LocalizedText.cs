// Este script lo agregas en el gameobject que tenga el TextMeshProUGUI y lle asignas un ID
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class LocalizedText : MonoBehaviour
{
    [Tooltip("ID del texto en el diccionario de Language, por ejemplo: title, buttons.start, buttons.exit")]
    public string textId;

    private TextMeshProUGUI textMesh;

    void Awake()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
    }

    void Start()
    {
        UpdateText();
    }

    // Llamado por LanguageManager cuando se cambia de idioma
    public void UpdateText()
    {
        if (LanguageManager.Instance != null && !string.IsNullOrEmpty(textId))
        {
            textMesh.text = LanguageManager.Instance.GetText(textId);
        }
        else
        {
            textMesh.text = $"#{textId}"; // fallback si falta algo
        }
    }
}
