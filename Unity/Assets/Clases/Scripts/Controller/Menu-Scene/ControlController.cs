using UnityEngine;
using UnityEngine.InputSystem;

public class ControlController : MonoBehaviour
{
    public UIController uiController;
    private float lastMoveTime;
    private float moveCooldown = 0.2f;

    public void OnNavigate(InputAction.CallbackContext context)
    {
        Vector2 moveInput = context.ReadValue<Vector2>();
        if (Time.time - lastMoveTime < moveCooldown) return;
        lastMoveTime = Time.time;

        if (uiController.navigationAxis == NavigationAxis.Vertical)
        {
            if (moveInput.y > 0.5f) uiController.MoveSelection(-1);
            else if (moveInput.y < -0.5f) uiController.MoveSelection(1);
        }
        else
        {
            if (moveInput.x > 0.5f) uiController.MoveSelection(1);
            else if (moveInput.x < -0.5f) uiController.MoveSelection(-1);
        }
    }

    public void OnSubmit(InputAction.CallbackContext context)
    {
        if (context.performed)
            uiController.SelectCurrent();
    }
}
