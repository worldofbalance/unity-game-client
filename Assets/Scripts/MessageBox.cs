using UnityEngine;
using System;
using System.Collections;

public class MessageBox : MonoBehaviour {
	
	private GameObject mainObject;
	// Window Properties
	private float width = 250;
	private float height = 100;
	// Other
	private int window_id;
	private string message = "s";
	private Rect windowRect;
	
	void Awake() {
		mainObject = GameObject.Find("MainObject");
		window_id = Constants.GetUniqueID();
	}
	
	// Use this for initialization
	void Start() {
		windowRect = new Rect ((Screen.width - width) / 2, (Screen.height - height) / 2, width, height);
	}
	
	void OnGUI() {
		windowRect = GUILayout.Window(window_id, windowRect, MakeWindow, "Message");
		
		if (Event.current.type == EventType.KeyUp && Event.current.keyCode == KeyCode.Return) {
			Submit();
		}
	}
	
	void MakeWindow(int id) {
		GUILayout.Space(10);

		GUIStyle style = new GUIStyle();
		style.alignment = TextAnchor.MiddleCenter;
		style.normal.textColor = Color.white;
		
		GUILayout.Label(message, style);

		GUILayout.Space(30);
		
		if (GUI.Button(new Rect(windowRect.width / 2 - 50, windowRect.height - 40, 100, 30), "Continue")) {
			Submit();
		}

		/*
		if (GUI.Button(new Rect(windowRect.width / 2 - 110, windowRect.height - 40, 100, 30), "Yes")) {
			Submit();
		}
		
		if (GUI.Button(new Rect(windowRect.width / 2 + 10, windowRect.height - 40, 100, 30), "No")) {
			Submit();
		}
		*/

		GUI.BringWindowToFront(window_id);
		GUI.DragWindow();
	}
	
	public void setMessage(string message) {
		this.message = message;
	}
	
	public void Submit() {
		Destroy(gameObject);
	}
	
	// Update is called once per frame
	void Update() {
	}
}
