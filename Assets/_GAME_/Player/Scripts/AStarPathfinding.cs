using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarPathfinding : MonoBehaviour
{

    private int[,] grid;
    private int width, height;
    private Vector2Int[] directions;

    // this is the constructor for this class, it requires you to provide the grid to find out the required shortest path
    public AStarPathfinding(int[,] grid, bool allowDiagonal)                  
    {
        this.grid = grid;
        width = grid.GetLength(0);
        height = grid.GetLength(1);

        directions = allowDiagonal ? new Vector2Int[]
        {
            new Vector2Int(-1, 0), new Vector2Int(1, 0),
            new Vector2Int(0, -1), new Vector2Int(0, 1),
            new Vector2Int(-1, -1), new Vector2Int(1, -1),
            new Vector2Int(-1, 1), new Vector2Int(1, 1)
        }
        : new Vector2Int[]
        {
            new Vector2Int(-1, 0), new Vector2Int(1, 0),
            new Vector2Int(0, -1), new Vector2Int(0, 1)
        }; 
    }

    // finds the A* path and returns the required path as a list(array) of tile positions on the grid
    // Takes the starting position (where the player is) and the target position (where the player clicked)
    // Returns a list of tile positions that the player will follow to go according to the shortest A* algo
    public List<Vector2Int> FindPath(Vector2Int start, Vector2Int target, bool allowDiagonal)           
    {
        if (grid[target.x, target.y] == 0)
        {
            Debug.Log("Blocked tile detected! Pathfinding aborted. click an area where the player can move");
            return new List<Vector2Int>();        // Exit early without trying to pathfind
        }
        List<Node> openSet = new List<Node>();                            // the nodes that we have to search
        HashSet<Node> closedSet = new HashSet<Node>();                    // the nodes that we have already searched

        Node startNode = new Node(start, null, 0, GetHeuristic(start, target));
        openSet.Add(startNode);

        //  we keep searching for the target node untill
        // current node == target node or
        // if the openlist is empty (no nodes left to check)
        while (openSet.Count > 0)
        {
            openSet.Sort((a, b) => (a.FCost).CompareTo(b.FCost));
            Node currentNode = openSet[0];

            if (currentNode.Position == target)
                return RetracePath(currentNode);

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            foreach (Vector2Int neighbor in GetNeighbors(currentNode.Position, allowDiagonal))
            {
                if (closedSet.Contains(new Node(neighbor, null, 0, 0)))
                    continue;

                int gCost = currentNode.GCost + 1;
                Node neighborNode = new Node(neighbor, currentNode, gCost, GetHeuristic(neighbor, target));

                if (!openSet.Exists(n => n.Position == neighbor && n.FCost <= neighborNode.FCost))
                {
                    openSet.Add(neighborNode);
                }
            }
        }

        return new List<Vector2Int>();                                                                  // No path found
    }

    private List<Vector2Int> GetNeighbors(Vector2Int position, bool allowDiagonal)
    {
        List<Vector2Int> neighbors = new List<Vector2Int>();
        // Vector2Int[] directions = allowDiagonal ? new Vector2Int[]
        // {
        //     new Vector2Int(-1, 0), new Vector2Int(1, 0),
        //     new Vector2Int(0, -1), new Vector2Int(0, 1),
        //     new Vector2Int(-1, -1), new Vector2Int(1, -1),
        //     new Vector2Int(-1, 1), new Vector2Int(1, 1)
        // }
        // : new Vector2Int[]
        // {
        //     new Vector2Int(-1, 0), new Vector2Int(1, 0),
        //     new Vector2Int(0, -1), new Vector2Int(0, 1)
        // };

        foreach (var dir in this.directions)
        {
            Vector2Int newPos = position + dir;
            if (IsInBounds(newPos) && grid[newPos.x, newPos.y] == 1)
                neighbors.Add(newPos);
        }

        return neighbors;
    }

    private bool IsInBounds(Vector2Int pos)
    {
        return pos.x >= 0 && pos.x < width && pos.y >= 0 && pos.y < height;
    }

    private int GetHeuristic(Vector2Int a, Vector2Int b)
    {
        // octile movement - best followed when you wanna give more preference to the diagonal movement when compared to 2 straight line moves
        int dx = Mathf.Abs(a.x - b.x);
        int dy = Mathf.Abs(a.y - b.y);
        // return 10 * (dx + dy) + (4 * Mathf.Min(dx, dy));
        return 14 * Mathf.Min(dx, dy) + 10 * (dx - dy);
        // manhattn distance heuristic - for 4 directional movement 
        // return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }

    private List<Vector2Int> RetracePath(Node endNode)
    {
        List<Vector2Int> path = new List<Vector2Int>();
        Node currentNode = endNode;
        while (currentNode != null)
        {
            path.Add(currentNode.Position);
            currentNode = currentNode.Parent;
        }
        path.Reverse();
        return path;
    }

    private class Node
    {
        public Vector2Int Position;
        public Node Parent;
        public int GCost;
        public int HCost;
        public int FCost => GCost + HCost;

        public Node(Vector2Int position, Node parent, int gCost, int hCost)
        {
            Position = position;
            Parent = parent;
            GCost = gCost;
            HCost = hCost;
        }
    }

}
