using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MapSpawner : MonoBehaviour
{
    public GameObject[] maps;
    public int mapIndex = 0;
    GameObject currentMap;
    public GameObject navMeshFloor;
    public GameObject player;

    public GameObject resultUI;
    public Text resultText;

    public float limitTime;
    public int hp;

    // Start is called before the first frame update
    void Start()
    {
        if (player != null)
            hp = player.GetComponent<Character>().hp;

        resultUI.SetActive(false);

        if (maps.Length > 0)
        {
            currentMap = Instantiate(maps[0]);
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F3))
            player.GetComponent<Character>().TakeDamage(hp);

        if (player == null)
            OnResultUI(false);

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
            }
        }
    }

    public void CreateNewMap(int index)
    {
        if (index < maps.Length)
        {
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
        }
    }

    public void OnExitButtonClick(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void OnResultUI(bool victory)
    {
        resultUI.SetActive(true);

        if (victory)
            resultText.text = "승리";
        else
            resultText.text = "패배";

    }

}
