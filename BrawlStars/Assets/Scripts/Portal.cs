using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    MapSpawner map;
    public int targetIndex;
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        map = GameObject.Find("MapManager").GetComponent<MapSpawner>();
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject == player)
        {
            map.CreateNewMap(targetIndex);
        }
    }
}
