using UnityEngine;
using System.Collections;

public class rotate3DItem : MonoBehaviour {

    public float speed = 20f;

    void Start()
    {
        GetComponent<Rigidbody>().isKinematic = true;
    }

    void LateUpdate()
    {
        transform.Rotate(new Vector3(0, speed * Time.deltaTime, 0));
    }
}
