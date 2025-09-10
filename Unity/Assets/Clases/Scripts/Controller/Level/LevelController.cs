using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelController : MonoBehaviour
{
    public GameObject mapaPrefab;       // Prefab contenedor con piezas como hijos
    public Canvas pantallaCarga;        // Canvas con fondo y barra
    public Slider barraCarga;           // Slider UI que muestra progreso

    void Start()
    {
        StartCoroutine(CargarMapaConProgreso());
    }

    IEnumerator CargarMapaConProgreso()
    {
        pantallaCarga.gameObject.SetActive(true);
        barraCarga.value = 0;

        // Instanciar el contenedor temporalmente
        GameObject mapaTemp = Instantiate(mapaPrefab, Vector3.zero, Quaternion.identity);
        int totalPiezas = mapaTemp.transform.childCount;

        for (int i = 0; i < totalPiezas; i++)
        {
            Transform pieza = mapaTemp.transform.GetChild(i);

            // Instanciar la pieza en su posición local
            Instantiate(pieza.gameObject, pieza.localPosition, pieza.localRotation);

            // Actualizar barra de carga
            barraCarga.value = (float)(i + 1) / totalPiezas;

            yield return new WaitForSeconds(0.05f); // Simula carga progresiva
        }

        Destroy(mapaTemp); // Eliminar el contenedor original

        pantallaCarga.gameObject.SetActive(false);
        Debug.Log("Mapa generado con progreso real.");
    }
}
