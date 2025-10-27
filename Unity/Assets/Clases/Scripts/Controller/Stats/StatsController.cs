using UnityEngine;
using TMPro;

public class StatsController : MonoBehaviour
{
    [Header("UI")]
    public Canvas statusCanvas;
    public Canvas canvas;
    public TMP_Text hoursText;
    public TMP_Text coinsText;
    public TMP_Text lockedText;
    public TMP_Text unlockedText;

    private void Start()
    {
        if (statusCanvas != null) statusCanvas.enabled = false; // iniciar oculto
    }

    public void OpenStatus()
    {
        if (statusCanvas == null) return;

        // Leer de StatsReader y rellenar textos
        if (StatsReader.Instance != null)
        {
            hoursText.text = StatsReader.Instance.GetPlaytimeHMS();
            coinsText.text = StatsReader.Instance.GetCoins().ToString();
            lockedText.text = StatsReader.Instance.GetLevelsLocked().ToString();
            unlockedText.text = StatsReader.Instance.GetLevelsUnlocked().ToString();
        }
        else
        {
            // Fallback si por alguna razón no está cargado
            hoursText.text = "00:00:00";
            coinsText.text = "0";
            lockedText.text = "0";
            unlockedText.text = "0";
        }

        statusCanvas.enabled = true;
        canvas.enabled = false;
    }

    public void CloseStatus()
    {
        if (statusCanvas == null) return;
        statusCanvas.enabled = false;
        canvas.enabled = true;
    }
}
