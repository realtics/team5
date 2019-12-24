using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBox : MonoBehaviour
{
    public int hp;
    public int maxHp;
    public bool Crash = false;

    public MapGenerator mapGenerator;

    //큐브 부서지면 이동하는지에 대한 실험.
    // Start is called before the first frame update
    void Start()
    {
        //mapGenerator = GameObject.Find("Map").GetComponent<MapGenerator>();
        hp = maxHp;
    }

    // Update is called once per frame
    void Update()
    {
        if (Crash)
        {
            if (Input.GetMouseButtonUp(0))
            {
                if (hp > 1)
                {
                    hp -= 1;
                    Debug.Log(hp);
                }
                else
                {
                    Destroy(gameObject);

                    //if (mapGenerator.mapIndex < mapGenerator.maps.Length - 1)
                    //    mapGenerator.mapIndex++;
                    //else
                    //    mapGenerator.mapIndex = 0;

                    Debug.Log("Destroy!!");

                    //mapGenerator.LoadMap();
                }
            }
        }
    }
    
    public void TakeDamage(int damage)
    {
        hp -= damage;

        Debug.Log(hp);
    }
}
