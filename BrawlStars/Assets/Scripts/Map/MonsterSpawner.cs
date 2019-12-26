using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
	public Monster monsterPrefab;
	Monster monster;

	public void Start()
	{
		if(monster != null)
			monster = Instantiate(monsterPrefab, transform.position, Quaternion.identity);
	}

	public void Reset()
	{
		monster.Alive();
	}
}
