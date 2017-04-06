/*
 *  ---- MouseController ----
 *  This class handles all the universal controls of the mouse.
 *  This means:
 *  - selecting the units trough LEFT CLICK (single click, shift + click, click and drag)
 *  - spacing out the units if multiple are selected at the same time
 *  - managing the unit active state
 *  - hovering: over items, doors, e.t.c
 * 
 *  This class is very top level, as it handles every mouse control that is not specific to unit dependency, 
 *      for example double clicking to run. This is relies on the unit itself, and therefor is placed in UnitController
 */



using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class MouseController : MonoBehaviour
{

    public Texture2D dragBoxTexture = null;
    [Range(0f, 1f)]
    public float dragBoxTransparency = .3f; // max values between 0 and 1
    public Color dragBoxColour = new Color(0, 0, 0);
    public float dragActivationBound = 50f;

    public float cusrorWidth = 2f;
    public float cursorHeight = 2f;
    public Texture2D cursor_main;
    public Texture2D cursor_inspect;
    public Texture2D cursor_grab;

    private Vector2 dragOrigin = -Vector2.one;
    private bool isDragging = false;
    private Rect dragBox;
    
    private UnitHandler uh;
    private UIHandler ui_h;

    private GameObject currentUnit;
    private GameObject currentTarget;
    private InventoryHandler SecondaryInventory;
    private bool useInv = false;
    private bool useItem = false;

    private void Start()
    {
        uh = this.GetComponent<UnitHandler>();
        ui_h = this.GetComponent<UIHandler>();

        if(cursor_main == null || cursor_inspect == null || cursor_grab == null)
            DebugHandler.printWarning("MouseController", "Missing cursor texture, using defaults"); 
    }


    // Handling the drawing of the dragging box
    private void OnGUI()
    {
        if (dragOrigin != -Vector2.one)
        {
            GUI.color = new Color(dragBoxColour.r, dragBoxColour.g, dragBoxColour.b, dragBoxTransparency);
            GUI.DrawTexture(dragBox, dragBoxTexture);
        }
    }

    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Change cursor to main or inspect, depending on if inspection mode is activated
        Cursor.SetCursor((ui_h.isInspectorModeActivated) ? cursor_inspect : cursor_main, Vector2.zero, CursorMode.ForceSoftware);

        if(useItem)
        {
            if (Vector3.Distance(uh.CurrentUnit.transform.position, currentTarget.transform.position) <= UIHandler.selectedItem.GetComponent<Item>().usageDistance || UIHandler.selectedItem.GetComponent<Item_Throwable>())
            {
                UIHandler.selectedItem.transform.SetParent(uh.CurrentUnit.transform);
                UIHandler.selectedItem.transform.position = uh.CurrentUnit.GetComponent<UnitController>().itemDropper.transform.position;
                if (UIHandler.selectedItem && UIHandler.selectedItem.GetComponent<Item>().Action(currentTarget))
                {
                    ui_h.removeItem();
                    currentTarget = null;
                }
            }

            useItem = false;
        }


        // Checks the pending inventory
        if (useInv)
        {
            if(currentUnit.GetComponent<UnitMovementController>().InventoryWithinReach)
            {
                ui_h.openExternalInventory(SecondaryInventory);
                currentUnit = null;
                SecondaryInventory = null;
                ui_h.isInspectorModeActivated = false;
                useInv = false;
            }
        }
            

        if (Physics.Raycast(ray, out hit) && !ui_h.isInventoryWindowOpen && (!EventSystem.current.IsPointerOverGameObject(-1) || UIHandler.selectedItem != null)) // EventSystem is to make sure that you cannot click through the UI, the "-1" is to check for the mouse position
        {
            // Set cursor to be item selection
            if (Input.GetAxisRaw("Multi Select") == 1 && hit.transform.tag == TagHandler.ITEM)
                Cursor.SetCursor(cursor_grab, Vector2.zero, CursorMode.ForceSoftware);

            #region Left Click
            // ------------------ LEFT CLICK
            if (Input.GetButtonDown("Fire1"))
            {
                if (!isDragging)
                    dragOrigin = Input.mousePosition;

                // ----------- If the player only wants to select a single unit
                if ((hit.transform.tag == TagHandler.PLAYER || hit.transform.tag == TagHandler.INVENTORY) && !UIHandler.selectedItem) // Checks to see if a playable unit is being clicked, if not then deselect all units
                {
                    if(ui_h.isInspectorModeActivated)
                    {
                        // Setting up a pending inventory check (making sure that the player has to walk to the given inventory before opening it)
                        currentUnit = uh.CurrentUnit;
                        SecondaryInventory = hit.transform.GetComponent<InventoryHandler>();
                        useInv = true;
                    }
                    else
                    {
                        if (Input.GetAxisRaw("Multi Select") == 1)  // If Shift is being pressed then it allows multiple selection, otherwise only one unit can be select at a time
                        {
                            if (uh.isSelected(hit.transform.gameObject)) // If shift is pressed, then it allows you to deselect selected units whilst still having the others selected
                                uh.deselect(hit.transform.gameObject);
                            else
                                uh.selectWithoutAutoDeselect(hit.transform.gameObject);
                        }
                        else
                            uh.select(hit.transform.gameObject);    // (auto deselect previous selection)


                        ui_h.updateItemSelection();
                    }     
                }
                else if(UIHandler.selectedItem)
                {
                    useItem = true;
                    currentTarget = hit.transform.gameObject;
                }

                else
                {
                    if(Input.GetAxisRaw("Multi Select") != 1 )
                        uh.deselectAll();

                    ui_h.isInspectorModeActivated = false;
                }
                    
            } // end button down
            #endregion

            // reset drag box
            if (Input.GetButtonUp("Fire1"))
            {
                dragBox.width = 0;
                dragBox.height = 0;
                dragOrigin = -Vector2.one;
                isDragging = false;
            }

            #region Left Click - Drag
            // Dragging
            if (Input.GetButton("Fire1"))
            {
                isDragging = dragBox.width > dragActivationBound || dragBox.height > dragActivationBound;

                dragBox = new Rect(dragOrigin.x, MouseYToRect(dragOrigin.y), Input.mousePosition.x - dragOrigin.x, MouseYToRect(Input.mousePosition.y) - MouseYToRect(dragOrigin.y));

                if (dragBox.width < 0)
                {
                    dragBox.x += dragBox.width;
                    dragBox.width = -dragBox.width;
                }

                if (dragBox.height < 0)
                {
                    dragBox.y += dragBox.height;
                    dragBox.height = -dragBox.height;
                }


                for (int i = 0; i < uh.size; i++)
                {
                    Vector3 screenPos = Camera.main.WorldToScreenPoint(uh[i].transform.position);
                    Vector2 screenPoint = new Vector2(screenPos.x, Screen.height - screenPos.y);

                    if (dragBox.Contains(screenPoint))
                        uh.selectWithoutAutoDeselect(uh[i]);
                    else
                    {
                        if (isDragging)
                            uh.deselect(uh[i]);
                    }
                }
            } // end LEFT CLICK
            #endregion

            #region Right Click
            if (Input.GetButtonDown("Fire2"))
            {
                ui_h.isInspectorModeActivated = false; // right clicking terminates inspector mode
                UIHandler.selectedItem = null; // right clicking terminates current Item

                // Handling the movement of multiple units. If more than 1 is moved, than increase the stoppingDistance, so that they won't run into each other
                if (uh.numberOfUnitSelected > 1)
                {
                    for (int i = 0; i < uh.size; i++)
                    {
                        if (uh.isSelected(uh[i]))
                            uh[i].GetComponent<NavMeshAgent>().stoppingDistance = uh[i].GetComponent<NavMeshAgent>().radius * uh.numberOfUnitSelected - 1;
                    }
                }
                else
                {
                    if(uh.CurrentUnit != null)
                        uh.CurrentUnit.GetComponent<NavMeshAgent>().stoppingDistance = 0;
                }  
            }
            #endregion
            
        }

        
    }

    // If you want to manually select a unit
    public void ManualSelect(int index)
    {
            uh.select(uh[index]);
    }

    // This class converts the standard (0,0) being in the bottom left corner, to be in the top left corner. Simply to make calculations easier.
    // It is static for easier access by other scripts.
    private float MouseYToRect(float y)
    {
        return Screen.height - y;
    }
}

