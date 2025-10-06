//si llegaste aqui busca los scripts LenguageManager y LocalizedText
//para mas informacion
using System.Collections.Generic;

public enum SupportedLanguage
{
    esp,
    ing
}

public static class Language
{
    private static SupportedLanguage currentLanguage = SupportedLanguage.esp;

    public static void SetLanguage(SupportedLanguage lang)
    {
        currentLanguage = lang;
    }

    public static SupportedLanguage GetCurrentLanguage()
    {
        return currentLanguage;
    }

    public static Dictionary<string, object> GetMainMenuText(SupportedLanguage lang)
    {
        var dict = new Dictionary<string, object>();
        dict["language"] = lang.ToString();

        switch (lang)
        {
            case SupportedLanguage.ing:
                dict["title"] = "Start Game";
                dict["buttons"] = new Dictionary<string, string>
                {
                    { "exit", "Exit" },
                    { "start", "Start" },
                    { "options", "Options" },
                    { "credits", "Credits" },
                    { "addPlayer", "Add Player" }
                };
                break;

            case SupportedLanguage.esp:
            default:
                dict["title"] = "Iniciar juego";
                dict["buttons"] = new Dictionary<string, string>
                {
                    { "exit", "Salir" },
                    { "start", "Iniciar" },
                    { "options", "Opciones" },
                    { "credits", "Creditos" },
                    { "addPlayer", "Añadir Jugadores" }
                };
                break;
        }

        return dict;
    }

}

