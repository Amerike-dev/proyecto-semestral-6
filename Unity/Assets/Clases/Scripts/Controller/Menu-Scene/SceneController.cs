using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    // crea un objeto de tipo GameObject que se llame "scene_manager" y agrega este script a �l
    // pon un bot�n en la ui y asignale la funci�n changeSceneByIndex
    // para cambiar de escena, asigna el �ndice de la escena que quieres cargar en el bot�n (main men� = 0, juego = 1, etc.)
    public void ChangeSceneByIndex(int sceneIndex)
    {
        if (sceneIndex >= 0 && sceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(sceneIndex);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
