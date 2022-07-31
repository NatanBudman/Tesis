using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    private float horizontalMove;
    private float verticalMove;
    public float MaxSpeed;
    public Animator animation;
    [SerializeField] private float _RunSpeed;

    [SerializeField]
    private float Speed ;
    [SerializeField]
    private CharacterController characterController;

    private bool IsRun;

    public Camera mainCamera;
    private Vector3 _camForward;
    private Vector3 _camRight;
    private Vector3 _movePlayer;
    private Vector3 playerInput;
    




    // salto
    [SerializeField] private float RayDistanceJumpObject;
    [SerializeField] private float JumpCoolDown;
    private float CurrenTimeJump;
    private bool IsGround;
    Vector3 velocity;
    // Start is called before the first frame update

    private bool IsPressAWSD;
    

    private void Start()
    {

        Speed = MaxSpeed;
        
        characterController = GetComponent<CharacterController>();
        
    }

    // Update is called once per frame
    private void Update()
    {
       
        Moves();
        PlayerInput();
        PlayerMovement();
        
        CamDirection();
        PlayerInput();
        AnimationController();
        
        Jumps();
        characterController.Move(_movePlayer * Time.deltaTime);
        
        
       
    }

    public void MoveController(bool IsCanMoved)
    {
        if (!IsCanMoved)
        {
            Speed = 0;
        }
        else
        {
            Speed = MaxSpeed;
        }
    }

    private void FixedUpdate()
    {
        Moved();
    }

    private void Moved() 
    {
        verticalMove = Input.GetAxis("Vertical");
        horizontalMove = Input.GetAxis("Horizontal");
    }

    private void Moves()
    {
        
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            IsPressAWSD = true;
        }
        else
        {
            IsPressAWSD = false;
        }

        // en este void se ajusta la velocidad de corrida
       if (!IsPressAWSD)
        {
            Speed = MaxSpeed;
        }
        if (MaxSpeed > 6)
        {
            IsRun = true;
        }
        else
        {
            IsRun = false;
        }

        if (Speed < 0 )
        {
            Speed = 0;
        }

        if (MaxSpeed < 0)
        {
            MaxSpeed = 4;
        }
        if (Speed < MaxSpeed)
        {
            Speed += 2 * Time.deltaTime;
        }
        if (Speed > MaxSpeed)
        {
            Speed -= 2 * Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && IsPressAWSD)
        {
            MaxSpeed = Mathf.Clamp(MaxSpeed, 0, _RunSpeed + MaxSpeed);
            MaxSpeed += _RunSpeed;

        } 
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            var ReturnSpeed = MaxSpeed - _RunSpeed;
            

            MaxSpeed = ReturnSpeed;
        }
    }
   

    private void Jumps()
    {
       
        if (IsGround)
        {
            CurrenTimeJump += Time.deltaTime;
        }
        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.down, out hit, RayDistanceJumpObject, LayerMask.GetMask("Floor")))
        {
            IsGround = true;
        }
        else
        {
            IsGround = false;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (IsGround && CurrenTimeJump >= JumpCoolDown)
            {
                
                var JumpForce = 10;
                velocity.y = Mathf.Sqrt(JumpForce);
             
                characterController.Move(velocity * Time.deltaTime);
                
            }

        }
      
        Debug.DrawLine(transform.position,Vector3.down * RayDistanceJumpObject,Color.black);
    }

    private void AnimationController()
    {

        animation.SetFloat("Y",verticalMove);
        animation.SetFloat("X",horizontalMove);
        animation.SetFloat("Speed",Speed);
        animation.SetBool("IsGround",IsGround);
    }

    private void PlayerInput()
    {
        verticalMove = Input.GetAxis("Vertical");
        horizontalMove = Input.GetAxis("Horizontal");
        playerInput = new Vector3(horizontalMove, 0, verticalMove);
        playerInput = Vector3.ClampMagnitude(playerInput, 1);
    }

    private void PlayerMovement()
    {
        _movePlayer = playerInput.x * _camRight + playerInput.z * _camForward;
        _movePlayer = _movePlayer * Speed;
        characterController.transform.LookAt(characterController.transform.position + _movePlayer);
    
    }

    private void CamDirection()
    {
        _camForward = mainCamera.transform.forward;
        _camRight = mainCamera.transform.right;
        _camForward.y = 0;
        _camRight.y = 0;
        _camForward = _camForward.normalized;
        _camRight = _camRight.normalized;
    }



  
}
