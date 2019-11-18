using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MoveStage : MonoBehaviour
{

    public MapGenerator mapGenerator;
    public GameObject ResultUI;
    public GameObject NextButton;

    public float LimitTime;
    
    float SelectTime;

    public float nextTime;

    public bool OnOff = false;

    public Text resultText;

    public GameObject character;
    

    int CharacterHp;
    

    // Start is called before the first frame update
    void Start()
    {
        ResultUI.SetActive(false);
        
        mapGenerator.mapIndex = 0;
        SelectTime = LimitTime;

        CharacterHp = character.GetComponent<Character>().hp;
    }

    // Update is called once per frame
    void Update()
    {
        //승리 패배 변환
        if (Input.GetKey(KeyCode.F3))
        {
            CharacterHp = 0;
            character.GetComponent<Character>().hp = CharacterHp;

            Debug.Log(character.GetComponent<Character>().hp);
        }

        if (character != null)
            CharacterHp = character.GetComponent<Character>().hp;
        

        if ((CharacterHp <= 0 && character != null) && OnOff == false)
        {
            OnOff = true;
            Debug.Log(OnOff);
        }

        if (OnOff)
        {
            if (LimitTime > 0)
            {
                LimitTime -= Time.deltaTime;
            }
            else
            {
                //체력에 따른 Text출력
                Victory(CharacterHp);
            }
        }
    }

    public void NextMap()
    {
        if (mapGenerator.mapIndex < mapGenerator.maps.Length - 1)
            mapGenerator.mapIndex++;
        else
            mapGenerator.mapIndex = 0;

        //맵의 인덱스가 마지막 인덱스이면 다음 버튼을 비활성화
        if (mapGenerator.mapIndex == mapGenerator.maps.Length - 1)
            NextButton.SetActive(false);
        else
            NextButton.SetActive(true);

        OnOff = false;

        LimitTime = SelectTime;

        ResultUI.SetActive(false);
        mapGenerator.LoadMap();
    }

    public void OnRestartButtonClick()
    {
        OnOff = false;

        LimitTime = SelectTime;
        ResultUI.SetActive(false);
        mapGenerator.LoadMap();
    }

    public void OnExitButtonClick(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    void Victory(int hp)
    {
        if (hp <= 0)
        {
            resultText.text = "패배";
            NextButton.SetActive(false);
        }
        else
        {
            resultText.text = "승리";
        }

        //결과창 켬
        ResultUI.SetActive(true);
    }

}
