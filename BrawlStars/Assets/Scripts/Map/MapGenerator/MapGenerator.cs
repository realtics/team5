using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class MapGenerator : MonoBehaviour
{
	//맵 툴 UI
	public InputField inputX; //X 크기
	public InputField inputY; //Y 크기

	public InputField inputSaveText;	//SaveText
	public InputField inputLoadText;	//LoadText

	public GameObject saveUI;
	public GameObject loadUI;
	public GameObject infoUI;

	public Text CubeName;


	//맵 만드는 거
	public Map maps;
        
    public GameObject[] obstaclePrefabs;
    
    public Transform tilePrefab;
    public Transform navmeshFloor;
    public Transform navmeshMaskPrefabMeshFloor;
    Vector3 maxMapSize;

	Transform mapHolder;
	string holderName;

    float tileSize = 1;
    List<Coord> allTileCoords;
    
    public int[,] obstacleMap;
	int cubeIndex = 0;
	
	//카메라
	public Camera camera;

	public float xSensitivity = 20.0f;
	public float ySensitivity = 20.0f;

	float yPos = 0.0f;
	float xPos = 0.0f;

	int index = 0;

	void Start()
    {
		saveUI.SetActive(false);
		loadUI.SetActive(false);
		infoUI.SetActive(false);

		camera.transform.position = Camera.main.transform.position;
	}

	void Update()
	{
		CameraControl();
		OnChangeCube();
		NowCube();
	}

	public void SwithcingCube(int cube)
	{
		cubeIndex = cube;
	}

	public void NowCube()
	{
		switch (cubeIndex)
		{
			case 1:
				CubeName.text = "WoodCube";
				break;
			case 2:
				CubeName.text = "Wallcube";
				break;
			case 3:
				CubeName.text = "SnowCube";
				break;
			case 4:
				CubeName.text = "SandCube";
				break;
			case 5:
				CubeName.text = "SandCube2";
				break;
			case 6:
				CubeName.text = "RockCube";
				break;
			case 7:
				CubeName.text = "RockCube";
				break;
			case 8:
				CubeName.text = "MossCube";
				break;
			case 9:
				CubeName.text = "DarkCube";
				break;
			case 19:
				CubeName.text = "StartinPoint";
				break;
			case 20:
				CubeName.text = "Portal";
				break;
			case 31:
				CubeName.text = "Monster1";
				break;
			case 32:
				CubeName.text = "Monster2";
				break;
			case 33:
				CubeName.text = "Monster3";
				break;
			case 34:
				CubeName.text = "Monster4";
				break;
			default:
				CubeName.text = "NullCube";
				break;
		}
		
	}

	void CameraControl()
	{
		if (!EventSystem.current.IsPointerOverGameObject())
		{
			if (Input.GetKeyDown(KeyCode.A))
				camera.transform.position -= new Vector3(1.0f, 0.0f, 0.0f);
			if (Input.GetKeyDown(KeyCode.D))
				camera.transform.position += new Vector3(1.0f, 0.0f, 0.0f);
			if (Input.GetKeyDown(KeyCode.S))
				camera.transform.position -= new Vector3(0.0f, 0.0f, 1.0f);
			if (Input.GetKeyDown(KeyCode.W))
				camera.transform.position += new Vector3(0.0f, 0.0f, 1.0f);
			if (Input.GetKeyDown(KeyCode.Q))
				camera.fieldOfView -= 1.0f;
			if (Input.GetKeyDown(KeyCode.E))
				camera.fieldOfView += 1.0f;

			float distance = Input.GetAxis("Mouse ScrollWheel") * -1 * 10.0f;

			if (distance != 0)
			{
				camera.fieldOfView += distance;
			}

			if (Input.GetMouseButton(2))
			{
				xPos += Input.GetAxis("Mouse X") * xSensitivity * Time.deltaTime;
				yPos += Input.GetAxis("Mouse Y") * ySensitivity * Time.deltaTime;

				camera.transform.position = new Vector3(-xPos, camera.transform.position.y, -yPos);
			}
		}
	}


	public void onOffSaveUI()
	{
		if (saveUI.activeSelf == false)
		{
			saveUI.SetActive(true);
			inputSaveText.text = null;
		}
		else
			saveUI.SetActive(false);
	}

	public void onOffLoadUI()
	{
		if (loadUI.activeSelf == false)
		{
			loadUI.SetActive(true);
			inputLoadText.text = null;
		}
		else
			loadUI.SetActive(false);
	}

	void OnChangeCube()
	{
		if (Input.GetMouseButton(0))
		{
			if (!EventSystem.current.IsPointerOverGameObject())
			{
				RaycastHit hit = new RaycastHit();

				Ray ray = camera.ScreenPointToRay(Input.mousePosition);

				if (Physics.Raycast(ray.origin, ray.direction, out hit))
				{
					if (hit.collider.gameObject.tag == "Obstacle" ||
						hit.collider.gameObject.tag == "Monster" ||
						hit.collider.gameObject.tag == "Portal" || 
						hit.collider.gameObject.tag == "StartingPoint")
					{
						GameObject newObstacle;

						float setYPosition = 0.25f;

						Destroy(hit.collider.gameObject);

						mapHolder = transform.Find("Generated Map Cube").gameObject.transform;

						GameObject starting = GameObject.FindGameObjectWithTag("StartingPoint");

						if (starting != null)
						{
							newObstacle = Instantiate(obstaclePrefabs[0], new Vector3(starting.transform.position.x, setYPosition,
							starting.transform.position.z), Quaternion.identity);

							obstacleMap[(int)starting.transform.position.x,
								(int)starting.transform.position.z] = 0;

							newObstacle.gameObject.transform.parent = mapHolder;

							Destroy(starting);
						}
						

						newObstacle = Instantiate(obstaclePrefabs[cubeIndex], new Vector3(hit.transform.gameObject.transform.position.x, setYPosition,
								hit.transform.gameObject.transform.position.z), Quaternion.identity);

						obstacleMap[(int)hit.transform.gameObject.transform.position.x,
							(int)hit.transform.gameObject.transform.position.z] = cubeIndex;

						newObstacle.gameObject.transform.parent = mapHolder;
					}
				}
			}
		}
	}

	public void ClearNGenerate()
	{
		ClearMap();
		GenerateMap();
	}

	public void GenerateMap()
    {
		if (inputX.text != "" && inputX.text != "0")
		{
			maps.mapSize.x = int.Parse(inputX.text);
			inputX.text = null;
		}
		if (inputY.text != "" && inputY.text != "0")
		{ 
			maps.mapSize.y = int.Parse(inputY.text);
			inputY.text = null;
		}

		//최대 맵은 설정한 mapSize의 값에 + 1으로 설정
		maxMapSize.x = maps.mapSize.x + 1;
        maxMapSize.y = maps.mapSize.y + 1;
           
		// 좌표 생성 Generating coords
		allTileCoords = new List<Coord>();

        for (int y = 0; y < maps.mapSize.y; y++)
        {
            for (int x = 0; x < maps.mapSize.x; x++)
            {
                allTileCoords.Add(new Coord(y, x));
            }
        }

		// 타일 홀드 오브젝트 생성 Create map holder object
		holderName = "Generated Map Tile";

        if (transform.Find(holderName))
        {
            Destroy(transform.Find(holderName).gameObject);
        }

        mapHolder = new GameObject(holderName).transform;
        mapHolder.parent = transform;

        // 타일 스폰 Spawning tiles
        for (int y = 0; y < maps.mapSize.y; y++)
        {
            for (int x = 0; x < maps.mapSize.x; x++)
            {
                Vector3 tilePosition = CoordToPosition(y, x);
                Transform newTile = Instantiate(tilePrefab, tilePosition, Quaternion.Euler(Vector3.right * 90)) as Transform;
                newTile.localScale = Vector3.one * tileSize;
                newTile.parent = mapHolder;
            }
        }

		// 큐브 홀드 오브젝트 생성 Create map holder object
		holderName = "Generated Map Cube";

		if (transform.Find(holderName))
		{
			Destroy(transform.Find(holderName).gameObject);
		}

		mapHolder = new GameObject(holderName).transform;
		mapHolder.parent = transform;

		List<Coord> allOpenCoords = new List<Coord>(allTileCoords);

        for (int j = 0; j < maps.mapSize.y; j++)
        {
            for (int i = 0; i < maps.mapSize.x; i++)
            {
				float obstacleHeight = 0.5f;

				//해당 큐브의 위치
				Vector3 obstaclePosition = CoordToPosition(j, i);
				
				if (obstacleMap[j, i] >= 20 && obstacleMap[j, i] < 30)
				{
					index = obstacleMap[j, i] - 20;
					obstacleMap[j, i] -= index;
										
					obstaclePrefabs[20].GetComponent<Portal>().targetIndex = index;
				}

				//해당 위치에 큐브를 설치
				GameObject newObstacle = Instantiate(obstaclePrefabs[obstacleMap[j, i]], obstaclePosition + Vector3.up * obstaclePrefabs[obstacleMap[j, i]].transform.localScale.y / 2, Quaternion.identity) as GameObject;
                
				newObstacle.gameObject.transform.parent = mapHolder;
                newObstacle.gameObject.transform.localScale = new Vector3(1, obstaclePrefabs[obstacleMap[j, i]].transform.localScale.y, 1);
            }
        }

        // 네비매쉬 마스크 생성 Creating navmesh mask
        //필드의 좌측
        Transform maskLeft = Instantiate(navmeshMaskPrefabMeshFloor, new Vector3( -0.75f , 0, (maps.mapSize.x / 2f)-0.5f) , Quaternion.identity) as Transform;
        maskLeft.parent = mapHolder;
        maskLeft.localScale = new Vector3((maxMapSize.y - maps.mapSize.y)/2f, 1, maps.mapSize.x) * tileSize;

		//필드의 우측
		Transform maskRight = Instantiate(navmeshMaskPrefabMeshFloor, new Vector3(maps.mapSize.y - 0.25f, 0, (maps.mapSize.x / 2f) - 0.5f), Quaternion.identity) as Transform;
		maskRight.parent = mapHolder;
		maskRight.localScale = new Vector3((maxMapSize.y - maps.mapSize.y) / 2f, 1, maps.mapSize.x) * tileSize;

		//필드의 앞
		Transform maskTop = Instantiate(navmeshMaskPrefabMeshFloor, new Vector3((maps.mapSize.y / 2f) - 0.5f, 0, maps.mapSize.x - 0.25f), Quaternion.identity) as Transform;
		maskTop.parent = mapHolder;
		maskTop.localScale = new Vector3(maxMapSize.y, 1, (maxMapSize.x - maps.mapSize.x) / 2f) * tileSize;

		//필드의 뒤
		Transform maskBottom = Instantiate(navmeshMaskPrefabMeshFloor, new Vector3((maps.mapSize.y / 2f) - 0.5f, 0, -0.75f), Quaternion.identity) as Transform;
		maskBottom.parent = mapHolder;
		maskBottom.localScale = new Vector3(maxMapSize.y, 1, (maxMapSize.x - maps.mapSize.x) / 2f) * tileSize;

		navmeshFloor.transform.position = new Vector3(maps.mapSize.x / 2f - 0.5f, 0, maps.mapSize.y / 2f - 0.5f);
		navmeshFloor.localScale = new Vector3(maxMapSize.x, maxMapSize.y) * tileSize;
    }

    public void SaveMap()
    {
		if (inputSaveText.text != null && inputSaveText.text != "")
			maps.MapName = inputSaveText.text;

		if (maps.MapName != "" || maps.MapName == null)
        {
            using (StreamWriter outputFile = new StreamWriter(@"Assets\StageMaps\" + maps.MapName + ".txt"))
            {
				outputFile.WriteLine(maps.mapSize.y);
                outputFile.WriteLine(maps.mapSize.x);

                for (int j = 0; j < maps.mapSize.y; j++)
                {
                    for (int i = 0; i < maps.mapSize.x; i++)
                    {

						if (obstacleMap[j, i] >= 20 && obstacleMap[j, i] < 30)
						{
							/*GameObject[] games = GameObject.FindGameObjectsWithTag("Portal");

							obstacleMap[j, i] += games[portalIndex].GetComponent<Portal>().targetIndex;

							if (portalIndex < games.Length)
								portalIndex++;*/

							GameObject games = GameObject.FindGameObjectWithTag("Portal");

							obstacleMap[j, i] += games.GetComponent<Portal>().targetIndex;
						}

						outputFile.Write(obstacleMap[j, i]);
						outputFile.Write(" ");
                    }
                    outputFile.Write("\n");
                }

				infoUI.SetActive(true);
				//using을 쓰면 자동으로 outputFile.Close()해준다.
			}
        }
	}

    public void LoadMap()
    {
		if (inputLoadText.text != null && inputLoadText.text != "")
			maps.MapName = inputLoadText.text;

		ClearMap();//기존 맵 초기화

        if (maps.MapName != "")
        {
            using (StreamReader inputFile = new StreamReader(@"Assets\StageMaps\" + maps.MapName + ".txt"))
            {
                Debug.Log(maps.MapName);

                //string으로 값을 읽기 때문에 int로 컨버전 해줌.
                maps.mapSize.y = int.Parse(inputFile.ReadLine());
                maps.mapSize.x = int.Parse(inputFile.ReadLine());

                //maps.mapSize 크기로 맵을 다시 그려줌.
                obstacleMap = new int[(int)maps.mapSize.y, (int)maps.mapSize.x];

                string str;

                for (int j = 0; j < maps.mapSize.y; j++)
                {
                    //가로줄 전체를 읽어온다.
                    str = inputFile.ReadLine();

                    for (int i = 0; i < maps.mapSize.x; i++)
                    {
                        //메모장에서 읽어온 가로줄에서 ' '를 잘라내고 data에 넣음.
                        string[] data = str.Split(new char[] { ' ' });

                        obstacleMap[j, i] = int.Parse(data[i]);
                    }
                }

				infoUI.SetActive(true);
			}
        }
	}

    public void ClearMap()
    {
		if (inputX.text != "" && inputX.text != "0")
		{
			maps.mapSize.x = int.Parse(inputX.text);
			inputX.text = null;
		}
		
		if (inputY.text != "" && inputY.text != "0")
		{
			maps.mapSize.y = int.Parse(inputY.text);
			inputY.text = null;
		}

		GameObject[] DeleteMonster = GameObject.FindGameObjectsWithTag("Monster");
		
		for (int i = 0; i < DeleteMonster.Length; i++)
		{
			Destroy(DeleteMonster[i]);
		}

		//맵을 NullCube로 초기화
		obstacleMap = new int[(int)maps.mapSize.y, (int)maps.mapSize.x];
    }

    Vector3 CoordToPosition(int y, int x)
    {
        //큐브와 타일의 위치를 지정
        return new Vector3(y, 0, x) * tileSize;
    }

    [System.Serializable]
    public struct Coord
    {
        public int x;
        public int y;

        public Coord(int _x, int _y)
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

	public void OkButton()
	{
		infoUI.SetActive(false);

		if (saveUI.activeSelf == true)
			saveUI.SetActive(false);

		if (loadUI.activeSelf == true)
			loadUI.SetActive(false);

		GenerateMap();
	}

    [System.Serializable]
    public class Map
    {
        public string MapName;
        public Coord mapSize;

        public Coord mapCentre
        {
            get
            {
                return new Coord(mapSize.x / 2, mapSize.y / 2);
            }
        }
    }
}
