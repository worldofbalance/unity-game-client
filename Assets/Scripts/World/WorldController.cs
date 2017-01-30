using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System;

public class WorldController : MonoBehaviour {

	private GameObject globalObject;
	private GameState gs;
	private Graph graph;
	private Dictionary<int, int> results = new Dictionary<int, int>();
	public static bool speciesLocCurrent = false;


  void Awake() {
    try {
      Game.networkManager.Send(WorldProtocol.Prepare(), ProcessWorld);
    } catch (Exception) {
    }

    globalObject = GameObject.Find ("Global Object");
	gs = globalObject.GetComponent<GameState> ();

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
		if (!speciesLocCurrent) {
			UpdateSpeciesLoc ();
		}
  }
  
  public void ProcessWorld(NetworkResponse response) {
    ResponseWorld args = response as ResponseWorld;

    if (args.status == 0) {
      GameState.world = args.world;

      SwitchToTileSelect(1);

      LoadComponents();

	  // This is to run simulation of the user's ecosystem upon login
	  // Debug.Log("WorldController: Send PredictionProtocol");
	  // Game.networkManager.Send (PredictionProtocol.Prepare (), ProcessPrediction);
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
	
	  if (GameObject.Find("Cube").GetComponent<Graph>() == null) {
		GameObject.Find("Cube").AddComponent<Graph>();
	  }
	  graph = GameObject.Find("Cube").GetComponent<Graph>();
    }
  }
  
  public void SwitchToTileSelect(int numTilesOwned) {
    //If player owns no tiles, they will need to pick a new home tile
    if (numTilesOwned == 0) {
      GameObject.Find("MapCamera").GetComponent<MapCamera>().FirstTileProcess(true);
      gameObject.AddComponent<TileSelectGUI>();
    }
  }


	public void ProcessPrediction(NetworkResponse response) {
		ResponsePrediction args = response as ResponsePrediction;
		Debug.Log("WorldController, ProcessPrediction: status = " + args.status);
		if (args.status == 0) {
			Dictionary<int, Species> speciesList = gs.speciesList;
			results = args.results;
			foreach (KeyValuePair<int, int> entry in results) {
				Debug.Log("WorldController, ProcessPrediction: k/v:" + entry.Key + " " + entry.Value);
				if (speciesList.ContainsKey (entry.Key)) {
					speciesList [entry.Key].biomass += entry.Value;
					Debug.Log("WorldController, ProcessPrediction: new value:" + speciesList [entry.Key].biomass);
				} else {
					Debug.Log("WorldController, ProcessPrediction: Could not find key:" + entry.Key);
				}
			}
		}
	}
		
	void UpdateSpeciesLoc() {
		Species.xIdx = 0;
		Species.zIdx = 0;
		int player_id = GameState.player.GetID ();
		GameObject zoneObject = GameObject.Find ("Map").GetComponent<Map> ().FindPlayerOwnedTile (player_id);
		if (zoneObject == null) {
			speciesLocCurrent = true;
			return;
		}
		Zone zone = zoneObject.GetComponent<Zone> ();
		float baseX = (zone.column - 20) * 13.85f + (zone.row % 2 == 0 ? 7 : 0) - 1;
		float baseZ = (zone.row - 19) * -11.95f + 3.5f;
		Debug.Log ("WorldController: zone.row, zone.column: " + zone.row + " " + zone.column);
		foreach (KeyValuePair<int, Species> entry in GameObject.Find("Global Object").GetComponent<GameState>().speciesList) {
			Species species = entry.Value;
			foreach (var organism in species.speciesList) {
				organism.transform.position = 
						new Vector3 (baseX + Species.xIdx * Species.step, 0, baseZ + Species.zIdx * Species.step);
				Species.UpdateIdx ();
			}
		}
		speciesLocCurrent = true;
	}
}
