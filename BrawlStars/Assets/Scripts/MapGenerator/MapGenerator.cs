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
    
    Transform[,] tileMap;

    Map currentMap;
    
    public int[,] obstacleMap;

    void Start()
    {
        
        LoadMap();

        
    }

    public void GenerateMap()
    {
        //맵의 갯수 설정
        currentMap = maps[mapIndex];

        SetCharacterPosition();

        //
        tileMap = new Transform[currentMap.mapSize.x, currentMap.mapSize.y];

        //최대 맵은 설정한 mapSize의 값에 + 10으로 설정
        maxMapSize.x = currentMap.mapSize.x + 1;
        maxMapSize.y = currentMap.mapSize.y + 1;
        
        //임시 값(obstacle의 설치를 시험)
        //randomSeed = Random.Range(0, 4);
        //
        
        //충돌체 크기 설정
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
            DestroyImmediate(transform.Find(holderName).gameObject);
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
                tileMap[x, y] = newTile;
            }
        }

        // 장매물 생성 Spawning obstacles
        //int[,] obstacleMap = new int[(int)currentMap.mapSize.x, (int)currentMap.mapSize.y];
        //obstacleMap = new int[(int)currentMap.mapSize.x, (int)currentMap.mapSize.y];

        //임시 값
        //int[,] obstacleMap = new int[5, 5] { { 0, 1, 2, 3, 0 },
        //                                     { 0, 1, 2, 3, 0 },
        //                                     { 0, 1, 2, 3, 0 },
        //                                     { 0, 1, 2, 3, 0 },
        //                                     { 0, 1, 2, 3, 0 }};

        //obstacleMap = new int[,]   { { 0, 1, 2, 3, 0, 0, 0, 0, 3, 0 },
        //                            { 0, 1, 2, 3, 0, 0, 0, 0, 3, 0 },
        //                            { 0, 1, 2, 0, 0, 0, 0, 0, 3, 0 },
        //                            { 0, 0, 0, 0, 0, 0, 3, 0, 3, 0 },
        //                            { 0, 1, 2, 3, 0, 0, 3, 0, 0, 0 },
        //                            { 0, 1, 2, 3, 0, 0, 3, 0, 0, 0 },
        //                            { 0, 0, 0, 3, 0, 0, 3, 0, 3, 0 },
        //                            { 0, 1, 0, 0, 0, 0, 0, 0, 3, 0 },
        //                            { 0, 1, 2, 0, 0, 0, 0, 0, 3, 0 },
        //                            { 0, 1, 2, 3, 0, 0, 0, 0, 3, 0 } };

        List<Coord> allOpenCoords = new List<Coord>(allTileCoords);

        for (int i = 0; i < currentMap.mapSize.x; i++)
        {
            for (int j = 0; j < currentMap.mapSize.y; j++)
            {
                //임시 값
                //if (i % 2 == 0 && j % 2 == 0)
                //{
                //    obstacleMap[i, j] = 1;
                //}
                //else if (i % 3 == 0 && j % 2 == 0)
                //{
                //    obstacleMap[i, j] = 2;
                //}
                //else if (i % 1 == 0 && j % 2 != 0)
                //{
                //    obstacleMap[i, j] = 3;
                //}
                //else
                //    obstacleMap[i, j] = 0;
                //

                float obstacleHeight;
                //selectObstacle이 obstacleMap[i, j]의 값과 같으면 크기를 조정 할 수 있도록 한다.(Ex:물 큐브)
                if (currentMap.selectObstacleElement == obstacleMap[i, j])
                {
                    obstacleHeight = Mathf.Lerp(0, currentMap.maxObstacleHeight, 1);
                }
                else
                    obstacleHeight = 1;

                //해당 큐브의 위치
                Vector3 obstaclePosition = CoordToPosition(i, j);

                //allOpenCoords.Remove(Random)

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
            //using (StreamWriter outputFile = new StreamWriter(@"Assets\StageMaps\NextFile.txt"))
            using (StreamWriter outputFile = new StreamWriter(@"Assets\StageMaps\" + maps[mapIndex].MapName + ".txt"))
            {
                outputFile.WriteLine(currentMap.mapSize.x);
                outputFile.WriteLine(currentMap.mapSize.y);

                for (int i = 0; i < currentMap.mapSize.x; i++)
                {
                    for (int j = 0; j < currentMap.mapSize.y; j++)
                    {
                        //outputFile.WriteLine(obstacleMap[i, j]);
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
        //txt.gameObject;

        ClearMap();

        //string[] lines = File.ReadAllLines(@"C:\Users\ICT03_10\Desktop\BrawlStars\Assets\01.Scenes\MapGenerator\New_TEXT_File.txt");
        if (maps[mapIndex].MapName != "")
        {
            using (StreamReader inputFile = new StreamReader(@"Assets\StageMaps\" + maps[mapIndex].MapName + ".txt"))
            {
                currentMap.mapSize.x = int.Parse(inputFile.ReadLine());//string으로 값을 읽기 때문에 int로 컨버전 해줌.
                currentMap.mapSize.y = int.Parse(inputFile.ReadLine());

                //obstacleMap = new int[(int)currentMap.mapSize.x, (int)currentMap.mapSize.y];

                string str;

                for (int i = 0; i < currentMap.mapSize.x; i++)
                {
                    str = inputFile.ReadLine();

                    for (int j = 0; j < currentMap.mapSize.y; j++)
                    {
                        string[] data = str.Split(new char[] { ' ' });

                        obstacleMap[j, i] = int.Parse(data[j]);
                        //obstacleMap[i, j] = int.Parse(inputFile.ReadLine());

                        //obstacleMap[i, j] = inputFile.Read();
                    }
                }
                //using을 쓰면 자동으로 inputFile.Close()해준다.
            }
        }
            //ClearMap();
        GenerateMap();
    }

    public void SetCharacterPosition()
    {
        startingObject.transform.position = currentMap.startingPosition;
        
        GameObject.Find("Character").transform.position = new Vector3(startingObject.transform.position.x, 0.5f, startingObject.transform.position.y);
    }


    public void ClearMap()
    {
        //맵의 갯수 설정
        currentMap = maps[mapIndex];

        //if(currentMap.mapSize.x >= 0 && currentMap.mapSize.y >= 0)
        obstacleMap = new int[(int)currentMap.mapSize.x, (int)currentMap.mapSize.y];
        
        //GenerateMap();
    }

    Vector3 CoordToPosition(int x, int y)
    {
        return new Vector3(-currentMap.mapSize.x / 2f + 0.5f + x, 0, -currentMap.mapSize.y / 2f + 0.5f + y) * tileSize;
    }

    public Transform GetTileFromPosition(Vector3 position)
    {
        int x = Mathf.RoundToInt(position.x / tileSize + (currentMap.mapSize.x - 1) / 2f);
        int y = Mathf.RoundToInt(position.z / tileSize + (currentMap.mapSize.y - 1) / 2f);
        x = Mathf.Clamp(0, 0, tileMap.GetLength(0) - 1);
        y = Mathf.Clamp(y, 0, tileMap.GetLength(1) - 1);
        return tileMap[x, y];
    }

    public Transform GetRandomSpawnTile()
    {
        //Coord randomCoord = shuffledOpenTileCoord.Dequeue();
        //shuffledOpenTileCoord.Enqueue(randomCoord);
        return tileMap[2, 2];
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
        public override bool Equals(object obj)
        {
            return true;
            //return Mathf.Abs(ID - ((Person)obj).ID) <= 5;
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
        public Vector2 startingPosition;

        public Coord mapCentre
        {
            get
            {
                return new Coord(mapSize.x / 2, mapSize.y / 2);
            }
        }
    }
}
