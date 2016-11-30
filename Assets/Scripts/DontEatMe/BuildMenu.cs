using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;
using System;


/**
    Script responsible for creating and managing the menu elements of the UI.
*/
public class BuildMenu : MonoBehaviour
{
    // Background material
    public Material backgroundMaterial;

    // Access to DemButton script for button creation
    public DemButton demButton;

    //Access to DemRectUI script for RectUI creation
    public DemRectUI demRectUI;
    private GameObject quitUI;	//instance of UI for after pressed quit button
    private GameObject instructionUI; //instance of UI for after pressed how to play button
    private GameObject gameOverUI;  //instance of UI for after pressed quit button

    // Statistics UI components
    private GameObject statUI; // Main container
    public Statistic statistic; // Statistical data
    private GameObject t1, t2, t3, t4, t5, t6; // Text objects for displaying statistical data

    // Player build menu components and variables
    public DemAnimalFactory currentAnimalFactory; // Factory for current player plant/prey selection...
    public BuildInfo currentlyBuilding; // ... and its info
    bool selectingPlant = true; // True if the current build menu is plants, false if prey
    public bool currentlyDeleting = false; // True if currently deleting a game object

    // Player properties
    public int currentResources = 250; // Current player resources
    public int score = 0; // Current player score

    // Prefabs 
    public DemAnimalFactory[] plants; // Plants...
    public DemAnimalFactory[] prey; // Prey...

    // Additional game components
    public GameObject[] menuButtons; // Menu buttons
    private GameObject mainObject; // Master game object
    private DemTurnSystem turnSystem; // Enables turn system functionality
    private DemMain main; // Main Don't Eat Me script

    // Typically serves as a center pivot point for button objects.
    // As this is a commonly-used Vector2, defining a single static entity eliminates the need to construct a new
    // Vector2 instance in every case, akin to Vector2.zero, eg. 
    private static Vector2 vector2_half = new Vector2(0.5f, 0.5f);
    // Property declaration for vector2_half
    public static Vector2 Vector2_half
    {
        get
        {
            return vector2_half;
        }
    }

    // Biomass levels
    private int plantBiomass; // Tier 1 (plant) biomass level (change name to tier1Biomass for consistency)
    private int tier2Biomass; // Tier 2 (prey) biomass level
    private int tier3Biomass; // Tier 3 (predator) biomass level

    // Other sh!t
    // TODO: figure out what these are so they can be accurately commented.
    // Note to future programmers: all code and no documentation makes Jack a dull boy.
    public GameObject panelObject;
    public GameObject menuPanel;
    public GameObject scoreText;
    public GameObject livesText;
    public GameObject plantBioText;
    public GameObject tier2BioText;
    public GameObject tier3BioText;
    public GameObject turnSystemText;
    private Font fontFamily;
    private Sprite popupBackground;
    private Sprite infoWidget;

    //mainUI
    public GameObject mainUIObject;
    public GameObject canvasObject;

    // Tutorial / instructions
    public GameObject instructionPanelObj; // Main panel / canvas for tutorial display
    private Button continuePageButton; // Continue button
    private Sprite[] instructionPages = new Sprite[9]; // Sequential array of instruction pages

    string[] headerText = new string[9]
    {
        "Welcome to Don't Eat Me!",
        "How do we stop the predators?",
        "",
        "",
        "What are Prey?",
        "",
        "",
        "",
        "Remember!"
    };

    string[] text = new string[9]
    {
        "The objective of this game is to prevent the predators from reaching the left of the screen. Once three predators make it to the left it's game over!",
        "Before you can stop the predators, first you must place a plant on the board!",
        "When you select a plant, certain tiles will flash blue to indicate that a plant can be placed.",
        "Certain adjacent tiles will flash green to indicate that a prey can be placed. Different plants will have different adjacent tiles to choose from." ,
        "Prey are animals that the predators feed on to satisfy their hunger.",
        "Once the predator's hunger is satisfied they will leave the field and you will earn a point.",
        "Tap on the Plant button to switch to the prey menu! Once you select a prey, certain tiles will flash blue to indicate a prey can be placed.",
        "Tap on the Plant button to switch to the prey menu! Once you select a prey, certain tiles will flash blue to indicate a prey can be placed.",
        "You can only place plants or prey when it's your turn!"
    };

    /**
        Increases player build resources (i.e. Tier 1 Biomass).
    */
    void increaseResources ()
    {
        currentResources += 50;
    }

    /**
        Displays a fading tooltip at a certain location.
    */
    //IEnumerator displayTextBubble (string text, float duration, Vector3 position)
    //{
        
    //}

