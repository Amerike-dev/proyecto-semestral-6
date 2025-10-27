using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelTimerUI : MonoBehaviour
{
    [Header("Referencias UI")]
    public TextMeshProUGUI countdownText;
    public TextMeshProUGUI timerText;

    public void UpdateCountdown(int number)
    {
        countdownText.gameObject.SetActive(true);
        countdownText.text = number.ToString();
        timerText.gameObject.SetActive(false);
    }

    public void ShowGoSignal()
    {
        countdownText.text = "GO!";
    }

    public void UpdateLevelTimer(float time)
    {
        if (!timerText.gameObject.activeSelf)
        {
            countdownText.gameObject.SetActive(false);
            timerText.gameObject.SetActive(true);
        }

        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        timerText.text = $"{minutes:00}:{seconds:00}";
    }
}
