using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using System.Collections.Generic;

public class UI_InventoryController : MonoBehaviour
{
    public static bool inventoryChanged = false;

    public Image UI_unitFace;
    public Text UI_unitName;
    public Transform Container;
    public Text UI_totalWeight;
    public Transform Item3DPos;
    public RawImage Item3DCanvas;
    public Camera Item3DRenderer;
    public int CameraResolution  = 300;

    private GameObject curr3DItem;
    private InventoryHandler currInventory;
    private GameObject itemBox;
    private Vector2 startPos;
    private RenderTexture curr3DItemRendTexture;

    private BoxCollider2D externalInventoryBoundsReference; // stores a reference to the other item inventory
    private InventoryHandler externalInventoryReference;

    public BoxCollider2D ExternalInventoryBoundsReference
    { set { externalInventoryBoundsReference = value; } }

    public InventoryHandler ExternalInventoryReference
    { set { externalInventoryReference = value; } }

    void Start()
    {
        if (Container == null)
            DebugHandler.printError("UI_InventoryController", "no Container was found!");

        if (Item3DRenderer == null)
            DebugHandler.printError("UI_InventoryController", "no camera was found!");
        else
        {
            curr3DItemRendTexture = new RenderTexture(CameraResolution, CameraResolution, 1);
            Item3DRenderer.targetTexture = curr3DItemRendTexture;
            Item3DCanvas.texture = curr3DItemRendTexture;
        }
    }

    // +---------------------------- Setter methods
    public Sprite FaceIcon  
    {
        set { UI_unitFace.transform.GetChild(0).GetComponent<Image>().sprite = value; }
    }

    public string UnitName
    {
        set { UI_unitName.text = value; }
    }

    // +---------------------------- Closing the inventory
    public void closeInventory()
    {
        if (externalInventoryBoundsReference != null)
            externalInventoryBoundsReference.gameObject.SetActive(false);

        this.gameObject.SetActive(false);
    }

    // +---------------------------- Opening & updating the inventory
    public void openInventory(InventoryHandler unitInventory)
    {
        for (int i = 0; i < Container.childCount; i++)
            Container.transform.GetChild(i).GetComponent<UI_ItemListInfo>().Clear();

        // Populate list
        for (int i = 0; i < Container.childCount && i < unitInventory.items.Count; i++)
        {
            UI_ItemListInfo item = Container.transform.GetChild(i).GetComponent<UI_ItemListInfo>();

            if (unitInventory.items[i].GetComponent<Item>().isStackable)
            {
                int numOfItems = unitInventory.numOfItem(unitInventory.items[i].name);
                item.ItemCount = numOfItems;
            }
                

            item.IconSprite = unitInventory.items[i].GetComponent<Item>().icon;
            item.icon.gameObject.SetActive(item.icon != null);

            item.ItemName = unitInventory.items[i].transform.name;
        }

        // Populate Inventory details
        UI_totalWeight.text = unitInventory.Weight.ToString() + "/" + unitInventory.maxWeight.ToString();


        Destroy(curr3DItem); // to prevent it being copied over to other players inventories


        this.gameObject.SetActive(true);

        currInventory = unitInventory;
    }

    public void selectItem(int index)
    {
        if(currInventory != null && index < currInventory.items.Count)
        {
            Destroy(curr3DItem);
            curr3DItem = Instantiate(currInventory.items[index], Item3DPos.position, Quaternion.identity) as GameObject;
            curr3DItem.AddComponent<rotate3DItem>();
            curr3DItem.SetActive(true);
        }
    }

    // +------------------------ Dragging mechanic
    public void StartDragItem(GameObject box)
    {
        if(currInventory != null && currInventory.items.Count > 0)
        {
            itemBox = box;
            startPos = itemBox.transform.position;
        }
        
    }

    public void DraggingItem()
    {
        if (itemBox != null)
            itemBox.transform.position = Input.mousePosition;
    }

    public void StopDragItem(int index)
    {
        if(itemBox != null)
        {
            // IF the item box is dragged outside of the inventory boundaries
            if(!isWithinBounds(itemBox.transform.position, GetComponent<BoxCollider2D>()))
            {
                if(index < currInventory.items.Count)
                {
                    // IF the external inventory is open and IF the item box is inside its boundaries
                    if (externalInventoryReference != null && isWithinBounds(itemBox.transform.position, externalInventoryBoundsReference.GetComponent<BoxCollider2D>()))
                    {
                        if (externalInventoryReference.AddItem(currInventory.items[index]))
                            currInventory.removeItem(currInventory.items[index]);

                        externalInventoryBoundsReference.GetComponent<UI_InventoryController>().openInventory(externalInventoryReference);
                    }
                    else
                        currInventory.DropItem(currInventory.items[index]);
                }
            }

            Debug.Log(externalInventoryReference == null);

            openInventory(currInventory);
            itemBox.transform.position = startPos;

            inventoryChanged = true;
        } 
    }

    
    public bool isOpen
    {
        get { return gameObject.activeSelf; }
    }

    private bool isWithinBounds(Vector2 pos, BoxCollider2D inventory)
    {
        Bounds bounds = inventory.bounds;
        return pos.x >= bounds.min.x && pos.y >= bounds.min.y && 
               pos.x <= bounds.max.x && pos.y <= bounds.max.y; 
    }
}


/*
// ELSE IF the item is within the bounds of this one, put is not within the bounds of the external inventory, then add to this inventory
            else if (externalInventoryReference != null && externalInventoryBoundsReference.GetComponent<UI_InventoryController>().isWithinBounds(itemBox.transform.position, GetComponent<BoxCollider2D>()))
            {
                if (index < externalInventoryReference.items.Count)
                {
                    if (currInventory.AddItem(externalInventoryReference.items[index]))
                        externalInventoryReference.removeItem(externalInventoryReference.items[index]);
                }
            }

*/