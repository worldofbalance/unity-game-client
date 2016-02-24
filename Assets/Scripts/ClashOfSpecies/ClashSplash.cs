using UnityEngine;

using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;

public class ClashSplash : MonoBehaviour {
	
	// Other
	private ClashGameManager manager;
	
	public Texture animals;
	private Rect windowRect;
	
	void Awake() {
		manager = GameObject.Find("MainObject").AddComponent<ClashGameManager>();
	}
	
	// Use this for initialization
	IEnumerator Start() {
		manager.currentPlayer = GameState.player;
		
		yield return StartCoroutine(Execute(ClashSpeciesListProtocol.Prepare(), (res) => {
			var response = res as ResponseClashSpeciesList;
			manager.availableSpecies = response.speciesList;
		}));
		
		yield return StartCoroutine(Execute(ClashEntryProtocol.Prepare(), (res) => {
			var response = res as ResponseClashEntry;
			if (response.config != null) {
				manager.defenseConfig = new ClashDefenseConfig();
				foreach (var pair in response.config) {
					var species = manager.availableSpecies.SingleOrDefault((el) => el.id == pair.Key);
					manager.defenseConfig.layout.Add(species, pair.Value);
				}
				manager.defenseConfig.terrain = response.terrain;
				Game.LoadScene("ClashMain");
			} else {
				Game.LoadScene("ClashDefenseShop");
			}
		}));
	}
	
	IEnumerator Execute(NetworkRequest req, NetworkManager.Callback cb) {
		bool done = false;
		NetworkManager.Send(req, (res) => {
			cb(res);
			done = true;
		});
		
		while (!done) yield return null;
	}
	
	void OnGUI() {
		// Background
		GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), animals);
		
		// Client Version Label
		GUI.Label(new Rect(Screen.width - 75, Screen.height - 30, 65, 20), "v" + Constants.CLIENT_VERSION + " Test");
		
	}
	
	// Update is called once per frame
	void Update() {}
}