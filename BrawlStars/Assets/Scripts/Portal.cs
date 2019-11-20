using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    MapSpawner mapSpawner;
    public int targetIndex;
    public Character player;

    // Start is called before the first frame update
    void Start()
    {
        mapSpawner = GameManager.GetInstance().mapSpawner;
        player = GameManager.GetInstance().player;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject == player.gameObject)
        {
            mapSpawner.DestroyItem();

            mapSpawner.CreateNewMap(targetIndex);
        }
    }
}
