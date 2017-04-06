using UnityEngine;
using System.Collections;

public class CreditsScript : MonoBehaviour {


    public GameObject CreditWindow;

    void Start()
    {
        if (CreditWindow == null)
            DebugHandler.printError("CreditScript", "Missing CreditWindow");
    }

    public void CreditButtonEvent()
    {
        CreditWindow.gameObject.SetActive(!CreditWindow.gameObject.activeSelf);
    }
}
