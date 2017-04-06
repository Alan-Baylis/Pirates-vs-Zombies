using UnityEngine;
using System.Collections;

public class projectorRotation : MonoBehaviour {

    public float speed = 10f;
	
	// Update is called once per frame
	void Update () {
        if (this.transform.gameObject.activeSelf)
            this.transform.Rotate(Vector3.forward, speed * Time.deltaTime);
	}
}
