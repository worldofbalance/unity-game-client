using UnityEngine;

using System;
using System.Collections;
using System.IO;
using System.Net.Sockets;

public class MainGUI : MonoBehaviour {

	// Window Properties
	private float width = 220;
	private float height = 75;
	// Other
	private Rect windowRect;
	private Rect windowRect2;
	private bool isHidden = false;
	private string username;
	
	private Vector2 scrollViewVector = Vector2.zero;
	private string innerText = "| Pic | Species Name | Biomass |\n| Pic | Species Name | Biomass |\n| Pic | Species Name | Biomass |";

	void Awake() {
		username = GameState.player.name;
	}
	
	// Use this for initialization
	void Start() {
	}
	
	void OnGUI() {
		if (!isHidden) {
			windowRect = new Rect(0f, 0f, width, height);
			windowRect2 = new Rect(0f, 295f, width, Screen.height - 295);
			windowRect = GUILayout.Window(Constants.LOGIN_WIN, windowRect, MakeWindow, username);
			windowRect2 = GUILayout.Window(Constants.DATABASE_WIN, windowRect2, MakeSpeciesGUI, "Species");
		}
	}
	
	void MakeWindow(int id) {
		GUILayout.Label("Environment Score: ");
		GUILayout.Label("Biomass Points: ");
	}

	void MakeSpeciesGUI(int id) {
		GUILayout.Label("");
		if (GUI.Button(new Rect(4, 16, width - 8, 26), "Store")) Submit();
		// Begin the ScrollView
		scrollViewVector = GUI.BeginScrollView(new Rect(4, 40, width - 8, Screen.height - 339), scrollViewVector, new Rect(0, 0, width -24f, Screen.height));
		// Put something inside the ScrollView
		innerText = GUI.TextArea (new Rect (0, 0, width, Screen.height), innerText);
		
		// End the ScrollView
		GUI.EndScrollView();
	}

	public void Submit() {
		//gameObject.GetComponent<Shop>().isHidden = !gameObject.GetComponent<Shop>().isHidden;
	}
	
	public void Show() {
		isHidden = false;
	}
	
	public void Hide() {
		isHidden = true;
	}
	
	// Update is called once per frame
	void Update() {
	}
}
