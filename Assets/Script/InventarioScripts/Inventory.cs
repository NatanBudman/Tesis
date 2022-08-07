using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class Inventory : MonoBehaviour
{
    private bool IsInventoryEnabled;

    public GameObject inventory;

    private int _allSlots;

    private int _emptySlots;

    private GameObject[] slots;

    public GameObject SlotsHolder;

    public GameObject ObjectSelected;
    public GameObject Canvas;
    
    public Sprite SlotsSprite;

    public GraphicRaycaster GraphicRaycasterrahicRay;
    private PointerEventData pointerEventData;
    private List<RaycastResult> _raycastResults;
    
    public GameObject PoolingItems;
    
    [Header("Extern Inventory")]
    [SerializeField]
    private GameObject TradeInvetory;
    private void Awake()
    {
        PoolingItems = GameObject.FindGameObjectWithTag("PoolingManager");
    }

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

        pointerEventData = new PointerEventData(null);
        _raycastResults = new List<RaycastResult>();
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

        DragItem();

    }

    private GameObject Chess;
    private GameObject ChessInentory;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Item") 
        {
            GameObject ItemPickUpObject = other.gameObject;

            Items item = ItemPickUpObject.GetComponent<Items>();

            AddItem(ItemPickUpObject,item.ID,item.amount,item.type,item.Description,item.icon);

        }

        if (other.tag == "InteractuableObject/Chess")
        {
            ChessInentory = other.gameObject.GetComponent<Chess>().ChessInvetory;
            Chess = other.gameObject;
            ChessInentory.transform.SetParent(TradeInvetory.transform);
            ChessInentory.transform.position = TradeInvetory.transform.position;
        }
    
    }

    private float coolDownOpenChessInventory;
    private void OnTriggerStay(Collider other)
    {
        
        if (other.tag == "InteractuableObject/Chess")
        {
            if (coolDownOpenChessInventory < 0.2f)
            {
                coolDownOpenChessInventory += Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.E) && !TradeInvetory.gameObject.activeSelf && coolDownOpenChessInventory >= 0.2f)
            {
                TradeInvetory.gameObject.SetActive(true);
                ChessInentory.SetActive(true);
                coolDownOpenChessInventory = 0;
            }
            else if (Input.GetKey(KeyCode.E) && TradeInvetory.gameObject.activeSelf && coolDownOpenChessInventory >= 0.2f)
            {
                TradeInvetory.gameObject.SetActive(false);
                ChessInentory.SetActive(false);
                coolDownOpenChessInventory = 0;
            }
           
        }
    }

    private void OnTriggerExit(Collider other)
    {
        ChessInentory.transform.SetParent(Chess.transform);
        ChessInentory.SetActive(false);
        TradeInvetory.SetActive(false);
    }

    void DragItem()
    {
      
        if (Input.GetMouseButtonDown(0))
        {
            pointerEventData.position = Input.mousePosition;
            GraphicRaycasterrahicRay.Raycast(pointerEventData,_raycastResults);

            if (_raycastResults.Count > 0)
            {
                if (_raycastResults[0].gameObject.GetComponent<Slot>())
                {
                    ObjectSelected = _raycastResults[0].gameObject;

                }

                if (_raycastResults[0].gameObject.tag == "InteractuableObject/Canvas")
                {
                    ObjectSelected = _raycastResults[0].gameObject;
                    Debug.Log(ObjectSelected.gameObject.name);

                }
            }

        }
// Objecto selecionado con el raton
        if ( ObjectSelected != null)
        {
            if (ObjectSelected.GetComponent<Slot>() != null)
            {
                if (ObjectSelected.GetComponent<Slot>().empty == false)
                {
                    ObjectSelected.GetComponent<RectTransform>().localPosition = CanvasScreen(Input.mousePosition);

                }
            }

            if (ObjectSelected.GetComponent<Image>() != null)
            {
                if (ObjectSelected.gameObject.tag == "InteractuableObject/Canvas")
                {
                    ObjectSelected.GetComponent<RectTransform>().localPosition = CanvasScreen(Input.mousePosition);
                }
            }
        }

        
        if (Input.GetMouseButtonUp(0))
        {
            pointerEventData.position = Input.mousePosition;
            _raycastResults.Clear();
            GraphicRaycasterrahicRay.Raycast(pointerEventData,_raycastResults);
            if (_raycastResults.Count > 0)
            {
                foreach (var result in _raycastResults)
                {
                  
                    if (result.gameObject.tag == "slot")
                    {
                        ObjectSelected.GetComponent<Slot>();
                        
                        Slot ExtractionSlot = ObjectSelected.GetComponent<Slot>();
                        
                        if (result.gameObject.name == ObjectSelected.gameObject.name)
                        {
                            Debug.Log("Same slot");
                         
                           ObjectSelected.GetComponent<Slot>().gameObject.SetActive(false);
                           ObjectSelected.GetComponent<Slot>().gameObject.SetActive(true);
                        }
                        else if (result.gameObject.name != ObjectSelected.gameObject.name)
                        {                           
                            Debug.Log("Diferent slot");
                            MoveItemSlot(ObjectSelected,result.gameObject,ExtractionSlot.item,
                                ExtractionSlot.ID,ExtractionSlot.amount,ExtractionSlot.type,ExtractionSlot.Description,ExtractionSlot.icon);
                        }
                    }

                    
                }
            }
            ObjectSelected = null;
        }
        _raycastResults.Clear();
    }
