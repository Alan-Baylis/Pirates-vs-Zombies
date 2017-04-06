using UnityEngine;
using System.Collections;

public class Unfreeze : MonoBehaviour {

    ActivateSlideText slide;
    public GameObject frozenUnit;

    private UnitSelected us;

    void Start()
    {
        us = frozenUnit.GetComponent<UnitSelected>();
        Debug.Log(frozenUnit);
        slide = GetComponent<ActivateSlideText>();
    }

    void Update()
    {
        if(slide.isRead)
        {
            us.freezeUnit = false;
            this.gameObject.SetActive(false);
        }
    }
}
