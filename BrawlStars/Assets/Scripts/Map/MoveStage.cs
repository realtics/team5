using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MoveStage : MonoBehaviour
{

    public MapGenerator mapGenerator;
    public GameObject ResultUI;

    //지정된 시간을 지나면 종료
    public float LimitTime;
    
    //지전된 시간을 저장
    float SelectTime;

    //자동 진행 타임
    public float nextTime;

    public bool OnOff = false;

    public Text secondText;

    // Start is called before the first frame update
    void Start()
    {
        ResultUI.SetActive(false);
        
        mapGenerator.mapIndex = 0;
        SelectTime = LimitTime;
    }

    // Update is called once per frame
    void Update()
    {
        //secondText.text = Mathf.Round(nextTime) + " 초 뒤 자동 진행";

        if (OnOff)
        {
            if (LimitTime > 0)
            {
                LimitTime -= Time.deltaTime;
            }
            else
            {
                //결과창 켬
                ResultUI.SetActive(true);

                //자동 진행 타임
                if (nextTime > 0)
                {
                    nextTime -= Time.deltaTime;
                }
                else
                    NextMap();

            }
        }
    }

    public void NextMap()
    {
        if (mapGenerator.mapIndex < mapGenerator.maps.Length - 1)
            mapGenerator.mapIndex++;
        else
            mapGenerator.mapIndex = 0;

        LimitTime = SelectTime;

        //결과창 끄고 다음 인덱스 로딩
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
