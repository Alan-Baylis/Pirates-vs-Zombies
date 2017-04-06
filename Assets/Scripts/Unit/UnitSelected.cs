using UnityEngine;
using System.Collections;

public class UnitSelected : MonoBehaviour {

    public bool unitSelected = false;
    public bool unitDead = false;
    public bool freezeUnit = false;

    private bool projectorPresent = true;
    public Projector unitSelectHighlight = null;
    public SpriteRenderer playerMapIcon;
    public Color originalIconColor = Color.white;
    public Color selectedIconHiglight = Color.blue;
    private void Start()
    {
        if (unitSelectHighlight == null)
            projectorPresent = false;   
        else
            projectorPresent = true;

        if(playerMapIcon == null)
            DebugHandler.printWarning(this.name + "- UnitSelected", "No Sprite was found! in order to change the colour of selected player, then add one");

    }

    private void Update()
    {
        if(projectorPresent)
        {
            unitSelectHighlight.gameObject.SetActive(unitSelected);
        }

        if (unitSelected)
            playerMapIcon.color = selectedIconHiglight;
        else
            playerMapIcon.color = originalIconColor;
    }


    public bool isProjectorPresent
    {
        get { return projectorPresent; }
    }
}
