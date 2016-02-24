using UnityEngine;

using System;
using System.Collections;

public class TileInfoGUI : MonoBehaviour {

	// Window Properties
	private float width = 200;
	private float height = 40;
	// Other
	private Texture2D bgTexture;
	private Font font;
	private Rect windowRect;
	private bool isActive = false;
	public Vector2 position { get; set; }
	private string terrain_type;
	private int v_capacity;
	private string owner_name;
	private string[] terrain = new string[]{"Desert", "Jungle", "Grasslands", "Arctic"};
	public Color bgColor { get; set; }
	
	void Awake() {
		bgTexture = Resources.Load<Texture2D>(Constants.THEME_PATH + Constants.ACTIVE_THEME + "/info_bg");
		font = Resources.Load<Font>("Fonts/" + "Chalkboard");
		
		DefaultColor();
	}
	
	// Use this for initialization
	void Start() {

	}
	
	// Update is called once per frame
	void Update() {

	}
	
	void OnGUI() {
		if (isActive) {
			if (owner_name == "") {
				height = 95;
			} else {
				height = 120;
			}

			windowRect = new Rect(position.x - width / 2 + 180, Screen.height - position.y - 50, width, height);
			windowRect = GUI.Window(Constants.ZONE_WIN, windowRect, MakeWindow, "", GUIStyle.none);
		}
	}
	
	void MakeWindow(int id) {
		Functions.DrawBackground(new Rect(0, 0, windowRect.width, windowRect.height), bgTexture, bgColor);
		
		GUIStyle style = new GUIStyle(GUI.skin.label);
		style.alignment = TextAnchor.UpperCenter;
		style.font = font;
		style.fontSize = 16;

		if (bgColor.r > 0.5f && bgColor.g > 0.5f && bgColor.b > 0.5f) {
			style.normal.textColor = new Color(0.2f, 0.2f, 0.2f);
		} else {
			style.normal.textColor = Color.white;
		}
		
		GUI.Label(new Rect((windowRect.width - 100) / 2, 0, 100, 30), "Information", style);

		style.alignment = TextAnchor.UpperLeft;
		style.fontSize = 14;

		GUI.BeginGroup(new Rect(10, 30, windowRect.width, 100));
			if (owner_name != "") {
				GUI.Label(new Rect(0, 0, windowRect.width, 25), "Owner: " + owner_name, style);
			}

			GUI.Label(new Rect(0, owner_name == "" ? 0 : 25, windowRect.width, 25), "Terrain Type: " + terrain_type, style);
			GUI.Label(new Rect(0, owner_name == "" ? 25 : 50, windowRect.width, 25), "Vegetation Capacity: " + v_capacity, style);
		GUI.EndGroup();
	}

	public void SetActive(bool active) {
		isActive = active;
	}

	public void SetInformation(short terrain_type, int v_capacity, string owner_name = "") {
		this.terrain_type = terrain[terrain_type - 1];
		this.v_capacity = v_capacity;
		this.owner_name = owner_name;
	}

	public void DefaultColor() {
		bgColor = new Color32(70, 77, 94, 255);
	}
}
