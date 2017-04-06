using UnityEngine;
using System.Collections;

public class Restart : MonoBehaviour {

    public void OnRestartEvent()
    {
        Application.LoadLevel("TreasureIsland");    
    }
}
