using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
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
