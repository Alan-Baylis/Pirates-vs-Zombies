using UnityEngine;
using System.Collections;   
using System.Collections.Generic;
using UnityEngine.UI;

public class UI_ItemButtonController : MonoBehaviour {

    public ItemType HoldsItemType;
    public Image itemIconCanvas;

    public Image moreItemsButton;
    public Image moreItemsBackground;
    
    public Image itemSelectionButton;
    public Color itemSelectionColor_Default;
    public Color itemSelectionColor_Selected;

    // More items properties
    private Vector2 originalPosition;
    private bool moreItemsOpen = false;

    private GameObject currentUnit;
    private GameObject currentItem;
    private List<GameObject> itemReference;

    public GameObject CurrentUnit
    {
        set { currentItem = value; }
    }
    
    void Start()
    {
        if (itemIconCanvas == null)
            DebugHandler.printWarning("UI_ItemButtonController", "ItemIconCanvas is missing (an image to display the icon on)");

        if (moreItemsButton == null)
            DebugHandler.printWarning("UI_ItemButtonController", "MoreItemsButton is missing");
        else
        {
            if(moreItemsBackground == null)
                DebugHandler.printWarning("UI_ItemButtonController", "MoreItemsBackground is missing");
            else
            {
                originalPosition = moreItemsButton.transform.position;
                moreItemsBackground.gameObject.SetActive(false);

                // Set the parameter of each button event (button 0 returning an index value of = 0)
                for(int i = 0; i < moreItemsBackground.transform.childCount; i++)
                {
                    int number = i;
                    moreItemsBackground.transform.GetChild(i).GetComponent<Button>().onClick.AddListener(() => this.SelectItem(number));
                }
            }
        }

        itemReference = new List<GameObject>();
        UpdateButton();
        UpdateList();
    }

    public void AddItem(GameObject item)
    {
        if(item.GetComponent<Item>().Type == HoldsItemType)
        {
            if (itemReference.Count == 0)
                currentItem = item;

            itemReference.Add(item);
            UpdateButton();
            UpdateList();
        }
    }

    public void RemoveItem(GameObject item)
    {
        itemReference.Remove(item);

        if (itemReference.Count == 0)
            currentItem = null;
        else
        SelectItem(0);

        UpdateButton();
        UpdateList();

    }

    public void Clear()
    {
        itemReference.Clear();
        currentItem = null;

        UpdateButton();
        UpdateList();
    }

    public void UpdateButton()
    {
        if (itemReference.Count > 0 && itemIconCanvas != null)
        {
            itemIconCanvas.gameObject.SetActive(true);
            itemIconCanvas.sprite = currentItem.GetComponent<Item>().icon;
        }
        else
            itemIconCanvas.gameObject.SetActive(false);
    }

    public void UpdateList()
    {
        // shows all except the first one, because its been shown on the primary button
        for (int i = 0; i < moreItemsBackground.transform.childCount; i++)
        {
            Transform icon = moreItemsBackground.transform.GetChild(i).GetChild(0);

            if (i >= itemReference.Count)
                icon.gameObject.SetActive(false);
            else
            {
                if (itemReference[i] == currentItem)
                    moreItemsBackground.transform.GetChild(i).GetComponent<Image>().color = itemSelectionColor_Selected;
                else
                    moreItemsBackground.transform.GetChild(i).GetComponent<Image>().color = itemSelectionColor_Default;

                icon.gameObject.SetActive(true);
                icon.GetComponent<Image>().sprite = itemReference[i].GetComponent<Item>().icon;
            }
        }
    }

    public void ShowMoreItems()
    {
        moreItemsOpen = !moreItemsOpen; // switch

        moreItemsBackground.gameObject.SetActive(moreItemsOpen);
        if(moreItemsOpen)
        {
            moreItemsButton.transform.position = new Vector2(moreItemsButton.transform.position.x,
                                                             moreItemsBackground.transform.position.y * 2 - moreItemsBackground.rectTransform.rect.yMax*2);
        }
        else
            moreItemsButton.transform.position = originalPosition;
    }


    // +----------------- EVENTS
    public void SelectItem(int index)
    {
        if(index < itemReference.Count)
        {
            currentItem = itemReference[index];
            UpdateButton();
            UpdateList();
        }
    }

    public void UseItem()
    {
        if(currentItem != null)
        {
            UIHandler.selectedItem = currentItem;
        }
    }
}
