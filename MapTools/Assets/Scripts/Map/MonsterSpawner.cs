using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
	public Monster monsterPrefab;
	Monster monster;

	public void Start()
	{
		if(monster == null)
			monster = Instantiate(monsterPrefab, transform.position, Quaternion.identity);
	}

	public void ResetState()
	{
		monster.Alive();
		monster.transform.position = transform.position;
	}

	public bool IsMonsterDestroyed()
	{
		return !(monster != null && monster.gameObject.activeSelf);
	}
}
