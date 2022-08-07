using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Chess : MonoBehaviour
{
    
    public GameObject ChessInvetory;
    [SerializeField]
    private GameObject ChessSlotsHolder;
    private int _allSlotInTrade;
    private GameObject[] TradeSlots;

    private bool UpdateChess;
    // Start is called before the first frame update
    void Start()
    {
        UpdateChess = true;
        
        _allSlotInTrade = ChessSlotsHolder.transform.childCount;
        
        TradeSlots = new GameObject[_allSlotInTrade];

        for (int i = 0; i < _allSlotInTrade; i++)
        {
            TradeSlots[i] = ChessSlotsHolder.transform.GetChild(i).gameObject;

            if (TradeSlots[i].GetComponent<Slot>().item == null) 
            {
                TradeSlots[i].GetComponent<Slot>().empty = true;
            }
            
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (UpdateChess)
        {
            for (int i = 0; i < _allSlotInTrade; i++)
            {
                UpdateInventoryChess(i);
                if (i == _allSlotInTrade)
                {
                    UpdateChess = false;
                }
            }
        }
        
    }

    void UpdateInventoryChess(int Slot)
    {
        if (TradeSlots[Slot].transform.childCount > 0)
        {

            Items item = TradeSlots[Slot].transform.GetChild(0).GetComponent<Items>();
                
            AddItemsInSlotsChess(item.gameObject,item.ID,item.amount,item.type,item.Description,item.icon,Slot);
        }
        
    }
    public void AddItemsInSlotsChess(GameObject ItemgameObject, int ItemID,int amount,string ItemType, string ItemDescription, Sprite icon, int slot)
    {
       
            if (TradeSlots[slot].GetComponent<Slot>().empty) 
            {
                ItemgameObject.GetComponent<Items>().pickUp = true;

                TradeSlots[slot].GetComponent<Slot>().item = ItemgameObject;
                TradeSlots[slot].GetComponent<Slot>().ID = ItemID;
                TradeSlots[slot].GetComponent<Slot>().amount = amount;
                TradeSlots[slot].GetComponent<Slot>().type = ItemType;
                TradeSlots[slot].GetComponent<Slot>().Description = ItemDescription;
                TradeSlots[slot].GetComponent<Slot>().icon = icon;

                ItemgameObject.transform.parent = TradeSlots[slot].transform;
                ItemgameObject.SetActive(false);

                TradeSlots[slot].GetComponent<Slot>().UpdateSlot();


                TradeSlots[slot].GetComponent<Slot>().empty = false;
            }
            
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PJ"))
        {
          
        }
    }
}
