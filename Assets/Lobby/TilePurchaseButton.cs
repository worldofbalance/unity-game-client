using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TilePurchaseButton : MonoBehaviour, IPointerClickHandler
{
    public GameObject tileUi;

    // via script
    public Button button1;
    public Text text1;

    // via editor
    public Button currentButton;
    public Text currentText;

    // getting the current tile id 
    GameObject localObject;
    GameObject map;
    Zone currentTile;
    Zone currentWorldMouseTile;
    GameObject purchaseCursor;
    GameObject tilePurchaseSuccess;
    GameObject welcome;

    GameObject mapTileSelected;
    


    void Awake()
    {
        // setting up gameobject via script
        tileUi = GameObject.Find("Tilepurchasedialog");
        tilePurchaseSuccess = GameObject.Find("Canvas/PurchaseSuccess") as GameObject;
        welcome = GameObject.Find("Canvas/FirstWelcome") as GameObject;
        // button1 = tileUi.transform.GetChild(...).GetComponent<Button>();
        //  text1 = button1.transform.GetChild(0).GetComponent<Text>();


        // if script is attached to button in editor
        currentButton = this.gameObject.GetComponent<Button>();
        currentText = currentButton.transform.GetChild(0).GetComponent<Text>();

    }

    void Start()
    {
        localObject = GameObject.Find("Local Object");
        map = GameObject.Find("Map");
        currentTile = localObject.GetComponent<WorldMouse>().currentSelectedTile;
        purchaseCursor = GameObject.Find("PurchaseCursor") as GameObject;

    }
    void Update()
    {
        currentTile = localObject.GetComponent<WorldMouse>().currentSelectedTile;
        mapTileSelected = GameObject.Find("Zone " + currentTile.row + "-" + currentTile.column) as GameObject;
    }

    public void OnPointerClick(PointerEventData eventdata)
    {
        if (currentButton.IsInteractable())
        {


            if (currentText.text == "Cancel")
            {
                tileUi.SetActive(false);
                purchaseCursor.SetActive(false);


            }
            else if (currentText.text == "Purchase")

            {
                Debug.Log("Tile Id of Current Tile: " + currentTile.zone_id);
                Debug.Log("TilePurchase Successful");
                Game.networkManager.Send(
                                TilePurchaseProtocol.Prepare(currentTile.zone_id), processTilePurchase);

            }
            else if (currentText.text == "Close")
            {
                welcome.SetActive(false);
                tilePurchaseSuccess.SetActive(false);
            }

        }
        // does what you need when you click stuff
        // code here will run for the button you attach this to
        // if buttons have different functions set up the logic to differentiate
        // functions
        // Suggestion: using button text to check for button and separating the functions
    }
    public void processTilePurchase(NetworkResponse response)
    {
        TilePurchase args = response as TilePurchase;
        Debug.Log("inside process TilePurchase Process ");

        if (args.status == 0)
        {
            //Setting the new player credits, and relaunching the map 
            GameState.player.credits = args.credits; 

            tileUi.SetActive(false);
            purchaseCursor.SetActive(false);
            //Updating the information just for the user..
            //Note: the actual map will not reflect the purchases of others until the user restarts their client
            mapTileSelected.GetComponent<Zone>().player_id = GameState.player.GetID();
            Color playerColor = GameState.player.color;
            currentTile.transform.GetChild(0).GetComponent<Renderer>().material.color = playerColor;

            //adding the player's new tile to their territory 
            if (!map.GetComponent<Map>().playerTiles.ContainsKey(GameState.player.GetID()))
            {
                map.GetComponent<Map>().playerTiles.Add(GameState.player.GetID(), new List<GameObject>());
            }

            if (!map.GetComponent<Map>().playerList.ContainsKey(GameState.player.GetID()))
            {
                map.GetComponent<Map>().playerList.Add(GameState.player.GetID(), GameState.player);
            }

            //Add the tile to the playerTile List 
            map.GetComponent<Map>().playerTiles[GameState.player.GetID()].Add(mapTileSelected);


            //adding the territory ui to the map 
            GameObject select = Instantiate(map.GetComponent<Map>().hexSelect) as GameObject;
            select.transform.position = currentTile.transform.position + new Vector3(0, 0.1f, 0);
            select.transform.parent = currentTile.transform.GetChild(0);
            select.GetComponent<Renderer>().material.color = playerColor;
            select.SetActive(false);

            tilePurchaseSuccess.transform.GetChild(3).GetComponent<Text>().text = args.credits.ToString();
            tilePurchaseSuccess.SetActive(true);

        }
        else
        {
            Debug.Log("Failed to send response");
        }


    }

    public void regenerateMap()
    {


    }

}
