using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    // crea un objeto de tipo GameObject que se llame "scene_manager" y agrega este script a él
    // pon un botón en la ui y asignale la función changeSceneByIndex
    // para cambiar de escena, asigna el índice de la escena que quieres cargar en el botón (main menú = 0, juego = 1, etc.)
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
