using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{

    public Canvas uiCanvas;
    public static GameObject selectedItem;


    // Top UI for unit selector
    public Image ui_unitSelectorPrefab;
    public Sprite ui_unitSelector_defaultSprite;
    public Sprite ui_unitSelector_deadSprite;
    public int unitSelector_distanceBetween = 10;
    public int unitSelector_rightOffset = 10;
    public int unitSelector_topOffset = 30;

    // Face Camera UI
    public RawImage faceCamCanvas;
    public int unitFaceCam_Resolution = 300;

    // Inventory UI
    public Image ui_inventoryPrefab;
    public float inventoryWindowOffset = 2f;

    // Inventory Button
    public Image ui_inventoryButton;
    // Inspector Button
    public Image ui_inspectorButton;

    public GameObject ui_itemButtonSelection;
    public Image ItemPointer;

    private UnitHandler uh;
    private ArrayList unitSelectors;
    private RawImage faceCam;

    private Image unitInventory;
    private Image externalInventory;

    // inspector mode check
    private bool isInInspectorMode = false;

    // Use this for initialization
    void Start()
    {
        // Error Handling
        if (uiCanvas == null)
            DebugHandler.printError("UIHandler", "No Canvas was found, please add one");

        if ((uh = GetComponent<UnitHandler>()) == null)
            DebugHandler.printError("UIHandler", "No UnitHandler was found on this object, please add one");

        #region UI - Unit Selector 
        // UI - Unit Selector (in the top)
        if (ui_unitSelectorPrefab == null)
            DebugHandler.printWarning("UIHandler", "No UI Image (prefab) was found, if you want unit selection, then please add one");
        else
        {
            // if no default image is found, then simply print out a warning message
            if (ui_unitSelector_defaultSprite == null)
                DebugHandler.printWarning("UIHandler", "No default icon was found, if none is found, then a white box will be displayed");

            unitSelectors = new ArrayList(uh.size);
            // Loop through all the unit actors and add a selection box for each one
            for (int i = 0; i < uh.size; i++)
            {
                Image img = Instantiate(ui_unitSelectorPrefab);

                // Game Object properties
                img.transform.SetParent(uiCanvas.transform);
                img.transform.position = new Vector2((unitSelector_rightOffset + img.rectTransform.rect.width + unitSelector_distanceBetween) * (i + 1),
                                                      uiCanvas.GetComponent<RectTransform>().rect.height - (img.rectTransform.rect.height / 2) - unitSelector_topOffset);   // divided by 2, since the anchor point is in the center of the image
                img.transform.name = "inst_UI_UnitSelector" + i;

                // +Properties handling
                for (int j = 0; j < img.transform.childCount; j++)
                {
                    // Placing a face icon on the unit selector (if none is found, then put the default one)
                    if (img.transform.GetChild(j).tag == TagHandler.UI_ICON)
                    {
                        Sprite tempSprite = ui_unitSelector_defaultSprite;

                        if (uh[i].GetComponent<UnitController>().Icon != null)
                            tempSprite = uh[i].GetComponent<UnitController>().Icon;

                        img.transform.GetChild(j).GetComponent<Image>().sprite = tempSprite;
                    }
                }

                // Setting the max health of the player and his current Health
                img.GetComponent<UI_UnitSelectorController>().MaxHealth = uh[i].GetComponent<UnitController>().maxHealth;

                unitSelectors.Add(img.GetComponent<UI_UnitSelectorController>());
            }
        }
        #endregion

        // FaceCam
        if (faceCamCanvas == null)
            DebugHandler.printWarning("UIHandler", "No RawImage was found to hold the face cam");


        #region Inventory
        // Inventory init
        if (ui_inventoryPrefab == null)
            DebugHandler.printError("UIHandler", "No UI Inventory prefab was found");
        else
        {
            if (ui_inventoryPrefab.GetComponent<BoxCollider2D>() == null)
                DebugHandler.printError("UIHandler", "No box collider 2d was found! This component makes sure that you can drag items out of the units inventory");
            else
            {
                unitInventory = CreateInventoryWindow("inst_ui_Inventory_player(PRIMARY)", 0f);
                externalInventory = CreateInventoryWindow("inst_ui_Inventory_external(SECONDARY)", inventoryWindowOffset);

                unitInventory.GetComponent<UI_InventoryController>().ExternalInventoryBoundsReference = externalInventory.GetComponent<BoxCollider2D>();
                externalInventory.GetComponent<UI_InventoryController>().ExternalInventoryBoundsReference = unitInventory.GetComponent<BoxCollider2D>();
            }
        }
        #endregion

        // Inventory Button
        if (ui_inventoryButton == null)
            DebugHandler.printWarning("UIHandler", "No inventory button was found!");

        if (ui_inspectorButton == null)
            DebugHandler.printWarning("UIHandler", "No inspector button was found!");

        // If the unit has a UnitMovementController scripts attached then give it a reference to this UIHandler, so that it can manipulate it
        for (int i = 0; i < uh.size; i++)
        {
            if (uh[i].GetComponent<UnitMovementController>() != null)
                uh[i].GetComponent<UnitMovementController>().UI_Reference = this;
        }
    }

    void FixedUpdate()
    {
        #region Unit Selector
        // The Unit Selector at the top of the screen
        int i = 0;
        foreach(UI_UnitSelectorController unitSelect in unitSelectors)
        {
            if (unitInventory.GetComponent<UI_InventoryController>().isOpen)
                unitSelect.gameObject.SetActive(false);
            else
            {
                unitSelect.gameObject.SetActive(true);

                // update the player's health
                unitSelect.Health = uh[i].GetComponent<UnitController>().Health;

                // Check if the button click event on the UI_UnitSelectorController was triggered
                if (unitSelect.EventTriggered)
                {
                    this.GetComponent<MouseController>().ManualSelect(i);
                    unitSelect.EventTriggered = false;
                }
                i++;
            }
            
        }
        #endregion

        #region Face Cam
        // The bottom left face cam
        if(uh.numberOfUnitSelected == 1)
        {
            if(faceCamCanvas != null)
            {
                Camera tempCamera;
                if(tempCamera = uh.CurrentUnit.GetComponentInChildren<Camera>())
                {
                    RenderTexture cameraTexture = new RenderTexture(unitFaceCam_Resolution, unitFaceCam_Resolution, 1);
                    tempCamera.targetTexture = cameraTexture;
                    faceCamCanvas.texture = cameraTexture;

                    faceCamCanvas.gameObject.SetActive(true);
                }
            }
        }
        else
            faceCamCanvas.gameObject.SetActive(false);
        #endregion       

        #region Inventory Button
        if (uh.CurrentUnit == null && ui_inventoryButton != null)
            ui_inventoryButton.color = Color.gray;
        else if(ui_inventoryButton != null)
            ui_inventoryButton.color = Color.white;
        #endregion

        #region Inspector Button
        if(ui_inspectorButton != null)
        {
            if (uh.CurrentUnit == null)
                ui_inspectorButton.color = Color.gray;
            else
            {
                ui_inspectorButton.color = Color.white;
                if (unitInventory != null && unitInventory.GetComponent<UI_InventoryController>().isOpen)
                    isInInspectorMode = false;
            }
        }            
        #endregion

        if (selectedItem != null)
        {
            if (!ItemPointer.gameObject.activeSelf)
            {
                ItemPointer.sprite = selectedItem.GetComponent<Item>().icon;
                ItemPointer.gameObject.SetActive(true);
            }

            ItemPointer.transform.position = Input.mousePosition;
        }
        else
            ItemPointer.gameObject.SetActive(false);

        if(UI_InventoryController.inventoryChanged)
        {
            updateItemSelection();
            UI_InventoryController.inventoryChanged = false;
        }
            
    }

    public void updateItemSelection()
    {
        if (uh.CurrentUnit != null)
        {
            for (int j = 0; j < ui_itemButtonSelection.transform.childCount; j++)
            {
                UI_ItemButtonController boxInfo = ui_itemButtonSelection.transform.GetChild(j).GetComponent<UI_ItemButtonController>();
                boxInfo.Clear();
                for (int a = 0; a < uh.CurrentUnit.GetComponent<InventoryHandler>().items.Count; a++)
                {
                    boxInfo.AddItem(uh.CurrentUnit.GetComponent<InventoryHandler>().items[a]);
                }
            }
        }
    }

    // Creates an Inventory Window
    private Image CreateInventoryWindow(string InstanceName, float spawnOffset)
    {
        Image currInventory = Instantiate(ui_inventoryPrefab);
        currInventory.transform.SetParent(uiCanvas.transform);
        currInventory.transform.position = new Vector2(unitSelector_rightOffset + currInventory.rectTransform.rect.width * 3 + spawnOffset, 
                                                       uiCanvas.GetComponent<RectTransform>().rect.height - (currInventory.rectTransform.rect.height*3) - unitSelector_topOffset);
        currInventory.name = InstanceName;

        // if the core controls of the inventory window is not present, then "forcefully" add it
        if(currInventory.GetComponent<UI_InventoryController>() == null)
            currInventory.gameObject.AddComponent<UI_InventoryController>();

        currInventory.GetComponent<UI_InventoryController>().closeInventory();

        return currInventory;
    }


    // Inventory UI Handling
    public void openPlayerInventory()
    {
        if (uh.CurrentUnit != null && uh.numberOfUnitSelected == 1)
        {
            unitInventory.GetComponent<UI_InventoryController>().FaceIcon = uh.CurrentUnit.GetComponent<UnitController>().unitFaceIcon;
            unitInventory.GetComponent<UI_InventoryController>().UnitName = uh.CurrentUnit.transform.name;
            unitInventory.GetComponent<UI_InventoryController>().openInventory(uh.CurrentUnit.GetComponent<InventoryHandler>());
        }
    }

    public void openExternalInventory(InventoryHandler inventory)
    {
        openPlayerInventory();
        if (unitInventory.GetComponent<UI_InventoryController>().isOpen)
        {
            if (inventory.GetComponent<UnitController>() != null && inventory.GetComponent<UnitController>().Icon != null)
                externalInventory.GetComponent<UI_InventoryController>().FaceIcon = inventory.GetComponent<UnitController>().Icon;
            else
                externalInventory.GetComponent<UI_InventoryController>().UI_unitFace.gameObject.SetActive(false);
            
            externalInventory.GetComponent<UI_InventoryController>().UnitName = inventory.transform.name;
            externalInventory.GetComponent<UI_InventoryController>().openInventory(inventory);

            externalInventory.GetComponent<UI_InventoryController>().ExternalInventoryReference = uh.CurrentUnit.GetComponent<InventoryHandler>();
            unitInventory.GetComponent<UI_InventoryController>().ExternalInventoryReference = inventory;
        }
    }

    public void removeItem()
    {
        for(int i = 0; i < ui_itemButtonSelection.transform.childCount; i++)
        {
            if(ui_itemButtonSelection.transform.GetChild(i).GetComponent<UI_ItemButtonController>().HoldsItemType == selectedItem.GetComponent<Item>().Type)
            {
                uh.CurrentUnit.GetComponent<InventoryHandler>().removeItem(selectedItem);
                ui_itemButtonSelection.transform.GetChild(i).GetComponent<UI_ItemButtonController>().RemoveItem(selectedItem);
                selectedItem = null;
                break;
            }
        }
    }

    // Inspect button
    public void ActivateInspectionMode()
    {
        if(uh.CurrentUnit != null)
            isInInspectorMode = true;
    }

    public bool isInspectorModeActivated
    {
        get { return isInInspectorMode; }
        set { isInInspectorMode = value; }
    }

    public Image ExternalInventoryPrefab
    {
        get { return externalInventory; }
    }

    public bool isInventoryWindowOpen
    {
        get { return uh.CurrentUnit != null && unitInventory.GetComponent<UI_InventoryController>().isOpen; }
    }
}