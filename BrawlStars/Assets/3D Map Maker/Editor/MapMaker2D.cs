using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

public class MapMaker : EditorWindow
{
    public static MapMaker instance;

    Vector2 scrollPos;

    int updateCounter;

    public static bool overWrite;
    public static bool activateTools;
    public static bool snapping;

    public bool areaDeletion;
    public bool areaInsertion;

    GUIStyle style;
    //int indexToDelete;

    //Aligner
    Vector2 align;
    int alignId;

    bool playing;

    static MapMaker window;

    //rotation
    public static float rotation;
    public bool alreadyRotated;

    static SpriteRenderer gizmoTilesr;

    bool switchTool;

    public float layerDepthMultiplier = 0.1f;
    //Pos on drag
    public Vector3 beginPos;
    public Vector3 endPos;

    //drag 
    bool layerChanged;
    public static int curLayer;
    public static Layer activeLayer;

    //Holding certain keys
    public bool holdingR, holdingEscape, holdingRightMouse;
    public bool holdingTab, holdingS, holdingA;

    public Tool lasTool;

    public static bool mouseDown;

    public static GameObject gizmoCursor, gizmoTile;

    public Vector3 mousePos;

    public int layerId;
    public bool showConsole = false;

    static List<GameObject> allPrefabs;

    List<Layer> layers;

    public int controlID;

    static int selGridInt = 0;

    //Aligner GUI
    bool showAlign = true;
    string foldoutStr = "Alignment";

    //CurrentTile
    public GameObject curPrefab;

    [MenuItem("Window/2D MapEditor/Open Map Editor %m", false, 1)]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        window = (MapMaker)EditorWindow.GetWindow(typeof(MapMaker));
        window.Show();

