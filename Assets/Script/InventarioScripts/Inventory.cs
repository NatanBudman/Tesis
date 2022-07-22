using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private bool IsInventoryEnabled;

    public GameObject inventory;

    private int _allSlots;

    private int _emptySlots;

    private GameObject[] slots;

    public GameObject SlotsHolder;



    void Start()
    {
        // referencia el numero de spacios que hay
        _allSlots = SlotsHolder.transform.childCount;
        
        slots = new GameObject[_allSlots];

        for (int i = 0; i < _allSlots; i++) 
        {
            slots[i] = SlotsHolder.transform.GetChild(i).gameObject;

            if (slots[i].GetComponent<Slot>().item == null) 
            {
                slots[i].GetComponent<Slot>().empty = true;
            }
        }
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I)) 
        {
            IsInventoryEnabled = !IsInventoryEnabled;
        }

        if (IsInventoryEnabled) 
        {
            inventory.SetActive(true);
        }
        else 
        {
            inventory.SetActive(false);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Item") 
        {
            GameObject ItemPickUpObject = other.gameObject;

            Items item = ItemPickUpObject.GetComponent<Items>();

            AddItem(ItemPickUpObject,item.ID,item.type,item.Description,item.icon);

          
        }
    }
// toma datos del items cogido
    public void AddItem(GameObject ItemgameObject, int ItemID,string ItemType, string ItemDescription, Sprite icon) 
    {
        for (int i = 0; i < _allSlots; i++) 
        {
            if (slots[i].GetComponent<Slot>().empty) 
            {
                ItemgameObject.GetComponent<Items>().pickUp = true;

                slots[i].GetComponent<Slot>().item = ItemgameObject;
                slots[i].GetComponent<Slot>().ID = ItemID;
                slots[i].GetComponent<Slot>().type = ItemType;
                slots[i].GetComponent<Slot>().Description = ItemDescription;
                slots[i].GetComponent<Slot>().icon = icon;

                ItemgameObject.transform.parent = slots[i].transform;
                ItemgameObject.SetActive(false);

                slots[i].GetComponent<Slot>().UpdateSlot();


                slots[i].GetComponent<Slot>().empty = false;
                return;
            }
            
        }
    }
}
