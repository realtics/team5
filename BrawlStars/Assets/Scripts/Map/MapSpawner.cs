using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapSpawner : MonoBehaviour
{
    public GameObject[] maps;
    public int mapIndex = 0;
    GameObject currentMap;
    public GameObject navMeshFloor;
    public GameObject player;

    public GameObject resultUI;
    public Text resultText;


    // Start is called before the first frame update
    void Start()
    {
        resultUI.SetActive(false);

        if (maps.Length > 0)
        {
            currentMap = Instantiate(maps[0]);
        }
    }

    private void Update()
    {
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");
        if (monsters.Length == 0)
        {
            GameObject[] portals = GameObject.FindGameObjectsWithTag("Portal");
            for (int i = 0; i < portals.Length; i++)
            {
                portals[i].gameObject.SetActive(true);
            }
        }
    }

    public void CreateNewMap(int index)
    {
        if (index < maps.Length)
        {
            Destroy(currentMap);
            currentMap = Instantiate(maps[index]);
            player.transform.position = new Vector3(0, 0, 0);
        }
    }

    public void NextMap()
    {
        if (mapIndex < maps.Length - 1)
        {
            Destroy(currentMap);
            currentMap = Instantiate(maps[mapIndex++]);
            player.transform.position = new Vector3(0, 0, 0);
        }
    }

    public void RestartMap()
    {
        Destroy(currentMap);
        currentMap = Instantiate(maps[mapIndex]);
        player.transform.position = new Vector3(0, 0, 0);

    }

    public void OnResultUI(bool victory)
    {
        resultUI.SetActive(true);

        if(victory)
            resultText.text = "패배";


    }

}
