using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IPointerClickHandler
{
    public GameObject item;
    public int ID;
    public string type;
    public string Description;
    public bool empty;
    public Sprite icon;

    public Transform slotIconGameObject;
// cambia el icono en los slots llenos
    private void Start()
    {
        slotIconGameObject = transform.GetChild(0);
    }
    public void UpdateSlot() 
    {
        slotIconGameObject.GetComponent<Image>().sprite = icon;
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

