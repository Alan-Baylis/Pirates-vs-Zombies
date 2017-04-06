using UnityEngine;
using System.Collections;

public class UI_ItemButtonHandler : MonoBehaviour {

    GameObject currentUnit;
    public GameObject CurrentUnit
    {
        get { return currentUnit; }
        set { currentUnit = value; }
    }
}
