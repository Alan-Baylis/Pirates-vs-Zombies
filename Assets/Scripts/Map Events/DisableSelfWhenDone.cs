using UnityEngine;
using System.Collections;

public class DisableSelfWhenDone : MonoBehaviour {

    ActivateSlideText slide;
	
    void Start()
    {
        slide = GetComponent<ActivateSlideText>();
    }

	// Update is called once per frame
	void Update () {

        if (slide.isRead)
            gameObject.SetActive(false);
	}
}
