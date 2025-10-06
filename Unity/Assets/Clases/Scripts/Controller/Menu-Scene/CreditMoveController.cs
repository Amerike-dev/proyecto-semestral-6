using UnityEngine;

public class CreditMoveController : MonoBehaviour
{
    public float scrollSpeed = 50f;
    public int maxHeight;

    void Update()
    {

        if (transform.position.y <= maxHeight) 
        {
        transform.Translate(Vector3.up * scrollSpeed * Time.deltaTime);
        }
    }
}
