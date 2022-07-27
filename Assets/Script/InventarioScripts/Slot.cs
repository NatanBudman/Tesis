using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IPointerClickHandler
{
    public GameObject item;
    public int ID;
    public int amount;
    public string type;
    public string Description;
    public bool empty;
    public Sprite icon;

    public Transform SlotPosition;

    private GameObject _SlotHandler;
    private GameObject _Inventory;
 


// cambia el icono en los slots llenos
    private void Start()
    {
        SlotPosition.position = this.GetComponent<Slot>().transform.position;
        _SlotHandler = GameObject.FindWithTag("Slot_Halder");
        _Inventory = GameObject.FindWithTag("Inventory");
    }

    private void Update()
    {
   
    }

    public void UpdateSlot() 
    {
       
        this.gameObject.GetComponent<Image>().sprite = icon;
    }
    public void UseItem() 
    {
        
        item.GetComponent<Items>().ItemUsage();


    }
    public void OnPointerClick(PointerEventData pointerEventData) 
    {
     
        UseItem();
   
    }
  

}

