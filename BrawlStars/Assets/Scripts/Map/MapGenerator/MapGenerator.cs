using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;

public class MapGenerator : MonoBehaviour
{
    public GameObject startingObject;

    public Map[] maps;
    public Transform[] obstaclePrefabs;
    public int mapIndex;

    public Transform tilePrefab;
    public Transform navmeshFloor;
    public Transform navmeshMaskPrefabMeshFloor;
    public Vector3 maxMapSize;

    [Range(0, 1)]
    public float outlinePercent;

    public float tileSize;
    List<Coord> allTileCoords;
    
    Map currentMap;
    
    public int[,] obstacleMap;

    void Start()
    {
        //LoadMap();
    }

    public void GenerateMap()
    {
        //맵의 갯수 설정
        currentMap = maps[mapIndex];

        SetCharacterPosition();

        //최대 맵은 설정한 mapSize의 값에 + 1으로 설정
        maxMapSize.x = currentMap.mapSize.x + 1;
        maxMapSize.y = currentMap.mapSize.y + 1;
           
        GetComponent<BoxCollider>().size = new Vector3(currentMap.mapSize.x * tileSize, .05f, currentMap.mapSize.y * tileSize);

        // 좌표 생성 Generating coords
        allTileCoords = new List<Coord>();

        for (int x = 0; x < currentMap.mapSize.x; x++)
        {
            for (int y = 0; y < currentMap.mapSize.y; y++)
            {
                allTileCoords.Add(new Coord(x, y));
            }
        }

        // 맵 홀드 오브젝트 생성 Create map holder object
        string holderName = "Generated Map";
        if (transform.Find(holderName))
        {
            //DestroyImmediate(transform.Find(holderName).gameObject);
            Destroy(transform.Find(holderName).gameObject);
        }

        Transform mapHolder = new GameObject(holderName).transform;
        mapHolder.parent = transform;

        // 타일 스폰 Spawning tiles
        for (int x = 0; x < currentMap.mapSize.x; x++)
        {
            for (int y = 0; y < currentMap.mapSize.y; y++)
            {
                Vector3 tilePosition = CoordToPosition(x, y);
                Transform newTile = Instantiate(tilePrefab, tilePosition, Quaternion.Euler(Vector3.right * 90)) as Transform;
                newTile.localScale = Vector3.one * (1 - outlinePercent) * tileSize;
                newTile.parent = mapHolder;
            }
        }

        List<Coord> allOpenCoords = new List<Coord>(allTileCoords);

        for (int i = 0; i < currentMap.mapSize.x; i++)
        {
            for (int j = 0; j < currentMap.mapSize.y; j++)
            {
                float obstacleHeight;

                //currentMap.selectObstacleElement의 obstacleMap[i, j]의 값과 같으면 크기를 조정 할 수 있도록 한다.(Ex:물 큐브)
                if (currentMap.selectObstacleElement == obstacleMap[i, j])
                {
                    obstacleHeight = Mathf.Lerp(0, currentMap.maxObstacleHeight, 1);
                }
                else
                    obstacleHeight = 0.5f;

                //해당 큐브의 위치
                Vector3 obstaclePosition = CoordToPosition(i, j);

                //해당 위치에 큐브를 설치
                Transform newObstacle = Instantiate(obstaclePrefabs[obstacleMap[i, j]], obstaclePosition + Vector3.up * obstacleHeight / 2, Quaternion.identity) as Transform;
                newObstacle.parent = mapHolder;
                newObstacle.localScale = new Vector3((1 - outlinePercent) * tileSize, obstacleHeight, (1 - outlinePercent) * tileSize);

            }
        }

        // 네비매쉬 마스크 생성 Creating navmesh mask
        //필드의 좌측
        Transform maskLeft = Instantiate(navmeshMaskPrefabMeshFloor, Vector3.left * (currentMap.mapSize.x + maxMapSize.x) / 4f * tileSize, Quaternion.identity) as Transform;
        maskLeft.parent = mapHolder;
        maskLeft.localScale = new Vector3((maxMapSize.x - currentMap.mapSize.x) / 2f, 1, currentMap.mapSize.y) * tileSize;

        //필드의 우측
        Transform maskRight = Instantiate(navmeshMaskPrefabMeshFloor, Vector3.right * (currentMap.mapSize.x + maxMapSize.x) / 4f * tileSize, Quaternion.identity) as Transform;
        maskRight.parent = mapHolder;
        maskRight.localScale = new Vector3((maxMapSize.x - currentMap.mapSize.x) / 2f, 1, currentMap.mapSize.y) * tileSize;

        //필드의 앞
        Transform maskTop = Instantiate(navmeshMaskPrefabMeshFloor, Vector3.forward * (currentMap.mapSize.y + maxMapSize.y) / 4f * tileSize, Quaternion.identity) as Transform;
        maskTop.parent = mapHolder;
        maskTop.localScale = new Vector3(maxMapSize.x, 1, (maxMapSize.y - currentMap.mapSize.y) / 2f) * tileSize;

        //필드의 뒤
        Transform maskBottom = Instantiate(navmeshMaskPrefabMeshFloor, Vector3.back * (currentMap.mapSize.y + maxMapSize.y) / 4f * tileSize, Quaternion.identity) as Transform;
        maskBottom.parent = mapHolder;
        maskBottom.localScale = new Vector3(maxMapSize.x, 1, (maxMapSize.y - currentMap.mapSize.y) / 2f) * tileSize;

        navmeshFloor.localScale = new Vector3(maxMapSize.x, maxMapSize.y) * tileSize;
    }

