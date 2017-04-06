using UnityEngine;
using System.Collections;

public class MapZoomHandler : MonoBehaviour {

    public float maxZoom_out = 12f;
    public float maxZoom_in = 3f;
    public float zoomPerClick = 1f;
    public Camera cameraMap;

    private int zoom = 0;

    void Start()
    {
        if (cameraMap == null)
            DebugHandler.printError("MapZoomHandler", "no camera was found!");
    }
	
	// Update is called once per frame
	void Update () {

        if (zoom != 0)
        {
            // Makes sure that the zooming doesn't go out of bounds
            if (cameraMap.orthographicSize < maxZoom_in)
                cameraMap.orthographicSize = maxZoom_in;
            else if (cameraMap.orthographicSize > maxZoom_out)
                cameraMap.orthographicSize = maxZoom_out;



            if(zoom < 0)
                cameraMap.orthographicSize += zoomPerClick;
            else if (zoom > 0)
                cameraMap.orthographicSize -= zoomPerClick;

            zoom = 0;
        }


        
	}

    public void ButtonClick_ZoomEvent(int direction)
    {
            zoom = direction;
    }
}
