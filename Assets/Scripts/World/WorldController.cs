using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System;

public class WorldController : MonoBehaviour {

	private GameObject globalObject;
	private GameState gs;

	private Dictionary<int, int> results = new Dictionary<int, int>();

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
  }
  
  public void ProcessWorld(NetworkResponse response) {
    ResponseWorld args = response as ResponseWorld;

    if (args.status == 0) {
      GameState.world = args.world;

      SwitchToTileSelect(1);

      LoadComponents();

	  // Debug.Log("WorldController: Send PredictionProtocol");
	  // Game.networkManager.Send (PredictionProtocol.Prepare (), ProcessPrediction);

	  Debug.Log("WorldController: Send SpeciesActionProtocol");
	  int action = 2;
	  Game.networkManager.Send (SpeciesActionProtocol.Prepare ((short) action), ProcessSpeciesAction);
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

	public void ProcessSpeciesAction (NetworkResponse response)
	{
		ResponseSpeciesAction args = response as ResponseSpeciesAction;
		int action = args.action;
		int status = args.status;
		if ((action != 2) || (status != 0)) {
			Debug.Log ("ResponseSpeciesAction unexpected result2");
			Debug.Log ("action, status = " + action + " " + status);
		}
		Dictionary<int, int> speciesList = args.speciesList;
		Debug.Log ("WorldController, ProcessSpeciesAction, size = " + speciesList.Count);
		foreach (KeyValuePair<int, int> entry in speciesList) {
			Debug.Log ("species, biomass = " + entry.Key + " " + entry.Value);
			int action2 = 4;
			Game.networkManager.Send (SpeciesActionProtocol.Prepare ((short) action2, entry.Key), ProcessSpeciesHistory);
		}
	}


	public void ProcessSpeciesHistory (NetworkResponse response)
	{
		ResponseSpeciesAction args = response as ResponseSpeciesAction;
		int action = args.action;
		int status = args.status;
		int species_id = args.species_id;
		if ((action != 4) || (status != 0)) {
			Debug.Log ("ResponseSpeciesAction unexpected result4");
			Debug.Log ("action, status = " + action + " " + status);
		}
		Dictionary<int, int> speciesList = args.speciesHistoryList;
		Debug.Log ("WorldController, ProcessSpeciesHistory, species_id = " + species_id);
		Debug.Log ("WorldController, ProcessSpeciesHistory, size = " + speciesList.Count);
		foreach (KeyValuePair<int, int> entry in speciesList) {
			Debug.Log ("day, biomass change = " + entry.Key + " " + entry.Value);
		}
	}
}
