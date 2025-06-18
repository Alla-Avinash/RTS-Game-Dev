using System.Collections;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using UnityEngine.Tilemaps;

// public class LevelGraphEditor : EditorWindow
// {

//     Tilemap tilemap;
//     List<Vector3Int> selectedTiles = new List<Vector3Int>();
//     Dictionary<Vector3Int, TileNodeData> graphNodes = new();


//     [MenuItem("Tools/Level Graph Editor")]
//     public static void ShowWindow() {
//         GetWindow<LevelGraphEditor>("Level Graph Editor");
//     }


//     void OnGUI() {
//         tilemap = EditorGUILayout.ObjectField("Tilemap", tilemap, typeof(Tilemap), true) as Tilemap;

//         if (GUILayout.Button("Connect Selected Tiles")) {
//             if (selectedTiles.Count == 2) {
//                 AddConnection(selectedTiles[0], selectedTiles[1]);
//                 selectedTiles.Clear();
//             }
//         }

//         if (GUILayout.Button("Clear Selections"))
//             selectedTiles.Clear();

//         if (GUILayout.Button("Save Graph to JSON")) {
//             string path = EditorUtility.SaveFilePanel("Save Graph", "Assets", "TileGraph", "json");
//             SaveGraph(path);
//         }
//     }

//     void OnSceneGUI(SceneView sceneView) {
//         Event e = Event.current;
//         if (e.type == EventType.MouseDown && e.button == 0 && tilemap != null) {
//             Vector3 mouseWorld = HandleUtility.GUIPointToWorldRay(e.mousePosition).origin;
//             Vector3Int tilePos = tilemap.WorldToCell(mouseWorld);
//             if (!selectedTiles.Contains(tilePos))
//                 selectedTiles.Add(tilePos);
//             e.Use();
//         }
//     }

//     void OnEnable() {
//         SceneView.duringSceneGui += OnSceneGUI;
//     }

//     void OnDisable() {
//         SceneView.duringSceneGui -= OnSceneGUI;
//     }

//     void AddConnection(Vector3Int a, Vector3Int b) {
//         if (!graphNodes.ContainsKey(a))
//             graphNodes[a] = new TileNodeData { position = a };
//         if (!graphNodes.ContainsKey(b))
//             graphNodes[b] = new TileNodeData { position = b };

//         if (!graphNodes[a].neighbors.Contains(b))
//             graphNodes[a].neighbors.Add(b);
//         if (!graphNodes[b].neighbors.Contains(a))
//             graphNodes[b].neighbors.Add(a);
//     }

//     void SaveGraph(string path) {
//         TileGraphData graphData = new TileGraphData();
//         graphData.nodes.AddRange(graphNodes.Values);
//         File.WriteAllText(path, JsonUtility.ToJson(graphData, true));
//         AssetDatabase.Refresh();
//     }

// }

public class LevelGraphEditor : EditorWindow
{
    private GraphData graphData = new GraphData();
    private Vector2Int? firstNode = null;

    private int currentFloor = 0;
    private NodeType selectedNodeType = NodeType.Normal;

    private string levelName = "Level";
    [SerializeField] private Tilemap tilemap;
    private bool showGrid = true;


    // creates an editor window which will be accessible in the editor window
    [MenuItem("Custom Tools/Level Graph Editor")]
    public static void ShowWindow()
    {
        GetWindow<LevelGraphEditor>("Graph Editor");
    }


    // Lets your tool listen for mouse clicks in the scene, Hooks into Unity’s SceneView
    // on mouse click does the OnSceneGUI
    private void OnEnable()
    {
        graphData = (LevelData)target;
        SceneView.duringSceneGui += OnSceneGUI;
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
    }


    void OnGUI()
    {
        DrawDefaultInspector();

        tilemap = (Tilemap)EditorGUILayout.ObjectField("Tilemap", tilemap, typeof(Tilemap), true);

        showGrid = EditorGUILayout.Toggle("Show Grid", showGrid);
        currentFloor = EditorGUILayout.IntField("Floor Number", currentFloor);
        selectedNodeType = (GraphNode.NodeType)EditorGUILayout.EnumPopup("Node Type", selectedNodeType);

        if (GUILayout.Button("Clear All Nodes"))
        {
            Undo.RecordObject(graphData, "Clear All Nodes");
            graphData.nodes.Clear();
            EditorUtility.SetDirty(graphData);
        }
    }


