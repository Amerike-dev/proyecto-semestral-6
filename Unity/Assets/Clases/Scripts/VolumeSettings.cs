using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;
public class VolumeSettings : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider MusicSlider;
    [SerializeField] private Slider SFXSlider;
    [SerializeField] private Button MusicToggleButton;
    [SerializeField] private Button SFXToggleButton;
    [SerializeField] private TextMeshProUGUI MusicButtonText;
    [SerializeField] private TextMeshProUGUI SFXButtonText;

    private float lastMusicVolume = 1f;
    private float lastSFXVolume = 1f;
    private bool isMusicOn = true;
    private bool isSFXOn = true;

    private void Start()
    {
        if (PlayerPrefs.HasKey("MusicVolume")) MusicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        if (PlayerPrefs.HasKey("SFXVolume")) SFXSlider.value = PlayerPrefs.GetFloat("SFXVolume");

        if (PlayerPrefs.HasKey("MusicMuted")) isMusicOn = PlayerPrefs.GetInt("MusicMuted") == 0;
        if (PlayerPrefs.HasKey("SFXMuted")) isSFXOn = PlayerPrefs.GetInt("SFXMuted") == 0;

        lastMusicVolume = Mathf.Max(MusicSlider.value, 0.0001f);
        lastSFXVolume = Mathf.Max(SFXSlider.value, 0.0001f);

        UpdateMusicState(false);
        UpdateSFXState(false);

        MusicSlider.onValueChanged.AddListener(delegate { OnMusicSliderChanged(); });
        SFXSlider.onValueChanged.AddListener(delegate { OnSFXSliderChanged(); });

        MusicToggleButton.onClick.AddListener(ToggleMusic);
        SFXToggleButton.onClick.AddListener(ToggleSFX);
    }

    private void ToggleMusic()
    {
        if (isMusicOn)
        {
            // Guardamos el volumen antes de apagar
            lastMusicVolume = Mathf.Max(MusicSlider.value, 0.0001f);
            isMusicOn = false;
        }
        else
        {
            isMusicOn = true;
        }

        UpdateMusicState(true);
    }

    private void ToggleSFX()
    {
        if (isSFXOn)
        {
            lastSFXVolume = Mathf.Max(SFXSlider.value, 0.0001f);
            isSFXOn = false;
        }
        else
        {
            isSFXOn = true;
        }

        UpdateSFXState(true);
    }

    private void OnMusicSliderChanged()
    {
        if (!isMusicOn && MusicSlider.value > 0f)
        {
            isMusicOn = true;
        }

        if (isMusicOn)
        {
            lastMusicVolume = MusicSlider.value;
            audioMixer.SetFloat("Music", Mathf.Log10(MusicSlider.value) * 10);
            PlayerPrefs.SetFloat("MusicVolume", MusicSlider.value);
            PlayerPrefs.SetInt("MusicMuted", 0);
        }

        UpdateMusicButtonText();
    }

    private void OnSFXSliderChanged()
    {
        if (!isSFXOn && SFXSlider.value > 0f)
        {
            isSFXOn = true;
        }

        if (isSFXOn)
        {
            lastSFXVolume = SFXSlider.value;
            audioMixer.SetFloat("SFX", Mathf.Log10(SFXSlider.value) * 10);
            PlayerPrefs.SetFloat("SFXVolume", SFXSlider.value);
            PlayerPrefs.SetInt("SFXMuted", 0);
        }

        UpdateSFXButtonText();
    }

    private void UpdateMusicState(bool save)
    {
        MusicSlider.onValueChanged.RemoveAllListeners(); // evitar doble cambio

        if (isMusicOn)
        {
            MusicSlider.value = lastMusicVolume;
            audioMixer.SetFloat("Music", Mathf.Log10(lastMusicVolume) * 10);
            if (save)
            {
                PlayerPrefs.SetFloat("MusicVolume", lastMusicVolume);
                PlayerPrefs.SetInt("MusicMuted", 0);
            }
        }
        else
        {
            audioMixer.SetFloat("Music", -80f);
            MusicSlider.value = 0f;
            if (save) PlayerPrefs.SetInt("MusicMuted", 1);
        }

        UpdateMusicButtonText();

        // Volvemos a activar el listener
        MusicSlider.onValueChanged.AddListener(delegate { OnMusicSliderChanged(); });
    }

    private void UpdateSFXState(bool save)
    {
        SFXSlider.onValueChanged.RemoveAllListeners();

        if (isSFXOn)
        {
            SFXSlider.value = lastSFXVolume;
            audioMixer.SetFloat("SFX", Mathf.Log10(lastSFXVolume) * 10);
            if (save)
            {
                PlayerPrefs.SetFloat("SFXVolume", lastSFXVolume);
                PlayerPrefs.SetInt("SFXMuted", 0);
            }
        }
        else
        {
            audioMixer.SetFloat("SFX", -80f);
            SFXSlider.value = 0f;
            if (save) PlayerPrefs.SetInt("SFXMuted", 1);
        }

        UpdateSFXButtonText();

        SFXSlider.onValueChanged.AddListener(delegate { OnSFXSliderChanged(); });
    }

    private void UpdateMusicButtonText()
    {
        MusicButtonText.text = isMusicOn ? "On" : "Off";
    }

    private void UpdateSFXButtonText()
    {
        SFXButtonText.text = isSFXOn ? "On" : "Off";
    }
}
