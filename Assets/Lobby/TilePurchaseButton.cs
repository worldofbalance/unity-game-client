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
    Zone currentTile;
    GameObject purchaseCursor;


    void Awake()
    {
        // setting up gameobject via script
        tileUi = GameObject.Find("Tilepurchasedialog");
       // button1 = tileUi.transform.GetChild(...).GetComponent<Button>();
      //  text1 = button1.transform.GetChild(0).GetComponent<Text>();


        // if script is attached to button in editor
        currentButton = this.gameObject.GetComponent<Button>();
        currentText = currentButton.transform.GetChild(0).GetComponent<Text>();

    }

    void Start()
    {
        localObject = GameObject.Find("Local Object");
        currentTile = localObject.GetComponent<WorldMouse>().currentSelectedTile;
        purchaseCursor = GameObject.Find("PurchaseCursor") as GameObject;
    }
    void Update()
    {
        currentTile = localObject.GetComponent<WorldMouse>().currentSelectedTile;
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
            tileUi.SetActive(false);
            purchaseCursor.SetActive(false);

        }
        else
        {
            Debug.Log("Failed to send response");
        }


    }

}