    //Loading Resources
    void Awake()
    {
        infoWidget = Resources.Load<Sprite>("DontEatMe/Sprites/infoWidget");
        popupBackground = Resources.Load<Sprite>("DontEatMe/Sprites/popup");
        fontFamily = Resources.Load<Font>("Fonts/Chalkboard");

        backgroundMaterial = Resources.Load<Material>("DontEatMe/Materials/DontEatMeBg");

        mainObject = GameObject.Find("MainObject");

        main = mainObject.GetComponent<DemMain>();

        turnSystem = mainObject.GetComponent<DemTurnSystem>();

        //mainUI add here
        canvasObject = GameObject.Find("Canvas");
        mainUIObject = GameObject.Find("Canvas/mainUI");
        mainUIObject.transform.SetParent(canvasObject.transform);
        mainUIObject.GetComponent<RectTransform>().anchoredPosition = Vector2.zero; // new Vector2(0, 0);
        mainUIObject.GetComponent<RectTransform>().anchorMin = Vector2.zero; //new Vector2(0, 0);
        mainUIObject.GetComponent<RectTransform>().anchorMax = Vector2.one; //new Vector2(1, 1);
        mainUIObject.GetComponent<RectTransform>().offsetMax = Vector2.zero; //new Vector2(0, 0);
        mainUIObject.GetComponent<RectTransform>().offsetMin = Vector2.zero; //new Vector2(0, 0);

        panelObject = GameObject.Find("Canvas/Panel");
        //panelObject = GameObject.Find("Canvas/mainUI/Panel");
        menuPanel = GameObject.Find("Canvas/mainUI/MenuPanel");
        scoreText = GameObject.Find("Canvas/mainUI/CreditsWidget/CreditWidgetText");
        livesText = GameObject.Find("Canvas/mainUI/LivesWidget/LivesWidgetText");
        turnSystemText = GameObject.Find("Canvas/mainUI/TurnWidget/TurnWidgetText");
        plantBioText = GameObject.Find("Canvas/mainUI/Tier1Widget/PlantBioWidgetText");
        tier2BioText = GameObject.Find("Canvas/mainUI/Tier2Widget/WidgetText");
        tier3BioText = GameObject.Find("Canvas/mainUI/Tier3Widget/WidgetText");



        panelObject.GetComponent<Image>().sprite = infoWidget;

        // Instruction Panel
        instructionPanelObj = GameObject.Find("Canvas/InstructionPanel");

        for (int i = 0; i < 8; i++)
            instructionPages[i] = Resources.Load<Sprite>("DontEatMe/InstructionPictures/instructions_p" + (i+2));
        
        /*
            scoreText.GetComponent<Text> ().font = fontFamily;
            //scoreText.GetComponent<Text> ().alignment = TextAnchor.MiddleCenter;
            scoreText.GetComponent<Text> ().alignment = TextAnchor.MiddleCenter;
            scoreText.GetComponent<RectTransform> ().localPosition = new Vector3 (90,0,0);
            scoreText.GetComponent<RectTransform>().anchorMin = new Vector2(0.0f, 0.5f);
            scoreText.GetComponent<RectTransform>().anchorMax = new Vector2(0.0f, 0.5f);
            scoreText.GetComponent<Text> ().color = Color.white;
            scoreText.GetComponent<Text> ().text = "Score: 0";

            livesText.GetComponent<Text> ().font = fontFamily;
            livesText.GetComponent<Text> ().color = Color.white;
            livesText.GetComponent<Text> ().alignment = TextAnchor.MiddleCenter;
            livesText.GetComponent<RectTransform> ().localPosition = new Vector3 (-90,0,0);
            livesText.GetComponent<Text> ().text = "Lives: 3";

            turnSystemText.GetComponent<Text> ().font = fontFamily;
            turnSystemText.GetComponent<Text> ().color = Color.white;
            turnSystemText.GetComponent<Text> ().alignment = TextAnchor.MiddleCenter;
            turnSystemText.GetComponent<RectTransform> ().localPosition = new Vector3 (90,0,0);
            turnSystemText.GetComponent<Text> ().text = "Your Turn!";
    */
        quitUI = null;
        statUI = null;
        statistic = new Statistic();
    }

