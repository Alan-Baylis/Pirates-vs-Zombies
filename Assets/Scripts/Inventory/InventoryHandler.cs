using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InventoryHandler : MonoBehaviour {

    public int maxWeight = 20;
    public  List<GameObject> items;

    private int currentWeight = 0;

    void Start()
    {
        // init the inventory with given items
        foreach(GameObject item in items)
        {
            if (isItem(item))
            {
                currentWeight += item.GetComponent<Item>().weight;

                // if the weight is to create, then simply drop the rest of the items
                if(currentWeight > maxWeight)
                {
                    DropItem(item);
                }
            }
            else
                items.Remove(item);
        }
    }

    public void DropItem(GameObject item)
    {
        item.transform.position = this.GetComponent<UnitController>().itemDropper.position;
        item.gameObject.SetActive(true);
        removeItem(item);
    }

    public void removeItem(GameObject item)
    {
        currentWeight -= item.GetComponent<Item>().weight;
        items.Remove(item);
    }

    public int numOfItem(string itemName)
    {
        int i = 1;
        if(items.Count > 1)
        {
            foreach (GameObject item in items)
            {
                if (item.name == itemName)
                    i++;
            }
        }
        

        return i;
    }

    public bool AddItem(GameObject item)
    {
        if(isItem(item) && currentWeight + item.GetComponent<Item>().weight <= maxWeight)
        {
            items.Add(item);
            currentWeight += item.GetComponent<Item>().weight;
            item.SetActive(false);

            return true;
        }

        return false;
    }

    public int Weight
    {
        get { return currentWeight; }
    }


    // check to see if the item has an "item" tag
    private bool isItem(GameObject item)
    {
        return item.tag == TagHandler.ITEM && item.GetComponent<Item>() != null;
    }
}
