using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public enum NavigationAxis { Vertical, Horizontal }

public class UIController : MonoBehaviour
{
    public NavigationAxis navigationAxis = NavigationAxis.Vertical;
    private List<UIButton> buttons = new List<UIButton>();
    private int currentIndex = 0;

    void Start()
    {
        buttons = FindObjectsByType<UIButton>(FindObjectsSortMode.None).ToList();

        if (navigationAxis == NavigationAxis.Vertical)
            buttons = buttons.OrderByDescending(b => b.transform.position.y).ToList();
        else
            buttons = buttons.OrderBy(b => b.transform.position.x).ToList();

        HighlightCurrent();
    }

    public void MoveSelection(int direction)
    {
        buttons[currentIndex].SetHighlighted(false);
        currentIndex = Mathf.Clamp(currentIndex + direction, 0, buttons.Count - 1);
        HighlightCurrent();
    }

    public void SelectCurrent()
    {
        buttons[currentIndex].Activate();
    }

    private void HighlightCurrent()
    {
        buttons[currentIndex].SetHighlighted(true);
    }
}
