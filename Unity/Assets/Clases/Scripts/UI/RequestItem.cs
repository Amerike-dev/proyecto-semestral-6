using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RequestItem : MonoBehaviour
{
    [Header("UI References")]
    public Image iconImage;
    public TMP_Text nameText;
    public Image timerBar;

    [Header("Settings")]
    public float lifetime = 10f; // Duración total del request

    private float timeRemaining;

    void OnEnable()
    {
        timeRemaining = lifetime;
        UpdateTimerBar();
    }

    void Update()
    {
        timeRemaining -= Time.deltaTime;
        if (timeRemaining <= 0f)
        {
            Destroy(gameObject);
            return;
        }

        UpdateTimerBar();
    }

    private void UpdateTimerBar()
    {
        float fillAmount = Mathf.Clamp01(timeRemaining / lifetime);
        if (timerBar != null)
            timerBar.fillAmount = fillAmount;
    }

    
    public void Setup(string itemName, Sprite itemSprite, float duration)
    {
        if (nameText != null)
            nameText.text = itemName;

        if (iconImage != null)
            iconImage.sprite = itemSprite;

        lifetime = duration;
        timeRemaining = duration;
    }
}
