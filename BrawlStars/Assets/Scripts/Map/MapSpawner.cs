using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MapSpawner : MonoBehaviour
{
    public stage[] stages;

    public int stageIndex;
    public Map currentMap;
    public GameObject navMeshFloor;
    public Character player;

    public GameObject resultUI;
    public Text resultText;

    public float limitTime;

    // Start is called before the first frame update
    void Start()
    {
        resultUI.SetActive(false);
	}

    public void Init(Character player)
	{
		this.player = player;
		stageIndex = GameManager.GetInstance().stageIndex;
		CreateNewMap(0);
    }

    private void Update()
    {
		if (currentMap.IsStageFinished())
		{
			if (currentMap.portals.Length == 0)
			{
				if (!BattleManager.GetInstance().IsAnyItemOnMap())
					OnResultUI(StageResult.WIN);
			}
			else
			{
				currentMap.ActivatePortals();
			}
		}

        if (player.gameObject.activeSelf == false)
            OnResultUI(StageResult.LOSE);
    }

    public void CreateNewMap(int index)
    {
		BattleManager.GetInstance().ClearAllItem();

		if (stageIndex < stages.Length && index < stages[stageIndex].maps.Length)
		{
			resultUI.SetActive(false);

			if (currentMap != null)
				Destroy(currentMap.gameObject);

			currentMap = Instantiate(stages[stageIndex].maps[index]);
			player.transform.position = currentMap.startingPoint.transform.position;
			for (int i = 0; i < currentMap.portals.Length; i++)
			{
				currentMap.portals[i].mapSpawner = this;
				currentMap.portals[i].player = player;
			}

			SetCharacterPosition();
		}
    }

    public void OnExitButtonClick(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void OnResultUI(StageResult result)
    {
        resultUI.SetActive(true);
		BattleManager.GetInstance().DeActivateInputHandler();

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

    public void ResetState()
    {
		BattleManager.GetInstance().logView.Init();
        player.Alive();        
    }
}

[System.Serializable]
public struct stage
{
	public Map[] maps;
}

public enum StageResult
{
	WIN, LOSE
}