using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Player_TileDetection : MonoBehaviour

/* 
    only for keyboard movement of the inidividual player/warrior

*/


{

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private int currentFloor = 0;                     // 0 = Ground, 1 = Floor 1
    // public Tilemap tilemap;                        // Reference to your Tilemap
    // public TileBase targetTile;                    // Tile you want to detect (assign this in the inspector)
    [SerializeField] TilemapCollider2D GroundBoundaries;  
    [SerializeField] TilemapCollider2D Floor1Area;
    // private bool isOnBridge = false;
    private bool is_Triggered = false;
    private bool trigger_stay_active = false;
    // private int prevFloor = 0;
    

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // // Convert player's world position to tilemap cell position
        // Vector3Int playerTilePosition = tilemap.WorldToCell(transform.position);

        // // Get the tile at the player's current position
        // TileBase tileAtPlayerPosition = tilemap.GetTile(playerTilePosition);

        // // Check if the tile matches the target tile
        // if (tileAtPlayerPosition == targetTile)
        // {
        //     Debug.Log("Player is on the target tile!");
        // }
   
    }



    /*
        the OnTrigger methods (like OnTriggerEnter, OnTriggerStay, and OnTriggerExit) are called based on physics updates, which occur during the FixedUpdate cycle, not every frame
        By default:
            Unity's Fixed Timestep is 0.02 seconds (50 times per second).
            OnTriggerStay is called every physics update as long as the collider remains inside the trigger.
            OnTriggerEnter is called once when a collider first enters a trigger.
            OnTriggerExit is called once when a collider exits the trigger.
    */

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Trigger G")
        {
            is_Triggered = true;
        }

        if (other.gameObject.tag == "Ground Bridge")
        {   
            spriteRenderer.sortingLayerName = "Player Floor1";  

            if (is_Triggered == true || trigger_stay_active)
            {   
                // prevFloor = currentFloor;
                currentFloor = 0;
                Debug.Log("Player is on the Ground Bridge => current floor = " + currentFloor.ToString());
                // spriteRenderer.sortingLayerName = "Player Ground"; ---------------------------------------------
                GroundBoundaries.enabled = true;
                Floor1Area.enabled = false;
                // as player moves to floor 0 we need to turn off the triggers and maket them inactive
                trigger_stay_active = false;
            }
        }
    }


    private void OnTriggerExit2D(Collider2D other) 
    {

        if (other.gameObject.tag == "Trigger G")
        {
            is_Triggered = false;
        }

        if (other.gameObject.tag == "Ground Bridge")
        {   
            // if (prevFloor == 0 )
            // {
            //     spriteRenderer.sortingLayerName = "Player Ground";
            // }
            // else if (prevFloor == 1 )
            // {
            //     spriteRenderer.sortingLayerName = "Player Floor1";
            // }
            if (currentFloor == 0 && is_Triggered == false)
            {
                spriteRenderer.sortingLayerName = "Player Ground";
            }
    
            if (is_Triggered == true || trigger_stay_active)
            {
                // prevFloor = currentFloor;
                currentFloor = 1;
                Debug.Log("Player is on the Ground Bridge => current floor = " + currentFloor.ToString());
                // spriteRenderer.sortingLayerName = "Player Floor1"; -----------------------------------------------------------
                GroundBoundaries.enabled = false;
                Floor1Area.enabled = true;
                // as player moves to floor 1 we need to always keep the triggers active
                trigger_stay_active = true;
                // Bridge.isTrigger = false;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {     
        if (other.gameObject.tag == "Ground")
        {   
            Debug.Log("Player is on the Ground and he is trying to go out of the 'Ground' Boundaries => current floor = " + currentFloor.ToString());
        }
    }

    private void OnCollisionExit2D(Collision2D collision) {
        
    }



}

// PROBLEM
// 1. when transitioning from ground to f1 or from f1 to ground, you are seeing the sprite transitining in a bad way of sorting layers need to be adjusted at the right moment.
// got this fixed