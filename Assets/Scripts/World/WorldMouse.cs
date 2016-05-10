using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WorldMouse : MonoBehaviour {
	
	public GameObject defenderCursor;
	public GameObject roamingCursor;
	public GameObject attackerCursor;
	public Zone currentTile { get; set; }

	public TileInfoGUI tileInfoGUI;
    private string[] terrain = new string[] { "Desert", "Jungle", "Grasslands", "Arctic" };

    GameObject tileUi;


    private int prevPlayerID;

	private MapCamera mapCamera;

	// Use this for initialization
	void Start() {
		tileInfoGUI = gameObject.AddComponent<TileInfoGUI>();
		mapCamera = GameObject.Find("MapCamera").GetComponent<MapCamera>();
        tileUi = GameObject.Find("Canvas/Tilepurchasedialog") as GameObject;
        tileUi.SetActive(false);

    }
    void Awake()
    {
        
    }
	
	// Update is called once per frame
	void Update() {
		
		RaycastHit hit = new RaycastHit();

		if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit)&& (!EventSystem.current.IsPointerOverGameObject ())) {
			
			if (hit.transform.gameObject.tag == "Zone") {
				currentTile = hit.transform.gameObject.GetComponent<Zone>();

				if (currentTile.player_id != prevPlayerID) {
					if (prevPlayerID > 0) {
						GameObject.Find("Map").GetComponent<Map>().SelectTerritory(prevPlayerID, false);
					}

					GameObject.Find("Map").GetComponent<Map>().SelectTerritory(currentTile.player_id, true);
					prevPlayerID = currentTile.player_id;
				}

				string owner_name = "";
	
				if (currentTile.player_id > 0) {
					owner_name = GameObject.Find("Map").GetComponent<Map>().playerList[currentTile.player_id].name;

					Color playerColor = GameObject.Find("Map").GetComponent<Map>().playerList[currentTile.player_id].color;;
					tileInfoGUI.bgColor = new Color(playerColor.r, playerColor.g, playerColor.b, 0.8f);
				} else {
					tileInfoGUI.DefaultColor();
				}
	
				tileInfoGUI.SetInformation(currentTile.terrain_type, currentTile.v_capacity, owner_name);
				tileInfoGUI.position = Camera.main.WorldToScreenPoint(currentTile.transform.position);

				tileInfoGUI.SetActive(true);

				roamingCursor.SetActive(true);
				roamingCursor.transform.position = currentTile.transform.position + new Vector3(0, 0.1f, 0);
//				roamingCursor.renderer.material.color = new Color32(0, 181, 248, 255);
			}
		} else {
			tileInfoGUI.SetActive(false);
			roamingCursor.SetActive(false);
			currentTile = null;
		}

        //used for tile purchasing 
		switch (InputExtended.GetMouseNumClick(0)) {
			case 1: // Single Click
                    //modifies which tile you are currently selecting 
                {
                    if (!EventSystem.current.IsPointerOverGameObject())
                    {
                        //Check to make sure they are not currently using shop (Raycasting issues) 
                        if (!Shop.gInshop)
                        {
                            if(tileUi.activeSelf && currentTile.player_id == 0)
                            {
                                Debug.Log(tileUi.name);
                                tileUi.transform.GetChild(2).GetComponent<Text>().text = terrain[currentTile.terrain_type - 1];
                                tileUi.transform.GetChild(3).GetComponent<Text>().text = currentTile.v_capacity.ToString();
                            }
                        }
                    }
                 }
				break;

		    case 2: // Double Click
                // This case is being used for purchasing tiles, and accessing your ecosystem 
			if (!EventSystem.current.IsPointerOverGameObject ()) {
                    //Check to make sure they are not currently using shop (Raycasting issues) 
                if (!Shop.gInshop)
                    {
                        //Launch the ecosystem if they are clicking on owned tiles 
                        if (currentTile.player_id == GameState.player.GetID())
                        {
                            mapCamera.Move(currentTile.transform.position);

                        } else if (currentTile.player_id == 0 && !tileUi.activeSelf) // check to see if tile can be purchased 
                            // **NOTE** if the tile's owner is "NULL" in the db, player_id = 0
                        {
                            Debug.Log(tileUi.name);
                            tileUi.transform.GetChild(2).GetComponent<Text>().text = terrain[currentTile.terrain_type -1 ] ;
                            tileUi.transform.GetChild(3).GetComponent<Text>().text = currentTile.v_capacity.ToString();
                            // set the price using Request/REsponse 
                            // tileUi.transform.GetChild(4).GetComponent<Text>().text = "XXXX";
                            tileUi.SetActive(true);


                        }
                          //View other player's ecosystem (Future )
                          else
                        {
                            //content 
                        }
                    }
	         }
                Debug.Log("double click event success");
                //Exit the case
                break;       
	   }
	}
}