    // Use this for initialization
    void Start()
    {
        plantBiomass = 12000;
        tier2Biomass = 0;
        tier3Biomass = 0;

        turnSystem.IsTurnLocked();
        turnSystemText.GetComponent<Text>().text = "Your Turn!";

        currentAnimalFactory = null;
        currentlyBuilding = null;

        // Increases resources (i.e. Tier 1 Biomass) over time
        // ???: Since Don't Eat Me is turn-based, a time-based increase seems out of place...
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


        // Constructing the plants and prey the player can use

        plants = new DemAnimalFactory[6];
        plants[0] = new DemAnimalFactory("Acacia");
        plants[1] = new DemAnimalFactory("Baobab");
        plants[2] = new DemAnimalFactory("Big Tree");
        plants[3] = new DemAnimalFactory("Fruits And Nectar");
        plants[4] = new DemAnimalFactory("Grass And Herbs");
        plants[5] = new DemAnimalFactory("Trees And Shrubs");

   
        prey = new DemAnimalFactory[6];
        prey[0] = new DemAnimalFactory("Tree Mouse");
        prey[1] = new DemAnimalFactory("Bat-Eared Fox");
        prey[2] = new DemAnimalFactory("Kori Buskard");
        prey[3] = new DemAnimalFactory("Crested Porcupine");
        prey[4] = new DemAnimalFactory("Oribi");
        prey[5] = new DemAnimalFactory("Buffalo");

        // NEW BUTTON CREATION STARTS HERE
        // To use old buttons comment out the following lines (until line 287) and uncomment OnGUI()


        // Building the buttons
        demButton = gameObject.AddComponent<DemButton>(); // Don't Eat Me button script (consider renaming to DemButtonFactory?)

        // building the RectUI (whatever that is)
        demRectUI = gameObject.AddComponent<DemRectUI>();

        // Toggle button to switch between plant and prey menu
        demButton.Width = 0.1f * Screen.width;
        demButton.Height = Screen.height / 14;
        GameObject toggleButton = demButton.CreateButton(0, 0, "toggle_button");
        demButton.SetButtonText(toggleButton, "Plants");


        // Creates a buttons for plant/prey menu
        demButton.Height = Screen.height / 7;
        menuButtons = new GameObject[6];
        for (int i = 0; i < 6; i++)
        {
            GameObject button = demButton.CreateButton(0, 0 - ((Screen.height / 14) + 10 + i * (demButton.Height - 2)), i.ToString());

            // Set the button images
            demButton.SetButtonImage(plants[i], button);
            demButton.SetButtonImage(prey[i], button);

            // Set the images of the untoggled menu to inactive
            button.transform.Find(prey[i].GetName()).gameObject.SetActive(false);

            // Add an onClick listener to detect button clicks
            button.GetComponent<Button>().onClick.AddListener(() => { selectSpecies(button); });
            // Add a proper pointer handler
            button.AddComponent<BuildButtonPointerHandler>();

            menuButtons[i] = button;
        }

        // Add an onClick listener to dectect button clicks
        toggleButton.GetComponent<Button>().onClick.AddListener(() => { selectMenu(toggleButton, menuButtons); });


        // UPPER RIGHT BUTTONS //
        /*      
                C1               C0

            R0  [Instructions]  [Quit]
            R1                  [Stats]

            Orientation is (0,0) @ upper right.
        */

        float qBX = 0.075f * Screen.width;
        float qBY = 0.075f * Screen.height;
        demButton.Width = qBX;
        demButton.Height = qBY;
        GameObject quitButton = demButton.CreateButton(Screen.width - qBX, 0, "quit_button");
        demButton.SetButtonText(quitButton, "Quit");
        quitButton.GetComponent<Button>().onClick.AddListener(() => { selectQuit(); });
        // quitButton.transform.SetParent(menuPanel.transform); 

        /*  Define the quit button's RectTransform properties

                        .-----------o <---- anchorMax
                        |           |
                        |     o <---+---- pivot
                        |           |
          anchorMin --> o-----------.
        */
        RectTransform qbTransform = quitButton.GetComponent<RectTransform>();
        qbTransform.pivot = vector2_half; // (0.5, 0.5)
        qbTransform.anchorMin = Vector2.one; // (1, 1)
        qbTransform.anchorMax = Vector2.one; // (1, 1)

        Vector2 qbOffset = new Vector2(-0.5f * qBX, -0.5f * qBY);
        qbTransform.offsetMin = qbOffset;
        qbTransform.offsetMax = qbOffset;

        qbTransform.sizeDelta = new Vector2(qBX, qBY);


        // Statistics button 
        // Set dimensions
        float sBX = 0.075f * Screen.width;
        float sBY = 0.075f * Screen.height;
        demButton.Width = sBX;
        demButton.Height = sBY;
        // Create button
        GameObject statButton = demButton.CreateButton(Screen.width - sBX, -sBY, "stats_button");
        demButton.SetButtonText(statButton, "Stats");
        statButton.GetComponent<Button>().onClick.AddListener(() => { selectStatistic(); });

        RectTransform sbTransform = statButton.GetComponent<RectTransform>();
        sbTransform.pivot = vector2_half;
        sbTransform.anchorMin = Vector2.one;
        sbTransform.anchorMax = Vector2.one;

        Vector2 sbOffset = new Vector2(-0.5f * sBX, -(qBY + 0.5f * sBY));
        sbTransform.offsetMin = sbOffset;
        sbTransform.offsetMax = sbOffset;

        sbTransform.sizeDelta = new Vector2(sBX, sBY);

        // Instructions button
        // TODO: resize the button and place it in a better spot
        float iBX = Screen.width / 20.0f;//5.0f;
        float iBY = Screen.height / 15.0f;
        demButton.Width = iBX;
        demButton.Height = iBY;
        GameObject tutorialButton = demButton.CreateButton(Screen.width - iBX, 10, "tutorial_button");
        demButton.SetButtonText(tutorialButton, "?");

        tutorialButton.GetComponent<Image>().sprite = Resources.Load<Sprite>("DontEatMe/Sprites/buttonBackgroundRound");
        tutorialButton.GetComponent<Button>().onClick.AddListener(() => { selectInstruction(); });

        tutorialButton.GetComponent<RectTransform>().pivot = vector2_half; // new Vector2(0.5f, 0.5f);

        tutorialButton.GetComponent<RectTransform>().anchorMin = Vector2.one; //new Vector2(1, 1);
        tutorialButton.GetComponent<RectTransform>().anchorMax = Vector2.one; //new Vector2(1, 1);

        //tutorialButton.GetComponent<RectTransform>().offsetMax = new Vector2(-154, -22);
        //tutorialButton.GetComponent<RectTransform>().offsetMin = new Vector2(-145, -22);
        tutorialButton.GetComponent<RectTransform>().offsetMax = new Vector2(-254, -22);
        tutorialButton.GetComponent<RectTransform>().offsetMin = new Vector2(-245, -22);

        //tutorialButton.GetComponent<RectTransform>().sizeDelta = new Vector2(30, 30);
        tutorialButton.GetComponent<RectTransform>().sizeDelta = new Vector2(50, 50);

        // Reset sizes for quit popup buttons
        demButton.Width = qBX;
        demButton.Height = qBY;
        UpdatePlantBiomass();
        UpdateBuildableAnimals();
    }


