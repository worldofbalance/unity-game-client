using UnityEngine;
using System.Collections;

public class ClashBattleCameraOld : MonoBehaviour
{

    private SphereCollider reticle = null;

    //    [SerializeField]
    public Terrain surface = null;

    private Bounds bounds;
    private Vector3 offset;


    public Terrain target {
        set {
            surface = value;
            reticle = new GameObject ("Reticle", typeof(SphereCollider), typeof(ClashReticle)).GetComponent<SphereCollider> ();
            reticle.GetComponent<SphereCollider> ().isTrigger = true;
            reticle.GetComponent<SphereCollider> ().radius = 1.0f;

            // Set the reticle position to the center of the terrain.
            var reticlePos = value.transform.position + (value.terrainData.size * 0.5f);
            reticlePos.y = value.SampleHeight (reticle.transform.position);
            reticle.transform.position = reticlePos;
            reticle.transform.rotation = Quaternion.identity;

            // Set transform position to the initial offset. Assign the offset vector and make camera look at reticle.
            transform.position = reticle.transform.position + ((Vector3.back + Vector3.up).normalized * zoomLevel);
            offset = transform.position - reticle.transform.position;
            transform.LookAt (reticle.transform);


            bounds = new Bounds (surface.terrainData.size * 0.5f, surface.terrainData.size);
        }
    }

    public bool dragging = false;
    public Vector3 lastMouse;
    public float yawSpeed = 5.0f;
    public float pitchSpeed = 5.0f;
    public float moveSpeed = 5.0f;
    public float zoomLevel = 100.0f;

    public float minFOV = 10f;
    public float maxFOV = 79.9f;
    private float minX, maxX, minZ, maxZ, minY = 35.0f, maxY = 80.0f;
    public float terrainCameraPadding = 40;

    Camera _camera;

    // Update is called once per frame
    void OnDrawGizmosSelected ()
    {
        Gizmos.DrawCube (bounds.center, bounds.size);
    }

    public void setDraging (bool isDragging)
    {
        dragging = isDragging;
        lastMouse = Input.mousePosition;
    }

    public void Start ()
    {
        target = surface;

        _camera = Camera.main;
        minX = terrainCameraPadding;
        maxX = Terrain.activeTerrain.terrainData.size.x - terrainCameraPadding;
        minZ = terrainCameraPadding;
        maxZ = Terrain.activeTerrain.terrainData.size.z - terrainCameraPadding;
    }

    public void Update ()
    {
        if (!reticle)
            return;

        if (Input.GetMouseButtonDown (1)) {
            dragging = true;
            lastMouse = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp (1))
            dragging = false;

        var offset = reticle.transform.position - transform.position;
        var tempX = Vector3.Cross (Vector3.up, offset).normalized;
        var tempZ = Vector3.Cross (tempX, Vector3.up).normalized;

        Debug.DrawRay (reticle.transform.position, tempX, Color.red);
        Debug.DrawRay (reticle.transform.position, tempZ, Color.blue);

        if (dragging) {
            var delta = Input.mousePosition - lastMouse;
            transform.RotateAround (reticle.transform.position, Vector3.up, yawSpeed * delta.x);
            transform.RotateAround (reticle.transform.position, tempX, -pitchSpeed * delta.y);
//            if (transform.position.y >= minY && transform.position.y <= maxY)
//                transform.RotateAround(reticle.transform.position, tempX, -pitchSpeed * delta.y);
//            else
//                transform.position = new Vector3(transform.position.x,
//                    Mathf.Clamp(_camera.fieldOfView, minY, maxY),
//                    transform.position.z);

            // Counter-rotate to offset overpitch.
            if (transform.rotation.eulerAngles.x > 80.0f) {
                if (transform.position.y > 35.0f) {
                    transform.RotateAround (reticle.transform.position, tempX, -1.0f * (transform.rotation.eulerAngles.x - 85.0f));
                } else {
                    transform.RotateAround (reticle.transform.position, tempX, 360.0f - transform.rotation.eulerAngles.x);
                }
            }
            transform.LookAt (reticle.transform, Vector3.up);
            lastMouse = Input.mousePosition;
        } else {
            dragging = false;
        }

        // Calculate the attempted translation.
        var attempt = (tempX * Input.GetAxis ("Horizontal")) + (tempZ * Input.GetAxis ("Vertical")) * moveSpeed;

        // Attempt to move the reticle, ensuring it remains on the board.
        var height = surface.SampleHeight (reticle.transform.position + attempt);
        var valid = bounds.Contains (reticle.transform.position + attempt);

        attempt.y = height - reticle.transform.position.y;

        if (!valid)
            return;

        // If succesful, translate both the camera transform and the reticle by the same amount.
        reticle.transform.Translate (attempt);
        transform.Translate (attempt, Space.World);

        CheckZoom ();

    }

    void LateUpdate ()
    {
        Vector3 pos = new Vector3 (
                          Mathf.Clamp (_camera.transform.position.x, minX, maxX),
                          _camera.transform.position.y,
                          Mathf.Clamp (_camera.transform.position.z, minZ, maxZ));
        _camera.transform.position = pos;
    }

    public void CheckZoom ()
    {
        

        // Handle zoom.
        var zoomAxis = (transform.position - reticle.transform.position).normalized;
        if (Input.GetKey (KeyCode.Q)) {
//            zoomLevel = Mathf.Max(20.0f, zoomLevel - 2.0f);
            _camera.fieldOfView -= 1.0f;
        }

        if (Input.GetKey (KeyCode.Z)) {
//            zoomLevel = Mathf.Min(50.0f, zoomLevel + 2.0f);
            _camera.fieldOfView += 1.0f;
        }
        // Clamp the field of view to make sure it's between minFOV and maxFOV.
        _camera.fieldOfView = Mathf.Clamp (_camera.fieldOfView, minFOV, maxFOV);



//        transform.position = reticle.transform.position + (zoomAxis * zoomLevel);
//        transform.LookAt(reticle.transform);
    }
}
