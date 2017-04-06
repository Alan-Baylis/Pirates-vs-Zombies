/*
 *      ---- UnitController ----
 *  This class is in charge of handling everything that every player should be able to do interdependently
 *  This is somewhat of an arbitrary Unit class, as all units can do what is put in here, but cannot be put in the MouseControl class, as it is very dependent on the units themselves.
 *  
 *  such as:
 *      - path finding based on the click
 *      - running from right double click
 *  
 * 
 */



using UnityEngine;
using System.Collections;

public class UnitController : MonoBehaviour
{
    public float maxHealth = 100; 
    public float runningSpeedAmplifier = 2f;

    public Sprite unitFaceIcon;
    public Transform itemDropper;

    // Movement properties
    protected float walkingSpeed = 0f;
    protected float doubleClickStart;

    // component handlers
    protected UnitSelected us;
    protected NavMeshAgent agent;

    // unit stats
    private float health = 0;
    private int count = 0;
    
    // Inventory
    protected InventoryHandler inventory;

    void Awake()
    {
        us = GetComponent<UnitSelected>();
        agent = GetComponent<NavMeshAgent>();

        inventory = GetComponent<InventoryHandler>();
        if (inventory == null)
            DebugHandler.printError("UnitController - " + this.name, "No InventoryHandler script was found!");
        else if(itemDropper == null)
        {
            DebugHandler.printError("UnitController - " + this.name, "No item dropper game object was found!");
        }

        walkingSpeed = agent.speed;

        heal(maxHealth);
    }


    // Getter functions
    public float WalkingSpeed
    {
        get { return walkingSpeed; }
    }

    public float Health
    {
        get { return health; }
    }

    public Sprite Icon
    {
        get { return unitFaceIcon; }
        set { unitFaceIcon = value; }
    }

    public InventoryHandler Inventory
    {
        get { return inventory; }
    }

    // --------------- Actions
        // Take Damage
    public void damage(float damageAmount)
    {
        if ((health -= damageAmount) <= 0)
        {
            us.unitDead = true;
            health = 0;
        } 
    } 
        // Heal
    public void heal(float healthAmount)
    {
        if((health += healthAmount) > maxHealth)
        {
            us.unitDead = false;
            health = maxHealth;
        }
    }
}