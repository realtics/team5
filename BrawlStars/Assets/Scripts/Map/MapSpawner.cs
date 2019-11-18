using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSpawner : MonoBehaviour
{
    public GameObject[] maps;
    GameObject currentMap;
    public GameObject navMeshFloor;
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
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
}
