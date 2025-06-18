
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Player_Mouse_Controller : MonoBehaviour
{
    // public Animator animator;
    public float moveSpeed = 100f;
    public int[,] grid;
    private AStarPathfinding pathfinder;
    private List<Vector2Int> path;
    private Vector2Int currentTarget;

    public int[,] gridArrayG;
    // [SerializeField] private Class classObject;
    [SerializeField] private MapManager mapManager;
    [SerializeField] private Tilemap GridLayoutBounds;
    private bool isMoving;

    private enum Directions { UP, DOWN, LEFT, RIGHT}

    #region Editor Data                   
        [Header("Movement Attributes")]
        [SerializeField] float _moveSpeed = 100f;

        [Header("Dependencies")]                // this header is gonna show up in the inspector
        [SerializeField] Rigidbody2D _rb;       
        [SerializeField] Animator _animator;
        [SerializeField] SpriteRenderer _spriteRenderer;
    #endregion

    private Vector2 _movDir = Vector2.zero;     // we use _ before names as naming convention for private and protected variables
    private Directions _facingDirection = Directions.RIGHT;

    private readonly int _animMoveRight = Animator.StringToHash("Anim_Player_Move");
    private readonly int _animIdleRight = Animator.StringToHash("Anim_Player_Idle");

    private Vector2Int nextTilePos;
    private Vector2Int nextWorldPos;
    private Vector2Int playerGridPosition;  
    private Vector2Int gridOrigin;



    #region Movement Logic
        private void MovementUpdate()
        {
            _rb.velocity = _movDir.normalized *_moveSpeed * Time.fixedDeltaTime;             // _movDir.normalized stops you from moving too fast in the diagonal directions compared to the cardinal directions
        }
    #endregion

    #region Animation Logic
        private void CalculateFacingDirection() 
        {
            if (_movDir.x != 0)
            {
                if(_movDir.x > 0) // Moving RIGHT
                {
                    _facingDirection = Directions.RIGHT;
                }
                else if (_movDir.x < 0) // Moving LEFT
                {
                    _facingDirection = Directions.LEFT;
                }
            }
            // Debug.log(_facingDirection);
            // print(_facingDirection);
        }

        private void UpdateAnimation()
        {
            if (_facingDirection == Directions.LEFT) 
            {
                _spriteRenderer.flipX = true;
            }
            else if (_facingDirection == Directions.RIGHT)
            {
                _spriteRenderer.flipX = false;
            }
            if (_movDir.SqrMagnitude() > 0)   // it tells us we are moving  // we can also just take magnitude but sqr magnitude is a bit more performant
            {
                _animator.CrossFade(_animMoveRight, 0);    // we give it an animation and a transition iteration and it will swap to that animation, in this game as we are using pixel art it is fine to just snap to that animation directly
            }
            else
            {
                _animator.CrossFade(_animIdleRight, 0);
            }
        }
    #endregion


    void Start()
    {        
    //     // how to access public variables from another game object's script in unity
    //     // if your grid generator is a singleton this works ortherwise
    //     // gridArray = MapManager.Instance.GetGridArray();
    //     // Search in all active GameObjects in the current scene (not all assets).
    //     // Find the first GameObject that has the GridGenerator script attached.
    //     // Assign that instance to gridGen
    //     // Downside: This is slower because it searches the entire scene at runtime.

    //     // best approach give a editor reference to the other game object's script
    //     // If you drag and drop the "Grid" GameObject (which has GridGenerator.cs attached) into the gridGen field in another script, Unity already knows exactly where to find it.

        gridArrayG = mapManager.gridArrayG;
        void PrintGridArray(int[,] gridArray)
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
        PrintGridArray(gridArrayG);
        pathfinder = new AStarPathfinding(gridArrayG, true);
        path = new List<Vector2Int>();
        isMoving = false;
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Left mouse button clicked! -------------------------------------------------------------------------------");
            Vector2 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // Vector2Int gridPosition = new Vector2Int(Mathf.RoundToInt(worldPosition.x), Mathf.RoundToInt(worldPosition.y));
            Vector3Int tilePosition = GridLayoutBounds.WorldToCell(worldPosition);
            Vector2Int targettilePosition = new Vector2Int(tilePosition.x, tilePosition.y);

            Vector3 playerworldPosition = transform.position;  // Player's world position
            Vector3Int playerPosition = GridLayoutBounds.WorldToCell(transform.position);
            // Debug.Log("Player Cell Position a/c to grid layout bounds: " + playerPosition);
            Vector2Int playertilePosition = new Vector2Int(playerPosition.x, playerPosition.y);
            // Debug.Log("Player Grid Position: " + playertilePosition);   -------------------------------------------------------------------------------");

            // Debug.Log($"target position = {targettilePosition}");       -------------------------------------------------------------------------------");




            // Print the origin of the Tilemap's grid (in world space)
            Vector2 gridOrigin_ = (Vector2)GridLayoutBounds.CellToWorld(GridLayoutBounds.cellBounds.min);        // Convert cellBounds.min to world position
            gridOrigin = new Vector2Int((int)gridOrigin_.x, (int)gridOrigin_.y);
            // Debug.Log("Tilemap Origin (according to World Space): " + gridOrigin);   -------------------------------------------------------------------------------");

            // To convert a tile position on a Tilemap to its respective position in a grid array
            // Get the Tilemap’s Origin in World Coordinates
            // Convert target/player's World Position to Tilemap Coordinates
            // Map Tilemap Cell Position to Grid Array Position
            // Vector2Int gridPosition = tilePos - origin of tilemap in the world;
            playerGridPosition = playertilePosition - gridOrigin;
            Vector2Int targetGridPosition = targettilePosition - gridOrigin;
            Debug.Log("Player grid array position : " + playerGridPosition + " ,is_walkable = " + gridArrayG[playerGridPosition.x, playerGridPosition.y ]);
            Debug.Log("Target gird array position : " + targetGridPosition + " ,is_walkable = " + gridArrayG[targetGridPosition.x, targetGridPosition.y]);


            path = pathfinder.FindPath(playerGridPosition, targetGridPosition, true);

        }


        // if (path != null && path.Count > 0 && )
        if ( path.Count > 0 && !isMoving)
        {
            Debug.Log("Path that the Player follows as in Tilemap Grid Positions:");
            for (int i = 0; i < path.Count; i++)
            {
                Debug.Log($"Step {i}: {path[i]}");
            }

            Debug.Log("Player grid position ----------- : " + playerGridPosition + " ,is_walkable = " + gridArrayG[playerGridPosition.x, playerGridPosition.y ]);
            StartCoroutine(MoveAlongPath(playerGridPosition));             // -------------------- only this is there in the if condition 
        }



        if (isMoving)   // it tells us we are moving  
        {
            _animator.CrossFade(_animMoveRight, 0);    // we give it an animation and a transition iteration and it will swap to that animation, in this game as we are using pixel art it is fine to just snap to that animation directly
        }
        else
        {
            _animator.CrossFade(_animIdleRight, 0);
        }

    }


    // IEnumerator → This makes the function a coroutine, which allows pausing execution (yield return) without blocking the rest of the game.
    // Purpose → Moves the player along a list of path positions step by step
    IEnumerator MoveAlongPath(Vector2Int playerGridPosition)
    {
        Debug.Log("Player grid position inside coroutine -------------- : " + playerGridPosition + " ,is_walkable = " + gridArrayG[playerGridPosition.x, playerGridPosition.y ]);
        isMoving = true;           
        Vector2Int currentGridPos = playerGridPosition;
        // Debug.Log("Player grid position : " + playerGridPosition);
        // Debug.Log("Grid origin : " + gridOrigin);
        
        foreach (var pos in path)
        {
            Debug.Log("next step: " + pos);
            Vector2Int nextGridPos = pos;                           // now getting this tile position to world coordinates so that the player can move accordingly

            if (currentGridPos.x > nextGridPos.x) 
            {
                _facingDirection = Directions.LEFT;
                _spriteRenderer.flipX = true;
            }
            else 
            {
                _facingDirection = Directions.RIGHT;
                _spriteRenderer.flipX = false;
            }
            

            
            Vector2Int nextTilePos_ = nextGridPos + gridOrigin;
            // Vector2Int nextWorldPos = GridLayoutBounds.CellToWorld(nextTilePos);

            Vector3Int nextTilePos = new Vector3Int(nextTilePos_.x, nextTilePos_.y, 0);
            Vector3 nextWorldPos = GridLayoutBounds.CellToWorld(nextTilePos);

            // Debug.Log("next tile position : " + nextTilePos);
            // Debug.Log("next world position : " + nextWorldPos);

            // Determine movement direction
            // Vector2 moveDirection = (nextGridPos - currentGridPos).normalized;
            Vector2 moveDirection = ((Vector2)nextGridPos - (Vector2)currentGridPos).normalized;

            // Update animation parameters
            // animator.SetFloat("MoveX", moveDirection.x);
            // animator.SetFloat("MoveY", moveDirection.y);
            // animator.SetBool("IsMoving", true);
 

            Vector3 stepPos = new Vector3(pos.x, pos.y, 0);
            while (Vector3.Distance(transform.position, nextWorldPos) > 0.001f)
            {
                // Vector3 moveDirection = (stepPos - transform.position).normalized;


    //             animator.SetFloat("MoveX", moveDirection.x);
    //             animator.SetFloat("MoveY", moveDirection.y);
    //             animator.SetBool("IsMoving", true);

                transform.position = Vector3.MoveTowards(transform.position, nextWorldPos, moveSpeed * Time.deltaTime);
                yield return null;     // yield return null; → Waits for the next frame before running the next loop.
                // yield return null; does not reset the loop, it just waits for the next frame to continue execution.
            }

            currentGridPos = nextGridPos;
        }

        // animator.SetBool("IsMoving", false);
        path.Clear();
        isMoving = false;
    }

}


