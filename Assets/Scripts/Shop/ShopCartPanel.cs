using UnityEngine;

using System;
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
	private GameState gs;
	private int creditsRemaining;
	private int cartSum;
	private int speciesToBuy_id;
	private float speciesToBuy_biomassServer;
	private int speciesToBuy_cost;
	private bool showPurchase;
	private bool showPurchaseError;
	private bool showPurchaseComplete;
	private bool isInitial;
	private Rect purchaseRect;
	private Rect purchaseErrorRect;
	private Rect purchaseCompleteRect;
	private Rect entryRect;
	private int purWindowWidth = 300;
	private int purWindowHeight = 200;
	private int purErrWindowWidth = 240;
	private int purErrWindowHeight = 140;
	private Texture2D bgTexture;
	private string purStr;
    
    // Use this for initialization
    void Start () {
        shopObject = GameObject.Find("Cube");
        shop = shopObject.GetComponent<Shop>();
        cartList = new Dictionary<int, int>();
		gs = GameObject.Find ("Global Object").GetComponent<GameState> ();
        Hide();
		showPurchase = false;
		showPurchaseError = false;
		showPurchaseComplete = false;
		purchaseRect = new Rect ((Screen.width - purWindowWidth) / 2, (Screen.height - purWindowHeight) / 2, 
				purWindowWidth, purWindowHeight);
		purchaseErrorRect = new Rect ((Screen.width - purErrWindowWidth) / 2, (Screen.height - purErrWindowHeight) / 2, 
				purErrWindowWidth, purErrWindowHeight);
		purchaseCompleteRect = new Rect ((Screen.width - purErrWindowWidth) / 2, (Screen.height - purErrWindowHeight) / 2, 
			purErrWindowWidth, purErrWindowHeight);
		entryRect = new Rect ((purWindowWidth - 45)/2, 90, 50, 25);
		bgTexture = Resources.Load<Texture2D> (Constants.THEME_PATH + Constants.ACTIVE_THEME + "/gui_bg");
		purStr = "0";
		isInitial = true;
    }
    
    // Update is called once per frame
    void Update () {
        
    }
    
    void OnGUI() {
        if (!isAnHidden) {
           //  GUI.Label(new Rect(620, 30, 200, 200), "Biomass Capacity");
            int height = 20 + cartList.Count * 90;
            
			cartSum = 0;
            totalBiomass = 0;
            
            scrollPosition = GUI.BeginScrollView(new Rect(580, 140, 200, 400), scrollPosition, new Rect(0, 0, 100, height));
            GUI.Box(new Rect(0, 0, 200, Mathf.Max(500, height)), "");
            
            List<int> items = new List<int>(cartList.Keys);
            //Debug.Log(cartList.Count);
            
            for (int i = 0; i < cartList.Count; i++) {
                int species_id = items[i];
                SpeciesData species = shopObject.GetComponent<Shop>().itemList[species_id];
                
                totalBiomass += cartList[species_id];
				SpeciesData speciesData = SpeciesTable.speciesList [species_id];

				if (speciesData.biomassServer > 0) {
					cartSum += (int) (cartList [species_id] * (speciesData.cost / speciesData.biomassServer));
				}

                
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
            
            // GUI.Label(new Rect(620, 50, 200, 200), totalBiomass.ToString());
            
            {
                GUIStyle style = new GUIStyle(GUI.skin.label);
                style.alignment = TextAnchor.UpperCenter;
                
                GUI.Label(new Rect(550, 555, 100, 100), cartList.Count.ToString() + " / 10", style);
            }
            
            {
                GUIStyle style = new GUIStyle(GUI.skin.button);
                style.alignment = TextAnchor.MiddleCenter;
                style.normal.textColor = Color.white;
                
				if (GUI.Button(new Rect(710, 550, 70, 30), "Cancel", style) && !showPurchaseComplete)
                {
                    cartList.Clear();
                    shopObject.GetComponent<Shop>().Show();
                    GameObject.Find("Cube").GetComponent<ShopPanel>().Hide();
                    GameObject.Find("Cube").GetComponent<ShopCartPanel>().Hide();
                    GameObject.Find("Cube").GetComponent<ShopInfoPanel>().Hide();
                }

				if ((GUI.Button(new Rect(630, 550, 70, 30), "Purchase", style)) && (cartList.Count != 0) && !showPurchaseComplete) {
					Game.networkManager.Send(ShopActionProtocol.Prepare(0, cartList, cartSum), ResponseShopAction);

					int group_id = 0;
					// Adds species/biomass to your in memory ecosystem 
					foreach (KeyValuePair<int, int> entry in cartList) {
						gs.PurchaseSpecies (group_id, entry.Key, entry.Value);
						Debug.Log("" + entry.Key + " " + entry.Value);
					}
                }
            }

			creditsRemaining = Mathf.Max (0, GameState.player.credits - cartSum);

			GUI.BeginScrollView(new Rect(580, 70, 200, 60), scrollPosition, new Rect(0, 0, 200, 60));

			GUI.Box(new Rect(0, 0, 200, 60), "");

			GUIStyle style1 = new GUIStyle(GUI.skin.label);
			style1.alignment = TextAnchor.UpperLeft;

			GUI.color = Color.white;

			if (!showPurchaseComplete) {
				GUI.Label(new Rect(10, 20, 180, 30), "Remaining Credits: " + creditsRemaining, style1);
			}
				
			GUI.EndScrollView();

			if (showPurchase) {
				GUI.Window (Constants.SHOP_PURCHASE_POPUP, purchaseRect, MakePurchaseWindow, "Purchase", GUIStyle.none);
			}

			if (showPurchaseError) {
				GUI.Window (Constants.SHOP_PURCHASE_ERROR, purchaseErrorRect, 
						MakePurchaseErrorWindow, "Purchase Error", GUIStyle.none);
			}

			if (showPurchaseComplete) {
				GUI.Window (Constants.SHOP_PURCHASE_COMPLETE, purchaseCompleteRect, 
						MakePurchaseCompleteWindow, "Purchase Complete", GUIStyle.none);
			}

        }
        
    }    

	void MakePurchaseWindow(int id) {
		Functions.DrawBackground(new Rect(0, 0, purWindowWidth, purWindowHeight), bgTexture);
		GUIStyle style = new GUIStyle(GUI.skin.label);
		style.alignment = TextAnchor.UpperCenter;
		style.fontSize = 14;

		GUI.Label(new Rect((purWindowWidth - 300)/2, 20, 300, 30), "Please enter Biomass to purchase", style);

		int biomassMax = (int) (creditsRemaining * (speciesToBuy_biomassServer / speciesToBuy_cost));
		if (cartList.ContainsKey (speciesToBuy_id)) {
			SpeciesData speciesData = SpeciesTable.speciesList [speciesToBuy_id];
			biomassMax += cartList [speciesToBuy_id];
		}

		GUI.Label(new Rect((purWindowWidth - 250)/2, 40, 250, 30), "up to a maximum of " + biomassMax, style);

		GUI.SetNextControlName("biomassEntry");
		purStr = GUI.TextField(entryRect, purStr);

		if (GUI.Button(new Rect(40, purWindowHeight - 50, 80, 30), "Purchase")) {
			showPurchase = false;

			int purBiomass;
			if (!Int32.TryParse(purStr, out purBiomass)) {
				purBiomass = -1;
			}
			if (purBiomass == 0) {
				if (cartList.ContainsKey (speciesToBuy_id)) {
					cartList.Remove (speciesToBuy_id);
				}					
			} else if ((purBiomass > 0) && (purBiomass <= biomassMax)) {

				if (!cartList.ContainsKey(speciesToBuy_id)) {
					cartList.Add(speciesToBuy_id, 0);
				}
				cartList[speciesToBuy_id] = purBiomass;
			} else {
				showPurchaseError = true;
			}
		}

		if (GUI.Button(new Rect(purWindowWidth - 100,  purWindowHeight - 50, 60, 30), "Cancel")) {
			showPurchase = false;
		}

		if (isInitial) {  // && GUI.GetNameOfFocusedControl() == "") {
			GUI.FocusControl("biomassEntry");
			isInitial = false;
		}
	}
		
	void MakePurchaseErrorWindow(int id) {
		Functions.DrawBackground(new Rect(0, 0, purErrWindowWidth, purErrWindowHeight), bgTexture);
		GUIStyle style = new GUIStyle(GUI.skin.label);
		style.alignment = TextAnchor.UpperCenter;
		style.fontSize = 14;

		GUI.Label(new Rect((purErrWindowWidth - 120)/2, 25, 120, 30), "Invalid Entry", style);

		if (GUI.Button(new Rect((purErrWindowWidth - 100)/2,  purErrWindowHeight - 50, 100, 30), "OK")) {
			showPurchaseError = false;
		}			
	}

	void MakePurchaseCompleteWindow(int id) {
		Functions.DrawBackground(new Rect(0, 0, purErrWindowWidth, purErrWindowHeight), bgTexture);
		GUIStyle style = new GUIStyle(GUI.skin.label);
		style.alignment = TextAnchor.UpperCenter;
		style.fontSize = 14;

		GUI.Label(new Rect((purErrWindowWidth - 200)/2, 20, 200, 30), "Purchase Complete!", style);

		GUI.Label(new Rect((purErrWindowWidth - 220)/2, 45, 220, 30), "New Credit Balance: " + GameState.player.credits, style);

		if (GUI.Button(new Rect((purErrWindowWidth - 100)/2,  purErrWindowHeight - 50, 100, 30), "OK")) {
			showPurchaseComplete = false;
			shopObject.GetComponent<Shop>().Show();
			GameObject.Find("Cube").GetComponent<ShopPanel>().Hide();
			GameObject.Find("Cube").GetComponent<ShopCartPanel>().Hide();
			GameObject.Find("Cube").GetComponent<ShopInfoPanel>().Hide();

			cartList = new Dictionary<int, int>();
		}			
	}

    public void Add(SpeciesData species) {
		if (showPurchaseComplete) {
			return;
		}
		speciesToBuy_id = species.species_id;
		SpeciesData speciesData = SpeciesTable.speciesList [species.species_id];
		speciesToBuy_cost = speciesData.cost;
		speciesToBuy_biomassServer = speciesData.biomassServer;

		if (cartList.ContainsKey (species.species_id)) {
			purStr = "" + cartList [species.species_id];
		} else {
			purStr = "0";
		}
		isInitial = true;
		showPurchase = true;
    }

    public void ResponseShopAction(NetworkResponse response) {
        ResponseShopAction args = response as ResponseShopAction;
		Debug.Log("ResponseShopAction, a/s: " + args.action + " " + args.status);
		Debug.Log("ResponseShopAction, new credit: " + args.credits);
		GameState.player.credits = args.credits;
		showPurchaseComplete = true;
    }

    public void Show() {
        isAnHidden = false;
    }
    
    public void Hide() {
        isAnHidden = true;
    }		
}
