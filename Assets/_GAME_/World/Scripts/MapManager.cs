using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{

    // we want this to be a singleton 
    // we want only one one instance of this class at any time
    // we want this to be unique to our level
    private static MapManager _instance;
    public static MapManager Instance { get {return _instance; }}

    // to create a Singleton class
    private void Awake() 
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else 
        {
            _instance = this;
        }
    }


    // Assign your Tilemap in Unity Inspector
    [SerializeField] Tilemap GridLayoutBounds;             // this represent the final array dimensions for the outer grid layout, this represents the grid and only considers the walkable areas in this grid and not any other area
    [SerializeField] Tilemap Ground;                       // the walkable areas in the ground that the player can walk
    [SerializeField] Tilemap Floor1;                       // the walkable areas in floor 1 which includes the areas of bridge 
    
    private int[,] gridArray;
    public int[,] gridArrayG;
    public int[,] gridArrayF1;
    private Vector3Int gridSize;
    
    // public int[,] gridArray;
    private Vector3Int gridMin;
    private Vector3Int gridMax;
    private int gridWidth;
    private int gridHeight;


    // Start is called before the first frame update
    void Start()
    {

        // Get the bounds of the Tilemap
        gridMin = GridLayoutBounds.cellBounds.min;
        gridMax = GridLayoutBounds.cellBounds.max;

        gridWidth = gridMax.x - gridMin.x;
        gridHeight = gridMax.y - gridMin.y;

        // decision choice
        // 1. to make a global grid for both the floors(tilemaps) -> ground, floor1
        gridArrayG = GenerateGrid(Ground);
        PrintGridArray();
        gridArrayF1 = GenerateGrid(Floor1);
        PrintGridArray();
    }

    int[,] GenerateGrid(Tilemap floor)
    {
        Debug.Log($"{floor.name} - Min: {floor.cellBounds.min}, Max: {floor.cellBounds.max}, Size: {floor.cellBounds.size}");
        // gridSize = floor.cellBounds.size;
        
        // cellBounds.min → The bottom-left corner of the Tilemap (x, y, z).
        // cellBounds.max → The top-right corner +1 (x, y, z).
        // cellBounds.size → The total width and height of the Tilemap in cells. size => tells how many tiles the Tilemap spans.

        // When you duplicate a Tilemap in Unity, the new Tilemap inherits the cellBounds from the original Tilemap — including the entire area where tiles were placed before, even if you erased those tiles.
        // The cellBounds still stores the entire grid area that was used at any point, not just the active tiles
        // How to Fix It: Reset Tilemap Bounds
        // Unity doesn’t automatically shrink cellBounds when tiles are erased.
        // Here’s how you can manually shrink the Tilemap to fit only the remaining tiles -> use  Compress Bounds in the tilemap
        // Best Practice Tip for Development:
        // Whenever you erase tiles during development, always call CompressBounds() or press the button in the Inspector to clean up the bounds.

        // difference between world positions and tilepositions in grid
        // cellBounds.min, cellBounds.max, and cellBounds.size all refer to tile positions within the Tilemap's grid coordinate system, not world positions.


        // we don't do this for the individual layers as we have a common tilemap grid bounds layer to make the gridarray size to be a standard for all the tilemaps
        // // Get the bounds of the Tilemap
        // gridMin = floor.cellBounds.min;
        // gridMax = floor.cellBounds.max;

        // gridWidth = gridMax.x - gridMin.x;
        // gridHeight = gridMax.y - gridMin.y;

        gridArray = new int[gridWidth, gridHeight];

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                // Adjust position by adding gridMin to align with the Tilemap's actual positions
                Vector3Int tilePosition = new Vector3Int(x + gridMin.x, y + gridMin.y, 0);
                TileBase tile = floor.GetTile(tilePosition);

                if (tile != null)                        // Tile exists -> Walkable
                {
                    gridArray[x, y] = 1;
                }
                else                                     // No tile -> Not Walkable
                {
                    gridArray[x, y] = 0;
                }
            }
        }

        Debug.Log("Grid Generated Successfully!");
        return gridArray;

    }

    void PrintGridArray()
    {
        string gridString = "";
        for (int y = gridArray.GetLength(1) - 1; y >= 0; y--)   // Loop from top to bottom for better visualization
        {
            for (int x = 0; x < gridArray.GetLength(0); x++)
            {
                gridString += gridArray[x, y] + " ";
            }
            gridString += "\n";                                // New line after each row
        }
        Debug.Log(gridString);
    }


}


