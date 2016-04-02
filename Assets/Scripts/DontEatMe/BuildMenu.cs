using UnityEngine;
using System.Collections;

public class BuildMenu : MonoBehaviour
{

    // Background material
    public Material backgroundMaterial;

    // Button prefab
    public GameObject buttonPrefab;

    // Canvas Object
    public GameObject canvasObject;

    public DemButton buttons;

    // Currently building...
    public static BuildInfo currentlyBuilding;

    public static DemAnimalFactory currentAnimalFactory;

    // Currently about to delete?
    public static bool currentlyDeleting = false;

    // Player's current resource amount
    public static int currentResources = 250;

    // Player's current score amount
    public static int score = 0;

    // Plant prefabs
    public DemAnimalFactory[] plants;

    // Prey prefabs
    public DemAnimalFactory[] prey;






    // this method increases score every 2s
    void increaseResources()
    {
        currentResources += 50;
    }

    void MakeButton(int numberButtons, float xPos, float yPos, float zPos)
    {
        float buttonWidth = buttonPrefab.GetComponent<RectTransform>().sizeDelta.x;
        float buttonHeight = buttonPrefab.GetComponent<RectTransform>().sizeDelta.y;
        Debug.Log(buttonHeight);

        for(int i = 0; i < numberButtons; i++)
        {
            GameObject button = Instantiate(buttonPrefab) as GameObject;
            button.name = "Button" + i;

            button.transform.SetParent(canvasObject.transform);
            button.GetComponent<RectTransform>().anchoredPosition = new Vector2(xPos, 0 - yPos - i * buttonHeight);
        }
        
    }

    //Loading Resources
    void Awake()
    {
        backgroundMaterial = Resources.Load<Material>("DontEatMe/Materials/DontEatMeBg");
        buttonPrefab = Resources.Load<GameObject>("DontEatMe/Prefabs/Button2");
        canvasObject = GameObject.Find("Canvas");

    }
    // Use this for initialization
    void Start()
    {
        // set resources to grow over time
        InvokeRepeating("increaseResources", 2, 3.0F);
        
        /**
         * Elaine:
         * All this can be done in the editor beforehand since the background never changes
         * but we can decide on this later
         * I also commented out the background code in DemMain.cs since the background looks fine w/o it
         */

        // Creates the background manually
        GameObject backgroundImage = GameObject.CreatePrimitive(PrimitiveType.Quad);
        backgroundImage.name = "DemBackGround";

        Destroy(backgroundImage.GetComponent<MeshCollider>());

        backgroundImage.GetComponent<Renderer>().material = backgroundMaterial;
        
        backgroundImage.transform.localPosition = 
            new Vector3(backgroundImage.transform.localPosition.x, backgroundImage.transform.localPosition.y, 1);
        backgroundImage.transform.localScale = new Vector3(14, 7, backgroundImage.transform.localScale.z);

        // Creates the buttons
        buttons = gameObject.AddComponent<DemButton>();
        buttons.MakeButton(5, 2, 59, 80);

        plants = new DemAnimalFactory[6];

        plants[0] = new DemAnimalFactory("Acacia");
        plants[1] = new DemAnimalFactory("Baobab");
        plants[2] = new DemAnimalFactory("Big Tree");
        plants[3] = new DemAnimalFactory("Fruits And Nectar");
        plants[4] = new DemAnimalFactory("Grass And Herbs");
        plants[5] = new DemAnimalFactory("Trees And Shrubs");


        prey = new DemAnimalFactory[6];
        prey[0] = new DemAnimalFactory("Bohor Reedbuck");
        prey[1] = new DemAnimalFactory("Bat-Eared Fox");
        prey[2] = new DemAnimalFactory("Kori Buskard");
        prey[3] = new DemAnimalFactory("Black Backed Jackal");
        prey[4] = new DemAnimalFactory("Dwarf Mongoose");
        prey[5] = new DemAnimalFactory("Dwarf Epauletted Bat");


    }

    public void endGame()
    {
        /*
            Debug.Log("Game ended with X coins: " + coins);

            //LOBBY TEAM, PUT YOUR RETURN CODE HERE, PASS BACK
            //coins variable
            NetworkManager.Send(
                EndGameProtocol.Prepare(1, coins),
                ProcessEndGame
            );
      */
    }

    // Updates player's credits
    public void ProcessEndGame(NetworkResponse response)
    {
        ResponsePlayGame args = response as ResponsePlayGame;

        if (args.status == 1)
        {

            GameState.player.credits = args.creditDiff;
            Debug.Log(args.creditDiff);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }


}
