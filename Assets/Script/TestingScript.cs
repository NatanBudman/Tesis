using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingScript : MonoBehaviour
{
    public Transform pos;

    public int ID;

    public int amount;

    public GameObject weapons;
    
    public bool RemoveALL;

    private void Awake()
    {

    }

    private void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (amount > 0)
        {
            PoolingScript.Instantiate(weapons,-1,pos,this.gameObject.transform,Quaternion.identity);
            amount--;
        }

        if (amount < 0)
        {
           PoolingScript.Remove(weapons);
           amount++;
        }

        if (RemoveALL)
        {
            PoolingScript.RemovedAll(ID,"Item","Weapon");
        }
        
    }
}
