using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static List<GameObject> Players;

    private static Transform SpawnPoint;

    public GameObject PlayerCamera;

    private void Awake()
    {
    
    }

    // Start is called before the first frame update
    void Start()
    {
        SpawnPoint = GameObject.FindGameObjectWithTag("Spawn").gameObject.transform;
        
        Players = new List<GameObject>(Resources.LoadAll<GameObject>("Players"));
        
        Debug.Log(Players.Count);
        
        SetPause(false);

        InstacePlayer();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void SetPause(bool pause)
    {
        if (pause)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    public static void InstacePlayer()
    {
        foreach (var Player in Players)
        {
                Debug.Log("Player intanciado");
                Instantiate(Player, SpawnPoint.position, Quaternion.identity);
             
        }
    }

   
}
