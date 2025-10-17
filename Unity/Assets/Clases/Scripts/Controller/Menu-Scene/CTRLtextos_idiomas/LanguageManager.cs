// Este script es un singleton que controla el idioma global del juego,
// actualiza todos los textos LocalizedText y guarda la preferencia del jugador.

using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class LanguageManager : MonoBehaviour
{
    public static LanguageManager Instance { get; private set; }

    private SupportedLanguage currentLanguage = SupportedLanguage.esp;

    void Awake()
    {
        // Singleton: evita duplicados
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        // Cargar idioma guardado o detectar automáticamente
        if (PlayerPrefs.HasKey("language"))
        {
            string savedLang = PlayerPrefs.GetString("language");
            if (System.Enum.TryParse(savedLang, out SupportedLanguage lang))
            {
                SetLanguage(lang);
            }
        }
        else
        {
            // Detección automática del idioma del sistema (opcional)
            switch (Application.systemLanguage)
            {
                case SystemLanguage.English:
                    SetLanguage(SupportedLanguage.ing);
                    break;
                case SystemLanguage.Portuguese:
                    SetLanguage(SupportedLanguage.por);
                    break;
                default:
                    SetLanguage(SupportedLanguage.esp);
                    break;
            }
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Cuando se carga una nueva escena, actualiza todos los textos automáticamente
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UpdateAllLocalizedTexts();
    }

    // === Métodos públicos para botones ===
    public void SetLanguageEsp() => SetLanguage(SupportedLanguage.esp);
    public void SetLanguageIng() => SetLanguage(SupportedLanguage.ing);
    public void SetLanguagePor() => SetLanguage(SupportedLanguage.por);

    // === Cambiar idioma ===
    public void SetLanguage(SupportedLanguage lang)
    {
        currentLanguage = lang;
        PlayerPrefs.SetString("language", lang.ToString());
        PlayerPrefs.Save();

        Language.SetLanguage(lang); // sincroniza con la clase Language
        UpdateAllLocalizedTexts();
    }

    public SupportedLanguage GetLanguage() => currentLanguage;

    // === Obtener texto traducido por ID ===
    public string GetText(string id)
    {
        var dict = Language.GetMainMenuText(currentLanguage);

        // Soporta jerarquía como "buttons.start"
        if (id.Contains("."))
        {
            string[] parts = id.Split('.');
            if (parts.Length == 2 && dict.ContainsKey(parts[0]))
            {
                var subDict = dict[parts[0]] as Dictionary<string, string>;
                if (subDict != null && subDict.ContainsKey(parts[1]))
                    return subDict[parts[1]];
            }
        }
        else
        {
            if (dict.ContainsKey(id))
                return dict[id].ToString();
        }

        return $"#{id}"; // fallback si no se encuentra
    }

    // === Actualizar todos los textos localizados de la escena actual ===
    public void UpdateAllLocalizedTexts()
    {
        foreach (var localizedText in FindObjectsByType<LocalizedText>(FindObjectsSortMode.None))
        {
            localizedText.UpdateText();
        }
    }
}
