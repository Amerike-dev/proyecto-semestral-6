using UnityEngine;
// Instanciar el objeto Brick desde el ObjectController
public class ObjectController : MonoBehaviour
{
    public Brick brick;

    void Start()
    {
        // Instanciar el objeto Brick en la posición del ObjectController
        Instantiate(brick, transform.position, Quaternion.identity);
    }

}
//Agregar al spawner de ladrillos que cree un objeto Brick, este objeto debe ser manejado desde el ObjectController.