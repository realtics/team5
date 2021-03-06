﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    public int targetIndex;
	
	public MapSpawner mapSpawner;
	
	public Character player;

	bool isWaitingCollision = false;

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
		if (player == null)
			return;

		if (isWaitingCollision && collider.gameObject == player.gameObject)
		{
			BattleManager.GetInstance().mainCanvas.GetComponent<InputHandler>().Cancel();
			mapSpawner.CreateNewMap(targetIndex);
		}
		else
			isWaitingCollision = true;
    }
}
