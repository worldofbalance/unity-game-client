using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class WorldController : MonoBehaviour {

	void Awake() {
		Game.networkManager.Send(WorldProtocol.Prepare(), ProcessWorld);
	}
	
	// Use this for initialization
	void Start() {
		Game.StartEnterTransition ();
		if (GameState.world != null) {
			LoadComponents();
		}
	}

	// Update is called once per frame
	void Update() {
		
	}

	void OnGUI() {
	}
	
	public void ProcessWorld(NetworkResponse response) {
		ResponseWorld args = response as ResponseWorld;

		if (args.status == 0) {
			GameState.world = args.world;

			SwitchToTileSelect(1);

			LoadComponents();
		}
	}

	void LoadComponents() {
		GameObject gObject = GameObject.Find("Global Object");
		
		if (gObject != null) {
			if (gObject.GetComponent<EcosystemScore>() == null) {
				gObject.AddComponent<EcosystemScore>();
			}
			
			if (gObject.GetComponent<GameResources>() == null) {
				gObject.AddComponent<GameResources>();
			}
			
			if (gObject.GetComponent<Clock>() == null) {
				gObject.AddComponent<Clock>();
			}
			
			if (gObject.GetComponent<Chat>() == null) {
				gObject.AddComponent<Chat>();
			}
			
			if (GameObject.Find("Cube").GetComponent<Shop>() == null) {
				GameObject.Find("Cube").AddComponent<Shop>();
			}
		}
	}
	
	public void SwitchToTileSelect(int numTilesOwned) {
		//If player owns no tiles, they will need to pick a new home tile
		if (numTilesOwned == 0) {
			GameObject.Find("MapCamera").GetComponent<MapCamera>().FirstTileProcess(true);
			gameObject.AddComponent<TileSelectGUI>();
		}
	}
}
