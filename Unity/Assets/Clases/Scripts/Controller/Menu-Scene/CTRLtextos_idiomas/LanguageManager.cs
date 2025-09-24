using UnityEngine;
using TMPro;

public class LanguageManager : MonoBehaviour
{
    public static LanguageManager Instance { get; private set; }

    private SupportedLanguage currentLanguage = SupportedLanguage.esp;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Cambiar idioma (se conecta a botones en el inspector)
    public void SetLanguageEsp()
    {
        SetLanguage(SupportedLanguage.esp);
    }

    public void SetLanguageIng()
    {
        SetLanguage(SupportedLanguage.ing);
    }

    public void SetLanguage(SupportedLanguage lang)
    {
        currentLanguage = lang;
        Language.SetLanguage(lang); // sincroniza con la clase Language
        UpdateAllLocalizedTexts();
    }

    public SupportedLanguage GetLanguage()
    {
        return currentLanguage;
    }

    public string GetText(string id)
    {
        var dict = Language.GetMainMenuText(currentLanguage);

        // Soporta jerarquía como "buttons.start"
        if (id.Contains("."))
        {
            string[] parts = id.Split('.');
            if (parts.Length == 2 && dict.ContainsKey(parts[0]))
            {
                var subDict = dict[parts[0]] as System.Collections.Generic.Dictionary<string, string>;
                if (subDict != null && subDict.ContainsKey(parts[1]))
                {
                    return subDict[parts[1]];
                }
            }
        }
        else
        {
            if (dict.ContainsKey(id))
                return dict[id].ToString();
        }

        return $"#{id}"; // fallback
    }

    private void UpdateAllLocalizedTexts()
    {
        foreach (var localizedText in FindObjectsByType<LocalizedText>(FindObjectsSortMode.None))
        {
            localizedText.UpdateText();
        }
    }
}