    // Toggle between plant and prey menu when the toggle button is clicked
    void selectMenu(GameObject tButton, GameObject[] mButtons)
    {
        Debug.Log("Clicked " + tButton.name);


        //toggleCount = (toggleCount + 1) % 2;
        selectingPlant = !selectingPlant;

        //if (toggleCount == 0)
        if (selectingPlant)
        {
            tButton.GetComponentInChildren<Text>().text = "Plants";
            for (int i = 0; i < 6; i++)
            {
                menuButtons[i].transform.Find(prey[i].GetName()).gameObject.SetActive(false);
                menuButtons[i].transform.Find(plants[i].GetName()).gameObject.SetActive(true);
            }
        }

        else
        {
            tButton.GetComponentInChildren<Text>().text = "Prey";
            for (int i = 0; i < 6; i++)
            {
                menuButtons[i].transform.Find(plants[i].GetName()).gameObject.SetActive(false);
                menuButtons[i].transform.Find(prey[i].GetName()).gameObject.SetActive(true);
            }
        }

        UpdateMenuLocks();
    }


    /**
     * Use this method To set animals that go over the resource limit to not be buildable
     */

    public void UpdateMenuLocks()
    {
        UnlockAllMenuItems();
        UpdateBuildableAnimals();
    }

    public void UpdateBuildableAnimals()
    {
        for (int i = 0; i < 6; i++)
        {
            //if (toggleCount == 0)
            if (selectingPlant)
            {
                if (SpeciesConstants.Biomass(plants[i].GetName()) > plantBiomass)
                {
                    LockMenuButton(i);
                }

            }
            else
            {
                if (SpeciesConstants.Biomass(prey[i].GetName()) > tier2Biomass)
                {
                    LockMenuButton(i);
                }
            }

        }
    }

