using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBox : MonoBehaviour
{
    public int hp;
    public int maxHp;
    public bool Crash = false;

    public MapGenerator mapGenerator;

    // Start is called before the first frame update
    void Start()
    {
        mapGenerator = GameObject.Find("Map").GetComponent<MapGenerator>();
        hp = maxHp;
        //mapGenerator.mapIndex = 0;
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

                    if (mapGenerator.mapIndex < mapGenerator.maps.Length - 1)
                        mapGenerator.mapIndex++;
                    else
                        mapGenerator.mapIndex = 0;

                    //if (mapGenerator.mapIndex == 1)
                    //    mapGenerator.mapIndex = 0;
                    //else
                    //    mapGenerator.mapIndex = 1;

                    Debug.Log("Destroy!!");

                    mapGenerator.LoadMap();
                }
            }
        }
    }

    public void SetHp(int _hp)
    {
        hp = _hp;
        
    }

    public void TakeDamage(int damage)
    {
        SetHp(hp - damage);

        Debug.Log(hp);

        StartCoroutine(TakeDamageCoroutine());
    }

    IEnumerator TakeDamageCoroutine()
    {
        yield return new WaitForSeconds(0.1f);

        
    }
}