    // GUI Controls in Editor Window
    // void OnGUI()
    // {
    //     // 
    //     GUILayout.Label("Graph Node Settings", EditorStyles.boldLabel);
    //     currentFloor = EditorGUILayout.IntField("Current Floor", currentFloor);
    //     selectedNodeType = (NodeType)EditorGUILayout.EnumPopup("Node Type", selectedNodeType);
    //     levelName = EditorGUILayout.TextField("Level Name", levelName);
    //     tilemap = (Tilemap)EditorGUILayout.ObjectField("Tilemap", tilemap, typeof(Tilemap), true);

    //     if (GUILayout.Button("Clear All Nodes")) graphData = new GraphData();
    //     if (GUILayout.Button("Save Graph")) SaveGraph();
    //     if (GUILayout.Button("Load Graph")) LoadGraph();
    // }

    private void OnSceneGUI(SceneView sceneView)
    {
        if (graphData == null || tilemap == null) return;

        Event e = Event.current;

        if (e.type == EventType.MouseDown && e.button == 0 && !e.alt)
        {
            Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            Vector3 worldPoint = ray.origin;
            Vector3Int cellPos = tilemap.WorldToCell(worldPoint);
            Vector3 tileCenter = tilemap.GetCellCenterWorld(cellPos);
            Vector2Int gridPos = new Vector2Int(cellPos.x, cellPos.y);

            AddOrConnectNode(gridPos);
            e.Use();
        }

        if (showGrid)
        {
            DrawGridGizmos();
        }
    }

    // On Mouse click will make the selected point into the grid coordinate as a node and will connect the node to the previous node as an edge
    // then draws the graph of the graph structure
    // void OnSceneGUI(SceneView sceneView)
    // {
    //     Event e = Event.current;
    //     if (e.type == EventType.MouseDown && e.button == 0 && !e.alt)
    //     {
    //         // Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
    //         // Vector2 world = ray.origin;
    //         // Vector2Int gridPos = new Vector2Int(Mathf.RoundToInt(world.x), Mathf.RoundToInt(world.y));

    //         Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
    //         Vector3 worldPoint = ray.origin;
    //         if (tilemap == null)
    //         {
    //             Debug.LogWarning("Tilemap not assigned!");
    //             return;
    //         }
    //         Vector3Int cellPosition = tilemap.WorldToCell(worldPoint);
    //         Vector2Int gridPos = new Vector2Int(cellPosition.x, cellPosition.y);


    //         AddOrConnectNode(gridPos);
    //         e.Use();
    //     }

    //     DrawGraph();
    // }

    // void AddOrConnectNode(Vector2Int gridPos)
    // {
    //     GraphNode node = graphData.nodes.Find(n => n.gridPosition == gridPos && n.floorNumber == currentFloor);

    //     if (node == null)
    //     {
    //         var newNode = new GraphNode
    //         {
    //             gridPosition = gridPos,
    //             floorNumber = currentFloor,
    //             nodeType = selectedNodeType,
    //             objectName = $"Node_{gridPos.x}_{gridPos.y}_{currentFloor}"
    //         };
    //         graphData.nodes.Add(newNode);
    //     }

    //     if (firstNode == null)
    //     {
    //         firstNode = gridPos;
    //     }
    //     else
    //     {
    //         var a = graphData.nodes.Find(n => n.gridPosition == firstNode && n.floorNumber == currentFloor);
    //         var b = graphData.nodes.Find(n => n.gridPosition == gridPos && n.floorNumber == currentFloor);
    //         if (a != null && b != null && a != b)
    //         {
    //             if (!a.connections.Contains(b.gridPosition)) a.connections.Add(b.gridPosition);
    //             if (!b.connections.Contains(a.gridPosition)) b.connections.Add(a.gridPosition);
    //         }

    //         firstNode = null;
    //     }
    // }


    // void AddOrConnectNode(Vector2Int gridPos)
    // {
    //     GraphNode existingNode = graphData.nodes.Find(n => n.gridPosition == gridPos && n.floorNumber == currentFloor);

    //     if (existingNode == null)
    //     {
    //         // Register undo BEFORE the graphData is modified
    //         Undo.RecordObject(this, "Add Node");

