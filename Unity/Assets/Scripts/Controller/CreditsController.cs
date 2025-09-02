using UnityEngine;

public class CreditsController : MonoBehaviour
{
    [SerializeField] private string url;

    public void OpenWebPage()
    {
        if (!string.IsNullOrEmpty(url))
        {
            Application.OpenURL(url);
        }
    }
}
      
