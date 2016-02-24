using UnityEngine;

using System;
using System.Collections;
using System.IO;
using System.Net.Sockets;

public class TileSelectGUI : MonoBehaviour {
	
	private GameObject mainObject;
	private MapCamera mapCamera;
	// Window Properties
	private float width = 280;
	private float height = 100;
	// Other
	public Texture background;
	private string user_id = "";
	private string password = "";
	private Rect windowRect;
	private bool isHidden = false;
	
	void Awake() {
		mainObject = GameObject.Find("MainObject");
		mapCamera = GameObject.Find("MapCamera").GetComponent<MapCamera>();

		NetworkManager.Listen(
			NetworkCode.ZONE,
			ProcessZone
		);
	}
	
	// Use this for initialization
	void Start() {

	}

	// Update is called once per frame
	void Update() {

	}

	void OnDestroy() {
		NetworkManager.Ignore(
			NetworkCode.ZONE,
			ProcessZone
		);
	}
	
	void OnGUI() {
		// Background
		//GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), background);
		
		// Client Version Label
		GUI.Label(new Rect(Screen.width - 75, Screen.height - 30, 65, 20), "v" + Constants.CLIENT_VERSION + " Beta");
		
		// Login Interface
		if (!isHidden) {
			windowRect = new Rect((Screen.width - width) / 2, Screen.height - height - 10f, width, height);
			windowRect = GUILayout.Window(Constants.ZONE_WIN, windowRect, MakeWindow, "Select Your First Tile ...");
			
			if (Event.current.type == EventType.KeyUp && Event.current.keyCode == KeyCode.Return) {
				Submit();
			}
		}
	}
	
	void MakeWindow(int id) {
		GUILayout.Label("Click on a tile to select it wth the cursor:");
		GUI.SetNextControlName("username_field");
		if (mapCamera.mode == 4)
		{
			if (GUI.Button(new Rect(10, 50, width - 20, 30), "Select Tile")) Submit();
		}
	}
	
	public void Submit() {
		print ("submit!!!");
//		int tile_id = mapCamera.FirstTile.GetID();
//		mapCamera.FirstTileProcess(false);
		mapCamera.mode = 0;

		Hide();
		//gameObject.AddComponent<MainGUI>();
		//gameObject.AddComponent<Chart>();
		//gameObject.AddComponent<Battle>();
//		gameObject.AddComponent<Season>();

//		NetworkManager.Send(
//			ZoneProtocol.Prepare(tile_id, GameState.player.GetID()),
//			ProcessZone
//		);
	}

	public void Show() {
		isHidden = false;
	}
	
	public void Hide() {
		isHidden = true;
	}

	public void ProcessZone(NetworkResponse response) {
		ResponseZone args = response as ResponseZone;
	}

}