    //         GraphNode newNode = new GraphNode
    //         {
    //             gridPosition = gridPos,
    //             floorNumber = currentFloor,
    //             nodeType = selectedNodeType,
    //             objectName = $"Node_{gridPos.x}_{gridPos.y}_{currentFloor}"
    //         };
    //         graphData.nodes.Add(newNode);
    //         EditorUtility.SetDirty(this);
    //     }

    //     if (firstNode == null)
    //     {
    //         firstNode = gridPos;
    //     }
    //     else
    //     {
    //         GraphNode a = graphData.nodes.Find(n => n.gridPosition == firstNode && n.floorNumber == currentFloor);
    //         GraphNode b = graphData.nodes.Find(n => n.gridPosition == gridPos && n.floorNumber == currentFloor);

    //         if (a != null && b != null && a != b)
    //         {
    //             Undo.RecordObject(this, "Connect Nodes");

    //             if (!a.connections.Contains(b.gridPosition)) a.connections.Add(b.gridPosition);
    //             if (!b.connections.Contains(a.gridPosition)) b.connections.Add(a.gridPosition);

    //             EditorUtility.SetDirty(this);
    //         }

    //         firstNode = null;
    //     }
    // }


    // void DrawGraph()
    // {
    //     Handles.color = Color.green;
    //     foreach (var node in graphData.nodes)
    //     {
    //         Vector3 pos = new Vector3(node.gridPosition.x, node.gridPosition.y, 0);
    //         Handles.DrawSolidDisc(pos, Vector3.forward, 0.2f);
    //         Handles.Label(pos + Vector3.up * 0.3f, $"{node.nodeType}\n{node.floorNumber}");

    //         foreach (var conn in node.connections)
    //         {
    //             Vector3 connPos = new Vector3(conn.x, conn.y, 0);
    //             Handles.DrawLine(pos, connPos);
    //         }
    //     }
    // }
    void AddOrConnectNode(Vector2Int gridPos)
    {
        GraphNode existingNode = graphData.nodes.Find(n => n.gridPosition == gridPos && n.floorNumber == currentFloor);

        if (existingNode == null)
        {
            Undo.RecordObject(graphData, "Add Node");

            GraphNode newNode = new GraphNode
            {
                gridPosition = gridPos,
                floorNumber = currentFloor,
                nodeType = selectedNodeType,
                objectName = $"Node_{gridPos.x}_{gridPos.y}_{currentFloor}",
                connections = new List<Vector2Int>()
            };
            graphData.nodes.Add(newNode);
            EditorUtility.SetDirty(graphData);
        }

        if (firstNode == null)
        {
            firstNode = gridPos;
        }
        else
        {
            GraphNode a = graphData.nodes.Find(n => n.gridPosition == firstNode && n.floorNumber == currentFloor);
            GraphNode b = graphData.nodes.Find(n => n.gridPosition == gridPos && n.floorNumber == currentFloor);

            if (a != null && b != null && a != b)
            {
                Undo.RecordObject(graphData, "Connect Nodes");
                if (!a.connections.Contains(b.gridPosition)) a.connections.Add(b.gridPosition);
                if (!b.connections.Contains(a.gridPosition)) b.connections.Add(a.gridPosition);

                EditorUtility.SetDirty(graphData);
            }

            firstNode = null;
        }
    }

    void DrawGridGizmos()
    {
        Handles.color = Color.green;
        foreach (var node in graphData.nodes)
        {
            Vector3Int cell = new Vector3Int(node.gridPosition.x, node.gridPosition.y, 0);
            Vector3 center = tilemap.GetCellCenterWorld(cell);
            Handles.DrawSolidDisc(center, Vector3.forward, 0.15f);

            foreach (var conn in node.connections)
            {
                Vector3 connCenter = tilemap.GetCellCenterWorld(new Vector3Int(conn.x, conn.y, 0));
                Handles.DrawLine(center, connCenter);
            }
        }
    }

    void SaveGraph()
    {
        string path = $"Assets/GraphData_{levelName}.json";
        File.WriteAllText(path, JsonUtility.ToJson(graphData, true));
        AssetDatabase.Refresh();
        Debug.Log("Graph saved to " + path);
    }

    void LoadGraph()
    {
        string path = $"Assets/GraphData_{levelName}.json";
        if (File.Exists(path))
        {
            graphData = JsonUtility.FromJson<GraphData>(File.ReadAllText(path));
            Debug.Log("Graph loaded from " + path);
        }
        else
        {
            Debug.LogWarning("No graph file found at: " + path);
        }
    }
}
