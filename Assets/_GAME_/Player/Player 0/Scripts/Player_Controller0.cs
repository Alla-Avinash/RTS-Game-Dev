using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase] 
/* this is above and outside the class and this allows us to have additional functionality to this class 
    in this case this allows us to select the game objects that have this script, when cliking on any of its sub objects
*/

public class Player_Controller0 : MonoBehaviour
{

    #region Enums
    private enum Directions { UP, DOWN, LEFT, RIGHT}
    #endregion

    #region Editor Data                     
    // all the data in this section is going to be exposed in the editor

    [Header("Movement Attributes")]
    [SerializeField] float _moveSpeed = 100f;

    [Header("Dependencies")]                // this header is gonna show up in the inspector
    [SerializeField] Rigidbody2D _rb;       // we do plumbing here => putting the relevant components into the slot in the script that we got in th editor
    [SerializeField] Animator _animator;
    [SerializeField] SpriteRenderer _spriteRenderer;
    #endregion

    #region Internal Data
    private Vector2 _movDir = Vector2.zero;     // we use _ before names as naming convention for private and protected variables
    private Directions _facingDirection = Directions.RIGHT;

    private readonly int _animMoveRight = Animator.StringToHash("Anim_Player_Move");
    private readonly int _animIdleRight = Animator.StringToHash("Anim_Player_Idle");
    #endregion

    #region Tick
    private void Update()                   // Update is called once per frame
    {
        Gather_Input();
        CalculateFacingDirection();
        UpdateAnimation();
    }

    private void FixedUpdate()
    {
        MovementUpdate();
    }
    #endregion

    #region Input Logic
    private void Gather_Input()
    {
        _movDir.x = Input.GetAxisRaw("Horizontal");
        _movDir.y = Input.GetAxisRaw("Vertical");
    }
    #endregion

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




    // Start is called before the first frame update
    void Start()
    {
        
    }

    
}
