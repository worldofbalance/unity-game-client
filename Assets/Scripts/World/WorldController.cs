using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System;

public class WorldController : MonoBehaviour {

	private Dictionary<int, int> results = new Dictionary<int, int>();

  void Awake() {
    try {
      Game.networkManager.Send(WorldProtocol.Prepare(), ProcessWorld);
    } catch (Exception) {
    }
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
	  Debug.Log("WorldController: Send PredictionProtocol");
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
			results = args.results;
		}


		foreach (KeyValuePair<int, int> entry in results) {
			Debug.Log("WorldController, ProcessPrediction: k/v:" + entry.Key + " " + entry.Value);
		}



	}
}
