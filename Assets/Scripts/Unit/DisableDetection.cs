using UnityEngine;
using System.Collections;
using RAIN.Entities;

public class DisableDetection : MonoBehaviour {

    UnitSelected us;
    EntityRig entity;

    void Start()
    {
        us = transform.parent.GetComponent<UnitSelected>();
        entity = GetComponent<EntityRig>();
    }
	
	// Update is called once per frame
	void Update () 
    {
        if (us.unitDead)
            entity.Entity.Aspects[0].IsActive = false;
	}
}
