using UnityEngine;

using System.Collections;
using System.Collections.Generic;

public class Shop : MonoBehaviour {
  
  private GameObject worldObject;
  private GameObject shopObject;
  private GameObject mainObject;
  private ConvergeManager manager;
  
  public Dictionary<int, SpeciesData> itemList { get; set; }
  public SpeciesData selectedSpecies { get; set; }
  
  // Window Properties
  private float width = 120;
  private float height = 135;
  // Other
  private Rect windowRect;
  private Rect avatarRect;
  private Texture avatar;
  private Rect[] buttonRectList;
  private GameObject messageBox;
  private Vector2 scrollPosition = Vector2.zero;
  private bool isHidden { get; set; }
  private Database foodWeb = null;

  static public bool gInshop = false;
  
  void Awake() {
	manager = new ConvergeManager();
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
      windowRect = new Rect(25, Screen.height - height - 10f, width, height);
      windowRect = GUI.Window(Constants.SHOP_WIN, windowRect, ShopMakeWindow, "Shop");
    }
    
  }
  
  void ShopMakeWindow(int id) {
    if (GUI.Button(new Rect(20, 35, 80, 30), "Purchase")) {
	  // GameObject.Find("Cube").GetComponent<Graph>().Hide();
      GameObject.Find("Cube").GetComponent<ShopPanel>().Show();
      GameObject.Find("Cube").GetComponent<ShopCartPanel>().Show();
      GameObject.Find("Cube").GetComponent<ShopInfoPanel>().Show();
      GameObject.Find("MenuScript").GetComponent<MenuScript>().menuOpen = true;
      GameObject.Find("MenuScript").GetComponent<MenuScript>().disableDropDown();
      GameObject.Find("Local Object").GetComponent<WorldMouse>().popOversEnabled = false;
	  // GameObject.Find("Cube").GetComponent<Graph>().HideButton();
      Hide();
    }
	
	if (GUI.Button(new Rect(20, 85, 80, 30), "Owned")) {
  	  // GameObject.Find("Cube").GetComponent<Graph>().Hide();
	  Debug.Log ("Clicked owned button");
	  GameObject.Find("MenuScript").GetComponent<MenuScript>().menuOpen = true;
	  GameObject.Find("MenuScript").GetComponent<MenuScript>().disableDropDown();
	  GameObject.Find("Local Object").GetComponent<WorldMouse>().popOversEnabled = false;
	  foodWeb = Database.NewDatabase (gameObject, Constants.MODE_OWNED, manager);
	  foodWeb.SetActive (true, "");
	  // Hide();
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
    GameObject.Find("MenuScript").GetComponent<MenuScript>().menuOpen = false;
    GameObject.Find("Local Object").GetComponent<WorldMouse>().popOversEnabled = true;
    GameObject.Find("MenuScript").GetComponent<MenuScript>().enableDropdown();
    GameObject.Find("MenuScript").GetComponent<MenuScript>().showTopBar();
    gInshop = false;
    isHidden = false;
  }
  
  public void Hide() {
    GameObject.Find("MenuScript").GetComponent<MenuScript>().hideTopBar();
    GameObject.Find("MenuScript").GetComponent<MenuScript>().disableDropDown();
    GameObject.Find("Local Object").GetComponent<WorldMouse>().popOversEnabled = false;
    gInshop = true;
    isHidden = true;
  }
}
