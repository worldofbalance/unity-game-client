using UnityEngine;
using System.Collections;

public class ClashBattleCamera : MonoBehaviour
{

    private SphereCollider reticle = null;
    private Terrain surface = null;
    private Bounds bounds;
    private Vector3 offset;

    public Terrain target
    {
        set
        {
            surface = value;
            reticle = new GameObject("Reticle", typeof(SphereCollider), typeof(ClashReticle)).GetComponent<SphereCollider>();
            reticle.GetComponent<SphereCollider>().isTrigger = true;
            reticle.GetComponent<SphereCollider>().radius = 1.0f;

            // Set the reticle position to the center of the terrain.
            var reticlePos = value.transform.position + (value.terrainData.size * 0.5f);
            reticlePos.y = value.SampleHeight(reticle.transform.position);
            reticle.transform.position = reticlePos;
            reticle.transform.rotation = Quaternion.identity;

            // Set transform position to the initial offset. Assign the offset vector and make camera look at reticle.
            transform.position = reticle.transform.position + ((Vector3.back + Vector3.up).normalized * zoomLevel);
            offset = transform.position - reticle.transform.position;
            transform.LookAt(reticle.transform);


            bounds = new Bounds(surface.terrainData.size * 0.5f, surface.terrainData.size);
        }
    }

    //    public Terrain test;

    public bool dragging = false;
    public Vector3 lastMouse;
    public float yawSpeed = 5.0f;
    public float pitchSpeed = 5.0f;
    public float moveSpeed = 5.0f;
    public float zoomLevel = 100.0f;



    // Update is called once per frame
    void OnDrawGizmosSelected()
    {
        Gizmos.DrawCube(bounds.center, bounds.size);
    }

    public void setDraging(bool isDragging)
    {
        dragging = isDragging;
        lastMouse = Input.mousePosition;
    }

    public void Update()
    {
        if (!reticle)
            return;

//        if (Input.GetMouseButtonDown(1))
//        {
//            dragging = true;
//            lastMouse = Input.mousePosition;
//        }
//
//        if (Input.GetMouseButtonUp(1))
//            dragging = false;

        var offset = reticle.transform.position - transform.position;
        var tempX = Vector3.Cross(Vector3.up, offset).normalized;
        var tempZ = Vector3.Cross(tempX, Vector3.up).normalized;

        Debug.DrawRay(reticle.transform.position, tempX, Color.red);
        Debug.DrawRay(reticle.transform.position, tempZ, Color.blue);

        if (dragging)
        {
            var delta = Input.mousePosition - lastMouse;
            transform.RotateAround(reticle.transform.position, Vector3.up, yawSpeed * delta.x);
            transform.RotateAround(reticle.transform.position, tempX, -pitchSpeed * delta.y);

            // Counter-rotate to offset overpitch.
            if (transform.rotation.eulerAngles.x > 80.0f)
            {
                if (transform.position.y > 0.0f)
                {
                    transform.RotateAround(reticle.transform.position, tempX, -1.0f * (transform.rotation.eulerAngles.x - 85.0f));
                }
                else
                {
                    transform.RotateAround(reticle.transform.position, tempX, 360.0f - transform.rotation.eulerAngles.x);
                }
            }
            transform.LookAt(reticle.transform, Vector3.up);
            lastMouse = Input.mousePosition;
        }
        else
        {
            dragging = false;
        }

        // Calculate the attempted translation.
        var attempt = (tempX * Input.GetAxis("Horizontal")) + (tempZ * Input.GetAxis("Vertical")) * moveSpeed;

        // Attempt to move the reticle, ensuring it remains on the board.
        var height = surface.SampleHeight(reticle.transform.position + attempt);
        var valid = bounds.Contains(reticle.transform.position + attempt);

        attempt.y = height - reticle.transform.position.y;

        if (!valid)
            return;

        // If succesful, translate both the camera transform and the reticle by the same amount.
        reticle.transform.Translate(attempt);
        transform.Translate(attempt, Space.World);

        CheckZoom();

//        checkForPinchZoom();
    }

    public void CheckZoom()
    {
        

        // Handle zoom.
        var zoomAxis = (transform.position - reticle.transform.position).normalized;
        if (Input.GetKey(KeyCode.Q))
        {
            zoomLevel = Mathf.Max(20.0f, zoomLevel - 10.0f);
        }

        if (Input.GetKey(KeyCode.Z))
        {
            zoomLevel = Mathf.Min(100.0f, zoomLevel + 10.0f);
        }

        transform.position = reticle.transform.position + (zoomAxis * zoomLevel);
        transform.LookAt(reticle.transform);
    }

    void LateUpdate()
    {
        float pinchAmount = 0;
        Quaternion desiredRotation = transform.rotation;

        DetectTouchMovement.Calculate();

        if (Mathf.Abs(DetectTouchMovement.pinchDistanceDelta) > 0)
        { // zoom
            pinchAmount = DetectTouchMovement.pinchDistanceDelta;
        }

        if (Mathf.Abs(DetectTouchMovement.turnAngleDelta) > 0)
        { // rotate
            Vector3 rotationDeg = Vector3.zero;
            rotationDeg.z = -DetectTouchMovement.turnAngleDelta;
            desiredRotation *= Quaternion.Euler(rotationDeg);
        }


        // not so sure those will work:
        transform.rotation = desiredRotation;
        transform.position += Vector3.forward * pinchAmount;
    }

    //    public float perspectiveZoomSpeed = 0.5f;
    //    // The rate of change of the field of view in perspective mode.
    //    public float orthoZoomSpeed = 0.5f;
    //    // The rate of change of the orthographic size in orthographic mode.
    //    private Camera camera;

    //    void Start()
    //    {
    //        camera = Camera.main;
    //    }

    //    private void checkForPinchZoom()
    //    {
    //        // If there are two touches on the device...
    //        if (Input.touchCount == 2)
    //        {
    //            // Store both touches.
    //            Touch touchZero = Input.GetTouch(0);
    //            Touch touchOne = Input.GetTouch(1);
    //
    //            // Find the position in the previous frame of each touch.
    //            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
    //            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;
    //
    //            // Find the magnitude of the vector (the distance) between the touches in each frame.
    //            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
    //            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;
    //
    //            // Find the difference in the distances between each frame.
    //            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;
    //
    //            // If the camera is orthographic...
    //            if (camera.orthographic)
    //            {
    //                // ... change the orthographic size based on the change in distance between the touches.
    //                camera.orthographicSize += deltaMagnitudeDiff * orthoZoomSpeed;
    //
    //                // Make sure the orthographic size never drops below zero.
    //                camera.orthographicSize = Mathf.Max(camera.orthographicSize, 0.1f);
    //            }
    //            else
    //            {
    //                // Otherwise change the field of view based on the change in distance between the touches.
    //                camera.fieldOfView += deltaMagnitudeDiff * perspectiveZoomSpeed;
    //
    //                // Clamp the field of view to make sure it's between 0 and 180.
    //                camera.fieldOfView = Mathf.Clamp(camera.fieldOfView, 0.1f, 179.9f);
    //            }
    //        }
    //    }
}
