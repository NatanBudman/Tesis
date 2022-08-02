using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class Items : MonoBehaviour
{
    public int ID;
    public int amount;
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


    private GameObject lastPlayerTakeItem;
    private void Start()
    {
     //   Inventory = GameObject.FindGameObjectWithTag("Inventory");

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

    private void UpdateInterface()
    {
        if (weaponManager != null)
        {
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
    }
    private void OnTriggerEnter(Collider other)
    {
       
        if (other.CompareTag("PJ"))
        {
            if (weaponManager == null && lastPlayerTakeItem != other.gameObject)
            {
                weaponManager = other.GetComponent<WeaponController>().weaponManager.gameObject;
                UpdateInterface();
            }
            if (Inventory == null && lastPlayerTakeItem != other.gameObject)
            {
                Inventory = other.GetComponent<Inventory>().inventory.gameObject;
                Debug.Log(Inventory.gameObject.name);
            }
        }
        if (lastPlayerTakeItem == null)
        { 
            lastPlayerTakeItem = other.gameObject;
        }
    }

  

}