        window.minSize = new Vector3(200, 315);
        window.titleContent = new GUIContent("MapMaker 2D");

    }
    void OnEnable()
    {
        alignId = 4;

        style = new GUIStyle();
        style.fontStyle = FontStyle.Bold;
        style.onHover.textColor = Color.blue;
        style.normal.textColor = Color.blue;
        style = new GUIStyle(style);
        layerDepthMultiplier = 0.1f;
        lasTool = Tools.current;
        selGridInt = 0;
        switchTool = false;
        instance = this;
        //is this evenright?

        align = Vector3.zero;
        alreadyRotated = false;
        rotation = 0;
        overWrite = true;
        snapping = true;
        activateTools = true;

        layers = new List<Layer>();

        beginPos = Vector3.zero;
        endPos = Vector3.zero;

        LoadLayers();

        SceneView.onSceneGUIDelegate += SceneGUI;
    }

    void LoadLayers()
    {
        if (layers != null)
            layers.Clear();

        foreach (var item in Object.FindObjectsOfType<Layer>())
        {
            layers.Add(item);
        }

        ShowLog("Layers Updated");
    }

    void LoadPrefabs()
    {
        if (allPrefabs == null)
            allPrefabs = new List<GameObject>();

        allPrefabs.Clear();

        var loadedObjects = Resources.LoadAll("");

        foreach (var loadedObject in loadedObjects)
        {
            if (loadedObject.GetType() == typeof(GameObject))
                allPrefabs.Add(loadedObject as GameObject);
        }

        if (allPrefabs != null)
            ShowLog("Imported Prefabs:" + allPrefabs.Count);
    }

    GameObject isObjectAt(Vector3 tilePos, int curLayer)
    {
        object[] obj = GameObject.FindObjectsOfType(typeof(GameObject));
        foreach (object o in obj)
        {
            GameObject g = (GameObject)o;

            ArtificialPosition artPos = g.GetComponent<ArtificialPosition>();

            if (artPos == null)
            {
                if (g.transform.localPosition == (Vector3)tilePos && (g.name != "gizmoCursor" && g.name != "gizmoTile"))
                {
                    if (g.transform.parent != null && g.transform.parent.GetComponent<Layer>().priority == curLayer)
                    {
                        if (g.transform.parent.parent == null)
                        {
                            return g;
                        }
                    }
                }
            }
            else
            {
                if (artPos.position == tilePos && g.name != "gizmoCursor")
                {
                    return g;
                }

            }
        }

        return null;
    }

    void OnDisable()
    {
        Tools.current = lasTool;
        DestroyImmediate(GameObject.Find("gizmoTile"));
        DestroyImmediate(GameObject.Find("gizmoCursor"));
        SceneView.onSceneGUIDelegate -= SceneGUI;
    }


    //Happens Everytime the window is focused (clicked)
    void OnFocus()
    {
        LoadPrefabs();
        ShowLog("MapMaker Activated");
        if (Tools.current != Tool.None)
            lasTool = Tools.current;

        Tools.current = Tool.None;
        if (gizmoTile != null)
            gizmoTile.SetActive(true);
        if (gizmoCursor != null)
            gizmoCursor.SetActive(true);
        activateTools = true;

        if (gizmoTilesr == null && gizmoTile != null)
            gizmoTilesr = gizmoTile.GetComponent<SpriteRenderer>();
    }

    void ActivateTools()
    {
        Tools.current = Tool.None;
        if (gizmoTile != null)
            gizmoTile.SetActive(true);
        if (gizmoCursor != null)
            gizmoCursor.SetActive(true);
    }

    //Updates with the scene
    void SceneGUI(SceneView sceneView)
    {

        if (Application.isPlaying)
        {
            DestroyImmediate(GameObject.Find("gizmoTile"));
            DestroyImmediate(GameObject.Find("gizmoCursor"));
            activateTools = false;
        }
        else if (Application.isPlaying == false && playing == true)
        {
            DestroyImmediate(GameObject.Find("gizmoTile"));
            DestroyImmediate(GameObject.Find("gizmoCursor"));
            activateTools = true;
        }

        playing = Application.isPlaying;

        Event e = Event.current;

        if (e.type == EventType.KeyDown && e.keyCode == KeyCode.M)
        {
            ActivateTools();
            activateTools = true;
            Tools.current = Tool.None;
        };

        if (e.type == EventType.KeyDown && e.keyCode == KeyCode.Escape)
        {
            holdingEscape = true;
        }

        if (e.type == EventType.MouseDown && e.button == 1)
        {
            holdingEscape = true;
        }

        if (e.type == EventType.MouseUp && e.button == 1)
        {
            holdingEscape = true;
            switchTool = false;
        }


        if (e.type == EventType.KeyUp && e.keyCode == KeyCode.Escape)
        {
            holdingEscape = false;

            switchTool = false;
        }

        if (holdingEscape && switchTool == false)
        {
            activateTools = !activateTools;

            if (activateTools)
            {
                ActivateTools();
            }
            else
                Tools.current = lasTool;


            switchTool = true;

        }

        //if there is a too selected, then deactivate mapmaker
        if (Tools.current != Tool.None)
            activateTools = false;

        //getsMousePosition
        mousePos = HandleUtility.GUIPointToWorldRay(e.mousePosition).origin;

        if (activateTools == false)
        {
            if (gizmoTile != null)
                gizmoTile.SetActive(false);
            if (gizmoCursor != null)
                gizmoCursor.SetActive(false);
            return;
        }

        //Sets ControlID

        controlID = GUIUtility.GetControlID(FocusType.Passive);
        
        if (e.type == EventType.Layout)
        {
            HandleUtility.AddDefaultControl(controlID);

        }

        //Checks whats happening
        switch (e.type)
        {
            case EventType.MouseDown:
                {
                    if (e.button == 0) //LEFT CLICK DOWN
                    {

                        mouseDown = true;
                    }

                    if (e.button == 1) //RIGHT CLICK DOWN
                    {

                    }
                    break;
                }
            case EventType.MouseUp:
                {
                    if (e.button == 0) //LEFT CLICK UP
                    {

                        mouseDown = false;
                    }
                    if (e.button == 1) //RIGHT CLICK UP
                    {

                    }
                    break;
                }
            case EventType.KeyDown:
                {
                    if (e.keyCode == KeyCode.M)
                    {
                        activateTools = true;
                        Tools.current = Tool.None;

                    }

                    if (e.keyCode == KeyCode.T)
                    {
                        //HandleUtility.AddDefaultControl ();
                    }
                }
                break;
            case EventType.KeyUp:
                {

                }
                break;
        }

        //Code to rotate a piece
        if (e.shift && holdingR && alreadyRotated == false)
        {
            Debug.Log("RotatePiece");
            //RotateGizmo ();

            alreadyRotated = true;
        }

        //Add Single tile
        if (mouseDown && e.shift == false && areaInsertion == false)
        {
            if (snapping == false)
                mouseDown = false;

            AddTile(gizmoCursor.transform.position, curLayer);
        }


        //Add Multiple tiles
        if (mouseDown && e.shift == true && areaInsertion == false && e.control == false)
        {
            areaInsertion = true;
            beginPos = gizmoCursor.transform.position;
            ShowLog("StartedArea");
        }


        //Draws Rectangle
        if (areaInsertion || areaDeletion)
        {
            DrawAreaRectangle();
            SceneView.RepaintAll();
        }

        //Cancel Area insertion if shift in released
        if (mouseDown && e.shift == false && areaInsertion == true)
            areaInsertion = false;

        //Starts AreaDeletion
        if (mouseDown && e.shift == true && areaDeletion == false && e.control == true)
        {
            areaDeletion = true;
            beginPos = gizmoTile.transform.position;
            ShowLog("StartedAreaDELETION");
        }


        //Deletes Elements in that area
        if (mouseDown == false && areaDeletion == true && e.shift && e.control)
        {
            ShowLog("AreaDELETION");
            AreaDeletion();
            areaDeletion = false;
        }

        //Intantiates elements in that area
        if (mouseDown == false && areaInsertion == true && e.shift && e.control == false)
        {
            AreaInsertion();
            areaInsertion = false;

        }

        //Removes single tile
        if (mouseDown && e.control && areaDeletion == false)
        {

            RemoveTile();
        }
        CursorUpdate();

        Repaint();

    }

    [MenuItem("Window/2D MapEditor/Rotate CW &r", false, 12)]
    static void RotateGizmo()
    {
        if (instance == null)
            return;

        rotation -= 90;
        Undo.RecordObject(gizmoTile.transform, "Rotation");
        gizmoTile.transform.rotation = Quaternion.Euler(0, 0, rotation);

    }

    [MenuItem("Window/2D MapEditor/Rotate CCW #&r", false, 12)]
    static void RotateCounterGizmo()
    {
        if (instance == null)
            return;

        rotation += 90;
        Undo.RecordObject(gizmoTile.transform, "Rotation");
        gizmoTile.transform.rotation = Quaternion.Euler(0, 0, rotation);

    }
    
    [MenuItem("Window/2D MapEditor/Snap &s", false, 23)]
    static void ToggleSnapping()
    {
        if (instance == null)
            return;

        snapping = !snapping;
        Undo.RecordObject(instance, "Snapping");
        //gizmoTile.transform.rotation = Quaternion.Euler(0,0,rotation);
    }

    [MenuItem("Window/2D MapEditor/OverWrite &a", false, 24)]
    static void ToggleOverWrite()
    {
        if (instance == null)
            return;

        overWrite = !overWrite;
        Undo.RecordObject(instance, "Snapping");
        //gizmoTile.transform.rotation = Quaternion.Euler(0,0,rotation);
    }
    [MenuItem("Window/2D MapEditor/Increment Layer &d", false, 35)]
    static void IncrementLayer()
    {
        if (instance == null)
            return;

        curLayer++;

        Undo.RecordObject(instance, "Snapping");
    }
    [MenuItem("Window/2D MapEditor/Decrement Layer &#d", false, 36)]
    static void DecrementLayer()
    {
        if (instance == null)
            return;
        curLayer--;
        Undo.RecordObject(instance, "Snapping");
    }

    //Draws Rectangle Area
    void DrawAreaRectangle()
    {
        Vector4 area = GetAreaBounds();
        //topline
        Handles.DrawLine(new Vector3(area[3] + 0.5f, area[0] + 0.5f, 0), new Vector3(area[1] - 0.5f, area[0] + 0.5f, 0));
        //downline
        Handles.DrawLine(new Vector3(area[3] + 0.5f, area[2] - 0.5f, 0), new Vector3(area[1] - 0.5f, area[2] - 0.5f, 0));
        //leftline
        Handles.DrawLine(new Vector3(area[3] + 0.5f, area[0] + 0.5f, 0), new Vector3(area[3] + 0.5f, area[2] - 0.5f, 0));
        //rightline
        Handles.DrawLine(new Vector3(area[1] - 0.5f, area[0] + 0.5f, 0), new Vector3(area[1] - 0.5f, area[2] - 0.5f, 0));
    }

    //Returns The game object correnspondent to a layer, null if doesnt exits
    GameObject FindLayer(int currentLayer)
    {
        bool create = true;

        GameObject layer = null;

        if (layers.Count == 0)
        {

            foreach (Layer l in Object.FindObjectsOfType<Layer>())
            {
                layers.Add(l);
            }
        }

        foreach (Layer l in Object.FindObjectsOfType<Layer>())
        {
            if (l.priority == currentLayer)
            {
                layer = l.gameObject;
                create = false;
                break;
            }
        }

        if (create)
        {
            ShowLog("Creating New Layer");
            layer = new GameObject("Layer" + currentLayer + " (" + currentLayer + ")");
            layer.AddComponent<Layer>();
            layer.GetComponent<Layer>().priority = currentLayer;
            layer.GetComponent<Layer>().id = layer.transform.GetSiblingIndex();//layerId++;
            layer.transform.position = Vector3.forward * layerDepthMultiplier * currentLayer;

            //ORDERED INSERTION
            int i;
            for (i = 0; i < layers.Count && currentLayer > layers[i].priority; i++)
            {

            }
            layers.Insert(i, layer.GetComponent<Layer>());

            for (int j = 0; j < layers.Count; j++)
            {
                for (int k = 0; k < layers.Count - 1; k++)
                {

                    if (layers[k].transform.GetSiblingIndex() > layers[k + 1].transform.GetSiblingIndex())
                    {

                        int aux = layers[k].transform.GetSiblingIndex();
                        layers[k].transform.SetSiblingIndex(layers[k + 1].transform.GetSiblingIndex());
                        layers[k + 1].transform.SetSiblingIndex(aux);
                    }
                }
            }
            //ReadList();
        }

        return layer;
    }

    //Corrects area bounds
    Vector4 GetAreaBounds()
    {
        Vector3 topLeft;
        Vector3 downRight;

        endPos = gizmoCursor.transform.position;

        topLeft.y = endPos.y > beginPos.y ? endPos.y : beginPos.y;

        topLeft.x = endPos.x < beginPos.x ? beginPos.x : endPos.x;

        downRight.y = endPos.y > beginPos.y ? beginPos.y : endPos.y;

        downRight.x = endPos.x < beginPos.x ? endPos.x : beginPos.x;

        return new Vector4(topLeft.y, downRight.x, downRight.y, topLeft.x);
    }

    //SHOULD BE LOOKED AT AGAIN
    Vector3 OffsetWeirdTiles()
    {
        //TODO ONLY WORKS FOR ONE BIG OBJECT, instead of parent of several objects
        if (gizmoTilesr != null && gizmoTilesr.sprite != null && (gizmoTilesr.sprite.bounds.extents.x != 0.5f || gizmoTilesr.sprite.bounds.extents.y != 0.5f))
        {
            //the -0.5f is to center it correctly
            return new Vector3(-align.x * (gizmoTilesr.sprite.bounds.extents.x - 0.5f), align.y * (gizmoTilesr.sprite.bounds.extents.y - 0.5f), 0);
        }

        return Vector3.zero;

    }

    void AreaDeletion()
    {
        Vector3 topLeft;
        Vector3 downRight;

        endPos = gizmoTile.transform.position;

        topLeft.z = endPos.z > beginPos.z ? endPos.z : beginPos.z;

        topLeft.x = endPos.x < beginPos.x ? beginPos.x : endPos.x;

        topLeft.y = 0.5f;

        downRight.z = endPos.z > beginPos.z ? beginPos.z : endPos.z;

        downRight.x = endPos.x < beginPos.x ? endPos.x : beginPos.x;

        downRight.y = 0.5f;

        ShowLog(downRight);
        ShowLog(topLeft);

        //Goes througt all units
        for (float y = downRight.y; y <= topLeft.y; y++)
        {

            for (float x = downRight.x; x <= topLeft.x; x++)
            {

                GameObject GOtoDelete = isObjectAt(new Vector3(x, y, curLayer * layerDepthMultiplier), curLayer);
                //If theres something then delete it
                if (GOtoDelete != null)
                {
                    Undo.DestroyObjectImmediate(GOtoDelete);
                    DestroyImmediate(GOtoDelete);
                }
            }
        }
        ShowLog("Area Deleted");
    }

    void AreaInsertion()
    {
        Vector3 topLeft;
        Vector3 downRight;

        endPos = gizmoTile.transform.position;

        topLeft.z = endPos.z > beginPos.z ? endPos.z : beginPos.z;

        topLeft.x = endPos.x < beginPos.x ? beginPos.x : endPos.x;

        topLeft.y = 0.5f;

        downRight.z = endPos.z > beginPos.z ? beginPos.z : endPos.z;

        downRight.x = endPos.x < beginPos.x ? endPos.x : beginPos.x;

        downRight.y = 0.5f;

        ShowLog(downRight);
        ShowLog(topLeft);

        for (float z = downRight.z; z <= topLeft.z; z++)
        {
            for (float x = downRight.x; x <= topLeft.x; x++)
            {

                GameObject go = isObjectAt(new Vector3(x, curLayer * layerDepthMultiplier, z), curLayer);

                //If there no object than create it
                if (go == null)
                {

                    InstantiateTile(new Vector3(x, layerDepthMultiplier, z), curLayer);


                }//in this case there is go in there 
                else if (overWrite)
                {
                    Undo.DestroyObjectImmediate(go);
                    DestroyImmediate(go);

                    InstantiateTile(new Vector3(x, 0.5f, z), curLayer);

                }
            }
        }
        ShowLog("Area Inserted");
    }

    //Updates the gizmos on the screen
    void CursorUpdate()
    {
        //Creates the if they dont already exist
        if (gizmoCursor == null)
        {
            GameObject pointer = (GameObject)Resources.Load("TilePointerGizmo", typeof(GameObject));
            if (pointer != null)
                gizmoCursor = (GameObject)Instantiate(pointer);
            else
                gizmoCursor = new GameObject();

            gizmoCursor.name = "gizmoCursor";
            //	gizmoCursor.hideFlags = HideFlags.HideInHierarchy;
            ShowLog("Cursor Created");

        }
        if (gizmoTile == null)
        {
            if (allPrefabs != null && allPrefabs.Count > 0)
                ChangeGizmoTile();
            else
                gizmoTile = new GameObject();
        }
        if (gizmoCursor != null)
        {

            //check if snaping is active
            if (snapping)
            {
                Vector3 gizmoPos = Vector3.zero;
                if (mousePos.x - Mathf.Floor(mousePos.x) < 0.5f)
                {
                    gizmoPos.x = Mathf.Floor(mousePos.x) + 0.5f;
                }
                else if (Mathf.Ceil(mousePos.x) - mousePos.x < 0.5f)
                {
                    gizmoPos.x = Mathf.Ceil(mousePos.x) - 0.5f;
                }
                if (mousePos.y - Mathf.Floor(mousePos.y) < 0.5f)
                {
                    gizmoPos.y = Mathf.Floor(mousePos.y) + 0.5f;
                }
                else if (Mathf.Ceil(mousePos.y) - mousePos.y < 0.5f)
                {
                    gizmoPos.y = Mathf.Ceil(mousePos.y) - 0.5f;
                }

                gizmoCursor.transform.position = gizmoPos;
                gizmoTile.transform.position = gizmoPos + (Vector3)gizmoTile.transform.InverseTransformVector(OffsetWeirdTiles());
            }
            else
            {
                gizmoCursor.transform.position = mousePos;
                gizmoTile.transform.position = mousePos;

            }

            //Scale the scale correctly
            if (curPrefab != null)
                gizmoTile.transform.localScale = curPrefab.transform.localScale;
        }
    }

    //Instantiate one tile
    void InstantiateTile(Vector3 pos, int layer)
    {
        if (curPrefab == null)
            return;

        GameObject metaTile = (GameObject)Instantiate(curPrefab);

        metaTile.transform.rotation = Quaternion.Euler(0, 0, rotation);
        metaTile.transform.SetParent(FindLayer(layer).transform);
        metaTile.transform.localPosition = (Vector3)pos + metaTile.transform.InverseTransformVector(OffsetWeirdTiles());

        //IF it is a weird shape
        if (metaTile.transform.localPosition != (Vector3)pos)
        {
            ArtificialPosition artPos = metaTile.AddComponent<ArtificialPosition>();
            artPos.position = pos;
            artPos.offset = artPos.position - (Vector3)metaTile.transform.position;
            artPos.layer = curLayer;
        }

        Undo.RegisterCreatedObjectUndo(metaTile, "Created go");
    }

    void AddTile(Vector3 pos, int layer)
    {
        #region Add Tile to scene

        GameObject go = isObjectAt(pos, layer);

        if (go == null)
        {

            InstantiateTile(pos, layer);




        }
        else if (overWrite)
        {
            Undo.DestroyObjectImmediate(go);
            DestroyImmediate(go);

            InstantiateTile(pos, layer);

        }



        #endregion
    }


    void RemoveTile()
    {
        GameObject GOtoDelete = isObjectAt(new Vector3(gizmoCursor.transform.position.x, gizmoCursor.transform.position.y, curLayer * layerDepthMultiplier), curLayer);
        Undo.DestroyObjectImmediate(GOtoDelete);
        DestroyImmediate(GOtoDelete);
    }



    void OnDestroy()
    {
        Tools.current = lasTool;
        DestroyImmediate(GameObject.Find("gizmoTile"));
        DestroyImmediate(GameObject.Find("gizmoCursor"));
        SceneView.onSceneGUIDelegate -= SceneGUI;
    }


    static void RecorrenciaSR(GameObject go)
    {
        if (go.GetComponent<SpriteRenderer>() != null)
        {
            Color c = go.GetComponent<SpriteRenderer>().color;
            c.a = 0.5f;
            go.GetComponent<SpriteRenderer>().color = c;
        }

        foreach (Transform t in go.transform)
        {
            RecorrenciaSR(t.gameObject);
        }
    }


    void OnGUI()
    {
        EditorGUILayout.BeginVertical();

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, false, false);
        EditorGUILayout.LabelField("Select a prefab:");

        //If prefabs have been loaded

        if (allPrefabs != null && allPrefabs.Count > 0)
        {
            GUIContent[] content = new GUIContent[allPrefabs.Count];

            for (int i = 0; i < allPrefabs.Count; i++)
            {
                if (allPrefabs[i] != null && allPrefabs[i].name != "")
                    content[i] = new GUIContent(allPrefabs[i].name, AssetPreview.GetAssetPreview(allPrefabs[i]));


                if (content[i] == null)
                    content[i] = GUIContent.none;
            }

            //creates selection grid
            EditorGUI.BeginChangeCheck();

            //prevents from error if object are deleted by user
            while (selGridInt >= allPrefabs.Count)
                selGridInt--;

            selGridInt = GUILayout.SelectionGrid(selGridInt, content, 5, GUILayout.Height(50 * (Mathf.Ceil(allPrefabs.Count / (float)5))), GUILayout.Width(this.position.width - 30));
            if (EditorGUI.EndChangeCheck())
            {
                ChangeGizmoTile();
            }
            curPrefab = allPrefabs[selGridInt];
        }

        EditorGUILayout.Space();
        //undoManager.CheckUndo(instance);
        //the layer
        EditorGUI.BeginChangeCheck();

        curLayer = EditorGUILayout.IntField("Layer", curLayer);

        //bools

        rotation = EditorGUILayout.FloatField("Rotation", rotation);

        //Undo.RecordObject (curPrefab.transform.position, "Undone SPnapping");
        snapping = EditorGUILayout.Toggle(new GUIContent("Snapping", "Should tiles snap to the grid"), snapping);

        overWrite = EditorGUILayout.Toggle(new GUIContent("Overwrite", "Do you want to overwrite tile in the same layer and position"), overWrite);

        showConsole = EditorGUILayout.Toggle(new GUIContent("Show in Console", "Show Whats happening on the console"), showConsole);

        if (EditorGUI.EndChangeCheck())
        {
            // Code to execute if GUI.changed
            Undo.RecordObject(instance, "Name");
        }

        EditorGUILayout.Space();

        EditorGUI.BeginChangeCheck();
        showAlign = EditorGUILayout.Foldout(showAlign, foldoutStr);

        if (EditorGUI.EndChangeCheck())
        {
            // Code to execute if GUI.changed
            //NOT WORKING
        }
        if (showAlign)
        {
            EditorGUI.BeginChangeCheck();

            alignId = GUILayout.SelectionGrid(alignId, new string[9], 3, GUILayout.MaxHeight(100), GUILayout.MaxWidth(100));

            if (EditorGUI.EndChangeCheck())
            {
                // Code to execute if GUI.changed

                align = alignId3Vec(alignId);
            }
        }

        //Shows the prefab
        EditorGUI.BeginChangeCheck();

        curPrefab = (GameObject)EditorGUILayout.ObjectField("Current Prefab", curPrefab, typeof(GameObject), false);
        if (EditorGUI.EndChangeCheck())
        {
            // Code to execute if GUI.changed

            if (allPrefabs != null)
            {
                //finds prefabs in the list
                int activePre = allPrefabs.IndexOf(curPrefab);

                if (activePre > 0)
                {
                    selGridInt = activePre;
                    Debug.Log("JUST DO IT");
                }
                //if its not on the list, then addit to it
            }
            else
            {
                //TODO ADD IF NOT ON RESOURCES
                //allPrefabs.Add (curPrefab);
                //selGridInt = allPrefabs.Count - 1;
            }
        }

        Texture2D previewImage = AssetPreview.GetAssetPreview(curPrefab);
        GUILayout.Box(previewImage);

        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginHorizontal(GUILayout.MaxWidth(50), GUILayout.ExpandWidth(false));

        EditorGUILayout.EndHorizontal();
    }


    void ShowLog(object msg)
    {
        if (showConsole)
        {
            Debug.Log(msg);
        }
    }


    void OnHierarchyChange()
    {
        RemoveDeleteItems();

        ChangeLayerStuff();

        //ReadList (); 
    }

    void RemoveDeleteItems()
    {
        for (int i = 0; i < layers.Count; i++)
        {

            if (layers[i] == null)
            {
                layers.Remove(layers[i]);
                i = 0;
            }
        }
    }

    void ReadList()
    {
        foreach (var item in layers)
        {

            Debug.Log(item.name);
        }
    }

    void ChangeLayerStuff()
    {
        ShowLog("Number of layers: " + layers.Count);

        layers.Sort(delegate (Layer x, Layer y)
        {
            if (x.transform.GetSiblingIndex() < y.transform.GetSiblingIndex())
                return -1;
            else
                return 0;
        });


        //ORDER IN INSPECTOR BUBBLE SORT IMPROVE
        for (int j = 0; j < layers.Count; j++)
        {
            for (int k = 0; k < layers.Count - 1; k++)
            {
                if (layers[k].priority > layers[k + 1].priority)
                {
                    int aux = layers[k].priority;
                    layers[k].priority = layers[k + 1].priority;
                    layers[k + 1].priority = aux;

                }
            }
        }

        foreach (var item in layers)
        {
            item.transform.position = Vector3.forward * layerDepthMultiplier * item.priority;
        }

        //CheckNameStuff
        //Keep layer number in name
        foreach (var item in layers)
        {
            if (item == null)
                continue;

            Regex regex = new Regex("([0-9])");

            if (regex.IsMatch(item.gameObject.name) == true)
            {
                if (item.gameObject.name.Contains("(" + item.priority + ")") == false)
                {

                    item.gameObject.name = item.gameObject.name.Remove(item.gameObject.name.Length - 4);
                    item.transform.name += " (" + item.priority + ")";

                }
            }
            else
            {
                item.transform.name += " (" + item.priority + ")";
            }
        }
    }


    Vector3 alignId3Vec(int alignIndex)
    {
        Vector3 aux;

        aux.x = alignIndex % 3 - 1;
        aux.y = 0.5f;
        aux.z = alignIndex / 3 - 1;

        ShowLog(aux);
        return aux;

    }


    static void ChangeGizmoTile()
    {
        if (gizmoTile != null)
            DestroyImmediate(gizmoTile);
        if (allPrefabs != null && allPrefabs.Count > selGridInt && allPrefabs[selGridInt] != null)
            gizmoTile = Instantiate(allPrefabs[selGridInt]) as GameObject;
        else
            gizmoTile = new GameObject();

        rotation = allPrefabs[selGridInt].transform.rotation.eulerAngles.z;
        gizmoTile.name = "gizmoTile";
        //	gizmoTile.hideFlags = HideFlags.HideInHierarchy;
        if (gizmoTilesr == null)
            gizmoTilesr = gizmoTile.GetComponent<SpriteRenderer>();
        //make it transparent
        RecorrenciaSR(gizmoTile);
    }


    [MenuItem("Window/2D MapEditor/Select GameObject 1 _F1")]
    static void Sel1()
    {
        if (instance == null)
            return;

        if (allPrefabs.Count > 0)
            selGridInt = 0;

        ChangeGizmoTile();
    }

    [MenuItem("Window/2D MapEditor/Select GameObject 2 _F2")]
    static void Sel2()
    {
        if (instance == null)
            return;
        if (allPrefabs.Count > 1)
            selGridInt = 1;

        ChangeGizmoTile();
    }

    [MenuItem("Window/2D MapEditor/Select GameObject 3 _F3")]
    static void Sel3()
    {
        if (instance == null)
            return;
        if (allPrefabs.Count > 2)
            selGridInt = 2;

        ChangeGizmoTile();
    }

    [MenuItem("Window/2D MapEditor/Select GameObject 4 _F4")]
    static void Sel4()
    {
        if (instance == null)
            return;
        if (allPrefabs.Count > 3)
            selGridInt = 3;

        ChangeGizmoTile();
    }

    [MenuItem("Window/2D MapEditor/Select GameObject 5 _F5")]
    static void Sel5()
    {
        if (instance == null)
            return;
        if (allPrefabs.Count > 4)
            selGridInt = 4;

        ChangeGizmoTile();
    }

}
