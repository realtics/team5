using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;
using UnityEngine.EventSystems;

enum UISTATE
{ 
	UI_ON,
	UI_OFF
}

public class MapGenerator : MonoBehaviour
{
	//맵 툴 UI
	public InputField inputX; //X 크기
	public InputField inputY; //Y 크기

	public InputField inputPortalIndex;

	public InputField inputSaveText;	//SaveText
	public InputField inputLoadText;	//LoadText

	public GameObject saveUI;
	public GameObject loadUI;
	public GameObject infoUI;

	public Text CubeName;
	public Text MapName;

	FileWindow fileWindow;

	//public UISTATE uiState = UISTATE.UI_END;

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
	public Camera mCamera;

	float xSensitivity = 20.0f;
	float ySensitivity = 20.0f;

	float yPos = 0.0f;
	float xPos = 0.0f;

	int index = 1;

	void Start()
    {
		UIInitialized();
	}

	private void UIInitialized()
	{
		saveUI.SetActive(false);
		loadUI.SetActive(false);
		infoUI.SetActive(false);

		inputPortalIndex.text = index.ToString();
		MapName.text = null;

		mCamera.transform.position = Camera.main.transform.position;
	}

	void Update()
	{
		CameraControl();
		OnChangeCube();
		NowCubeString();
	}

	public void SwithcingCube(int cube)
	{
		cubeIndex = cube;
	}

