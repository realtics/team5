using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class MapGenerator2 : MonoBehaviour
{

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

    void Start()
    {
        //LoadMap();
    }

    public void GenerateMap()
    {
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

        // 맵 홀드 오브젝트 생성 Create map holder object
        holderName = "Generated Map Tile";
        if (transform.Find(holderName))
        {
            DestroyImmediate(transform.Find(holderName).gameObject);
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
            DestroyImmediate(transform.Find(holderName).gameObject);
        }

        mapHolder = new GameObject(holderName).transform;
        mapHolder.parent = transform;

        List<Coord> allOpenCoords = new List<Coord>(allTileCoords);

        for (int y = 0; y < maps.mapSize.y; y++)
        {
            for (int x = 0; x < maps.mapSize.x; x++)
            {
                //해당 큐브의 위치
                Vector3 obstaclePosition = CoordToPosition(y, x);

                //해당 위치에 큐브를 설치
                GameObject newObstacle = Instantiate(obstaclePrefabs[obstacleMap[y, x]],
                    obstaclePosition + Vector3.up * obstaclePrefabs[obstacleMap[y, x]].transform.localScale.y * 0.5f,
                    Quaternion.identity) as GameObject;

                newObstacle.gameObject.transform.parent = mapHolder;
                newObstacle.gameObject.transform.localScale = new Vector3(obstaclePrefabs[obstacleMap[y, x]].transform.localScale.x,
                    obstaclePrefabs[obstacleMap[y, x]].transform.localScale.y, obstaclePrefabs[obstacleMap[y, x]].transform.localScale.z);
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
        maskTop.name = "far";
        maskTop.parent = mapHolder;
        maskTop.localScale = new Vector3(maxMapSize.x, 1, (maxMapSize.y - maps.mapSize.y) * 0.5f) * tileSize;

        //필드의 가까운 부분
        Transform maskBottom = Instantiate(navmeshMaskPrefabMeshFloor, new Vector3((maps.mapSize.x * 0.5f) - 0.5f, 0, -0.75f), Quaternion.identity) as Transform;
        maskBottom.name = "Near";
        maskBottom.parent = mapHolder;
        maskBottom.localScale = new Vector3(maxMapSize.x, 1, (maxMapSize.y - maps.mapSize.y) * 0.5f) * tileSize;

        navmeshFloor.transform.position = new Vector3(maps.mapSize.x * 0.5f - 0.5f, 0f, maps.mapSize.y * 0.5f - 0.5f);
        navmeshFloor.localScale = new Vector3(maxMapSize.x, maxMapSize.y) * tileSize;
    }

    public void SaveMap()
    {
        ClearMap();

        if (maps.MapName != "" || maps.MapName == null)
        {
            using (StreamWriter outputFile = new StreamWriter(@"Assets\Resources\StageMaps\" + maps.MapName + ".txt"))
            {
                outputFile.WriteLine(maps.mapSize.y);
                outputFile.WriteLine(maps.mapSize.x);

                for (int y = 0; y < maps.mapSize.y; y++)
                {
                    for (int x = 0; x < maps.mapSize.x; x++)
                    {
                        outputFile.Write(obstacleMap[y, x]);
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

                for (int y = 0; y < maps.mapSize.y; y++)
                {
                    //가로줄 전체를 읽어온다.
                    str = inputFile.ReadLine();

                    for (int x = 0; x < maps.mapSize.x; x++)
                    {
                        //메모장에서 읽어온 가로줄에서 ' '를 잘라내고 data에 넣음.
                        string[] data = str.Split(new char[] { ' ' });

                        obstacleMap[y, x] = int.Parse(data[x]);
                    }
                }
            }
        }

        GenerateMap();
    }


    public void ClearMap()
    {
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
        public int x;
        public int y;

        public Coord(int _y, int _x)
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

    [System.Serializable]
    public class Map
    {
        public string MapName;
        public Coord mapSize;

        public Coord mapCentre
        {
            get
            {
                return new Coord(mapSize.y / 2, mapSize.x / 2);
            }
        }
    }
}
