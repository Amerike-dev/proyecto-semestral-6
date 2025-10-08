using UnityEngine;
using UnityEngine.UI;

public class UIButton : MonoBehaviour
{
    public Image highlightImage;

    public void SetHighlighted(bool isHighlighted)
    {
        if (highlightImage != null)
            highlightImage.enabled = isHighlighted;
    }

    public void Activate()
    {
        GetComponent<Button>().onClick.Invoke();
    }
}
