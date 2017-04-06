using UnityEngine;
using System.Collections;

public class StateHandler : MonoBehaviour {

    public string sceneName = "";

	public void ShutDownButtonEvent()
    {
        Application.Quit();
    }

    public void GoToSceneButtonEvent()
    {
        Application.LoadLevel(sceneName);
    }
}
