using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public GameObject[] portals;
    public GameObject[] monsters;

    bool isAllMonsterDestoyed;

    private void Start()
    {
        isAllMonsterDestoyed = false;
    }

    private void Update()
    {
        CheckMonsterDestroyed();
        ActivatePortals();
    }

    void CheckMonsterDestroyed()
    {
        isAllMonsterDestoyed = true;
        for (int i = 0; i < monsters.Length; i++)
        {
            if (monsters[i] != null)
            {
                isAllMonsterDestoyed = false;
                break;
            }
        }
    }

    void ActivatePortals()
    {
        if (isAllMonsterDestoyed)
        {
            for (int i = 0; i < portals.Length; i++)
            {
                portals[i].SetActive(true);
            }
        }
    }

    public bool IsStageFinished()
    {
        return isAllMonsterDestoyed;
    }
}
