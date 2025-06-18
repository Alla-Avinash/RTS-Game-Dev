using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using System.Serializable;

// [System.Serializable]
// public class GridData
// {
//     public int width;
//     public int height;
//     public List<int> flattenedGrid; // Flattened 2D array
// }

// [System.Serializable]
// public class GraphNode
// {
//     public int x;
//     public int y;
//     // public int floor;
//     public List<Vector2Int> connections = new List<Vector2Int>();
// }

// [System.Serializable]
// public class GraphData
// {
//     public List<GraphNode> nodes = new List<GraphNode>();
// }


[System.Serializable]
public enum NodeType
{
    Normal,
    Bridge
}

[System.Serializable]
public class GraphNode
{
    public Vector2Int gridPosition;
    public int floorNumber;
    public NodeType nodeType;
    public string objectName;  // optional
    public List<Vector2Int> connections = new List<Vector2Int>();
    public Dictionary<string, float> heuristicDistances = new Dictionary<string, float>();
}

[System.Serializable]
public class GraphData
{
    public List<GraphNode> nodes = new List<GraphNode>();
}


// for future implementations of floor data and floor connections

// [Serializable]
// public class FloorData {
//     public string floorName;
//     public int floorIndex;
//     public int width;
//     public int height;
//     public List<int> flattenedWalkableGrid; // 1 = walkable, 0 = not walkable
// }

// [Serializable]
// public class FloorConnection {
//     public Vector2Int fromPosition;
//     public int fromFloor;
//     public Vector2Int toPosition;
//     public int toFloor;
// }


// ---------------------------------------------------------------------------------------------------------------------
// not sure if the below is useful??

// [Serializable]
// public class LevelData {
//     public string levelName;
//     public List<FloorData> floors = new();
//     public List<FloorConnection> connections = new();
// }
