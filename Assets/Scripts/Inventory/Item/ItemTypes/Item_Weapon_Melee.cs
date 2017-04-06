using UnityEngine;
using System.Collections;

public class Item_Weapon_Melee : Item
{

    public int hitChanceInPercent = 80;
    public int damage = 10;

    void Start()
    {
        itemType = ItemType.Weapon_Melee;
    }

    public override bool Action(GameObject target)
    {
        if(isUnit(target) && target.tag == TagHandler.ENEMY)
        {
            if (PercentageCalculator.isWithinRange(hitChanceInPercent))
                target.GetComponent<UnitController>().damage(damage);
        }

        return false;
    }
}
