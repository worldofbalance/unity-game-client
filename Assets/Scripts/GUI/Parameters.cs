using UnityEngine;

using System;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;

public class Parameters : MonoBehaviour {

	private int window_id = Constants.PARAM_WIN;
	// Window Properties
	private float left;
	private float top;
	private float width = 400;
	private float height = 300;
	private Rect windowRect;
	private bool isActive;
	// Other
	private string species = "";
	private string k = ""; // Carrying Capacity
	private string r = ""; // Growth Rate
	private string x = ""; // Metabolic Rate
	private string[] texts = new string[]{"Species A", "Species B", "Species C", "Species D", "Species E", "Species F"};
	private int selected;
	private bool active;
	private Vector2 scrollPosition = Vector2.zero;
	
	void Awake() {
		left = (Screen.width - width) / 2;
		top = (Screen.height - height) / 2;

		windowRect = new Rect(left, top, width, height);
	}
	
	// Use this for initialization
	void Start() {
		
	}
	
	// Update is called once per frame
	void Update() {
		
	}
	
	void OnGUI() {
		if (isActive) {
			windowRect = GUI.Window(window_id, windowRect, MakeWindow, "Parameters");
		}
	}
	
	void MakeWindow(int id) {
		GUI.BeginGroup(new Rect(20, 25, 100, 300));
			GUIStyle style = new GUIStyle(GUI.skin.label);
			style.alignment = TextAnchor.UpperCenter;

			GUI.Label(new Rect(0, 0, 100, 100), "Species", style);

			GUI.BeginScrollView(new Rect(0, 25, 120, 200), scrollPosition, new Rect(0, 0, 100, 300));
				style = new GUIStyle(GUI.skin.button);
				style.alignment = TextAnchor.MiddleLeft;

				selected = GUI.SelectionGrid(new Rect(0, 0, 100, 200), selected, texts, 1, style);
			GUI.EndScrollView();
		GUI.EndGroup();

		GUI.Box(new Rect(140, 100, 100, 100), Resources.Load<Texture2D>(Constants.IMAGE_RESOURCES_PATH + "African Elephant"));

		GUI.BeginGroup(new Rect(windowRect.width - 140, 35, 140, 300));
			Rect textRect = new Rect(0, 0, 100, 30);

			GUI.BeginGroup(new Rect(0, 20, 250, 100));
				GUI.Label(new Rect(0, 0, 250, 30), "Carrying Capacity");
				k = GUI.TextField(new Rect(0, 25, 120, 30), k, 10);
				k = Regex.Replace(k, "[^0-9]", "");
			GUI.EndGroup();

			GUI.BeginGroup(new Rect(0, 80, 250, 100));
				GUI.Label(new Rect(0, 0, 250, 30), "Growth Rate");
				r = GUI.TextField(new Rect(0, 25, 120, 30), r, 10);
				r = Regex.Replace(r, "[^0-9]", "");
			GUI.EndGroup();

			GUI.BeginGroup(new Rect(0, 140, 250, 100));
				GUI.Label(new Rect(0, 0, 250, 30), "Metabolic Rate");
				x = GUI.TextField(new Rect(0, 25, 120, 30), x, 10);
				x = Regex.Replace(x, "[^0-9]", "");
			GUI.EndGroup();
		GUI.EndGroup();

		if (GUI.Button(new Rect((windowRect.width - 100) / 2, windowRect.height - 30 - 15, 100, 30), "Submit")) {
			Submit();
		}

		GUI.DragWindow();
	}
	
	public void Show() {
		isActive = true;
	}
	
	public void Hide() {
		isActive = false;
	}
	
	public void Submit() {
		int carrying_capacity = int.Parse(k);
		float growth_rate = float.Parse(r);
		float metabolic_rate = float.Parse(x);

//		NetworkManager.Send(
//			ParametersProtocol.Prepare(k, r, x),
//			ProcessParameters
//		);
	}

	public void ProcessParameters(NetworkResponse response) {
	}
}
