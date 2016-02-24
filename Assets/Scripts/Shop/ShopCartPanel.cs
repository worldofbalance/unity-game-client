using UnityEngine;

using System.Collections;
using System.Collections.Generic;

public class ShopCartPanel : MonoBehaviour {
	
	private GameObject shopObject;
	private GameObject mainObject;
	
	private Vector2 scrollPosition = Vector2.zero;
	private Shop shop;
	public Dictionary<int, int> cartList { get; set; }
	public ProgressBar biomassMeter;
	public int totalBiomass;
	private bool isAnHidden { get; set; }
	
	// Use this for initialization
	void Start () {
		shopObject = GameObject.Find("Cube");
		shop = shopObject.GetComponent<Shop>();
		cartList = new Dictionary<int, int>();
		Hide();
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnGUI() {
		if (!isAnHidden) {
			GUI.Label(new Rect(620, 30, 200, 200), "Biomass Capacity");
			int height = 20 + cartList.Count * 90;
			
			totalBiomass = 0;
			
			scrollPosition = GUI.BeginScrollView(new Rect(580, 80, 200, 460), scrollPosition, new Rect(0, 0, 100, height));
			GUI.Box(new Rect(0, 0, 555, Mathf.Max(500, height)), "");
			
			List<int> items = new List<int>(cartList.Keys);
			//Debug.Log(cartList.Count);
			
			for (int i = 0; i < cartList.Count; i++) {
				int species_id = items[i];
				SpeciesData species = shopObject.GetComponent<Shop>().itemList[species_id];
				
				totalBiomass += cartList[species_id];
				
				GUI.BeginGroup (new Rect(10, 20 + i * 90, 160, 160));
				if (shopObject.GetComponent<Shop>().selectedSpecies != null) {
					if (shopObject.GetComponent<Shop>().selectedSpecies.species_id == species.species_id) {
						GUI.backgroundColor = Color.black;
						GUI.color = Color.yellow;
					} else if (shopObject.GetComponent<Shop>().selectedSpecies.predatorList.ContainsKey(species.species_id)) {
						GUI.backgroundColor = Color.red;
						GUI.color = Color.red;
					} else if (shopObject.GetComponent<Shop>().selectedSpecies.preyList.ContainsKey(species.species_id)) {
						GUI.backgroundColor = Color.green;
						GUI.color = Color.green;
					}
				}
				
				if (GUI.Button(new Rect(0, 0, 160, 80), "")) {
				}
				GUI.backgroundColor = Color.white;
				
				GUI.DrawTexture(new Rect(10, 10, 60, 60), species.image);
				
				{
					GUIStyle style = new GUIStyle(GUI.skin.label);
					style.alignment = TextAnchor.MiddleLeft;
					style.normal.textColor = Color.white;
					style.normal.background = null;
					style.wordWrap = false;
					
					GUI.Label(new Rect(75, 10, 70, 30), species.name, style);
				}
				
				{
					GUIStyle style = new GUIStyle(GUI.skin.label);
					style.alignment = TextAnchor.UpperLeft;
					
					GUI.Label(new Rect(75, 40, 100, 30), "Total B: " + cartList[species_id].ToString(), style);
				}
				GUI.EndGroup();
				
			}
			GUI.EndScrollView();
			
			GUI.Label(new Rect(620, 50, 200, 200), totalBiomass.ToString());
			
			{
				GUIStyle style = new GUIStyle(GUI.skin.label);
				style.alignment = TextAnchor.UpperCenter;
				
				GUI.Label(new Rect(625, 555, 100, 100), cartList.Count.ToString() + " / 10", style);
			}
			
			{
				GUIStyle style = new GUIStyle(GUI.skin.button);
				style.alignment = TextAnchor.MiddleCenter;
				style.normal.textColor = Color.white;
				
				if (GUI.Button(new Rect(700, 550, 70, 30), "Finish", style)) {
					NetworkManager.Send(
						ShopActionProtocol.Prepare(0, cartList),
						ResponseShopAction
						);
					
					shopObject.GetComponent<Shop>().Show();
					GameObject.Find("Cube").GetComponent<ShopPanel>().Hide();
					GameObject.Find("Cube").GetComponent<ShopCartPanel>().Hide();
					GameObject.Find("Cube").GetComponent<ShopInfoPanel>().Hide();
					//GameObject.Find("MapCamera").GetComponent<MapCamera>().enabled = true;
					//GameObject.Find("MapCamera").GetComponent<MapCamera>().RoamingCursor.SetActive(true);
					
					/*
					foreach (int species_id in cartList.Keys) {
						var state_obj = shopObject.GetComponent<GameState>();
						state_obj.CreateSpecies(
								species_id,
						    	SpeciesTable.speciesList[species_id].name, 
						        "Animal", 
						        cartList[species_id]);
					}
					*/
					cartList = new Dictionary<int, int>();
				}
			}
		}
		
	}
	/*
	public void MakeWindow() {
		GUI.Label(new Rect(620, 30, 200, 200), "Biomass Capacity");
		int height = 20 + cartList.Count * 90;

		totalBiomass = 0;

		scrollPosition = GUI.BeginScrollView(new Rect(580, 80, 200, 460), scrollPosition, new Rect(0, 0, 100, height));
			GUI.Box(new Rect(0, 0, 555, Mathf.Max(500, height)), "");

			List<int> items = new List<int>(cartList.Keys);

			for (int i = 0; i < cartList.Count; i++) {
				int species_id = items[i];
				SpeciesData species = shop.itemList[species_id];
			
				totalBiomass += cartList[species_id];

				GUI.BeginGroup (new Rect(10, 20 + i * 90, 160, 160));
					if (shop.selectedSpecies != null) {
						if (shop.selectedSpecies.species_id == species.species_id) {
							GUI.backgroundColor = Color.black;
							GUI.color = Color.yellow;
						} else if (shop.selectedSpecies.predatorList.ContainsKey(species.species_id)) {
							GUI.backgroundColor = Color.red;
							GUI.color = Color.red;
						} else if (shop.selectedSpecies.preyList.ContainsKey(species.species_id)) {
							GUI.backgroundColor = Color.green;
							GUI.color = Color.green;
						}
					}

					if (GUI.Button(new Rect(0, 0, 160, 80), "")) {
					}
					GUI.backgroundColor = Color.white;

					GUI.DrawTexture(new Rect(10, 10, 60, 60), species.image);

					{
						GUIStyle style = new GUIStyle(GUI.skin.label);
						style.alignment = TextAnchor.MiddleLeft;
						style.normal.textColor = Color.white;
						style.normal.background = null;
						style.wordWrap = false;

						GUI.Label(new Rect(75, 10, 70, 30), species.name, style);
					}

					{
						GUIStyle style = new GUIStyle(GUI.skin.label);
						style.alignment = TextAnchor.UpperLeft;
			
						GUI.Label(new Rect(75, 40, 100, 30), "Total B: " + cartList[species_id].ToString(), style);
					}
				GUI.EndGroup();
			}
		GUI.EndScrollView();

		GUI.Label(new Rect(620, 50, 200, 200), totalBiomass.ToString());

		{
			GUIStyle style = new GUIStyle(GUI.skin.label);
			style.alignment = TextAnchor.UpperCenter;

			GUI.Label(new Rect(625, 555, 100, 100), cartList.Count.ToString() + " / 10", style);
		}

		{
			GUIStyle style = new GUIStyle(GUI.skin.button);
			style.alignment = TextAnchor.MiddleCenter;
			style.normal.textColor = Color.white;
	
			if (GUI.Button(new Rect(700, 550, 80, 30), "Finish", style)) {
				foreach (int species_id in cartList.Keys) {
					GetComponent<GameState>().CreateSpecies(species_id, SpeciesTable.speciesList[species_id].name, "Animal", cartList[species_id]);
				}

				ConnectionManager cManager = GameObject.Find("MainObject").GetComponent<ConnectionManager>();
				
				if (cManager) {
					cManager.Send(RequestShopAction(0, cartList));
				}

				cartList = new Dictionary<int, int>();
				shop.Hide();
			}
		}
	}
	*/
	public void Add(SpeciesData species) {
		if (!cartList.ContainsKey(species.species_id)) {
			cartList.Add(species.species_id, 0);
		}
		
		cartList[species.species_id] = cartList[species.species_id] + species.biomass;
	}

	public void ResponseShopAction(NetworkResponse response) {
		ResponseShopAction args = response as ResponseShopAction;
	}

	public void Show() {
		isAnHidden = false;
	}
	
	public void Hide() {
		isAnHidden = true;
	}
}
