using UnityEngine;

using System.Collections;
using System.Collections.Generic;

public class ShopInfoPanel : MonoBehaviour {
	
	private GameObject mainObject;
	private bool isTheHidden { get; set; }
	private Vector2 scrollPosition = Vector2.zero;
	public string text { get; set; }
	private Shop shop;
	
	// Use this for initialization
	void Start () {
		mainObject = GameObject.Find("Cube");
		shop = mainObject.GetComponent<Shop>();//transform.root.gameObject.GetComponent<Shop>();
		text = "";
		Hide();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnGUI() {
		if (!isTheHidden) {
			scrollPosition = GUI.BeginScrollView(new Rect(5, 380, 570, 200), scrollPosition, new Rect(0, 0, 300, 400));
			GUI.Box (new Rect (0, 0, 555, 400), "");
			
			if (mainObject.GetComponent<Shop>().selectedSpecies != null && mainObject.GetComponent<Shop>().itemList.ContainsKey(shop.selectedSpecies.species_id)) {
				SpeciesData species = mainObject.GetComponent<Shop>().itemList[mainObject.GetComponent<Shop>().selectedSpecies.species_id];
				
				List<string> predatorList = new List<string>(species.predatorList.Values);
				predatorList.Sort();
				string predatorText = predatorList.Count > 0 ? string.Join(", ", predatorList.ToArray()) : "None";
				
				List<string> preyList = new List<string>(species.preyList.Values);
				preyList.Sort();
				string preyText = preyList.Count > 0 ? string.Join(", ", preyList.ToArray()) : "None";
				
				text = species.name + "\n\n" + species.description + "\n\nPredators\n" + predatorText + "\n\nPrey\n" + preyText;
				GUI.Label(new Rect (10, 10, 500, 400), text);
			}
			GUI.EndScrollView();
			
			
		}
		
	}
	/*
	public void MakeWindow() {
		scrollPosition = GUI.BeginScrollView(new Rect(5, 380, 570, 200), scrollPosition, new Rect(0, 0, 300, 400));
			GUI.Box (new Rect (0, 0, 555, 400), "");
		
		if (mainObject.GetComponent<Shop>().selectedSpecies != null && mainObject.GetComponent<Shop>().itemList.ContainsKey(shop.selectedSpecies.species_id)) {
			SpeciesData species = mainObject.GetComponent<Shop>().itemList[mainObject.GetComponent<Shop>().selectedSpecies.species_id];

				List<string> predatorList = new List<string>(species.predatorList.Values);
				predatorList.Sort();
				string predatorText = predatorList.Count > 0 ? string.Join(", ", predatorList.ToArray()) : "None";

				List<string> preyList = new List<string>(species.preyList.Values);
				preyList.Sort();
				string preyText = preyList.Count > 0 ? string.Join(", ", preyList.ToArray()) : "None";

				text = species.name + "\n\n" + species.description + "\n\nPredators\n" + predatorText + "\n\nPrey\n" + preyText;
				GUI.Label(new Rect (10, 10, 500, 400), text);
			}
		GUI.EndScrollView();
	}

	*/
	public void Show() {
		isTheHidden = false;
	}
	
	public void Hide() {
		isTheHidden = true;
	}
}
