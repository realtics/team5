using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

//[ExecuteInEditMode]
public class MapSpawner : MonoBehaviour
{
    public stage[] stages;

	public string[] stageFileName;
	Dictionary<string, string> stageIndexs;

	public int stageIndex;
    public GameObject navMeshFloor;
    public Character player;

    public GameObject resultUI;
    public Text resultText;

    public float limitTime;

	//Load & Generated
	float mapX;
	float mapY;
	int[,] obstacleMap;
	public GameObject[] NowMap;
	List<Coord> allTileCoords;
	int tileSize = 1;

	public GameObject[] obstaclePrefabs;
	public int startingPointIndex;
	public int portalIndex;
	public int monsterIndex;
	public MonsterSpawner spawnerPrefab;
	public Transform tilePrefabs;
	public Transform navmeshFloor;
	public Transform navmeshMaskPrefabMeshFloor;
	Vector3 maxMapSize;
	int nowIndex = 0;
	int nowMapIndex = 0;

	Transform mapHolder;
	string holderName;

	// Start is called before the first frame update
	void Start()
    {
		resultUI.SetActive(false);
	}

    public void Init(Character player)
	{
		this.player = player;
		stageIndex = GameManager.GetInstance().stageIndex;
		TakeStage();
		CreateNewMap(0);
	}

    private void Update()
    {
        if (NowMap[nowIndex].transform.GetComponent<Map>().IsStageFinished())
        {
            if (NowMap[nowIndex].transform.GetComponent<Map>().portals.Length == 0)
            {
                if (!BattleManager.GetInstance().IsAnyItemOnMap())
                    OnResultUI(StageResult.WIN);
            }
            else
            {
                NowMap[nowIndex].transform.GetComponent<Map>().ActivatePortals();
            }
        }

        //if (currentMap.IsStageFinished())
        //{
        //    if (currentMap.portals.Length == 0)
        //    {
        //        if (!BattleManager.GetInstance().IsAnyItemOnMap())
        //            OnResultUI(StageResult.WIN);
        //    }
        //    else
        //    {
        //        currentMap.ActivatePortals();
        //    }
        //}

        if (player.gameObject.activeSelf == false)
            OnResultUI(StageResult.LOSE);
    }

    public void CreateNewMap(int index)
    {
		nowIndex = index;

		BattleManager.GetInstance().ClearAllItem();
		StopAllCoroutines();

        for (int i = 0; i < NowMap.Length; i++)
        {
            NowMap[i].SetActive(false);
        }

        if (index < NowMap.Length)
        {
            resultUI.SetActive(false);

            if (NowMap != null)
                NowMap[index].SetActive(true);

			Portal[] portals = NowMap[index].transform.GetComponent<Map>().portals;
			for (int i = 0; i < portals.Length; i++) {
				portals[i].gameObject.SetActive(false);
				portals[i].player = player;
				portals[i].mapSpawner = this;
			}

			SetCharacterPosition(index);
        }

        //if (stageIndex < stages.Length && index < stages[stageIndex].maps.Length)
        //{
        //    resultUI.SetActive(false);

        //    if (currentMap != null)
        //        Destroy(currentMap.gameObject);

        //    currentMap = Instantiate(stages[stageIndex].maps[index]);
        //    player.transform.position = currentMap.startingPoint.transform.position;
        //    for (int i = 0; i < currentMap.portals.Length; i++)
        //    {
        //        currentMap.portals[i].mapSpawner = this;
        //        currentMap.portals[i].player = player;
        //    }

        //    SetCharacterPosition();
        //}
    }

