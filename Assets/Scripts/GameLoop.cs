using UnityEngine;

using System.Collections;
using System.Collections.Generic;

public class GameLoop : MonoBehaviour {
	
	private Dictionary<int, int> results = new Dictionary<int, int>();
	private int currentMonth;
	private int currentDay;
	
	void Awake() {
		NetworkManager.Listen(
			NetworkCode.PREDICTION,
			ProcessPrediction
		);
		//currentMonth = GameState.world.month;
	}

	// Use this for initialization
	void Start() {
		gameObject.GetComponent<Clock>().ClockChange += new Clock.ClockChangeHandler(InterpolateChange);
	}
	
	// Update is called once per frame
	void Update() {
	
	}

	void OnDestroy() {
		NetworkManager.Ignore(
			NetworkCode.PREDICTION,
			ProcessPrediction
		);
	}
	
	public void InterpolateChange(Clock clock, ClockEventArgs args) {
		if (currentMonth != args.month) {
			currentMonth = args.month;
			
			NetworkManager.Send(
				PredictionProtocol.Prepare(),
				ProcessPrediction
			);
		}

		if (currentDay != args.day) {
			currentDay = args.day;

			if (results.Count > 0) {
				foreach (KeyValuePair<int, int> entry in results) {
					int group_id = entry.Key, biomass = entry.Value;

					Species speciesGroup = gameObject.GetComponent<GameState>().GetSpeciesGroup(group_id);

					if (speciesGroup != null) {
						int nextBiomass = speciesGroup.size + biomass / 30;
						gameObject.GetComponent<GameState>().UpdateSpecies(group_id, nextBiomass);

						if (nextBiomass < 0) {
							gameObject.GetComponent<Chat>().SetMessage(speciesGroup.name + " decreased by " + -nextBiomass);
						} else {
							gameObject.GetComponent<Chat>().SetMessage(speciesGroup.name + " increased by " + nextBiomass);
						}
					} else {
						Debug.Log("Missing Species");
					}
				}
			}
		}
	}
	
	public void ProcessPrediction(NetworkResponse response) {
		ResponsePrediction args = response as ResponsePrediction;
		
		if (args.status == 0) {
			results = args.results;
		}
	}
}
