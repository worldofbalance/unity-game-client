using UnityEngine;
using System;
using System.Collections;

public class ConnectionLostGUI : MonoBehaviour {
	
	private GameObject mainObject;

	// Window Properties
	private float width = 400;
	private float height = 100;

	// Other
	private int window_id;
	private string message = "The connection with server was lost, please restart the client!";
	private Rect windowRect;
	
	void Awake() {
		mainObject = GameObject.Find("MainObject");
		window_id = Constants.GetUniqueID();
	}
	
	// Use this for initialization
	void Start() {
		Game.StartEnterTransition ();
		windowRect = new Rect ((Screen.width - width) / 2, (Screen.height - height) / 2, width, height);
	}
	
	void OnGUI() {
		windowRect = GUILayout.Window(window_id, windowRect, MakeWindow, "Connection Lost");
		
		if (Event.current.type == EventType.KeyUp && Event.current.keyCode == KeyCode.Return) {
			Quit();
		}
	}
	
	void MakeWindow(int id) {
		GUILayout.Space(10);

		GUIStyle style = new GUIStyle();
		style.alignment = TextAnchor.MiddleCenter;
		style.normal.textColor = Color.white;
		
		GUILayout.Label(message, style);

		GUILayout.Space(30);
		
		if (GUI.Button(new Rect(windowRect.width / 2 + 80, windowRect.height - 40, 100, 30), "OK")) {
			Quit();
		}

		GUI.BringWindowToFront(window_id);
		GUI.DragWindow();
	}
	
	public void setMessage(string message) {
		this.message = message;
	}
	
	public void Quit() {
		Destroy(this);
	}
}
