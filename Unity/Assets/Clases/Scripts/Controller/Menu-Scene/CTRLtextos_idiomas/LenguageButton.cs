using UnityEngine;
using UnityEngine.UI;

public class LanguageButton : MonoBehaviour
{
    public SupportedLanguage language;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(ChangeLanguage);
    }

    void ChangeLanguage()
    {
        if (LanguageManager.Instance != null)
            LanguageManager.Instance.SetLanguage(language);
    }
}
