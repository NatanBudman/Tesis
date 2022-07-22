using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class Items : MonoBehaviour
{
    public int ID;
    public string type;
    public string Description;
    public Sprite icon;
    [HideInInspector]
    public bool pickUp;  
    [HideInInspector]
    public bool equipped;
    [HideInInspector]
    public GameObject weaponManager; 
    [HideInInspector]
    public GameObject weapon;

    public bool playerWeapon;

    [HideInInspector]
    private GameObject Inventory;

    
    

    private void Start()
    {
        weaponManager = GameObject.FindGameObjectWithTag("WeaponManager");
        Inventory = GameObject.FindGameObjectWithTag("Inventory");
       
       

        if (!playerWeapon) 
        {
            int allWeapon = weaponManager.transform.childCount;

            for (int i = 0; i < allWeapon; i++) 
            {
                if(weaponManager.transform.GetChild(i).gameObject.GetComponent<Items>().ID == ID)
                {
                    weapon = weaponManager.transform.GetChild(i).gameObject;
                }
            }
        }
    }
    private void Update()
    {
        if (equipped) 
        {
       
            
            if (Input.GetMouseButtonUp(1) && Inventory.activeSelf == true )
            {
                equipped = false;
            }
            if (equipped == false)
            {
                gameObject.SetActive(false);
            }
            

        }
  
    }

    public void ItemUsage() 
    {
        if(type == "Weapon")
        {
            weapon.SetActive(true); 

            weapon.GetComponent<Items>().equipped = true;
        }

        if (type == "Shield")
        {
            weapon.SetActive(true); 

            weapon.GetComponent<Items>().equipped = true;
        }
    }
}
