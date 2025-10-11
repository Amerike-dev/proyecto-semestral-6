using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Controla la secuencia de la escena de carga/intro: muestra nombre de grupo y nombre del juego,
/// hace fades y luego notifica a GameManager para continuar el flujo.
/// Diseñado para usarse desde una escena simple creada en el editor (ver README).
/// </summary>
[DisallowMultipleComponent]
public class LoadingSceneController : MonoBehaviour
{
    [Header("Contenido")]
    [Tooltip("Nombre del grupo/desarrollador que se mostrará")]
    public string groupName = "Mi Grupo";
    [Tooltip("Nombre del juego que se mostrará")]
    public string gameName = "Nombre del Juego";

    [Header("Referencias UI")]
    public TMP_Text groupText;
    public TMP_Text gameText;

    // Imagen que actúa como el 'blackout' final (debe estar delante de los textos)
    public Image blackoutOverlay;

    [Header("Timing / animación")]
    public float textFadeIn = 0.8f;
    public float textDelayBetween = 0.5f; // tiempo entre la aparición del grupo y del título
    public float showDuration = 2.0f;
    public float textFadeOut = 0.5f;
    public float blackoutFade = 1.0f;

    [Header("Opciones")]
    public bool autoPlay = true;

    void Start()
    {
        // Seguridad: si faltan referencias, buscamos en la escena por nombres comunes
    if (groupText == null) groupText = GameObject.Find("GroupText")?.GetComponent<TMP_Text>();
    if (gameText == null) gameText = GameObject.Find("GameText")?.GetComponent<TMP_Text>();
        if (blackoutOverlay == null) blackoutOverlay = GameObject.Find("BlackoutOverlay")?.GetComponent<Image>();

        if (groupText == null) Debug.LogWarning("[LoadingSceneController] groupText no asignado.");
        if (gameText == null) Debug.LogWarning("[LoadingSceneController] gameText no asignado.");
        if (blackoutOverlay == null) Debug.LogWarning("[LoadingSceneController] blackoutOverlay no asignado.");

        if (autoPlay) StartCoroutine(PlayIntro());
    }

    IEnumerator PlayIntro()
    {
        // Inicializa UI
        if (groupText != null) { groupText.text = groupName; SetGraphicAlpha(groupText, 0f); }
        if (gameText != null) { gameText.text = gameName; SetGraphicAlpha(gameText, 0f); }
        if (blackoutOverlay != null) { SetGraphicAlpha(blackoutOverlay, 0f); blackoutOverlay.raycastTarget = true; }

        // Fade IN groupText primero
        float t = 0f;
        if (groupText != null) groupText.gameObject.SetActive(true);
        if (gameText != null) gameText.gameObject.SetActive(true); // aseguramos que esté activo (pero transparent)

        while (t < textFadeIn)
        {
            t += Time.deltaTime;
            float a = Mathf.Clamp01(t / textFadeIn);
            if (groupText != null) SetGraphicAlpha(groupText, a);
            yield return null;
        }

        // Forzar opacidad total para evitar que posteriores cambios afecten
        if (groupText != null) SetGraphicAlpha(groupText, 1f);

        // Espera breve antes de mostrar el título
        yield return new WaitForSeconds(textDelayBetween);

        // Cross-fade: mientras gameText aparece, groupText desaparece para evitar superposición
        t = 0f;
        while (t < textFadeIn)
        {
            t += Time.deltaTime;
            float a = Mathf.Clamp01(t / textFadeIn);
            // game text goes 0 -> 1
            if (gameText != null) SetGraphicAlpha(gameText, a);
            // group text goes 1 -> 0
            if (groupText != null) SetGraphicAlpha(groupText, 1f - a);
            yield return null;
        }

        if (gameText != null) SetGraphicAlpha(gameText, 1f);
        // ensure group is fully hidden and disable it to avoid overlap
        if (groupText != null)
        {
            SetGraphicAlpha(groupText, 0f);
            groupText.gameObject.SetActive(false);
        }

        // Mantener visibles
        yield return new WaitForSeconds(showDuration);

        // Fade OUT del gameText antes del blackout para que desaparezca limpiamente
        if (gameText != null)
        {
            float td = 0f;
            while (td < textFadeOut)
            {
                td += Time.deltaTime;
                float a = 1f - Mathf.Clamp01(td / textFadeOut);
                SetGraphicAlpha(gameText, a);
                yield return null;
            }
            SetGraphicAlpha(gameText, 0f);
            gameText.gameObject.SetActive(false);
        }

        // Blackout: fade overlay desde 0 a 1 (se tapará todo)
        t = 0f;
        while (t < blackoutFade)
        {
            t += Time.deltaTime;
            float a = Mathf.Clamp01(t / blackoutFade);
            if (blackoutOverlay != null) SetGraphicAlpha(blackoutOverlay, a);
            yield return null;
        }

        // Pequeña espera para asegurar el blackout
        yield return new WaitForSeconds(0.2f);

        // Continuar con el flujo del juego: usar GameManager si existe
        if (GameManager.Instance != null)
        {
            GameManager.Instance.StartGame();
        }
        else
        {
            Debug.LogWarning("[LoadingSceneController] GameManager no encontrado. Debes cargar la siguiente escena manualmente o inicializar GameManager.");
        }
    }

    void SetGraphicAlpha(Graphic g, float a)
    {
        if (g == null) return;
        Color c = g.color;
        c.a = a;
        g.color = c;
    }
}