    public void SaveMap()
    {
        if (maps[mapIndex].MapName != "" || maps[mapIndex].MapName == null)
        {
            using (StreamWriter outputFile = new StreamWriter(@"Assets\StageMaps\" + maps[mapIndex].MapName + ".txt"))
            {
                outputFile.WriteLine(currentMap.mapSize.x);
                outputFile.WriteLine(currentMap.mapSize.y);

                for (int j = 0; j < currentMap.mapSize.y; j++)
                {
                    for (int i = 0; i < currentMap.mapSize.x; i++)
                    {
                        outputFile.Write(obstacleMap[i, j]);
                        outputFile.Write(" ");
                    }
                    outputFile.Write("\n");
                }
                //using을 쓰면 자동으로 outputFile.Close()해준다.
            }
        }

        GenerateMap();
    }

    public void LoadMap()
    {
        ClearMap();//기존 맵 초기화

        if (maps[mapIndex].MapName != "")
        {
            using (StreamReader inputFile = new StreamReader(@"Assets\StageMaps\" + maps[mapIndex].MapName + ".txt"))
            {
                Debug.Log(maps[mapIndex].MapName);

                //string으로 값을 읽기 때문에 int로 컨버전 해줌.
                currentMap.mapSize.x = int.Parse(inputFile.ReadLine());
                currentMap.mapSize.y = int.Parse(inputFile.ReadLine());

                //currentMap.mapSize 크기로 맵을 다시 그려줌.
                obstacleMap = new int[(int)currentMap.mapSize.x, (int)currentMap.mapSize.y];

                string str;

                for (int j = 0; j < currentMap.mapSize.y; j++)
                {
                    //가로줄 전체를 읽어온다.
                    str = inputFile.ReadLine();

                    for (int i = 0; i < currentMap.mapSize.x; i++)
                    {
                        //메모장에서 읽어온 가로줄에서 ' '를 잘라내고 data에 넣음.
                        string[] data = str.Split(new char[] { ' ' });

                        obstacleMap[i, j] = int.Parse(data[i]);
                    }
                }
            }
        }

        GenerateMap();
    }

    //캐릭터는 건들면 안되니 임시로 처리하였음.
    public void SetCharacterPosition()
    {
        //인스펙터에서 정한 좌표를 스타팅 좌표로 지정. 인덱스에 따라 바뀐다.
        startingObject.transform.position = currentMap.startingPosition;

        //지정된 좌표로 캐릭터를 찾아서 이동.
        GameObject.Find("Character").transform.position = new Vector3(startingObject.transform.position.x, 0.5f, startingObject.transform.position.z);
    }

    public void ClearMap()
    {
        //설정된 맵의 인덱스
        currentMap = maps[mapIndex];

        //맵을 NullCube로 초기화
        obstacleMap = new int[(int)currentMap.mapSize.x, (int)currentMap.mapSize.y];
    }

    Vector3 CoordToPosition(int x, int y)
    {
        //큐브와 타일의 위치를 지정
        return new Vector3(-currentMap.mapSize.x / 2f + 0.5f + x, 0, -currentMap.mapSize.y / 2f + 0.5f + y) * tileSize;
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
        //아래는 경고문 제거
        public static bool operator !=(Coord c1, Coord c2)
        {
            return !(c1 == c2);
        }
        public override bool Equals(object obj)
        {
            return true;
        }
        public override int GetHashCode()
        {
            return 0;
        }
    }

    [System.Serializable]
    public class Map
    {
        public string MapName;
        public Coord mapSize;
        public float maxObstacleHeight;
        public int selectObstacleElement;
        public Vector3 startingPosition;

        public Coord mapCentre
        {
            get
            {
                return new Coord(mapSize.x / 2, mapSize.y / 2);
            }
        }
    }
}
