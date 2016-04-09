using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BuildMenu : MonoBehaviour
{

    // Background material
    public Material backgroundMaterial;

    public static DemButton buttons;

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


    //Loading Resources
    void Awake()
    {
        backgroundMaterial = Resources.Load<Material>("DontEatMe/Materials/DontEatMeBg");

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


        buttons = gameObject.AddComponent<DemButton>();
 
        // Creates a button for each plant
        int i = 0;
        foreach (DemAnimalFactory plant in plants)
        {
            GameObject button = buttons.CreateButton(59, 0 - (80 + i * (buttons.getYSize() - 2)));

            buttons.MakeButtonImage(plant, button);

            // Add an onClick listener to detect button clicks
            button.GetComponent<Button>().onClick.AddListener(() => { selectSpecies(button); });

            i++;
        }
 
    }


    void selectSpecies(GameObject button)
    {
        Debug.Log("Clicked " + button.name);
        

        DemAudioManager.audioClick.Play();

        // If a selection is currently in progress...
        if (DemMain.currentSelection)
        {
            // Ignore button click if for the same species
            if (currentAnimalFactory == plants[System.Int32.Parse(button.name)])
                return;
            // Otherwise, destroy the current selection before continuing
            else
                Destroy(DemMain.currentSelection);
        }
        // Set / reset currentlyBuilding

        //currentlyBuilding = info;
        currentAnimalFactory = plants[System.Int32.Parse(button.name)];


        // Create the current prey object
        //GameObject currentPlant = plant.Create(); //DemAnimalFactory.Create(currentlyBuilding.name , 0 ,0) as GameObject;

        // Define the current prey's initial location relative to the world (i.e. NOT on a screen pixel basis)
        Vector3 init_pos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y));
        init_pos.z = -1.5f;

        // Instantiate the current prey
        DemMain.currentSelection = plants[System.Int32.Parse(button.name)].Create();
        DemMain.currentSelection.GetComponent<BuildInfo>().isPlant = true;


        // Set DemMain's preyOrigin as the center of the button
        DemMain.setBuildOrigin(Camera.main.ScreenToWorldPoint(Input.mousePosition));

        DemMain.boardController.SetAvailableTiles();
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