    public void OnExitButtonClick(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void OnResultUI(StageResult result)
    {
        resultUI.SetActive(true);
		BattleManager.GetInstance().SetActiveInputHandler(false);

        if (result == StageResult.WIN)
            resultText.text = "승리";
        else if(result == StageResult.LOSE)
            resultText.text = "패배";
    }

    public void SetCharacterPosition(int index)
    {
		Vector3 startingVector = NowMap[index].GetComponent<Map>().startingPoint.transform.position;//currentMap.startingPoint.transform.position;

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
		BattleManager.GetInstance().SetActiveInputHandler(true);
		BattleManager.GetInstance().ClearActorList();

		player.Alive();

		NowMap[nowIndex].transform.GetComponent<Map>().Reset();
	}

	public void TakeStage()
	{
		string folderName = @"Assets\StageMaps\";
		string[] fileName;

		fileName = Directory.GetFiles(folderName, stageIndex + "-" + "*.txt");

		NowMap = new GameObject[fileName.Length];

		for (nowMapIndex = 0; nowMapIndex < fileName.Length; nowMapIndex++)
		{
			using (StreamReader inputFile = new StreamReader(fileName[nowMapIndex]))
			{
				mapY = int.Parse(inputFile.ReadLine());
				mapX = int.Parse(inputFile.ReadLine());

				obstacleMap = new int[(int)mapY, (int)mapX];

				string txtElement;

				for (int y = 0; y < mapY; y++)
				{
					txtElement = inputFile.ReadLine();

					for (int x = 0; x < mapX; x++)
					{
						string[] data = txtElement.Split(new char[] {' '});

						obstacleMap[y, x] = int.Parse(data[x]);
					}
				}

				GeneratedMap();
			}
		}
	}

	public void GeneratedMap()
	{
		allTileCoords = new List<Coord>();

		for (int y = 0; y < mapY; y++)
		{
			for (int x = 0; x < mapX; x++)
			{
				allTileCoords.Add(new Coord(y, x));
			}
		}

		maxMapSize.x = mapX + 1;
		maxMapSize.y = mapY + 1;

		holderName = stageIndex + "-" + nowMapIndex;
		
		mapHolder = new GameObject(holderName).transform;
		mapHolder.parent = transform;

		NowMap[nowMapIndex] = GameObject.Find(holderName);

		Map currentMap = NowMap[nowMapIndex].gameObject.AddComponent<Map>();

		for (int y = 0; y < mapY; y++)
		{
			for (int x = 0; x < mapX; x++)
			{
				Vector3 tilePosition = CoordToPosition(y, x);
				Transform newTile = Instantiate(tilePrefabs, tilePosition, Quaternion.Euler(Vector3.right * 90.0f)) as Transform;
				newTile.localScale = Vector3.one * tileSize;
				newTile.parent = mapHolder;
			}
		}

		List<Portal> portalList = new List<Portal>();
		List<MonsterSpawner> monsterList = new List<MonsterSpawner>();

		for (int y = 0; y < mapY; y++)
		{
			for (int x = 0; x < mapX; x++)
			{
				GameObject newObstacle;
				if (obstacleMap[y, x] >= monsterIndex)
				{
					MonsterSpawner spawner = Instantiate(spawnerPrefab);
					spawner.Init(obstaclePrefabs[obstacleMap[y, x]].GetComponent<Monster>());
					monsterList.Add(spawner);
					newObstacle = spawner.gameObject;
				}
				else if (obstacleMap[y, x] >= portalIndex)
				{
					Portal portal = Instantiate(obstaclePrefabs[portalIndex]).GetComponent<Portal>();
					portal.targetIndex = obstacleMap[y, x] - portalIndex;
					portalList.Add(portal); 
					newObstacle = portal.gameObject;
				}
				else
				{
					newObstacle = Instantiate(obstaclePrefabs[obstacleMap[y, x]]);
					if(obstacleMap[y, x] == startingPointIndex)
						currentMap.startingPoint = newObstacle;
				}

				newObstacle.gameObject.transform.parent = mapHolder;
				Vector3 obstaclePosition = CoordToPosition(y, x);
				obstaclePosition.y = newObstacle.transform.localScale.y * 0.5f;
				newObstacle.transform.position = obstaclePosition;
			}
		}

		currentMap.portals = portalList.ToArray();
		currentMap.monsters = monsterList.ToArray();


		Transform maskLeft = Instantiate(navmeshMaskPrefabMeshFloor, new Vector3(-0.75f, 0, (mapY * 0.5f) - 0.5f), Quaternion.identity) as Transform;
		maskLeft.name = "Left";
		maskLeft.parent = mapHolder;
		maskLeft.localScale = new Vector3((maxMapSize.x - mapX) * 0.5f, 1, mapY) * tileSize;

		Transform maskRight = Instantiate(navmeshMaskPrefabMeshFloor, new Vector3(mapX - 0.25f, 0, (mapY * 0.5f) - 0.5f), Quaternion.identity) as Transform;
		maskRight.name = "Right";
		maskRight.parent = mapHolder;
		maskRight.localScale = new Vector3((maxMapSize.x - mapX) * 0.5f, 1, mapY) * tileSize;

		Transform maskTop = Instantiate(navmeshMaskPrefabMeshFloor, new Vector3((mapX * 0.5f) - 0.5f, 0, mapY - 0.25f), Quaternion.identity) as Transform;
		maskTop.name = "Top";
		maskTop.parent = mapHolder;
		maskTop.localScale = new Vector3(maxMapSize.x, 1, (maxMapSize.y - mapY) * 0.5f) * tileSize;

		Transform maskBottom = Instantiate(navmeshMaskPrefabMeshFloor, new Vector3((mapX * 0.5f) - 0.5f, 0, -0.75f), Quaternion.identity) as Transform;
		maskBottom.name = "Bottom";
		maskBottom.parent = mapHolder;
		maskBottom.localScale = new Vector3(maxMapSize.x, 1, (maxMapSize.y - mapY) * 0.5f) * tileSize;

		navmeshFloor.transform.position = new Vector3(mapX * 0.5f - 0.5f, 0, mapY * 0.5f - 0.5f);
		navmeshFloor.localScale = new Vector3(maxMapSize.x, maxMapSize.y) * tileSize;

		NowMap[nowMapIndex].SetActive(false);
	}

	Vector3 CoordToPosition(int y, int x)
	{
		//큐브와 타일의 위치를 지정
		return new Vector3(x, 0, y) * tileSize;
	}

	[System.Serializable]
	public struct Coord
	{
		public float x;
		public float y;

		public Coord(float _y, float _x)
		{
			x = _x;
			y = _y;
		}

		public static bool operator ==(Coord c1, Coord c2)
		{
			return c1.x == c2.x && c1.y == c2.y;
		}
		public static bool operator !=(Coord c1, Coord c2)
		{
			return !(c1 == c2);
		}
		//아래는 경고문 제거
		public override bool Equals(object obj)
		{
			return true;
		}
		public override int GetHashCode()
		{
			return 0;
		}
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