    // Specie selection based on the button clicked and setting down species on the gameboard
    void selectSpecies(GameObject button)
    {
        Debug.Log("Clicked " + button.name);
        turnSystem.IsTurnLocked();

        //Sets button menu b/t plants and prey
        DemAnimalFactory[] species;
        short speciesType = 0;

        //if (toggleCount % 2 == 0)
        if (selectingPlant)
        {
            species = plants;
            speciesType = 0;
        }


        else
        {
            species = prey;
            speciesType = 1;
        }



        DemAudioManager.audioClick.Play();

        // If a selection is currently in progress...
        if (main.currentSelection)
        {
            // Ignore button click if for the same species
            if (currentAnimalFactory == species[int.Parse(button.name)])
                return;
            // Otherwise, destroy the current selection before continuing
            else
                Destroy(main.currentSelection);

            main.boardController.ClearAvailableTiles();

        }
        // Set / reset currentlyBuilding

        //currentlyBuilding = info;
        currentAnimalFactory = species[int.Parse(button.name)];


        // Create the current prey object
        //GameObject currentPlant = plant.Create(); //DemAnimalFactory.Create(currentlyBuilding.name , 0 ,0) as GameObject;

        // Define the current prey's initial location relative to the world (i.e. NOT on a screen pixel basis)
        Vector3 init_pos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y));
        init_pos.z = -1.5f;

        // Instantiate the current prey
        main.currentSelection = species[int.Parse(button.name)].Create();
        //main.currentSelection.GetComponent<BuildInfo>().isPlant = true;
        main.currentSelection.GetComponent<BuildInfo>().speciesType = speciesType;


        // Set DemMain's preyOrigin as the center of the button
        main.setBuildOrigin(Camera.main.ScreenToWorldPoint(Input.mousePosition));

        main.boardController.SetAvailableTiles();
    }

    //click on quit button
    void selectQuit()
    {
        DemAudioManager.audioClick.Play();
        Debug.Log(Screen.width);

        if (main.currentSelection)
        {
            Destroy(main.currentSelection);
            main.boardController.ClearAvailableTiles();
        }

        if (quitUI == null)
        {
            quitUI = demRectUI.createRectUI("quitUI", 0, 0, Screen.width / 1.5f, Screen.height / 1.5f);
            quitUI.GetComponent<Image>().sprite = popupBackground;
            demRectUI.setUIText(quitUI, (GameState.player != null ? GameState.player.name : "[anonymous player]") 
                + "\nAre you sure you want to quit? \nYou currently have "
                + (GameState.player != null ? GameState.player.credits.ToString() : "???"));

            //Quit Button on Quit UI
            GameObject yesButton = demButton.CreateButton(0, 0, "yes_button");
            yesButton.transform.SetParent(quitUI.transform);
            yesButton.GetComponent<RectTransform>().anchoredPosition =
                new Vector2(quitUI.GetComponent<RectTransform>().sizeDelta.x / 5.0f,
                            -quitUI.GetComponent<RectTransform>().sizeDelta.y / 5.0f * 3.0f);
            demButton.SetButtonText(yesButton, "Quit");
            yesButton.GetComponent<Button>().onClick.AddListener(() => { DemAudioManager.audioClick.Play(); Game.SwitchScene("World"); });

            //back button on Quit UI
            GameObject noButton = demButton.CreateButton(0, 0, "no_button");
            noButton.transform.SetParent(quitUI.transform);
            noButton.GetComponent<RectTransform>().anchoredPosition =
                new Vector2(quitUI.GetComponent<RectTransform>().sizeDelta.x / 5.0f * 3.0f,
                    -quitUI.GetComponent<RectTransform>().sizeDelta.y / 5.0f * 3.0f);
            demButton.SetButtonText(noButton, "Back");
            //noButton.GetComponent<Button> ().onClick.AddListener (()=>{quitUI.SetActive(false);});
            noButton.GetComponent<Button>().onClick.AddListener(() => { DemAudioManager.audioClick.Play(); quitUI.SetActive(false); mainUIObject.SetActive(true); });

            mainUIObject.SetActive(false);

            return;
        }

        if (!quitUI.activeInHierarchy)
        {
            quitUI.SetActive(true);
            mainUIObject.SetActive(false);
        }
    }

    // when instruction button is clicked
    void selectInstruction()
    {
        DemAudioManager.audioClick.Play();

        if (main.currentSelection)
        {
            Destroy(main.currentSelection);
            main.boardController.ClearAvailableTiles();
        }

        mainUIObject.SetActive(false);
        // set instruction page to the first page
        int currentPage = 0;

        /**
        if (instructionUI == null)
        {
            instructionUI = demRectUI.createRectUI("instructionUI", 0, 0, Screen.width / 1.5f, Screen.height / 1.5f);
            instructionUI.GetComponent<Image>().sprite = popupBackground;
            demRectUI.setUIText(instructionUI, "These are the instructions");

            // OK button on Instruction UI
            GameObject okButton = demButton.CreateButton(0, 0, "Next");
            okButton.transform.SetParent(instructionUI.transform);
            okButton.GetComponent<RectTransform>().anchoredPosition =
                new Vector2(instructionUI.GetComponent<RectTransform>().sizeDelta.x / 5.0f * 3.4f,
                    - instructionUI.GetComponent<RectTransform>().sizeDelta.y / 5.0f * 3.3f);
            demButton.SetButtonText(okButton, "Next");
            okButton.GetComponent<Button>().onClick.AddListener(() => { CloseInstructionWindow(); });


            GameObject closeButton = demButton.CreateButton(0, 0, "Close");
            closeButton.GetComponent<Image>().sprite = Resources.Load<Sprite>("DontEatMe/Sprites/closeButton");
            closeButton.transform.SetParent(instructionUI.transform);
            closeButton.GetComponent<Button>().onClick.AddListener(() => { CloseInstructionWindow(); });
            closeButton.GetComponent<RectTransform> ().anchorMax = new Vector2 (1 , 1);
            closeButton.GetComponent<RectTransform> ().anchorMin = new Vector2 (1 , 1);
            //closeButton.GetComponent<RectTransform> ().pivot = new Vector2 (0.5, 0.5);

            mainUIObject.SetActive(false);

            return;
        }
        
        if (!instructionUI.activeInHierarchy)
        {
            instructionUI.SetActive(true);
            mainUIObject.SetActive(false);
        }
        */
        // Set size, instruction content, and move instruction panel to view
        continuePageButton = instructionPanelObj.transform.GetChild(3).GetComponent<Button>();
        instructionPanelObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        instructionPanelObj.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, Screen.height);
        instructionPanelObj.transform.GetChild(0).gameObject.GetComponent<Text>().fontSize = (int)(Screen.width / 30);
        instructionPanelObj.transform.GetChild(1).gameObject.GetComponent<Text>().fontSize = (int)(Screen.width / 35);
        continuePageButton.transform.GetChild(0).GetComponent<Text>().fontSize = (int)(Screen.width / 35);

        instructionPanelObj.transform.GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta =
            new Vector2(Screen.width * 0.85f, instructionPanelObj.transform.GetChild(1).gameObject.GetComponent<RectTransform>().sizeDelta.y);
        instructionPanelObj.transform.GetChild(1).gameObject.GetComponent<RectTransform>().sizeDelta = 
            new Vector2(Screen.width * 0.85f, instructionPanelObj.transform.GetChild(1).gameObject.GetComponent<RectTransform>().sizeDelta.y);
        instructionPanelObj.transform.GetChild(2).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width * 0.5f, Screen.height * 0.45f);
        continuePageButton.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width * 0.13f, Screen.height * 0.075f);

        instructionPanelObj.transform.GetChild(2).gameObject.GetComponent<RectTransform>().anchoredPosition = 
            new Vector2(instructionPanelObj.transform.GetChild(2).gameObject.GetComponent<RectTransform>().anchoredPosition.x, Screen.height * 0.40f);
        


        instructionPanelObj.transform.GetChild(0).gameObject.GetComponent<Text>().text = headerText[currentPage];
        instructionPanelObj.transform.GetChild(1).gameObject.GetComponent<Text>().text = text[currentPage];
        instructionPanelObj.transform.GetChild(2).gameObject.SetActive(false);
        continuePageButton.transform.GetChild(0).GetComponent<Text>().text = "Next!";
        

        continuePageButton.onClick.AddListener(() =>
        {
            // Increment page when button is clicked
            currentPage++;
            nextPage(currentPage);
            // reset page to first page
            if (currentPage == 9) currentPage = 0;
        });

        if (!instructionPanelObj.activeInHierarchy)
        {
            instructionPanelObj.SetActive(true);
            mainUIObject.SetActive(false);
        }

    }

    // moves on to the next instruction pages
    void nextPage (int cPage)
    {
        // if not the last page
        if (cPage < 9)
        {
            instructionPanelObj.transform.GetChild(2).gameObject.SetActive(true);
            // change button text for last instruction page
            if (cPage == 8)
            {
                instructionPanelObj.transform.GetChild(2).gameObject.SetActive(false);
                continuePageButton.transform.GetChild(0).GetComponent<Text>().text = "Got It!";
            }
            instructionPanelObj.transform.GetChild(0).gameObject.GetComponent<Text>().text = headerText[cPage];
            instructionPanelObj.transform.GetChild(1).gameObject.GetComponent<Text>().text = text[cPage];
            instructionPanelObj.transform.GetChild(2).gameObject.GetComponent<Image>().sprite = instructionPages[cPage - 1];
        }
        else
        {
            instructionPanelObj.SetActive(false);
            mainUIObject.SetActive(true);
        }

    }


    public void CloseInstructionWindow()
    {
        DemAudioManager.audioClick.Play();
        instructionUI.SetActive(false);
        mainUIObject.SetActive(true);
    }

    public void SetNextInstructionSlide()
    {
    }

    // Invoked on click from statistic button.
    public void selectStatistic()
    {
        DemAudioManager.audioClick.Play();

        if (main.currentSelection)
        {
            Destroy(main.currentSelection);
            main.boardController.ClearAvailableTiles();
        }

        if (statUI == null)
        {
            statUI = demRectUI.createRectUI("statUI", 0, 0, Screen.width / 1.2f, Screen.height / 1.2f);
            statUI.GetComponent<Image>().sprite = popupBackground;
            t1 = demRectUI.setUIText(statUI, "Plants placed: " + statistic.getTreeDown(), 0, 0);
            t2 = demRectUI.setUIText(statUI, "Prey placed: " + statistic.getPreyDown(), 0, 1);
            t3 = demRectUI.setUIText(statUI, "Plants destroyed: " + statistic.getTreeDestroy(), 1, 0);
            t4 = demRectUI.setUIText(statUI, "Prey consumed: " + statistic.getPreyEaten(), 1, 1);
            t5 = demRectUI.setUIText(statUI, "Turns taken: " + statistic.getTurnCount(), 2, 0);
            t6 = demRectUI.setUIText(statUI, "Player total credits: " + (GameState.player != null ? GameState.player.credits.ToString() : "???"), 2, 1);


            GameObject backButton = demButton.CreateButton(0, 0, "back_button");
            backButton.transform.SetParent(statUI.transform);
            backButton.GetComponent<RectTransform>().anchoredPosition =
                new Vector2(statUI.GetComponent<RectTransform>().sizeDelta.x / 2.0f - backButton.GetComponent<RectTransform>().sizeDelta.x / 2.0f,
                    -statUI.GetComponent<RectTransform>().sizeDelta.y / 5.0f * 3.0f - backButton.GetComponent<RectTransform>().sizeDelta.y);
            demButton.SetButtonText(backButton, "Back");
            backButton.GetComponent<Button>().onClick.AddListener(() =>
            {
                DemAudioManager.audioClick.Play();
                statUI.SetActive(false);
                mainUIObject.SetActive(true);
            });

            mainUIObject.SetActive(false);

            return;
        }

        t1.GetComponent<Text>().text = "Plants placed: " + statistic.getTreeDown();
        t2.GetComponent<Text>().text = "Prey placed: " + statistic.getPreyDown();
        t3.GetComponent<Text>().text = "Plants destroyed: " + statistic.getTreeDestroy();
        t4.GetComponent<Text>().text = "Prey consumed: " + statistic.getPreyEaten();
        t5.GetComponent<Text>().text = "Turns taken: " + statistic.getTurnCount();
        t6.GetComponent<Text> ().text = "Player total credits: " + (GameState.player != null ? GameState.player.credits.ToString() : "???");

        if (!statUI.activeInHierarchy)
        {
            statUI.SetActive(true);
            mainUIObject.SetActive(false);
        }

    }


    public void EndGame()
    {
        gameOverUI = demRectUI.createRectUI("quitUI", 0, 0, Screen.width / 2.0f, Screen.height / 2.0f);
        gameOverUI.GetComponent<Image>().sprite = popupBackground;
        demRectUI.setUIText(gameOverUI, "Game Over! Play Again?");

        //Quit Button on Quit UI
        GameObject yesButton = demButton.CreateButton(0, 0, "yes_button");
        yesButton.transform.SetParent(gameOverUI.transform);
        yesButton.GetComponent<RectTransform>().anchoredPosition =
          new Vector2(gameOverUI.GetComponent<RectTransform>().sizeDelta.x / 5.0f,
        -gameOverUI.GetComponent<RectTransform>().sizeDelta.y / 5.0f * 3.0f);
        demButton.SetButtonText(yesButton, "Yes");
        yesButton.GetComponent<Button>().onClick.AddListener(() => { DemAudioManager.audioClick.Play(); Game.SwitchScene("DontEatMe"); });

        //back button on Quit UI
        GameObject noButton = demButton.CreateButton(0, 0, "no_button");
        noButton.transform.SetParent(gameOverUI.transform);
        noButton.GetComponent<RectTransform>().anchoredPosition =
        new Vector2(gameOverUI.GetComponent<RectTransform>().sizeDelta.x / 5.0f * 3.0f,
        -gameOverUI.GetComponent<RectTransform>().sizeDelta.y / 5.0f * 3.0f);
        demButton.SetButtonText(noButton, "No");
        //noButton.GetComponent<Button> ().onClick.AddListener (()=>{quitUI.SetActive(false);});
        noButton.GetComponent<Button>().onClick.AddListener(() => { DemAudioManager.audioClick.Play(); Game.SwitchScene("World"); });

        mainUIObject.SetActive(false);
    }


    // Updates player's credits
    public void ProcessEndGame(NetworkResponse response)
    {
        ResponsePlayGame args = response as ResponsePlayGame;

        if (args.status == 1 && GameState.player != null)
        {
            GameState.player.credits = args.creditDiff;
            Debug.Log(args.creditDiff);
        }
    }


    // Update is called once per frame
    /*void Update()
    {

    }*/

    /**
        Fetches current animal factory.
    */
    public DemAnimalFactory GetCurrentAnimalFactory ()
    {
        return currentAnimalFactory;
    }

    /**
        Sets the current animal factory.

        @param  newAnimalFactory    a DemAnimalFactory object
    */
    public void SetCurrentAnimalFactory(DemAnimalFactory newAnimalFactory)
    {
        currentAnimalFactory = newAnimalFactory;
    }

    /**
        Toggles button locks based on player or Predator's turn.
    */
    public void ToggleButtonLocks ()
    {
        turnSystemText.GetComponent<Text>().text = "Your Turn!";
        if (turnSystem.IsTurnLocked())
        {
            for (int i = 0; i < 6; i++)
            {
                menuButtons[i].GetComponent<Button>().interactable = true;
                foreach (Image image in menuButtons[i].GetComponentsInChildren<Image>())
                {
                    image.color = new Color(1.0F, 1.0F, 1.0F, 1.0F);
                }
            }
        }
        else
        {
            turnSystemText.GetComponent<Text>().text = "Predator Turn!";
            for (int i = 0; i < 6; i++)
            {
                menuButtons[i].GetComponent<Button>().interactable = false;
                foreach (Image image in menuButtons[i].GetComponentsInChildren<Image>())
                {
                    image.color = new Color(1.0F, 1.0F, 1.0F, 0.8F);
                }
            }
        }

    }

    public void UnlockAllMenuItems ()
    {
        for (int i = 0; i < 6; i++)
        {
            menuButtons[i].GetComponent<Button>().interactable = true;
            foreach (Image image in menuButtons[i].GetComponentsInChildren<Image>())
            {
                image.color = new Color(1.0F, 1.0F, 1.0F, 1.0F);
            }
        }
    }

    public void LockMenuButton (int buttonNum)
    {
        menuButtons[buttonNum].GetComponent<Button>().interactable = false;
        foreach (Image image in menuButtons[buttonNum].GetComponentsInChildren<Image>())
        {
            image.color = new Color(1.0F, 1.0F, 1.0F, 0.8F);
        }
    }

    public void UpdateLives (int lives)
    {
        livesText.GetComponent<Text>().text = lives.ToString();
    }

    public void UpdateCredits (int credits)
    {
        scoreText.GetComponent<Text>().text = credits.ToString();
    }

    public void UpdatePlantBiomass ()
    {
        plantBioText.GetComponent<Text>().text = plantBiomass.ToString();
    }

    public void UpdatePlantBiomass (int biomass)
    {
        plantBiomass = biomass;
        UpdatePlantBiomass();
    }

    public int getPlantBiomass ()
    {
        return plantBiomass;
    }

    public int GetTier2Biomass ()
    {
        return tier2Biomass;
    }

    public int GetTier3Biomass ()
    {
        return tier3Biomass;
    }

    public void UpdateTier2Biomass ()
    {
        tier2BioText.GetComponent<Text>().text = tier2Biomass.ToString();
    }

    public void UpdateTier2Biomass (int biomass)
    {
        tier2Biomass = biomass;
        UpdateTier2Biomass();
    }

    public void AddTier2Biomass (int biomass)
    {
        tier2Biomass += biomass;
        UpdateTier2Biomass();
    }

    public void SubtractTier2Biomass (int biomass)
    {
        tier2Biomass -= biomass;
        UpdateTier2Biomass();
    }

    public void UpdateTier3Biomass ()
    {
        tier3BioText.GetComponent<Text>().text = tier3Biomass.ToString();
    }

    public void UpdateTier3Biomass (int biomass)
    {
        tier3Biomass = biomass;
        UpdateTier3Biomass();
    }

    public void AddTier3Biomass (int biomass)
    {
        tier3Biomass += biomass;
        UpdateTier3Biomass();
    }

    public void AddPlantBiomass (int biomass)
    {
        plantBiomass += biomass;
        UpdatePlantBiomass();
    }


    public void SubtractTier3Biomass (int biomass)
    {
        tier3Biomass -= biomass;
        UpdateTier3Biomass();
    }

    public void SubtractPlantBiomass (int biomass)
    {
        plantBiomass -= biomass;
        UpdatePlantBiomass();
    }
}