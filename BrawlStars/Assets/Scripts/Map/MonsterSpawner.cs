using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public Monster spawnedMonster;

    // Start is called before the first frame update
    void Start()
    {
        spawnedMonster = Instantiate(spawnedMonster, transform.position, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
