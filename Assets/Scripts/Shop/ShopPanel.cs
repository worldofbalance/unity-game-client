using UnityEngine;

using System.Collections;
using System.Collections.Generic;

public class ShopPanel : MonoBehaviour {
	
	private GameObject mainObject;
	private Rect windowRect;
	private Vector2 scrollPosition = Vector2.zero;
	private Shop shop;
	private List<SpeciesData> itemList;
	private bool isBeHidden { get; set; }
	// Use this for initialization
	void Start () {
		mainObject = GameObject.Find("Cube");
		shop = mainObject.GetComponent<Shop>();//transform.root.gameObject.GetComponent<Shop>();
		
		itemList = new List<SpeciesData>(shop.itemList.Values);
		//Debug.Log("Reached here!" + shop.itemList.Values);
		itemList.Sort(ShopPanel.SortByTrophicLevels);
		Hide();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnGUI() {
		if (!isBeHidden) {
			//windowRect = new Rect(0, 0, 0, 0);
			//windowRect = GUI.Window( (int)Constants.GUI_ID.ShopPanel, windowRect, ShopPanelMakeWindow, "");//
			int height = 20 + itemList.Count / 6 * 130;
			
			scrollPosition = GUI.BeginScrollView(new Rect(5, 70, 570, 300), scrollPosition, new Rect(0, 0, 300, height));
			GUI.Box(new Rect(0, 0, 555, height), "");
			
			for (int i = 0; i < itemList.Count; i++) {
				SpeciesData species = itemList[i];
				
				GUI.BeginGroup(new Rect(10 + i % 6 * 90, 20 + i / 6 * 130, 160, 160));
				if (mainObject.GetComponent<Shop>().selectedSpecies != null) {
					if (mainObject.GetComponent<Shop>().selectedSpecies.species_id == species.species_id) {
						GUI.backgroundColor = Color.black;
						GUI.color = Color.yellow;
					} else if (mainObject.GetComponent<Shop>().selectedSpecies.predatorList.ContainsKey(species.species_id)) {
						GUI.backgroundColor = Color.red;
						GUI.color = Color.red;
					} else if (mainObject.GetComponent<Shop>().selectedSpecies.preyList.ContainsKey(species.species_id)) {
						GUI.backgroundColor = Color.green;
						GUI.color = Color.green;
					}
				}
				
				Texture texture = Resources.Load("white") as Texture;
				GUI.DrawTexture(new Rect(0, 0, 80, 105), texture);
				GUI.color = Color.white;
				
				if (GUI.Button(new Rect(0, 0, 80, 105), "")) {
					SelectSpecies(species);
				}
				GUI.backgroundColor = Color.white;
				
				GUI.DrawTexture(new Rect(10, 25, 60, 55), species.image);
				
				GUIStyle style = new GUIStyle(GUI.skin.button);
				style.alignment = TextAnchor.MiddleLeft;
				style.normal.textColor = Color.white;
				style.normal.background = null;
				
				GUI.Label(new Rect(5, -3, 70, 30), species.name, style);
				GUI.EndGroup();
			}
			GUI.EndScrollView();
		}
		
	}
	/*
	public void ShopPanelMakeWindow(int id) {
		//Debug.Log("Reached Here!!!");
		int height = 700;//20 + itemList.Count / 6 * 130;

		scrollPosition = GUI.BeginScrollView(new Rect(5, 70, 570, 300), scrollPosition, new Rect(0, 0, 300, height));
			GUI.Box(new Rect(0, 0, 555, height), "");

			for (int i = 0; i < itemList.Count; i++) {
				SpeciesData species = itemList[i];

			GUI.BeginGroup(new Rect(20, 20, 600, 600));//10 + i % 6 * 90, 20 + i / 6 * 130, 160, 160));
			if (mainObject.GetComponent<Shop>().selectedSpecies != null) {
				if (mainObject.GetComponent<Shop>().selectedSpecies.species_id == species.species_id) {
							GUI.backgroundColor = Color.black;
							GUI.color = Color.yellow;
				} else if (mainObject.GetComponent<Shop>().selectedSpecies.predatorList.ContainsKey(species.species_id)) {
							GUI.backgroundColor = Color.red;
							GUI.color = Color.red;
				} else if (mainObject.GetComponent<Shop>().selectedSpecies.preyList.ContainsKey(species.species_id)) {
							GUI.backgroundColor = Color.green;
							GUI.color = Color.green;
						}
					}

					Texture texture = Resources.Load("white") as Texture;
					GUI.DrawTexture(new Rect(0, 0, 80, 105), texture);
					GUI.color = Color.white;

					if (GUI.Button(new Rect (0, 0, 80, 105), "")) {
						SelectSpecies(species);
					}
					GUI.backgroundColor = Color.white;

					GUI.DrawTexture(new Rect(10, 25, 60, 55), species.image);
	
					GUIStyle style = new GUIStyle(GUI.skin.button);
					style.alignment = TextAnchor.MiddleLeft;
					style.normal.textColor = Color.white;
					style.normal.background = null;
	
					GUI.Label(new Rect(5, -3, 70, 30), species.name, style);
				GUI.EndGroup ();
			}
		GUI.EndScrollView();
	}
	*/
	
	public void SelectSpecies(SpeciesData species) {
		if (mainObject.GetComponent<Shop>().selectedSpecies != null && 
		    species.species_id == mainObject.GetComponent<Shop>().selectedSpecies.species_id) {
			GameObject.Find("Cube").GetComponent<ShopCartPanel>().Add(species);
		}
		
		mainObject.GetComponent<Shop>().selectedSpecies = species;
	}
	
	
	public void Show() {
		isBeHidden = false;
	}
	
	public void Hide() {
		isBeHidden = true;
	}

	public static int SortByTrophicLevels(SpeciesData x, SpeciesData y) {
		return y.trophic_level.CompareTo(x.trophic_level);
	}
}
