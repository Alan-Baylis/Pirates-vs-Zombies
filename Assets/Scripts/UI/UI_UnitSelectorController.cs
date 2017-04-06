using UnityEngine;
using System.Collections;

public class UI_UnitSelectorController : MonoBehaviour {
        
    private float currHealth = 100;
    private float maxHealth = 100;
    private bool eventTriggered = false;

    private float healthYScale = 0; 
    private Transform hpController;
    void Start()
    {
        // get the max y scale (pulling the 
        for(int i = 0; i < transform.childCount; i++)
        {
            if(transform.GetChild(i).tag == TagHandler.UI_HEALTH)
            {
                hpController = transform.GetChild(i);
                healthYScale = hpController.localScale.y;
                break;
            }
        }
    }

	// Update is called once per frame
	void Update () 
    {
        hpController.localScale = new Vector3(hpController.localScale.x, calculateHealth(), hpController.localScale.z);
	}

    public void ButtonClickEvent()
    {
        eventTriggered = true;
    }

    private float calculateHealth()
    {
        // (maxHealth - currHealth) is because otherwise 0 is full and 100 is no health
        return (maxHealth - currHealth) / 100 * healthYScale;
    }

    public bool EventTriggered
    {
        set { eventTriggered = value; }
        get { return eventTriggered; }
    }

    public float MaxHealth
    {
        set { maxHealth = value; }
    }
    
    public float Health
    {
        set { currHealth = value; }
    }
}