// toma datos del items cogido
    public void AddItem(GameObject ItemgameObject, int ItemID,int amount,string ItemType, string ItemDescription, Sprite icon) 
    {
        for (int i = 0; i < _allSlots; i++) 
        {
            if (slots[i].GetComponent<Slot>().empty) 
            {
                Debug.Log($"Pick Up Item Name : {ItemgameObject.name}");
                
                ItemgameObject.GetComponent<Items>().pickUp = true;

                slots[i].GetComponent<Slot>().item = ItemgameObject;
                slots[i].GetComponent<Slot>().ID = ItemID;
                slots[i].GetComponent<Slot>().amount = amount;
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

    public void MoveItemSlot(GameObject SlotExtraction, GameObject SlotReceives,GameObject ItemgameObject, int ItemID,int amount,string ItemType, string ItemDescription, Sprite icon)
    {

        if (SlotReceives.GetComponent<Slot>().empty == true && SlotExtraction.GetComponent<Slot>().empty == false)
        {
            Debug.Log($"Extaction of Slot Item Name : {SlotExtraction.name}");
            Debug.Log($"Move Slot Item Name : {SlotReceives.name}");
            
            SlotExtraction.GetComponent<Slot>();
            
            var SlotExtractioIcon = SlotReceives.GetComponent<Slot>().icon;
            
            SlotReceives.GetComponent<Slot>().item = ItemgameObject;
            SlotReceives.GetComponent<Slot>().ID = ItemID;
            SlotReceives.GetComponent<Slot>().amount = amount;
            SlotReceives.GetComponent<Slot>().type = ItemType;
            SlotReceives.GetComponent<Slot>().Description = ItemDescription;
            SlotReceives.GetComponent<Slot>().icon = icon;
        
            SlotExtraction.GetComponent<Slot>().item = null;
            SlotExtraction.GetComponent<Slot>().ID = 0;
            SlotExtraction.GetComponent<Slot>().amount = 0;
            SlotExtraction.GetComponent<Slot>().type = null;
            SlotExtraction.GetComponent<Slot>().Description = null;
            SlotExtraction.GetComponent<Slot>().icon = SlotExtractioIcon;
        
            ItemgameObject.transform.SetParent(SlotReceives.transform);

            SlotReceives.GetComponent<Slot>().UpdateSlot();
            SlotExtraction.GetComponent<Slot>().UpdateSlot();
            
            SlotReceives.GetComponent<Slot>().empty = false;
            SlotExtraction.GetComponent<Slot>().empty = true;
            return;
        }

        if (SlotExtraction.GetComponent<Slot>().empty == false && SlotReceives.GetComponent<Slot>().empty == false  && SlotReceives.gameObject.name != SlotExtraction.gameObject.name)
        {
            Debug.Log($"Extaction of Slot Item Name : {SlotExtraction.name}");
            Debug.Log($"Move Slot Item Name : {SlotReceives.name}");

            SlotExtraction.GetComponent<Slot>();
            SlotReceives.GetComponent<Slot>();
            // = mismo objeto suma la cantidad
            if (SlotExtraction.GetComponent<Slot>().ID == SlotReceives.GetComponent<Slot>().ID && SlotExtraction.GetComponent<Slot>().gameObject.name != SlotReceives.GetComponent<Slot>().gameObject.name)
            {
                Debug.Log("ID: Equals");

                SlotReceives.GetComponent<Slot>().amount +=
                    SlotExtraction.GetComponent<Slot>().amount;
                
                SlotReceives.GetComponent<Slot>().item = ItemgameObject;
                SlotReceives.GetComponent<Slot>().ID = ItemID;
                SlotReceives.GetComponent<Slot>().type = ItemType;
                SlotReceives.GetComponent<Slot>().Description = ItemDescription;
                SlotReceives.GetComponent<Slot>().icon = icon;
                
                
                SlotExtraction.GetComponent<Slot>().item = null;
                SlotExtraction.GetComponent<Slot>().ID = 0;
                SlotExtraction.GetComponent<Slot>().amount = 0;
                SlotExtraction.GetComponent<Slot>().type = null;
                SlotExtraction.GetComponent<Slot>().Description = null;
                SlotExtraction.GetComponent<Slot>().icon = SlotsSprite;
                
                SlotReceives.GetComponent<Slot>().UpdateSlot();
                SlotExtraction.GetComponent<Slot>().UpdateSlot();
                
                ItemgameObject.transform.SetParent(PoolingItems.transform);
                SlotExtraction.GetComponentInChildren<Items>().gameObject.transform.SetParent(PoolingItems.transform);
            }
            else if (SlotExtraction.GetComponent<Slot>().ID != SlotReceives.GetComponent<Slot>().ID && SlotExtraction.GetComponent<Slot>().gameObject.name != SlotReceives.GetComponent<Slot>().gameObject.name)
            {
                Debug.Log("ID: Diferent");

                SlotExtraction.GetComponent<Slot>().item = SlotReceives.GetComponent<Slot>().item;
                SlotExtraction.GetComponent<Slot>().ID = SlotReceives.GetComponent<Slot>().ID;
                SlotExtraction.GetComponent<Slot>().type = SlotReceives.GetComponent<Slot>().type;
                SlotExtraction.GetComponent<Slot>().Description = SlotReceives.GetComponent<Slot>().Description;
                SlotExtraction.GetComponent<Slot>().icon = SlotReceives.GetComponent<Slot>().icon ;
                
                SlotExtraction.GetComponent<Slot>().amount = SlotReceives.GetComponent<Slot>().amount;
                SlotReceives.GetComponent<Slot>().amount = amount;
                
                SlotReceives.GetComponent<Slot>().item = ItemgameObject;
                SlotReceives.GetComponent<Slot>().ID = ItemID;
                SlotReceives.GetComponent<Slot>().type = ItemType;
                SlotReceives.GetComponent<Slot>().Description = ItemDescription;
                SlotReceives.GetComponent<Slot>().icon = icon;
                
                SlotReceives.GetComponent<Slot>().UpdateSlot();
                SlotExtraction.GetComponent<Slot>().UpdateSlot();
                
                ItemgameObject.transform.SetParent(SlotReceives.transform);
                
            }

          
            return;
        }

     

      
    }

    public Vector2 CanvasScreen(Vector2 ScreenPos)
    {
        Vector2 viewportPoint = Camera.main.ScreenToViewportPoint(ScreenPos);
        Vector2 canvasSize = Canvas.GetComponent<RectTransform>().sizeDelta;

        return (new Vector2(viewportPoint.x * canvasSize.x,viewportPoint.y * canvasSize.y) - (canvasSize/2));
    }

}
