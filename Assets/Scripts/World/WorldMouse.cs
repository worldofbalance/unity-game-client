using UnityEngine;
using System.Collections;

public class WorldMouse : MonoBehaviour {
	
	public GameObject defenderCursor;
	public GameObject roamingCursor;
	public GameObject attackerCursor;
	public Zone currentTile { get; set; }

	public TileInfoGUI tileInfoGUI;
	
	private int prevPlayerID;

	private MapCamera mapCamera;

	// Use this for initialization
	void Start() {
		tileInfoGUI = gameObject.AddComponent<TileInfoGUI>();
		mapCamera = GameObject.Find("MapCamera").GetComponent<MapCamera>();
	}
	
	// Update is called once per frame
	void Update() {
		RaycastHit hit = new RaycastHit();

		if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit)) {
			if (hit.transform.gameObject.tag == "Zone") {
				currentTile = hit.transform.gameObject.GetComponent<Zone>();

				if (currentTile.player_id != prevPlayerID) {
					if (prevPlayerID > 0) {
						GameObject.Find("Map").GetComponent<Map>().SelectTerritory(prevPlayerID, false);
					}

					GameObject.Find("Map").GetComponent<Map>().SelectTerritory(currentTile.player_id, true);
					prevPlayerID = currentTile.player_id;
				}

				string owner_name = "";
	
				if (currentTile.player_id > 0) {
					owner_name = GameObject.Find("Map").GetComponent<Map>().playerList[currentTile.player_id].name;

					Color playerColor = GameObject.Find("Map").GetComponent<Map>().playerList[currentTile.player_id].color;;
					tileInfoGUI.bgColor = new Color(playerColor.r, playerColor.g, playerColor.b, 0.8f);
				} else {
					tileInfoGUI.DefaultColor();
				}
	
				tileInfoGUI.SetInformation(currentTile.terrain_type, currentTile.v_capacity, owner_name);
				tileInfoGUI.position = Camera.main.WorldToScreenPoint(currentTile.transform.position);

				tileInfoGUI.SetActive(true);

				roamingCursor.SetActive(true);
				roamingCursor.transform.position = currentTile.transform.position + new Vector3(0, 0.1f, 0);
//				roamingCursor.renderer.material.color = new Color32(0, 181, 248, 255);
			}
		} else {
			tileInfoGUI.SetActive(false);
			roamingCursor.SetActive(false);
			currentTile = null;
		}

		switch (InputExtended.GetMouseNumClick(0)) {
			case 1: // Single Click
//				mouseDownPos = Input.mousePosition;
//				oldCameraPos = transform.position;
//				if (currentTile != null && currentTile.player_id > 0) {
//					mapCamera.Center(currentTile.player_id);
//					mapCamera.isLeaving = mapCamera.isZooming = true;
//				}
				break;
			case 2: // Double Click
				if (currentTile.player_id == GameState.player.GetID() && !Shop.gInshop) {
					mapCamera.Move(currentTile.transform.position);
				}
				break;
		}
	}
}
