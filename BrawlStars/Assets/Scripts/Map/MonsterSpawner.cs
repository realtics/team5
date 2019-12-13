using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
	public enum SpawnTypes
	{ 
		Normal, //그냥 생성
		Once,   //totalEmemy를 만족할때까지 계속 생성
		Wave,   //한방에 생성 0이면 다시 반복
		TimeWave//일정시간 동안 반복.
	}

	public enum EnemyStates
	{ 
		Monster1,
		Monster2,
		Monster3,
		Boss
	}

	public EnemyStates EnemyState = EnemyStates.Monster1;

	public GameObject Monster1;
	public GameObject Monster2;
	public GameObject Monster3;
	public GameObject BossEnemy;

	public int totalEnemy = 10;
	private int numEnemy = 0;
	private int spawndEnemy = 0;

	private bool waveSpawn = false;
	public bool Spawn = true;
	public SpawnTypes spawnType = SpawnTypes.Normal;

	public float waveTimer = 30.0f;
	private float timeWave = 0.0f;

	public int totalWave = 5;
	private int numWave = 0;


	// Start is called before the first frame update
	void Start()
    {
		
	}

	// Update is called once per frame
	void Update()
    {
		if (Spawn)
		{
			if (spawnType == SpawnTypes.Normal)
			{
				if (numEnemy < totalEnemy)
				{
					spawnEnemy();
				}
			}
			else if (spawnType == SpawnTypes.Once)
			{
				if (spawndEnemy >= totalEnemy)
				{
					Spawn = false;
				}
				else
				{
					spawnEnemy();
				}
			}
			else if (spawnType == SpawnTypes.Wave)
			{
				if (numWave < totalWave + 1)
				{
					if (waveSpawn)
					{
						spawnEnemy();
					}
					if (numEnemy == 0)
					{
						waveSpawn = true;
						numWave++;
					}
					if (numEnemy == totalEnemy)
					{
						waveSpawn = false;
					}
				}
			}
			else if (spawnType == SpawnTypes.TimeWave)
			{
				if (numWave <= totalWave)
				{
					timeWave += Time.deltaTime;

					if (waveSpawn)
					{
						spawnEnemy();
					}
					if (timeWave >= waveTimer)
					{
						waveSpawn = true;
						timeWave = 0.0f;
						numWave++;
						numEnemy = 0;
					}
					if (numEnemy >= totalEnemy)
					{
						waveSpawn = false;
					}
				}
				else
				{
					Spawn = false;
				}
			}
		}
	}

	private void spawnEnemy()
	{
		if (EnemyState == EnemyStates.Monster1)
		{
			if (Monster1 != null)
			{
				GameObject Enemy = (GameObject)Instantiate(Monster1, gameObject.transform.position, Quaternion.identity);
			}
			else
			{
				Debug.Log("Error: EasyEnemy의 프리팹을 로드할 수 없습니다.");
			}

		}
		else if (EnemyState == EnemyStates.Monster2)
		{
			if (Monster2 != null)
			{
				GameObject Enemy = (GameObject)Instantiate(Monster2, gameObject.transform.position, Quaternion.identity);
			}
			else
			{
				Debug.Log("Error: MediumEnemy의 프리팹을 로드할 수 없습니다.");
			}

		}
		else if (EnemyState == EnemyStates.Monster3)
		{
			if (Monster3 != null)
			{
				GameObject Enemy = (GameObject)Instantiate(Monster3, gameObject.transform.position, Quaternion.identity);
			}
			else
			{
				Debug.Log("Error: HardEnemy의 프리팹을 로드할 수 없습니다.");
			}

		}
		else if (EnemyState == EnemyStates.Boss)
		{
			if (BossEnemy != null)
			{
				GameObject Enemy = (GameObject)Instantiate(BossEnemy, gameObject.transform.position, Quaternion.identity);
			}
			else
			{
				Debug.Log("Error: BossEnemy의 프리팹을 로드할 수 없습니다.");
			}
		}

		numEnemy++;
		spawndEnemy++;
	}

	

}
