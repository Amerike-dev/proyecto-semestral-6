
//Usa este script para activar y desactivar un canvas, como para menús o HUDs.

using UnityEngine;

public class CanvasController : MonoBehaviour
{
    public Canvas canvas; 
    public void ActivaCanvas()
    {
        canvas.enabled = true;
    }

    public void DesactiCanvas()
    {
        canvas.enabled = false;
    }
}
