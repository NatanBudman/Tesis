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
    [SerializeField] private Animator animation;
    [SerializeField] private float _RunSpeed;

    [SerializeField]
    private float Speed ;
    [SerializeField]
    private CharacterController characterController;
    private Rigidbody _rb;

    private bool IsRun;

    public Camera mainCamera;
    private Vector3 _camForward;
    private Vector3 _camRight;
    private Vector3 _movePlayer;
    private Vector3 playerInput;
    private bool IsCanMove;
    
    // Diferentes Attaques y typo de arma equipada
    public bool IsSplashAttack = false;
    public float CoolwDownBasicSplachAttack;
    private float CurrenTimeBasicSplashAttack;
    
    public bool IsPunchAttack = false;
    public float CoolwDownPunchAttack;
    private float CurrenTimePunchAttack;

    private bool IsHasWeaponMele = false;
    private bool IsHasWeaponShoot = false;
    private bool IsHasBareHands = true;



    // salto
    [SerializeField] private float RayDistanceJumpObject;
    [SerializeField] private float JumpCoolDown;
    private float CurrenTimeJump;
    private bool IsGround;
    Vector3 velocity;
    // Start is called before the first frame update

    private bool IsPressAWSD;
    
    [SerializeField]
    private GameObject ItemEquiped;

    private GameObject weaponManager;
    //private Items ItemEquiped;
    private int _allItems;

    private bool IsCanDefend;
    private bool IsUseShield;
    private void Start()
    {
        weaponManager = GameObject.FindGameObjectWithTag("WeaponManager");
        
        _allItems = weaponManager.transform.childCount;
        
      
        
        for (int i = 0; i < _allItems; i++)
        {
            if (weaponManager.transform.GetChild(i).gameObject.GetComponent<Items>().equipped == true)
            {
                ItemEquiped = weaponManager.transform.GetChild(i).gameObject;
            }
        
        }
        Speed = MaxSpeed;
        
        characterController = GetComponent<CharacterController>();
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (IsCanMove)
        {
            Moves();
            PlayerInput();
            PlayerMovement();
        }
        CamDirection();
        PlayerInput();
        AnimationController();
        Splash();
        Jumps();
        characterController.Move(_movePlayer * Time.deltaTime);
        
        
       
    }

    
    private void FixedUpdate()
    {
        if (IsCanMove)
        {
            Moved();
        }
      
        
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
    private void Splash()
    {
       
        
        // tipo y arma equipada
        if ( ItemEquiped != null)
        {
            if (ItemEquiped.GetComponent<Items>().type == "Weapon")
            {
                if (ItemEquiped.GetComponent<Items>().type == "Weapon" && ItemEquiped.GetComponent<Items>().gameObject.activeSelf)
                {
                    IsHasWeaponMele = true;
                }else if ( ItemEquiped.GetComponent<Items>().type == "Weapon" && !ItemEquiped.GetComponent<Items>().gameObject.activeSelf)
                {
                    IsHasWeaponMele = false;
                }
            }

            if (ItemEquiped.GetComponent<Items>().type == "Shield")
            {
                if (ItemEquiped.GetComponent<Items>().type == "Shield" && ItemEquiped.GetComponent<Items>().gameObject.activeSelf)
                {
                    IsCanDefend = true;
                }else if ( ItemEquiped.GetComponent<Items>().type == "Shield" && !ItemEquiped.GetComponent<Items>().gameObject.activeSelf)
                {
                    IsCanDefend = false;
                }
            }
        }
        
        // splash del Player
        if (IsHasWeaponMele)
        {
            if (!IsSplashAttack)
            {
                CurrenTimeBasicSplashAttack += Time.deltaTime;
            }
       
            if (Input.GetMouseButtonDown(0) && CurrenTimeBasicSplashAttack >= CoolwDownBasicSplachAttack)
            {
                IsSplashAttack = true;
                IsCanMove = false;
                CurrenTimeBasicSplashAttack = 0;
            }
            else
            {
                IsSplashAttack = false;
            }

          
            if (CurrenTimeBasicSplashAttack >= CoolwDownBasicSplachAttack - 0.5f && IsCanMove == false)
            {
                IsCanMove = true;
            }
        }
   
        if (!IsHasWeaponShoot || !IsHasWeaponMele)
        {
            IsHasBareHands = true;
        }
        else
        {
            IsHasBareHands = false;
        }
        // puñetazo basico
        if (IsHasBareHands)
        {
         
           
            if (!IsPunchAttack)
            {
                CurrenTimePunchAttack += Time.deltaTime;
           
            }
        
            if (Input.GetMouseButtonDown(0) && CurrenTimePunchAttack >= CoolwDownPunchAttack)
            {
                IsPunchAttack = true;
                IsCanMove = false;
                CurrenTimePunchAttack = 0;
            }
            else
            {
                IsPunchAttack = false;
            }

            if (CurrenTimePunchAttack >= CoolwDownPunchAttack - 0.5f && IsCanMove == false)
            {
                IsCanMove = true;
            }

            if (IsCanDefend)
            {
                if (Input.GetMouseButton(1))
                {
                    IsUseShield = true;
                }
                if (Input.GetMouseButtonUp(1))
                {
                    IsUseShield = false;
                }
            }
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
        if (IsCanMove)
        {
            animation.SetFloat("Y",verticalMove);
            animation.SetFloat("X",horizontalMove);
            animation.SetFloat("Speed",Speed);
        }
  
        animation.SetBool("Splash",IsSplashAttack);
        animation.SetBool("Punch",IsPunchAttack);
        animation.SetBool("ShieldPosicion",IsUseShield);
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

    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<Items>())
        {
            ItemEquiped = other.gameObject;
           
        }
    }

  
}
