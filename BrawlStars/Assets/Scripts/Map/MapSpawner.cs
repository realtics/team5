using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum StageResult
{
    WIN, LOSE
}

public class MapSpawner : MonoBehaviour
{
    public Map[] maps;
    public int mapIndex = 0;
    Map currentMap;
    public GameObject navMeshFloor;
    public Character player;

    public GameObject resultUI;
    public Text resultText;

    public float limitTime;

    // Start is called before the first frame update
    void Start()
    {
        resultUI.SetActive(false);

        if (maps.Length > 0)
        {
            currentMap = Instantiate(maps[0].gameObject).GetComponent<Map>();
        }
    }

    private void Update()
    {
        if (player == null)
            OnResultUI(StageResult.LOSE);

        if(currentMap.IsStageFinished())
        {
            if(currentMap.portals.Length == 0)
            {
                OnResultUI(StageResult.WIN);
            }
        }
    }

    public void CreateNewMap(int index)
    {
        if (index < maps.Length)
        {
            resultUI.SetActive(false);

            Destroy(currentMap.gameObject);
            currentMap = Instantiate(maps[index].gameObject).GetComponent<Map>();
            player.transform.position = new Vector3(0, 0, 0);
        }
    }

    public void OnExitButtonClick(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void OnResultUI(StageResult result)
    {
        resultUI.SetActive(true);

        if (result == StageResult.WIN)
            resultText.text = "승리";
        else if(result == StageResult.LOSE)
            resultText.text = "패배";
    }

}
