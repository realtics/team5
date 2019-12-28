using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
	Monster monster;

	public void Init(Monster _monster)
	{
		monster = Instantiate(_monster, transform.position, Quaternion.identity);
		monster.transform.parent = transform;
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
