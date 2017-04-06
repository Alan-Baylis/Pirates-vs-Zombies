using UnityEngine;
using System.Collections;

public class DublinDisplayer : MonoBehaviour {

    private bool All4DublinsAdded = false;

    public GameObject hiddenChest;
    public GameObject dublinVisualParent;

    private InventoryHandler inventory;
    private int currentCount = 0;

    void Start()
    {
        inventory = GetComponent<InventoryHandler>();
    }

	// Update is called once per frame
	void LateUpdate()
    {        
        if(currentCount != inventory.items.Count && !All4DublinsAdded)
        {
            ClearTable();

            int count = 0;
            for (int i = 0; i < inventory.items.Count; i++)
            {
                if(inventory.items[i].GetComponent<Item_Quest>())
                {
                    dublinVisualParent.transform.GetChild(count).gameObject.SetActive(true);
                    count++;
                }

                if (count >= dublinVisualParent.transform.childCount - 1)
                {
                    All4DublinsAdded = true;
                    hiddenChest.GetComponent<BoxCollider>().enabled = true;
                    break;
                }
            }
                currentCount = inventory.items.Count;
        }
	}


    void ClearTable()
    {
        for(int i = 0; i < dublinVisualParent.transform.childCount; i++)
        {
            dublinVisualParent.transform.GetChild(i).gameObject.SetActive(false);
        }
    }
}
