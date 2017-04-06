using UnityEngine;
using System.Collections;

public class MapCompletion : MonoBehaviour {

    public GameObject youWinText;
    private InventoryHandler inventory;

    private int currNumberOfItems = 0;
	// Use this for initialization
	void Start () {
        inventory = GetComponent<InventoryHandler>();
	}
	
	// Update is called once per frame
	void Update () {

        if(inventory.items.Count != currNumberOfItems)
        {
            for(int i = 0; i < inventory.items.Count; i++)
            {
                if(inventory.items[i].GetComponent<Item_Quest>() && inventory.items[i].name == "Final Trophy")
                {
                    youWinText.SetActive(true);
                }
            }
        }

	}
}
