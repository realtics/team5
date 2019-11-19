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
<<<<<<< HEAD
        if (player != null)
            hp = player.GetComponent<Character>().hp;

=======
>>>>>>> master
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

<<<<<<< HEAD
        if (mapIndex == maps.Length - 1)
            if (limitTime > 0)
                limitTime -= Time.deltaTime;
            else
            {
                OnResultUI(true);
            }

        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");
        
        if (monsters.Length == 0)
        {
            GameObject[] portals = GameObject.FindGameObjectsWithTag("Portal");
            
            for (int i = 0; i < portals.Length; i++)
            {
                portals[i].gameObject.SetActive(true);
=======
        if(currentMap.IsStageFinished())
        {
            if(currentMap.portals.Length == 0)
            {
                OnResultUI(StageResult.WIN);
>>>>>>> master
            }
        }
    }

    public void CreateNewMap(int index)
    {
        if (index < maps.Length)
        {
<<<<<<< HEAD
            Destroy(currentMap);
            currentMap = Instantiate(maps[index]);

            //startingPoint.transform.position = mapObject.startingPoint.transform.position GetComponent<Map>;

            //if (startingPoint != null)
            //{
            player.transform.position = GameObject.Find("StartingObject").transform.position;//startingPoint.transform.position;
            //}
            //else
            //{
            //player.transform.position = new Vector3(0, 0, 0);
            //}
=======
            resultUI.SetActive(false);

            Destroy(currentMap.gameObject);
            currentMap = Instantiate(maps[index].gameObject).GetComponent<Map>();
            player.transform.position = new Vector3(0, 0, 0);
>>>>>>> master
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
