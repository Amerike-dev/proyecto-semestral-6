using UnityEngine;
using UnityEngine.SceneManagement;
public class TimerController : MonoBehaviour
{
    public float startertime;
    public int SceneIndex;

    private float RemainingTime;

    void Start()
    {
        RemainingTime = startertime;
    }

    void Update()
    {
        RemainingTime -= Time.deltaTime;
        if (RemainingTime <= 0)
        {
            SceneManager.LoadScene(SceneIndex);
        }
    }
}
