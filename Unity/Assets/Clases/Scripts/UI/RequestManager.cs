using UnityEngine;
using UnityEngine.UI;

public class RequestManager : MonoBehaviour
{
    public Transform requestPanel;
    public GameObject requestItemPrefab;

    public Sprite[] possibleSprites;
    public string[] possibleNames;
    public float spawnInterval = 5f;

    private float timer;

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            timer = spawnInterval;
            CreateNewRequest();
        }
    }

    void CreateNewRequest()
    {
        GameObject newItem = Instantiate(requestItemPrefab, requestPanel);
        RequestItem item = newItem.GetComponent<RequestItem>();

        int i = Random.Range(0, possibleNames.Length);
        item.Setup(possibleNames[i], possibleSprites[i], Random.Range(6f, 12f));
    }
}
