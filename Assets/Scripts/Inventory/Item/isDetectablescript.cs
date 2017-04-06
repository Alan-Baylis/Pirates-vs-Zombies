using UnityEngine;
using System.Collections;
using RAIN.Entities;

public class isDetectablescript : MonoBehaviour {

    EntityRig entity;

    void Start()
    {
        entity = GetComponent<EntityRig>();
        SetState(transform.parent.gameObject.activeSelf);
    }

    void OnEnable()
    {
        SetState(true);
    }

    void OnDisable()
    {
        SetState(false);
    }

    void SetState(bool state)
    {
        if(entity != null)
            entity.Entity.Aspects[0].IsActive = state;
    }
}