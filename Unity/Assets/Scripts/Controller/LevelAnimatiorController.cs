using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;

public class LevelAnimatiorController : MonoBehaviour
{
public GameObject startImage;
    public GameObject endImage;
    public float levelDuration = 30f; // tiempo en segundos del nivel
    public TextMeshProUGUI timerText;
    public Animator uiAnimator;

    // Descomentar cuando se tenga el script de movimiento del jugador y cambiar por el nombre correcto
    //public PlayerMovement player; // referencia al script de movimiento

    private float timeRemaining;
    private bool levelActive = false;

    void Start()
    {
        StartCoroutine(StartLevelRoutine());
    }

    IEnumerator StartLevelRoutine()
    {
        // Mostrar animación de inicio
        startImage.SetActive(true);
        yield return new WaitForSeconds(2f);
        startImage.SetActive(false);

        // Iniciar contador
        timeRemaining = levelDuration;
        levelActive = true;
    }

    void Update()
    {
        if (levelActive)
        {
            timeRemaining -= Time.deltaTime;
            if (timerText != null)
                timerText.text = Mathf.Ceil(timeRemaining).ToString();

            if (timeRemaining <= 0)
            {
                levelActive = false;
                EndLevel();
            }
        }
    }

    void EndLevel()
    {
        endImage.SetActive(true);
        uiAnimator.SetTrigger("ShowEnd");

        // Quitar comentario cuando se tenga la escena de menú principal
        //StartCoroutine(LoadSceneAfterAnim());

        // Bloquear movimiento (Quitar comentario cuando se tenga el script de movimiento)
        //if (player != null)
        //    player.enabled = false;
    }

    IEnumerator LoadSceneAfterAnim()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("MainMenu");
    }
}
