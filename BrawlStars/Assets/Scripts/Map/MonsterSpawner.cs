using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public GameObject spawnedMonster;
	public bool bOnOff;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		if (bOnOff)
		{
			spawnedMonster = Instantiate(spawnedMonster, transform.position, Quaternion.identity);
			bOnOff = !bOnOff;
		}
	}

	public void clickOn()
	{
		bOnOff = !bOnOff;
	}
}
