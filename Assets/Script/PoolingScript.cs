using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics.Tracing;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using JetBrains.Annotations;
using UnityEngine;
using Object = UnityEngine.Object;

public class PoolingScript : MonoBehaviour
{
    private static GameObject poolingParent;

    private static GameObject[] poolingChild;
    
    private static int ChildPoolingCount ;

    private static List<GameObject> pooling;

    private static bool IsHaveItem = false;

    private static GameObject[] findObjectRemove;

    private void Awake()
    {
        if (PoolingScript.poolingParent == null)
        {
            PoolingScript.poolingParent = this.gameObject;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        
        ChildPoolingCount = poolingParent.transform.childCount;
        poolingChild = new GameObject[ChildPoolingCount];
        
        // Agrega todos los prefabs que hay en la carpeta Items
        pooling = new List<GameObject>(Resources.LoadAll<GameObject>("Pooling"));
        
        Debug.Log($"GameObject Added to the Pool : {pooling.Count} (Sucesfull)");
    
    }


    public static void Instantiate(int id,Transform ObjectPosition,Transform parent, Quaternion rotacion)
    {
        GetChildInPool(id);
        
        foreach (GameObject ObjectInPool in poolingChild)
        {

            if (ObjectInPool.GetComponent<Items>().gameObject != null && IsHaveItem)
            {
                if (ObjectInPool.GetComponent<Items>().ID == id)
                {
                    Debug.Log($"Found Object ID :{id} (Sucesfull)");

                    ObjectInPool.SetActive(true);
                    ObjectInPool.gameObject.transform.position = ObjectPosition.position;
                    ObjectInPool.gameObject.transform.rotation = rotacion;
                    
                    if (parent != null)
                    {
                        ObjectInPool.gameObject.transform.SetParent(parent);
                    }
                }
            }
        }

        if (poolingChild.Length < 0 || IsHaveItem == false)
        {
            Debug.Log("No Found Object ");

            foreach (var FindObject in pooling)
            {
                if (FindObject.GetComponent<Items>().ID == id)
                {
                    Debug.Log($"Create New Item ID: {id} (Sucesfull)");

                    Instantiate(FindObject,ObjectPosition.position,rotacion,parent);
                }
            }
        }
        
        IsHaveItem = false;
     
    }

    public static void Remove(GameObject ObjectRemove)
    {
        Debug.Log($"Remove Item Name : {ObjectRemove.name} (Sucesfull) ");
        ObjectRemove.SetActive(false);
        ObjectRemove.transform.SetParent(poolingParent.transform);
    }
    
    public static void RemovedAll(int id, string TagObject, string TypeObject)
    {
        findObjectRemove = GameObject.FindGameObjectsWithTag(TagObject);
        if (TypeObject == null)
        {
            for (int i = 0; i < findObjectRemove.Length; i++)
            {
                if (findObjectRemove[i].GetComponent<Items>().ID == id)
                {
                    findObjectRemove[i].gameObject.SetActive(false);
                    findObjectRemove[i].gameObject.transform.SetParent(poolingParent.transform);
                }
            }
        }

        if (TypeObject != null)
        {
            for (int i = 0; i < findObjectRemove.Length; i++)
            {
                if (findObjectRemove[i].GetComponent<Items>().type == TypeObject)
                {
                    findObjectRemove[i].gameObject.SetActive(false);
                    findObjectRemove[i].gameObject.transform.SetParent(poolingParent.transform);
                }
            }
        }
    }
    
    // pre cargar recursos reutilizables
    public static void PreLoad(int id,int amount)
    {
            foreach (var FindObject in pooling)
            {
                if (FindObject.GetComponent<Items>().ID == id)
                {
                    Debug.Log($"Pre Load New Items ID: {id} (Sucesfull)");

                    for (int i = 0; i < amount; i++)
                    {
                        Instantiate(FindObject,poolingParent.transform.position,Quaternion.identity,poolingParent.transform);
                    }

                }
            }
    }
    private static void GetChildInPool(int id)
    {
        Debug.Log("Find Object in PoolParent");
        ChildPoolingCount = poolingParent.transform.childCount;
        
        for (int i = 0; i < ChildPoolingCount; i++) 
        {
            poolingChild[i] = poolingParent.transform.GetChild(i).gameObject;
        }
        
        for (int i = 0; i < ChildPoolingCount; i++)
        {
            if (IsHaveItem == false)
            {
                if (poolingChild[i].gameObject.GetComponent<Items>().ID == id)
                {
                    Debug.Log("Found Object : (Sucesfull)");
                    IsHaveItem = true;
                }
            }
        }
    }
}
