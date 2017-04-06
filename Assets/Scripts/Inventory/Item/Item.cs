using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour {

    public int weight = 0;
    public Sprite icon;
    public bool isStackable = true;
    public float usageDistance = 5f;

    protected ItemType itemType = ItemType.QuestItem;
    private int numOfItems = 1;
    

    public int ItemCount
    {
        get { return numOfItems; }
        set { numOfItems = (value < 0) ? 0 : value; }
    }

    public ItemType Type
    {
        get { return itemType; }
    }

    public virtual bool Action(GameObject target)
    {
        return false;
    }

    protected bool isUnit(GameObject target)
    {
        return target.GetComponent<UnitController>() != null;
    }

    protected bool isWithinRange(GameObject target)
    {
        if (isUnit(target))
            return target.GetComponent<UnitMovementController>().InventoryWithinReach;

        return false;
    }
}
