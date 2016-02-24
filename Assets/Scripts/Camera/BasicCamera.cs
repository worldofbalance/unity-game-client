using UnityEngine;
using System.Collections;

public class BasicCamera : MonoBehaviour {
	
	public int boundary = 1000;
	public int speed = 200;
	public int zoom_min = 8;
	public int zoom_max = 2;
	public Vector3 start_pos = new Vector3(0, 300, -12);

	// Use this for initialization
	void Start () {
	
	}

	public void cameraPan() {
		// Check Mouse Activity
		int mouse_up = Screen.height - boundary < Input.mousePosition.y && Input.mousePosition.y < Screen.height  ? 1 : 0;
		int mouse_down = 0 < Input.mousePosition.y && Input.mousePosition.y < boundary ? 1 : 0;
		int mouse_left = 0 < Input.mousePosition.x  && Input.mousePosition.x < boundary  ? 1 : 0;
		int mouse_right = Screen.width - boundary < Input.mousePosition.x && Input.mousePosition.x < Screen.width ? 1 : 0;

		int vAxisM = mouse_up - mouse_down;
		int hAxisM = mouse_right - mouse_left;
		vAxisM = hAxisM = 0;
		// Check Keyboard Activity
		int arrow_up = Input.GetAxis("Vertical") > 0 ? 1 : 0;
		int arrow_down = Input.GetAxis("Vertical") < 0 ? 1 : 0;
		int arrow_left = Input.GetAxis("Horizontal") < 0 ? 1 : 0;
		int arrow_right = Input.GetAxis("Horizontal") > 0 ? 1 : 0;
		arrow_up = arrow_down = 0;
		int vAxisA = arrow_up - arrow_down;
		int hAxisA = arrow_right - arrow_left;
		
		// Determine Pan Distance (Keyboard Priority)
		int vAxis = vAxisA != 0 ? vAxisA : vAxisM;
		int hAxis = hAxisA != 0 ? hAxisA : hAxisM;
		
		float xPos = transform.position.x;
		float zPos = transform.position.z;

		xPos += hAxis * speed * Time.deltaTime;
		zPos += vAxis * speed * Time.deltaTime;

		// Confine Boundaries
		xPos = xPos < -boundary ? -boundary : xPos;
		xPos = xPos > boundary ? boundary : xPos;
		
		zPos = zPos < -boundary ? -boundary : zPos;
		zPos = zPos > boundary ? boundary : zPos;
		
		// Perform Camera Pan
		transform.position = new Vector3(xPos, transform.position.y, zPos);
	}

	public void cameraZoom() {
		Vector3 lastPos = transform.position;

		float zAxisValue = Input.GetAxis("Mouse ScrollWheel");
	    transform.Translate(new Vector3(0, 0, zAxisValue));
		
		float zoom_level = transform.position.y;
		
		if (zoom_level < zoom_max || zoom_level > zoom_min) {
			transform.position = lastPos;
		}
	}

	// Update is called once per frame
	void Update () {
		cameraPan();
		cameraZoom();
		
		// Center Camera
		if (Input.GetKey(KeyCode.Space)) {
			transform.position = start_pos;
		}
	}
}
