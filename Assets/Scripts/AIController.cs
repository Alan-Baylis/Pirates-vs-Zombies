using UnityEngine;
using System.Collections;
using RAIN.Core;

public class AIController : UnitController {

    public float dmg = 10f;
    public int dmgPerSecond = 3;
    public GameObject Brain;

    private int counter = 0;

    void Start()
    {
        if(Brain.GetComponent<AIRig>() == null)
        {
            DebugHandler.printError("AIController", "No brain found!");
        }

        us = GetComponent<UnitSelected>();
    }

    void OnTriggerStay(Collider col)
    {
        if(!us.unitDead)
        {
            switch (col.gameObject.tag)
            {
                case TagHandler.PLAYER:
                    col.GetComponent<UnitController>().damage(dmg * Time.deltaTime);
                    break;

                case TagHandler.ITEM:
                    if (col.gameObject.GetComponent<Item_Throwable>())
                        GetComponent<InventoryHandler>().AddItem(col.gameObject);
                    break;
            }
        }
        
    }


    void Update()
    {
        if(us.unitDead)
        {
            if (Brain != null)
                Brain.GetComponent<AIRig>().AI.IsActive = false;

            tag = TagHandler.INVENTORY;
        }
    }
}
