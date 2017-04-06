/*
 *  Camera Controller 
 *  -----------------
 *  #Script uses controls provided by the InputManager
 *  This scripts adds controls to the camera. These controls include:
 *  - camera movement using the "Horizontal" and "Vertical" movements as well as "Mouse X" and "Mouse Y" controls.
 *  - camera rotation around a point*1 using the combination of "Fire3" and "Mouse X" (Required to hold down "Fire3" before using "Mouse X"
 *  - camera zoom using the "Mouse ScrollWheel"
 *  - speed up : using a custom Input called "Speed up" on the PC that is Shift and on the controller "ADD CONTROLLER HERE" 
 * 
 *  *1: This point is to be added as a parent of the camera (the object holding this script)
 * 
 */

using UnityEngine;

public class CameraController : MonoBehaviour {

    // Speed properties
    public int movementSpeed = 10;
    public float speedIncrease = 2;
    public int mousePanMovementSpeed = 2;
    public int rotationSpeed = 20;
    public int scrollSpeed = 10;
    public float edgeBounce = .01f;

    public float mousePanSensitivity = 1f;

    public GameObject BoundingBox;

    private Vector2 minBounds;
    private Vector2 maxBounds;
    private bool boundingBoxIsPresent = true;

    // World point properties

    /* This handles the constant movement, since when you rotate the camera the movement stays the same 
     * (that implies that moving forward makes it look like you're going, for example left). 
     * This object alters it to always stay constant. */
    private GameObject movementPoint;

    private Transform rotationPoint;
    private float amplify;
    
    void Start()
    {
        rotationPoint = this.transform.parent;
        amplify = 1;

        movementPoint = new GameObject("inst_camera_movement");
        movementPoint.transform.position = this.transform.position;

        if(rotationPoint == null)
            Debug.LogError("!No parent was found on the Camera Object (with the CameraController component)! Camera needs a parent to operate! This parent is what controls the rotation (The camera rotates around this object");

        if(BoundingBox == null)
        {
            Debug.LogWarning("!Warning! No bounding box was found; the bounding box allows you to limit the camera movement to a specific distance (so not to go completely out of bound");
            boundingBoxIsPresent = false;
        } 
        else
        {
            Renderer mapBounds = BoundingBox.GetComponent<Renderer>();

            if (mapBounds == null)
                Debug.LogError("ERROR: " + BoundingBox.name + " does not have a renderer!");

            minBounds = new Vector2(BoundingBox.transform.position.x - mapBounds.bounds.size.x / 2, BoundingBox.transform.position.z - mapBounds.bounds.size.z / 2);
            maxBounds = new Vector2(BoundingBox.transform.position.x + mapBounds.bounds.size.x / 2, BoundingBox.transform.position.z + mapBounds.bounds.size.z / 2);

            Debug.Log("Camera Bounds created!");

        }

        this.transform.LookAt(rotationPoint.transform); // Makes sure that the camera looks at the point at all time (for rotation purposes)
    }


    void Update ()
    {
        movementPoint.transform.position = this.transform.position;

        amplify = (Input.GetAxisRaw("Jump") == 1.0f) ? speedIncrease : 1;

        // ------- Controls the player movement
        TranslateCamera(Vector3.forward, Input.GetAxisRaw("Vertical"), movementSpeed);
        TranslateCamera(Vector3.right, Input.GetAxisRaw("Horizontal"), movementSpeed);

        // -------- Controls the scrolling mechanic
        this.transform.Translate(Vector3.forward * Input.GetAxisRaw("Mouse ScrollWheel") * scrollSpeed * amplify * Time.deltaTime, Space.Self);

        // --------- Controls the rotation mechanic
        if (Input.GetAxisRaw("Fire3") == 1.0f)
        {
            this.transform.RotateAround(rotationPoint.transform.position, Vector3.up, Input.GetAxis("Mouse X") * rotationSpeed * amplify * Time.deltaTime); // rotates around the y axis
            movementPoint.transform.RotateAround(rotationPoint.transform.position, Vector3.up, Input.GetAxis("Mouse X") * rotationSpeed * amplify * Time.deltaTime);

            this.transform.LookAt(rotationPoint.transform); // Makes sure that the camera looks at the point at all time (for rotation purposes)
        }
        else
        {
            //  --------- Camera movement based on mouse touching the edge
                // This is written in this "slavery" fashion because this seems to make it run most smoothly
            if(Input.GetAxisRaw("Fire1") != 1f) // So that you can use the drag functionality in the MouseController without moving the camera meanwhile
            {
                if (Input.mousePosition.x < mousePanSensitivity)
                    TranslateCamera(Vector3.left, mousePanMovementSpeed, 1);
                else if (Input.mousePosition.x >= Screen.width - mousePanSensitivity)
                    TranslateCamera(Vector3.right, mousePanMovementSpeed, 1);

                if (Input.mousePosition.y < mousePanSensitivity)
                    TranslateCamera(Vector3.back, mousePanMovementSpeed, 1);
                else if (Input.mousePosition.y >= Screen.height - mousePanSensitivity)
                    TranslateCamera(Vector3.forward, mousePanMovementSpeed, 1);
            }
        }

        

	}

    private void TranslateCamera(Vector3 direction, float force, float additionalSpeed)
    {
        Vector3 translation = direction * force * additionalSpeed * amplify * Time.deltaTime;

        // Out of boundaries - handling
        if (boundingBoxIsPresent)
        {
            Vector2 position = new Vector2(rotationPoint.transform.position.x, rotationPoint.transform.position.z);

            if (position.x > maxBounds.x)
                translation.x = -edgeBounce;
            else if (position.x < minBounds.x)
                translation.x = edgeBounce;

            if (position.y > maxBounds.y)
                translation.z = -edgeBounce;
            else if (position.y < minBounds.y)
                translation.z = edgeBounce;
        }


            rotationPoint.transform.Translate(translation, movementPoint.transform);
    }
}
