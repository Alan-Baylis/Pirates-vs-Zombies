using UnityEngine;
using System.Collections;

public class Item_Consumable : Item {

    public int HP = 10;

    void Start()
    {
        itemType = ItemType.Consumable;
    }

    public override bool Action(GameObject target)
    {
        if(isUnit(target) && target.tag == TagHandler.PLAYER && 
           target.GetComponent<UnitController>().Health != target.GetComponent<UnitController>().maxHealth)
        {
            target.GetComponent<UnitController>().heal(HP);
            return true;
        }

        return false;
    }
}
