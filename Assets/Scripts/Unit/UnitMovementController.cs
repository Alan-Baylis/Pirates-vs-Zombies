using UnityEngine;
using System.Collections;

public class UnitMovementController : UnitController {

    private UIHandler uiReference;

    public UIHandler UI_Reference
    { set { uiReference = value; } }


    private bool move = false;
    private bool itemWithinReach = false;
    private GameObject item;
    private InventoryHandler currentInventory;
    private GameObject unitTarget;

    private bool inventoryWithinReach = false;
    public bool InventoryWithinReach { get { return inventoryWithinReach; } }

	// Update is called once per frame
	void Update ()
    {

        if (us.unitSelected && !us.unitDead && !us.freezeUnit)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit) && (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(-1) || UIHandler.selectedItem))
            {
                // Left click - pick up item
                #region Left Click
                if (Input.GetButtonDown("Fire1"))
                {
                    // Select item to pickup
                    if (Input.GetAxisRaw("Multi Select") == 1 && hit.transform.tag == TagHandler.ITEM)
                    {
                        item = hit.transform.gameObject;
                        move = true;
                        agent.speed = walkingSpeed; // makes sure that you cannot double click with RMB and then just fly around using LMB
                    }
                    else if (uiReference.isInspectorModeActivated || UIHandler.selectedItem != null)
                        move = true;
                        
                }
                #endregion

                // Right click - move and running mechanic
                #region Right Click
                if (Input.GetButtonDown("Fire2") && hit.transform.tag != TagHandler.ITEM && !uiReference.isInspectorModeActivated && UIHandler.selectedItem == null)
                {
                    move = true;

                    // Running mechanic (double click)
                    if (Time.time - doubleClickStart < 0.3f)
                    {
                        agent.speed = runningSpeedAmplifier * walkingSpeed;
                        doubleClickStart = -1;
                    }
                    else
                    {
                        agent.speed = walkingSpeed;
                        doubleClickStart = Time.time;
                    }
                } // end fire2
                #endregion
                
                if (UIHandler.selectedItem && Vector3.Distance(this.transform.position, hit.transform.position) < UIHandler.selectedItem.GetComponent<Item>().usageDistance)
                {
                    move = false;
                    agent.ResetPath();
                }
            }

            // Move unit
            if (move)
            {
                agent.SetDestination(hit.point);
                move = false;
            }

                

            // pick up item
            if (item != null && item.activeSelf && itemWithinReach)
            {
                Inventory.AddItem(item);
                item.SetActive(false);
                item = null;
                itemWithinReach = false;
                
                if(uiReference != null)
                    uiReference.updateItemSelection();
            }
        }
	}

    public bool ManuallyMoveToTarget(RaycastHit target)
    {
        return agent.SetDestination(target.point);
    }

    // Within Range checks
    void OnTriggerEnter(Collider col)
    {
        switch(col.gameObject.tag)
        {
            case TagHandler.ITEM:
                itemWithinReach = item != null ? item.activeSelf : false;
                break;
            case TagHandler.PLAYER:
            case TagHandler.INVENTORY:
                inventoryWithinReach = true;
                break;
            case TagHandler.STORYTRIGGER:
                col.GetComponent<ActivateSlideText>().RunSlide();
                break;
        }
    }

    void OnTriggerExit(Collider col)
    {
        switch (col.gameObject.tag)
        {
            case TagHandler.ITEM:
                itemWithinReach = false;
                break;
            case TagHandler.PLAYER:
            case TagHandler.INVENTORY:
                inventoryWithinReach = false;
                break;
        }
    }
}
