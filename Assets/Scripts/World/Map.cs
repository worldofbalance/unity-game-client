using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

public class Map : MonoBehaviour {

	public short height;
	public short width;
	private Dictionary<int, GameObject> zoneList;
	public Dictionary<int, List<GameObject>> playerTiles = new Dictionary<int, List<GameObject>>();
	public Dictionary<int, Player> playerList { get; private set; }

	public Material[] colorMats = new Material[10];
	public GameObject desert_tile;
	public GameObject jungle_tile;
	public GameObject arctic_tile;
	public GameObject grasslands_tile;
	public GameObject owned_tile;
	public GameObject hexSelect;

	//Holds mouse mode; Mode 0 = normal, Mode 1 = Attack tile selected, Mode 2 = Defender tile selected
	int mode = 0;

	void Awake() {
		playerList = new Dictionary<int, Player>();

		NetworkManager.Send(
			ZoneListProtocol.Prepare(),
			ProcessZoneList
		);

		NetworkManager.Listen(
			NetworkCode.ZONE_UPDATE,
			ProcessZoneUpdate
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
			NetworkCode.ZONE_UPDATE,
			ProcessZoneUpdate
		);
	}
	
	public void ProcessZoneList(NetworkResponse response) {
		ResponseZoneList args = response as ResponseZoneList;

		if (args.status == 0) {
			playerList = args.players;

			height = args.height;
			width = args.width;

			Generate(args.height, args.width, args.zones);

			Game.StartEnterTransition();

			Camera.main.GetComponent<MapCamera>().Setup();
			Camera.main.GetComponent<MapCamera>().Center(GameState.player.GetID());
		}
	}

	public void Generate(short height, short width, List<Zone> zones) {
		zoneList = new Dictionary<int, GameObject>();

		foreach (Zone zone in zones) {
			GameObject zoneObject = null;
			
			switch (zone.terrain_type) {
				case 1:
					zoneObject = Instantiate(desert_tile) as GameObject;
					break;
				case 2:
					zoneObject = Instantiate(jungle_tile) as GameObject;
					break;
				case 3:
					zoneObject = Instantiate(grasslands_tile) as GameObject;
					break;
				case 4:
					zoneObject = Instantiate(arctic_tile) as GameObject;
					break;
			}
			
			if (zoneObject != null) {
				zoneObject.name = "Zone " + zone.row + "-" + zone.column;
				zoneObject.transform.parent = gameObject.transform;
				zoneObject.transform.position = new Vector3(zone.column * 13.85f + (zone.row % 2 == 0 ? 7 : 0), 0, zone.row * -11.95f);

				Zone ztmp = zoneObject.AddComponent<Zone>();
				ztmp.zone_id = zone.zone_id;
				ztmp.row = zone.row;
				ztmp.column = zone.column;
				ztmp.terrain_type = zone.terrain_type;
				ztmp.v_capacity = zone.v_capacity;
				ztmp.player_id = zone.player_id;
				
				zoneList.Add(zone.row * height + zone.column, zoneObject);
				// Borders
				GameObject highlight = Instantiate(owned_tile) as GameObject;
				highlight.transform.position = zoneObject.transform.position + new Vector3(0, 0.1f, 0);
				highlight.transform.parent = zoneObject.transform;

				if (ztmp.player_id == 0) {
					highlight.GetComponent<Renderer>().material.color = new Color32(119, 119, 119, 50);
				} else {
					if (!playerTiles.ContainsKey(ztmp.player_id)) {
						playerTiles.Add(ztmp.player_id, new List<GameObject>());
					}
					playerTiles[ztmp.player_id].Add(zoneObject);
					
					highlight.GetComponent<Renderer>().material.color = playerList[ztmp.player_id].color;
					
					GameObject select = Instantiate(hexSelect) as GameObject;
					select.transform.position = zoneObject.transform.position + new Vector3(0, 0.1f, 0);
					select.transform.parent = highlight.transform;
					select.GetComponent<Renderer>().material.color = playerList[ztmp.player_id].color;
					select.SetActive(false);
				}
			}
		}
		// Recenter
		Vector3 temp = Vector3.zero;
		foreach (GameObject o in zoneList.Values) {
			temp += o.transform.position;
			o.transform.parent = null;
		}
		temp /= zoneList.Count;
		transform.position = temp;
		foreach (GameObject o in zoneList.Values) {
			o.transform.parent = transform;
		}

		transform.position = Vector3.zero;
	}

	public GameObject FindPlayerOwnedTile(int player_id){
		foreach (GameObject zoneObject in zoneList.Values) {
			if (zoneObject.GetComponent<Zone>().player_id == player_id) {
				return zoneObject;
			}
		}

		return null;
	}

	public void SelectTerritory(int player_id, bool active) {
		if (!playerTiles.ContainsKey(player_id)) {
			return;
		}

		List<GameObject> zones = playerTiles[player_id];

		foreach (GameObject zone in zones) {
			zone.transform.GetChild(0).GetChild(0).gameObject.SetActive(active);
		}
	}

	public Vector3 GetCenterPoint(int player_id) {
		if (!playerTiles.ContainsKey(player_id)) {
			return Vector3.zero;
		}

		List<GameObject> zones = playerTiles[player_id];

		Vector3 center = Vector3.zero;
		foreach (GameObject o in zones) {
			center += o.transform.position;
		}
		center /= zones.Count;

		return center;
	}

	public void ProcessZoneUpdate(NetworkResponse response) {
		ResponseZoneUpdate args = response as ResponseZoneUpdate;

		if (args.status == 0) {
//			Debug.Log("update tile");
//			var tile = (GameObject)zoneList[args.tile_id];
//			
//			var oldOwner = tile.GetComponent<Zone>().player_id;
//			Debug.Log("Old Owner: " + oldOwner);
//			tile.GetComponent<Zone>().player_id = args.owner_id;
//			var ownerID = tile.GetComponent<Zone>().player_id;
//			Debug.Log("New Owner: " + ownerID);
//			if (oldOwner == 0 )
//			{
//				GameObject tileOwnershipGameObject = GameObject.Instantiate(owned_tile) as GameObject;
//				tileOwnershipGameObject.SetActive(true);
//				tileOwnershipGameObject.transform.position = tile.transform.position;
//				tileOwnershipGameObject.transform.parent = tile.transform;
//				tileOwnershipGameObject.renderer.material = colorMats[playerList[ownerID].color - 1];	
//			}		
		} 
	}
}
