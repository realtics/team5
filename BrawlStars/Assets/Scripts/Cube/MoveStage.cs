using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveStage : MonoBehaviour
{

    public MapGenerator mapGenerator;

    public float LimitTime;
    float SelectTime;
    public bool OnOff = false;

    // Start is called before the first frame update
    void Start()
    {
        mapGenerator.mapIndex = 0;
        SelectTime = LimitTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (OnOff)
        {
            if (LimitTime > 0)
            {
                Mathf.Round(LimitTime);
                LimitTime -= Time.deltaTime;

            }
            else
            {
                if (mapGenerator.mapIndex < mapGenerator.maps.Length - 1)
                    mapGenerator.mapIndex++;
                else
                    mapGenerator.mapIndex = 0;

                //if (mapGenerator.mapIndex == 1)
                //    mapGenerator.mapIndex = 0;
                //else
                //    mapGenerator.mapIndex = 1;

                LimitTime = SelectTime;

                mapGenerator.LoadMap();
            }
        }
    }
}