	public void NowCubeString()
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
				CubeName.text = "StartingPoint";
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
            case 35:
                CubeName.text = "Monster4";
                break;
            case 36:
                CubeName.text = "Boss1";
                break;
            case 37:
                CubeName.text = "Boss2";
                break;
            case 38:
                CubeName.text = "Boss3";
                break;
            case 39:
                CubeName.text = "Boss4";
                break;
            case 40:
                CubeName.text = "Boss5";
                break;
            default:
				CubeName.text = "Not Select";
				break;
		}
	}

	void CameraControl()
	{
		//마우스로 안쓸 때 카메라 이동
		if (!EventSystem.current.IsPointerOverGameObject())
		{
			if (Input.GetKeyDown(KeyCode.A))
				mCamera.transform.position -= new Vector3(1.0f, 0.0f, 0.0f);
			if (Input.GetKeyDown(KeyCode.D))
				mCamera.transform.position += new Vector3(1.0f, 0.0f, 0.0f);
			if (Input.GetKeyDown(KeyCode.S))
				mCamera.transform.position -= new Vector3(0.0f, 0.0f, 1.0f);
			if (Input.GetKeyDown(KeyCode.W))
				mCamera.transform.position += new Vector3(0.0f, 0.0f, 1.0f);
			if (Input.GetKeyDown(KeyCode.Q))
				mCamera.fieldOfView -= 1.0f;
			if (Input.GetKeyDown(KeyCode.E))
				mCamera.fieldOfView += 1.0f;

			float distance = Input.GetAxis("Mouse ScrollWheel") * -1 * 10.0f;

			if (distance != 0)
			{
				mCamera.fieldOfView += distance;
			}

			if (Input.GetMouseButton(2))
			{
				xPos += Input.GetAxis("Mouse X") * xSensitivity * Time.deltaTime;
				yPos += Input.GetAxis("Mouse Y") * ySensitivity * Time.deltaTime;

				mCamera.transform.position = new Vector3(-xPos, mCamera.transform.position.y, -yPos);
			}
		}
	}

	public void onOffSaveUI()
	{
		if (EventSystem.current.IsPointerOverGameObject())
		{
			if (loadUI.activeSelf == true)
			{
				FileWindowInfo(UISTATE.UI_OFF);
				loadUI.SetActive(false);
			}

			if (saveUI.activeSelf == false)
			{
				saveUI.SetActive(true);
				inputSaveText.text = null;

				FileWindowInfo(UISTATE.UI_ON);
			}
			else
			{
				FileWindowInfo(UISTATE.UI_OFF);
				saveUI.SetActive(false);
			}
		}
	}

	public void onOffLoadUI()
	{
		if (EventSystem.current.IsPointerOverGameObject())
		{
			if (saveUI.activeSelf == true)
			{
				FileWindowInfo(UISTATE.UI_OFF);
				saveUI.SetActive(false);
			}

			if (loadUI.activeSelf == false)
			{
				loadUI.SetActive(true);
				inputLoadText.text = null;

				FileWindowInfo(UISTATE.UI_ON);
			}
			else
			{
				FileWindowInfo(UISTATE.UI_OFF);
				loadUI.SetActive(false);
			}
		}
	}

	void OnChangeCube()
	{
		if (Input.GetMouseButton(0))
		{
			if (!EventSystem.current.IsPointerOverGameObject())
			{
				RaycastHit hit = new RaycastHit();

				Ray ray = mCamera.ScreenPointToRay(Input.mousePosition);

				if (Physics.Raycast(ray.origin, ray.direction, out hit))
				{
					if (hit.collider.gameObject.tag == "Obstacle" ||
						hit.collider.gameObject.tag == "Monster" ||
						hit.collider.gameObject.tag == "Portal" || 
						hit.collider.gameObject.tag == "StartingPoint")
					{
						Destroy(hit.collider.gameObject);

						mapHolder = transform.Find("Generated Map Cube").gameObject.transform;

						OnlyOneObjectTagName("StartingPoint");
						OnlyOneObjectTagName("Portal");

						GameObject newObstacle = Instantiate(obstaclePrefabs[cubeIndex], 
							new Vector3(hit.transform.gameObject.transform.position.x,
							obstaclePrefabs[cubeIndex].transform.localScale.y * 0.5f,
								hit.transform.gameObject.transform.position.z), Quaternion.identity);

						obstacleMap[(int)hit.transform.gameObject.transform.position.z,
							(int)hit.transform.gameObject.transform.position.x] = cubeIndex;

						newObstacle.gameObject.transform.parent = mapHolder;
					}
				}
			}
		}
	}

	void OnlyOneObjectTagName(string onlyOneName)
	{
		GameObject[] onlyOneObj = GameObject.FindGameObjectsWithTag(onlyOneName);

		if (onlyOneObj.Length > 1)
		{
			GameObject newObstacle = Instantiate(obstaclePrefabs[0], new Vector3(
			onlyOneObj[0].transform.position.x,
			obstaclePrefabs[0].transform.localScale.y * 0.5f,
			onlyOneObj[0].transform.position.z), Quaternion.identity);

			obstacleMap[(int)onlyOneObj[0].transform.position.x,
				(int)onlyOneObj[0].transform.position.z] = 0;

			newObstacle.gameObject.transform.parent = mapHolder;

			Destroy(onlyOneObj[0]);
		}
	}

	public void ClearNGenerate()
	{
		maps.MapName = null;
		ClearMap();
		GenerateMap();
	}

	void FileWindowInfo(UISTATE UiState)
	{
		fileWindow = GameObject.Find("FileScrollWindow").GetComponent<FileWindow>();

		if (UiState == UISTATE.UI_ON)
			fileWindow.CreateFileSlotsInWindow();
		else
			fileWindow.DeleteFileSlotsInWindow();
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

		MapName.text = maps.MapName;

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
				//해당 큐브의 위치
				Vector3 obstaclePosition = CoordToPosition(j, i);
				
				//포탈의 인덱스와 포탈이 가진 TargetIndex를 처리 숫자 20은 포탈의 인덱스
				if (obstacleMap[j, i] >= 20 && obstacleMap[j, i] < 30)
				{
					index = obstacleMap[j, i] - 20;
					obstacleMap[j, i] -= index;
										
					obstaclePrefabs[20].GetComponent<Portal>().targetIndex = index;
				}

				//해당 위치에 큐브를 설치
				GameObject newObstacle = Instantiate(obstaclePrefabs[obstacleMap[j, i]],
					obstaclePosition + Vector3.up * obstaclePrefabs[obstacleMap[j, i]].transform.localScale.y * 0.5f,
					Quaternion.identity) as GameObject;
                
				newObstacle.gameObject.transform.parent = mapHolder;
                newObstacle.gameObject.transform.localScale = new Vector3(obstaclePrefabs[obstacleMap[j, i]].transform.localScale.x,
					obstaclePrefabs[obstacleMap[j, i]].transform.localScale.y, obstaclePrefabs[obstacleMap[j, i]].transform.localScale.z);
            }
        }

		// 네비매쉬 마스크 생성 Creating navmesh mask
		//필드의 좌측
		Transform maskLeft = Instantiate(navmeshMaskPrefabMeshFloor, new Vector3(-0.75f, 0, (maps.mapSize.y * 0.5f) - 0.5f), Quaternion.identity) as Transform;
		maskLeft.name = "Left";
        maskLeft.parent = mapHolder;
        maskLeft.localScale = new Vector3((maxMapSize.x - maps.mapSize.x) * 0.5f, 1, maps.mapSize.y) * tileSize;

		//필드의 우측
		Transform maskRight = Instantiate(navmeshMaskPrefabMeshFloor, new Vector3(maps.mapSize.x - 0.25f, 0, (maps.mapSize.y * 0.5f) - 0.5f), Quaternion.identity) as Transform;
		maskRight.name = "Right";
		maskRight.parent = mapHolder;
		maskRight.localScale = new Vector3((maxMapSize.x - maps.mapSize.x) * 0.5f, 1, maps.mapSize.y) * tileSize;

		//필드의 먼 부분
		Transform maskTop = Instantiate(navmeshMaskPrefabMeshFloor, new Vector3((maps.mapSize.x * 0.5f) - 0.5f, 0, maps.mapSize.y - 0.25f), Quaternion.identity) as Transform;
		maskTop.name = "Back";
		maskTop.parent = mapHolder;
		maskTop.localScale = new Vector3(maxMapSize.x, 1, (maxMapSize.y - maps.mapSize.y) * 0.5f) * tileSize;

		//필드의 가까운 부분
		Transform maskBottom = Instantiate(navmeshMaskPrefabMeshFloor, new Vector3((maps.mapSize.x * 0.5f) - 0.5f, 0, -0.75f), Quaternion.identity) as Transform;
		maskBottom.name = "Front";
		maskBottom.parent = mapHolder;
		maskBottom.localScale = new Vector3(maxMapSize.x, 1, (maxMapSize.y - maps.mapSize.y) * 0.5f) * tileSize;

		navmeshFloor.transform.position = new Vector3(maps.mapSize.x * 0.5f - 0.5f, 0f, maps.mapSize.y * 0.5f - 0.5f);
		navmeshFloor.localScale = new Vector3(maxMapSize.x, maxMapSize.y) * tileSize;
    }

    public void SaveMap()
    {
		if (inputSaveText.text != null && inputSaveText.text != "")
			maps.MapName = inputSaveText.text;

		if (maps.MapName != "" || maps.MapName == null)
        {
            using (StreamWriter outputFile = new StreamWriter(@"Assets\Resources\StageMaps\" + maps.MapName + ".txt"))
            {
				outputFile.WriteLine(maps.mapSize.y);
                outputFile.WriteLine(maps.mapSize.x);

                for (int j = 0; j < maps.mapSize.y; j++)
                {
                    for (int i = 0; i < maps.mapSize.x; i++)
                    {
						if (obstacleMap[j, i] >= 20 && obstacleMap[j, i] < 30)
						{
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

    public void LoadMap(string mapName)
    {
		if (inputLoadText.text != null && inputLoadText.text != "")
			maps.MapName = inputLoadText.text;

		if (mapName != null && mapName != "")
			maps.MapName = mapName;

		ClearMap();//기존 맵 초기화

        if (maps.MapName != "")
        {
            using (StreamReader inputFile = new StreamReader(@"Assets\Resources\StageMaps\" + maps.MapName + ".txt"))
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
                        string[] data = str.Split(new char[] {' '});

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

	public void SubmitButton()
	{
		infoUI.SetActive(false);

        FileWindowInfo(UISTATE.UI_OFF);

        if (saveUI.activeSelf == true)
			saveUI.SetActive(false);

		if (loadUI.activeSelf == true)
			loadUI.SetActive(false);

        GenerateMap();
	}

	public void PlusPortalIndex()
	{
		if(index < 9)
			index++;

		obstaclePrefabs[20].GetComponent<Portal>().targetIndex = index;

		inputPortalIndex.text = index.ToString();
	}

	public void MinusPortalIndex()
	{
		if (index > 1)
			index--;

		obstaclePrefabs[20].GetComponent<Portal>().targetIndex = index;

		inputPortalIndex.text = index.ToString();
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
                return new Coord(mapSize.x * 0.5f, mapSize.y * 0.5f);
            }
        }
    }
}
