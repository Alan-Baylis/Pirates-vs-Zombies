/*
 * ----- Unit Handler -----
 * The Unit Handler, handles all the players.
 * It stores every player in a list and with it, everything the players share.
 * It manages the communication between scripts and the player info, keeping it simpler and more accessible
 * + a lot of error handling
 * 
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitHandler : MonoBehaviour
{
    public float defaultAgentRadius = 0.45f;
    public float defaultAgentHeight = 2.6f;

    private List<GameObject> units;
    private GameObject currentUnit;

    private int selectionCounter = 0;

    // Use this for initialization
    void Awake()
    {
        units = new List<GameObject>(GameObject.FindGameObjectsWithTag(TagHandler.PLAYER));

        // Error handling
        foreach (GameObject unit in units)
        {
            if (unit.GetComponent<UnitSelected>() == null)
            {
                unit.AddComponent<UnitSelected>();
                DebugHandler.printWarning("UnitController", "UnitSelected script was not found on unit, so it was added " + unit.gameObject.name + " automatically");
            }

            if(unit.GetComponent<NavMeshAgent>() == null)
            {
                NavMeshAgent r = unit.AddComponent<NavMeshAgent>();
                r.radius = defaultAgentRadius;
                r.height = defaultAgentHeight;
                DebugHandler.printWarning("UnitController", "A NavMeshAgent component was not found on unit, so it was added " + unit.gameObject.name + " automatically");
            }

            if(unit.GetComponent<InventoryHandler>() == null)
            {
                unit.AddComponent<InventoryHandler>();
                DebugHandler.printWarning("UnitController", "An InventoryHandler component was not found on unit, so it was added " + unit.gameObject.name + " automatically");
            }
        }

        DebugHandler.print("UnitController", "Number of valid player actors found: " + size);
    }

    // Unit Selection
    public void select(GameObject unit)
    {
        if (isPlayer(unit))
        {
            if (selectionCounter > 1)
                deselectAll();    
            else
            {
                if (currentUnit)
                    deselect(currentUnit);
            }

            unit.GetComponent<UnitSelected>().unitSelected = true;
            currentUnit = unit;
            selectionCounter = 1;
        }
    }

    public void selectWithoutAutoDeselect(GameObject unit)
    {
        if ((selectionCounter < size && !unit.GetComponent<UnitSelected>().unitSelected)) // stops it from being able to shift click same character twice (unless its only one, hence == 0)
            selectionCounter++;

        unit.GetComponent<UnitSelected>().unitSelected = true;

        // Makes sure that the target highlighting (projector) rotation is always in-sync with the previous selected unit ---- Thank you Hanna for this idea <3
        if (currentUnit != null && unit.GetComponent<UnitSelected>().unitSelectHighlight != null && currentUnit.GetComponent<UnitSelected>().unitSelectHighlight != null)
        {
            unit.GetComponent<UnitSelected>().unitSelectHighlight.transform.localEulerAngles = currentUnit.GetComponent<UnitSelected>().unitSelectHighlight.transform.localEulerAngles;
        }
            

        currentUnit = unit;
    }

    public void deselect(GameObject unit)
    {
        if (isPlayer(unit))
        {
            if (selectionCounter > 0 && unit.GetComponent<UnitSelected>().unitSelected)
                selectionCounter--;

            unit.GetComponent<UnitSelected>().unitSelected = false;
        }
    }

    public void deselectAll()
    {
        foreach (GameObject unit in units)
        {
            unit.GetComponent<UnitSelected>().unitSelected = false;
            currentUnit = null;
        }
        selectionCounter = 0;
    }


    // ----------- Getters: Information
    public bool isSelected(GameObject unit)
    {
        if (isPlayer(unit))
            return unit.GetComponent<UnitSelected>().unitSelected;


        DebugHandler.printWarning("UnitController", "isSelected() returned false, because gameobject did not have the " + TagHandler.PLAYER + " tag");
        return false;
    }

    public int size
    {
        get { return units.Count; }
    }

    public string unitTag
    {
        get { return TagHandler.PLAYER; }
    }

    public GameObject this[int i]
    {
        get { return units[i]; }
    }

    public int numberOfUnitSelected
    {
        get { return selectionCounter; }
    }

    public GameObject CurrentUnit
    {
        get { return currentUnit; }
    }

    // ----------- Unit checker
    private bool isPlayer(GameObject unit)
    {
        if (unit.gameObject.tag != TagHandler.PLAYER)
        {
            DebugHandler.printWarning("UnitController", "Given object does not have a player tag");
            return false;
        }

        return true;
    }
}
