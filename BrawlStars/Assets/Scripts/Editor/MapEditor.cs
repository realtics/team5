using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MapGenerator))]
public class MapEditor : Editor
{
    public override void OnInspectorGUI()
    {
        //bool 리턴. inspector에서 값이 갱신 되었을 때만 true
        //DrawDefaultInspector

        MapGenerator map = target as MapGenerator;

        if (DrawDefaultInspector())
        {
            map.ClearMap();
            map.GenerateMap();
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
            map.GenerateMap();
        }
    }
}
