using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBox : MonoBehaviour
{
    public int HP = 10;
    public bool Crash = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Crash)
        {
            if (Input.GetMouseButtonUp(0))
            {
                if (HP > 0)
                {
                    HP -= 1;
                    Debug.Log(HP);
                }
                else
                {
                    Destroy(gameObject);
                    Debug.Log("Destroy!!");
                }
            }
        }
    }
}
