using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelController : MonoBehaviour
{
    public GameObject mapaPrefab;
    public Canvas pantallaCarga;
    public Slider barraCarga;

    void Start()
    {
        StartCoroutine(CargarMapaConProgreso());
    }

    IEnumerator CargarMapaConProgreso()
    {
        pantallaCarga.gameObject.SetActive(true);
        barraCarga.value = 0;

        GameObject mapaTemp = Instantiate(mapaPrefab, Vector3.zero, Quaternion.identity);
        int totalPiezas = mapaTemp.transform.childCount;

        for (int i = 0; i < totalPiezas; i++)
        {
            Transform pieza = mapaTemp.transform.GetChild(i);

            Instantiate(pieza.gameObject, pieza.localPosition, pieza.localRotation);
            barraCarga.value = (float)(i + 1) / totalPiezas;

            yield return new WaitForSeconds(0.05f);
        }

        Destroy(mapaTemp);
        pantallaCarga.gameObject.SetActive(false);
        Debug.Log("Mapa generado con progreso real.");

        yield return new WaitForSeconds(0.2f); 

        LevelTimerManager timer = FindAnyObjectByType<LevelTimerManager>();
        if (timer != null)
        {
            StartCoroutine(timer.StartCountdownExternally());
        }
    }
}
