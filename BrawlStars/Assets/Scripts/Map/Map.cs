using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public Portal[] portals;
    public GameObject[] monsters;
    public GameObject[] items;
    public GameObject startingPoint;

    public bool isAllMonsterDestoyed;

    private void Start()
    {
        isAllMonsterDestoyed = false;
    }

    private void Update()
    {
        CheckMonsterDestroyed();
	}

    void CheckMonsterDestroyed()
    {
        isAllMonsterDestoyed = true;
        for (int i = 0; i < monsters.Length; i++)
        {
            if (monsters[i].activeSelf == true)
            {
                isAllMonsterDestoyed = false;
                break;
            }
        }
    }

    public void ActivatePortals()
    {
		for (int i = 0; i < portals.Length; i++)
		{
			portals[i].gameObject.SetActive(true);
		}
    }

    public bool IsStageFinished()
    {
        return isAllMonsterDestoyed;
    }
}
