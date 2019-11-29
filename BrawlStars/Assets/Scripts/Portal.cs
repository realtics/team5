using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    public int targetIndex;
	[HideInInspector]
	public MapSpawner mapSpawner;
	[HideInInspector]
	public Character player;

	bool isWaitingCollision;

    // Start is called before the first frame update
    void Start()
    {
		isWaitingCollision = false;
	}

    // Update is called once per frame
    void Update()
	{
		isWaitingCollision = true;
	}

    void OnTriggerEnter(Collider collider)
    {
		if (isWaitingCollision && collider.gameObject == player.gameObject)
		{
			mapSpawner.DestroyItem();
			MapSpawner.mapIndex = targetIndex;
			mapSpawner.CreateNewMap(MapSpawner.mapIndex);
		}
		else
			isWaitingCollision = true;
    }
}
