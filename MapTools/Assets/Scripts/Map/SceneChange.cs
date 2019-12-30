using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneChange : MonoBehaviour
{
    public string sceneName;

    void Update()
    {
        if(Input.GetMouseButton(0) && sceneName != "")
            LoadingManager.LoadScene(sceneName);
    }

    public void SceneChanger(string sceneName)
    {
        LoadingManager.LoadScene(sceneName);
    }
}
