using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    private PlayerController _player;
    [SerializeField]
    private GameObject ItemEquiped;
    public GameObject weaponManager;
    //private Items ItemEquiped;
    private int _allItems;

    private bool IsCanDefend;
    private bool IsUseShield;
    
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
    // Start is called before the first frame update
    void Start()
    {
        
        _player = GetComponent<PlayerController>();
        
        _allItems = weaponManager.transform.childCount;
        
      
        
        for (int i = 0; i < _allItems; i++)
        {
            if (weaponManager.transform.GetChild(i).gameObject.GetComponent<Items>().equipped == true)
            {
                ItemEquiped = weaponManager.transform.GetChild(i).gameObject;
            }
        
        }
        
      
    }

    // Update is called once per frame
    void Update()
    {
        ItemUsing();
        ItemsAnimations();
    }
     private void ItemUsing()
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
                CurrenTimeBasicSplashAttack = 0;
            }
            else
            {
                IsSplashAttack = false;
            }

          
            if (CurrenTimeBasicSplashAttack <= CoolwDownBasicSplachAttack )
            {
                _player.MoveController(false);
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
                _player.MoveController(false);
                CurrenTimePunchAttack = 0;
            }
            else
            {
                IsPunchAttack = false;
            }

            if (CurrenTimePunchAttack >= CoolwDownPunchAttack - 1f)
            {
                _player.MoveController(true);
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

     private void ItemsAnimations()
     {
         _player.animation.SetBool("Splash",IsSplashAttack);
         _player.animation.SetBool("Punch",IsPunchAttack);
         _player.animation.SetBool("ShieldPosicion",IsUseShield);
     }
     private void OnTriggerStay(Collider other)
     {
         if (other.GetComponent<Items>())
         {
             ItemEquiped = other.gameObject;
           
         }
     }
}
