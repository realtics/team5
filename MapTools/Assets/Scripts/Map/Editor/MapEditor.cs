using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MapGenerator2))]
public class MapEditor : Editor
{
    public override void OnInspectorGUI()
    {
        //DrawDefaultInspector
        //bool 리턴. inspector에서 값이 갱신 되었을 때만 true

        MapGenerator2 map = target as MapGenerator2;

        if (DrawDefaultInspector())
        {
            //맵을 로딩 해준다.
            //맵을 새로 설정하고 싶으면 파일이름 칸을 지워주면 된다.

            if (map.maps.mapSize.x >= 1 && map.maps.mapSize.y >= 1)
            {
                map.LoadMap();
                map.GenerateMap();
            }
        }

        if (GUILayout.Button("Clear Map"))
        {
            map.ClearMap();
            map.GenerateMap();
        }

        if (GUILayout.Button("Generated Map"))
        {
            map.ClearMap();
            map.GenerateMap();
        }

        if (GUILayout.Button("Save Map"))
        {
            map.SaveMap();
        }

        if (GUILayout.Button("Load Map"))
        {
            map.LoadMap();
        }
    }
}
