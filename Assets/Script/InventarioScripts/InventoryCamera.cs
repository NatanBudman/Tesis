using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryCamera : MonoBehaviour
{
    [SerializeField] private float RotateVelocity;
    public Transform Player;
    public Transform Camera_Inventory;
   
    public void ClickCameraRotateRight()
    {
        Camera_Inventory.RotateAround(Player.position,Vector3.up,RotateVelocity * Time.deltaTime);
    }

    public void ClickCameraRotateLeft()
    {
        Camera_Inventory.RotateAround(Player.position,Vector3.down,RotateVelocity * Time.deltaTime);
    }
}
