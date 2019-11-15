using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneChange : MonoBehaviour
{

    public string sceneName;

    void Update()
    {
        //오프닝 화면에서 클릭하면 맵 이동
        if(Input.GetMouseButton(0))
            SceneManager.LoadScene(sceneName);
    }

    //PersonalRoom 씬에서 임시로 Canvas에 넣고 Startbtn에서 버튼 클릭으로 진행함.
    public void SceneChanger(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
