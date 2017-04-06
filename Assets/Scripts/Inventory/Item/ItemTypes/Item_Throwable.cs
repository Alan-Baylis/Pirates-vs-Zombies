using UnityEngine;
using System.Collections;

public class Item_Throwable : Item {

    public float throwForce = 800f;

    void Start()
    {
        itemType = ItemType.Throwable;
    }

    public override bool Action(GameObject target)
    {
        if (!isUnit(target) && target.tag != TagHandler.PLAYER)
        {
            gameObject.SetActive(true);
            GetComponent<Rigidbody>().isKinematic = false;
            GetComponent<Rigidbody>().AddForce(transform.parent.transform.TransformDirection(Vector3.forward).normalized * throwForce, ForceMode.VelocityChange);
            transform.parent = null;
        }

        return true;
    }
}
