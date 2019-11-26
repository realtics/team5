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
    public stage[] stages;

    public static int stageIndex = 0;
    public static int mapIndex;
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

        if (stages.Length > 0)
        {
            if (stages[stageIndex].maps.Length > 0)
            {
                currentMap = Instantiate(stages[stageIndex].maps[mapIndex].gameObject).GetComponent<Map>();
            }
        }

        SetCharacterPosition();

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
                mapIndex = 0;
            }
        }
    }

    public void CreateNewMap(int index)
    {
        DestroyItem();

        if (player == null)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else
        {
            if (stageIndex < stages.Length)
            {
                if (index < stages[stageIndex].maps.Length)
                {
                    resultUI.SetActive(false);

                    Destroy(currentMap.gameObject);
                    currentMap = Instantiate(stages[stageIndex].maps[index].gameObject).GetComponent<Map>();

                    SetCharacterPosition();
                }
            }
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

    public void SetCharacterPosition()
    {
        Vector3 startingVector = currentMap.startingPoint.transform.position;

        if (startingVector == null)
        {
            player.transform.position = new Vector3(0f, 0.5f, 0f);
        }
        else
        {
            player.transform.position = startingVector;
        }
    }

    public void DestroyItem()
    {
        GameObject[] DropItems = GameObject.FindGameObjectsWithTag("Item");

        foreach (GameObject DeleteItem in DropItems)
        {
            Destroy(DeleteItem);
        }
    }

    [System.Serializable]
    public class stage
    {
        public Map[] maps;
    }

}
