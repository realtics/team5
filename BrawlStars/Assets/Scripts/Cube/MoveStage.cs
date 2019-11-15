using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveStage : MonoBehaviour
{

    public MapGenerator mapGenerator;
    public GameObject ResultUI;

    public float LimitTime;
    float SelectTime;

    public float nextTime;

    public bool OnOff = false;

    string secondText;

    // Start is called before the first frame update
    void Start()
    {
        //ResultUI = GameObject.Find("ResultUI");
        ResultUI.SetActive(false);
        
        mapGenerator.mapIndex = 0;
        SelectTime = LimitTime;

        nextTime = LimitTime;
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
                ResultUI.SetActive(true);

                if (nextTime > 0)
                {
                    nextTime -= Time.deltaTime;
                }
                else
                    OnNextBttonClick();

                //if (mapGenerator.mapIndex < mapGenerator.maps.Length - 1)
                //    mapGenerator.mapIndex++;
                //else
                //    mapGenerator.mapIndex = 0;

                //LimitTime = SelectTime;

                //mapGenerator.LoadMap();
            }
        }
    }

    public void OnNextBttonClick()
    {
        if (mapGenerator.mapIndex < mapGenerator.maps.Length - 1)
            mapGenerator.mapIndex++;
        else
            mapGenerator.mapIndex = 0;

        LimitTime = SelectTime;

        nextTime = SelectTime;

        ResultUI.SetActive(false);
        mapGenerator.LoadMap();
    }

    public void OnRestartBttonClick()
    {
        LimitTime = SelectTime;
        ResultUI.SetActive(false);
        mapGenerator.LoadMap();
    }


}
