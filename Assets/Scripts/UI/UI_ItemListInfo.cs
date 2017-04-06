using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_ItemListInfo : MonoBehaviour {

    public Text itemName;
    public Text itemCount;
    public Image icon;

    void Start()
    {
        if (itemName == null || itemCount == null)
            DebugHandler.printError("UI_ItemListInfo", "label missing!");
        if (icon == null)
            DebugHandler.printError("UI_ItemListInfo", "Image missing!");

        Clear();
    }

    public void Clear()
    {
        itemName.text = "";
        itemCount.text = "";

        icon.sprite = null;
        icon.gameObject.SetActive(false);
    }

    // Getters!
    public string ItemName
    {
        set { itemName.text = value; }
    }

    public int ItemCount
    {
        set { itemCount.text = value.ToString() + "x"; }
    }

    public Sprite IconSprite
    {
        set { icon.sprite = value; }
    }

}
