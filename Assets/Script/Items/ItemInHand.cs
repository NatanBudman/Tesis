using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInHand : MonoBehaviour
{
    [SerializeField]
    private Items item;
    [SerializeField]
    private GameObject RightHand;
    private GameObject LeftHand;
    // Start is called before the first frame update
    void Start()
    {
        item = GetComponent<Items>();

        RightHand = GameObject.FindGameObjectWithTag("Hand");
        LeftHand = GameObject.FindGameObjectWithTag("LeftHand");
    }

    // Update is called once per frame
    void Update()
    {
        
        if (item.equipped && item.type == "Weapon")
        {
            
            this.transform.position = RightHand.transform.position;
            this.transform.rotation = RightHand.transform.rotation;
            
        }

        if (item.equipped && item.type == "Shield")
        {
            this.transform.position = LeftHand.transform.position;
            this.transform.rotation = LeftHand.transform.rotation;
        }
    }
}
