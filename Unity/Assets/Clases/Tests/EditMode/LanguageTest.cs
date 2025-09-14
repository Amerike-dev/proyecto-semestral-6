using NUnit.Framework;
using System.Collections.Generic;

public class LanguageTests
{
    [Test]
    public void TestMainMenuEsp()
    {
        var dict = Language.GetMainMenuText(SupportedLanguage.esp);

        Assert.AreEqual("esp", dict["language"]);
        Assert.AreEqual("Iniciar juego", dict["title"]);

        var buttons = dict["buttons"] as Dictionary<string, string>;
        Assert.AreEqual("Iniciar", buttons["start"]);
        Assert.AreEqual("Salir", buttons["exit"]);
    }

    [Test]
    public void TestMainMenuIng()
    {
        var dict = Language.GetMainMenuText(SupportedLanguage.ing);

        Assert.AreEqual("ing", dict["language"]);
        Assert.AreEqual("Start Game", dict["title"]);

        var buttons = dict["buttons"] as Dictionary<string, string>;
        Assert.AreEqual("Start", buttons["start"]);
        Assert.AreEqual("Exit", buttons["exit"]);
    }
}
