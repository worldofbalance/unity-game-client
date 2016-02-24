using UnityEngine;
using System.Collections;

public class ClashBattleCamera : MonoBehaviour {

    private SphereCollider reticle = null;
    private Terrain surface = null;
    private Bounds bounds;
    private Vector3 offset;

    public Terrain target {
        set {
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

    public bool dragging = false;
    public Vector3 lastMouse;
    public float yawSpeed = 5.0f;
    public float pitchSpeed = 5.0f;
    public float moveSpeed = 5.0f;
    public float zoomLevel = 100.0f;

	// Update is called once per frame
    void OnDrawGizmosSelected() {
        Gizmos.DrawCube(bounds.center, bounds.size);
    }

	void Update() {
        if (!reticle) return;

        if (Input.GetMouseButtonDown(1)) {
            dragging = true;
            lastMouse = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(1))
            dragging = false;

        var offset = reticle.transform.position - transform.position;
        var tempX = Vector3.Cross(Vector3.up, offset).normalized;
        var tempZ = Vector3.Cross(tempX, Vector3.up).normalized;

        Debug.DrawRay(reticle.transform.position, tempX, Color.red);
        Debug.DrawRay(reticle.transform.position, tempZ, Color.blue);

        if (dragging) {
            var delta = Input.mousePosition - lastMouse;
            transform.RotateAround(reticle.transform.position, Vector3.up, yawSpeed * delta.x);
            transform.RotateAround(reticle.transform.position, tempX, -pitchSpeed * delta.y);

            // Counter-rotate to offset overpitch.
            if (transform.rotation.eulerAngles.x > 80.0f) {
                if (transform.position.y > 0.0f) {
                    transform.RotateAround(reticle.transform.position, tempX, -1.0f * (transform.rotation.eulerAngles.x - 85.0f));
                } else {
                    transform.RotateAround(reticle.transform.position, tempX, 360.0f - transform.rotation.eulerAngles.x);
                }
            }
            transform.LookAt(reticle.transform, Vector3.up);
            lastMouse = Input.mousePosition;
        } else {
            dragging = false;
        }

        // Calculate the attempted translation.
        var attempt = (tempX * Input.GetAxis("Horizontal")) + (tempZ * Input.GetAxis("Vertical")) * moveSpeed;

        // Attempt to move the reticle, ensuring it remains on the board.
        var height = surface.SampleHeight(reticle.transform.position + attempt);
        var valid = bounds.Contains(reticle.transform.position + attempt);

        attempt.y = height - reticle.transform.position.y;

        if (!valid) return;

        // If succesful, translate both the camera transform and the reticle by the same amount.
        reticle.transform.Translate(attempt);
        transform.Translate(attempt, Space.World);

        // Handle zoom.
        var zoomAxis = (transform.position - reticle.transform.position).normalized;
        if (Input.GetKey(KeyCode.Q)) {
            zoomLevel = Mathf.Max(20.0f, zoomLevel - 10.0f);
        }

        if (Input.GetKey(KeyCode.Z)) {
            zoomLevel = Mathf.Min(100.0f, zoomLevel + 10.0f);
        }

        transform.position = reticle.transform.position + (zoomAxis * zoomLevel);
        transform.LookAt(reticle.transform);
	}
}
