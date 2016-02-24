using UnityEngine;

using System.Collections;
using System.Collections.Generic;

public class Shop : MonoBehaviour {
	
	private GameObject worldObject;
	private GameObject shopObject;
	private GameObject mainObject;
	
	public Dictionary<int, SpeciesData> itemList { get; set; }
	public SpeciesData selectedSpecies { get; set; }
	
	// Window Properties
	private float width = 280;
	private float height = 100;
	// Other
	private Rect windowRect;
	private Rect avatarRect;
	private Texture avatar;
	private Rect[] buttonRectList;
	private GameObject messageBox;
	private Vector2 scrollPosition = Vector2.zero;
	private bool isHidden { get; set; }

	static public bool gInshop = false;
	
	void Awake() {
		buttonRectList = new Rect[3];
		
		itemList = new Dictionary<int, SpeciesData>();
		shopObject = GameObject.Find("Cube");
		mainObject = GameObject.Find("MainObject");
		shopObject.AddComponent<ShopPanel>();
		shopObject.AddComponent<ShopInfoPanel>();
		shopObject.AddComponent<ShopCartPanel>();
		//shopObject.AddComponent("GameState");
		isHidden = false;
		//mainObject.GetComponent<MessageQueue>().AddCallback(Constants.SMSG_SHOP, ResponseShop);
		//mainObject.GetComponent<MessageQueue>().AddCallback(Constants.SMSG_SHOP_ACTION, ResponseShopAction);
		//mainObject.GetComponent<MessageQueue>().AddCallback(Constants.SMSG_UPDATE_RESOURCES, ResponseUpdateResources);
		
		//mainObject.GetComponent<MessageQueue>().AddCallback(Constants.SMSG_CHART, ResponseChart);
		//shopObject.GetComponent<Shop>().enabled = true;
	}
	
	// Use this for initialization
	void Start () {
		//windowRect = new Rect (0, 0, 200, 200); //0,0,width,height
		//windowRect.x = (Screen.width - windowRect.width) / 2;
		//windowRect.y = (Screen.height - windowRect.height) / 2;
		//worldObject = GameObject.Find("WorldObject");

		/*
		ConnectionManager cManager = mainObject.GetComponent<ConnectionManager>();
		
		if (cManager) {
			//Debug.Log("Reached here!!");
			cManager.Send(RequestShop(1));
		}
		*/

		int[] temp = new int[SpeciesTable.speciesList.Count];
		int i = 0;
		foreach (KeyValuePair<int, SpeciesData> s in SpeciesTable.speciesList) {
			temp[i++] = s.Key;
		}
		Initialize(null, temp);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnGUI() {
		if (!isHidden) {
			windowRect = new Rect(25, Screen.height - height - 10f, 100, height);
			windowRect = GUI.Window(Constants.SHOP_WIN, windowRect, ShopMakeWindow, "Shop");
		}
		
	}
	
	void ShopMakeWindow(int id) {
		//if (GUI.Button(new Rect(10, 50, width - 20, 30), "Select Tile")) Submit();
		//GUILayout.Label("Species:");
		//GUI.SetNextControlName("username_field");
		//Debug.Log("Reached here!!!");
		if (GUI.Button(new Rect(10, 50, 80, 30), "Species")) {
			//GUIStyle style = new GUIStyle(GUI.skin.label);
			//style.fontSize = 18;
			//Debug.Log("Clicked button!!");
			
			//isHidden = true;
			//GUI.Label(new Rect(30, 30, 80, 50), "Choose Your Species", style);
			
			GameObject.Find("Cube").GetComponent<ShopPanel>().Show();
			GameObject.Find("Cube").GetComponent<ShopCartPanel>().Show();
			GameObject.Find("Cube").GetComponent<ShopInfoPanel>().Show();
//			GameObject.Find("MapCamera").GetComponent<MapCamera>().enabled = false;
//			GameObject.Find("MapCamera").GetComponent<MapCamera>().RoamingCursor.SetActive(false);
			Hide();
			/*
			//shopObject.GetComponent<ShopPanel>().MakeWindow();
						//shopObject.GetComponent<ShopInfoPanel>().MakeWindow();
						//shopObject.GetComponent<ShopCartPanel>().MakeWindow();
		
						GUI.DragWindow(); */
		}
	}
	
	public void Initialize(string[] config, int[] speciesList) {
		foreach (int species_id in speciesList) {
			if (!SpeciesTable.speciesList.ContainsKey(species_id)) {
				continue;
			}
			SpeciesData species = new SpeciesData(SpeciesTable.speciesList[species_id]);
			
			species.image = Resources.Load(Constants.IMAGE_RESOURCES_PATH + species.name) as Texture;
			
			if (!itemList.ContainsKey(species_id)) {
				itemList.Add(species_id, species);
			}
		}
	}

	public void Show() {
		gInshop = false;
		isHidden = false;
	}
	
	public void Hide() {
		gInshop = true;
		isHidden = true;
	}
}
