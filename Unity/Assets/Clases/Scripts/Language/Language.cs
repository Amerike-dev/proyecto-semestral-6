//si llegaste aqui busca los scripts LenguageManager y LocalizedText
//para mas informacion
using System.Collections.Generic;

public enum SupportedLanguage
{
    esp,
    ing,
    por
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
                dict["title"] = "Select Level";
                dict["buttons"] = new Dictionary<string, string>
                {
                    { "exit", "Exit" },
                    { "start", "Start" },
                    { "options", "Options" },
                    { "credits", "Credits" },
                    { "addPlayer", "Add Player" },
                    { "pause", "Pause"},
                    { "resume", "Resume" },
                    { "reload", "Reload" }
                };
                break;

            case SupportedLanguage.esp:
            default:
                dict["title"] = "Selecciona Nivel";
                dict["buttons"] = new Dictionary<string, string>
                {
                    { "exit", "Salir" },
                    { "start", "Iniciar" },
                    { "options", "Opciones" },
                    { "credits", "Creditos" },
                    { "addPlayer", "Añadir Jugadores" },
                    { "pause", "Pausa"},
                    { "resume", "Continuar" },
                    { "reload", "Reiniciar" }
                };
                break;
                case SupportedLanguage.por:
                dict["title"] = "Seleciona Nível";
                dict["buttons"] = new Dictionary<string, string>
                {
                    { "exit", "Fechar" },
                    { "start", "Começar" },
                    { "options", "Opições" },
                    { "credits", "Creditos" },
                    { "addPlayer", "Adicionar jogador" },
                    { "pause", "Pausa"},
                    { "resume", "Continuar" },
                    { "reload", "Reiniciar" }
                };
                break;
        }

        return dict;
    }

}

