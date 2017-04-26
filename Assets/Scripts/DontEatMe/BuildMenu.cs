using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;
using System;
using System.Runtime.Remoting;

public class BuildMenu : MonoBehaviour
{

    // Background material
    public Material backgroundMaterial;

    // Access to DemButtonFactory script for button creation
    public DemButtonFactory demButtonFactory;

    //Access to DemRectUI script for RectUI creation
    public DemRectUI demRectUI;
    private GameObject quitUI;	//instance of UI for after pressed quit button
    private GameObject instructionUI; //instance of UI for after pressed how to play button
    private GameObject gameOverUI;  //instance of UI for after pressed quit button

    //statiUI
    private GameObject statUI;
    public Statistic statistic;
    private GameObject t1;  //static text 1 to 6
    private GameObject t2;
    private GameObject t3;
    private GameObject t4;
    private GameObject t5;
    private GameObject t6;

    // Toggle counter
    int toggleCount;

    // Currently building...
    public BuildInfo currentlyBuilding;

    public DemAnimalFactory currentAnimalFactory;

    // Currently about to delete?
    public bool currentlyDeleting = false;

    // Player's current resource amount
    public int currentResources = 250;

    // Player's current score amount
    public int score = 0;

    // Plant prefabs
    public DemAnimalFactory[] plants;

    // Prey prefabs
    public DemAnimalFactory[] prey;

    // Menu buttons
    public GameObject[] plantBuildButtons;
    public GameObject[] preyBuildButtons;
    public GameObject plantMenuButton;
    public GameObject preyMenuButton;

    // Skip button (CONCEPTUAL)
    public GameObject skipTurnButton;

    // Remove / kill button
    public GameObject removeBuildButton;

    // COLOR CONSTANTS //
    public Color activeColor; // Active button color
    public Color inactiveColor; // Inactive / deactivated button color
    public Color inactiveIconColor; // Inactive / deactivated button icon (e.g. species image) color

    public Color lockedColor; // Locked button color (foreground)
    public Color lockedIconColor; // Locked button icon (e.g. species image) color (foreground)

    public Color selectedColor; // Selected / toggled on button color
    public Color deselectedColor; // Deselected / toggled off button color

    public Color plantIconColor; // Plant icon color
    public Color preyIconColor; // Prey icon color
    public Color predatorIconColor; // Predator icon color

    public Color tier1BiomassIconColor;
    public Color tier2BiomassIconColor;
    public Color tier3BiomassIconColor;
    // *************** //

    private bool plantMenuActive = true; // Plant menu active status

    private GameObject mainObject;

    private DemTurnSystem turnSystem;

    private DemMain main;

    // Biomass levels:
    private int tier1Biomass; // Plant
    private int tier2Biomass; // Prey
    private int tier3Biomass; // Predator

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

    // Instructions
    public GameObject instructionPanelObj;
    private Sprite page2, page3, page4, page5, page6, page7, page8, page9;
    private Button continuePageButton;
    private Sprite[] instructionPages = new Sprite[9];
    //private int currentPage;


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

    // this method increases score every 2s
    void increaseResources()
    {
        currentResources += 50;
    }


    //Loading Resources
    void Awake()
    {   
        // Define colors
        activeColor = Color.white; // White @ 100% alpha
        inactiveColor = new Color(0, 0, 0, 0.75f); // Black @ 75% alpha
        inactiveIconColor = Color.black; // Black @ 100% alpha
        lockedColor = new Color32(105, 50, 50, 255); // Dark earth red @ 100% alpha
        lockedIconColor = new Color32(178, 34, 34, 100); // HTML "FIREBRICK" @ ~39% alpha
        selectedColor = Color.yellow;
        deselectedColor = new Color32(75, 75, 75, 255); // Dark gray @ 100% alpha
        plantIconColor = new Color32(46, 139, 87, 255); // HTML "SEAGREEN" @ 100% alpha
        preyIconColor = new Color32(210, 105, 30, 255); // HTML "BROWN" @ 100% alpha
        predatorIconColor = new Color32(178, 34, 34, 255); // HTML "FIREBRICK" @ 100% alpha
        tier1BiomassIconColor = new Color32(0x78, 0xC8, 0x8A, 0xCB); // Transluscent blue-green (hex)
        tier2BiomassIconColor = new Color32(0xC8, 0xB6, 0x78, 0xCB); // Transluscent khaki brown (hex)
        tier3BiomassIconColor = new Color32(0xC8, 0x78, 0x78, 0xCB); // Transluscent salmon red (hex)

        infoWidget = Resources.Load<Sprite>("DontEatMe/Sprites/infoWidget");
        popupBackground = Resources.Load<Sprite>("DontEatMe/Sprites/popup");
        fontFamily = Resources.Load<Font>("Fonts/Dresden Elektronik");

        removeBuildButton = GameObject.Find("Canvas/StatsBox/RemoveBuildButton");

        backgroundMaterial = Resources.Load<Material>("DontEatMe/Materials/DontEatMeBg");

        mainObject = GameObject.Find("MainObject");

        main = mainObject.GetComponent<DemMain>();

        turnSystem = mainObject.GetComponent<DemTurnSystem>();

        //mainUI add here
        canvasObject = GameObject.Find("Canvas");
        mainUIObject = GameObject.Find("Canvas/mainUI");
        mainUIObject.transform.SetParent(canvasObject.transform);
        mainUIObject.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        mainUIObject.GetComponent<RectTransform>().anchorMin = Vector2.zero;
        mainUIObject.GetComponent<RectTransform>().anchorMax = Vector2.one;
        mainUIObject.GetComponent<RectTransform>().offsetMax = Vector2.zero;
        mainUIObject.GetComponent<RectTransform>().offsetMin = Vector2.zero;

        panelObject = GameObject.Find("Canvas/Panel");
        //panelObject = GameObject.Find("Canvas/mainUI/Panel");
        menuPanel = GameObject.Find("Canvas/mainUI/MenuPanel");
        scoreText = GameObject.Find("Canvas/mainUI/CreditsWidget/CreditWidgetText");
        livesText = GameObject.Find("Canvas/mainUI/LivesWidget/LivesWidgetText");
        turnSystemText = GameObject.Find("Canvas/mainUI/TurnWidget/TurnWidgetText");
        plantBioText = GameObject.Find("Canvas/mainUI/Tier1Widget/PlantBioWidgetText");
        tier2BioText = GameObject.Find("Canvas/mainUI/Tier2Widget/WidgetText");
        tier3BioText = GameObject.Find("Canvas/mainUI/Tier3Widget/WidgetText");



        //panelObject.GetComponent<Image>().sprite = infoWidget;

        // Instruction Panel
        instructionPanelObj = GameObject.Find("Canvas/InstructionPanel");
        page2 = Resources.Load<Sprite>("DontEatMe/InstructionPictures/instructions_p2");
        page3 = Resources.Load<Sprite>("DontEatMe/InstructionPictures/instructions_p3");
        page4 = Resources.Load<Sprite>("DontEatMe/InstructionPictures/instructions_p4");
        page5 = Resources.Load<Sprite>("DontEatMe/InstructionPictures/instructions_p5");
        page6 = Resources.Load<Sprite>("DontEatMe/InstructionPictures/instructions_p6");
        page7 = Resources.Load<Sprite>("DontEatMe/InstructionPictures/instructions_p7");
        page8 = Resources.Load<Sprite>("DontEatMe/InstructionPictures/instructions_p8");

        instructionPages[0] = page2;
        instructionPages[1] = page3;
        instructionPages[2] = page4;
        instructionPages[3] = page5;
        instructionPages[4] = page6;
        instructionPages[5] = page7;
        instructionPages[6] = page8;
        instructionPages[7] = page9;

        quitUI = null;
        statUI = null;
        statistic = new Statistic();
    }



    /**
        Used to initialize game objects and data..
        This method is invoked automatically and is called after the Awake method.
    */
    void Start ()
    {
        // Initialize biomass levels
        tier1Biomass = 5000;
        tier2Biomass = 0;
        tier3Biomass = 0;

        turnSystem.IsTurnLocked();
        turnSystemText.GetComponent<Text>().text = "Your Turn!";

        currentAnimalFactory = null;
        currentlyBuilding = null;

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
        // NOTE: this should NOT be hard-coded!
        // Use the SpeciesConstants script methods for dynamic creation (local).
        // Further development should see the integration of a remote database to replace and/or augment SpeciesConstants.
        plants = new DemAnimalFactory[6];
        plants[0] = new DemAnimalFactory("Acacia");
        plants[1] = new DemAnimalFactory("Baobab");
        plants[2] = new DemAnimalFactory("Big Tree");
        plants[3] = new DemAnimalFactory("Fruits And Nectar");
        plants[4] = new DemAnimalFactory("Grass And Herbs");
        plants[5] = new DemAnimalFactory("Trees And Shrubs");


        prey = new DemAnimalFactory[6];
        prey[0] = new DemAnimalFactory("Tree Mouse");
        prey[1] = new DemAnimalFactory("Bush Hyrax");
        prey[2] = new DemAnimalFactory("Kori Bustard");
        prey[3] = new DemAnimalFactory("Crested Porcupine");
        prey[4] = new DemAnimalFactory("Oribi");
        prey[5] = new DemAnimalFactory("Buffalo");


        // NEW BUTTON CREATION STARTS HERE
        // To use old buttons comment out the following lines (until line 287) and uncomment OnGUI()


        // Initialize the button factory
        gameObject.AddComponent<DemButtonFactory>();
        demButtonFactory = gameObject.GetComponent<DemButtonFactory>();

        // Initialize the DemRectUI object
        gameObject.AddComponent<DemRectUI>();
        demRectUI = gameObject.GetComponent<DemRectUI>();

        // Initialize plant and prey button menu
        InitializeBuildMenu();

        // Create skip turn button
        CreateSkipButton();

        //quit button 
        float qBX = Screen.width / 10.0f;
        float qBY = Screen.height / 10.0f;
        demButtonFactory.setSize(qBX, qBY);
        GameObject quitButton = demButtonFactory.CreateButton(Screen.width - qBX, 0, "Quit");
        demButtonFactory.SetButtonText(quitButton, "Quit");
        quitButton.GetComponent<Button>().onClick.AddListener(() => { selectQuit(); });
        // quitButton.transform.SetParent(menuPanel.transform); 

        quitButton.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
        quitButton.GetComponent<RectTransform>().anchorMin = new Vector2(1, 1);
        quitButton.GetComponent<RectTransform>().anchorMax = new Vector2(1, 1);

        quitButton.GetComponent<RectTransform>().offsetMax = new Vector2(-70, -22);
        quitButton.GetComponent<RectTransform>().offsetMin = new Vector2(-70, -22);

        quitButton.GetComponent<RectTransform>().sizeDelta = new Vector2(70, 30);


        //statistic button 
        float sBX = Screen.width / 10.0f;
        float sBY = Screen.height / 15.0f;
        demButtonFactory.setSize(sBX, sBY);
        GameObject statButton = demButtonFactory.CreateButton(Screen.width - qBX * 2, 0, "statistic");
        demButtonFactory.SetButtonText(statButton, "Statistic");
        statButton.GetComponent<Button>().onClick.AddListener(() => { selectStatistic(); });
        // quitButton.transform.SetParent(menuPanel.transform); 

        statButton.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
        statButton.GetComponent<RectTransform>().anchorMin = new Vector2(1, 1);
        statButton.GetComponent<RectTransform>().anchorMax = new Vector2(1, 1);

        statButton.GetComponent<RectTransform>().offsetMax = new Vector2(-70, -22 * 3);
        statButton.GetComponent<RectTransform>().offsetMin = new Vector2(-70, -22 * 3);

        statButton.GetComponent<RectTransform>().sizeDelta = new Vector2(70, 30);


        // Instructions button
        // TODO: consistent naming convention ("instruction" vs "how to play")
        float iBX = Screen.width / 20.0f;
        float iBY = Screen.height / 10.0f;
        demButtonFactory.setSize(iBX, iBY);
        GameObject instructionButton = demButtonFactory.CreateButton(Screen.width - iBX, 10, "How to Play");
        demButtonFactory.SetButtonText(instructionButton, "?");

        instructionButton.GetComponent<Image>().sprite = Resources.Load<Sprite>("DontEatMe/Sprites/buttonBackgroundRound");
        instructionButton.GetComponent<Button>().onClick.AddListener(() => { selectInstruction(); });

        instructionButton.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
        instructionButton.GetComponent<RectTransform>().anchorMin = new Vector2(1, 1);
        instructionButton.GetComponent<RectTransform>().anchorMax = new Vector2(1, 1);

        instructionButton.GetComponent<RectTransform>().offsetMax = new Vector2(-154, -22);
        instructionButton.GetComponent<RectTransform>().offsetMin = new Vector2(-145, -22);

        instructionButton.GetComponent<RectTransform>().sizeDelta = new Vector2(30, 30);

        // Reset sizes for quit popup buttons
        demButtonFactory.setSize(qBX, qBY);
        //UpdatePlantBiomass();
        UpdateTier1Biomass();
        UpdateMenuLocks();
    }
    // END Start() //

    /**
        Initializes the build menu buttons for plants and prey.

        Plant and prey buttons may be toggled between using the Toggle button; the active button set will remain on top
        while the inactive will move behind the active.
    */
    void InitializeBuildMenu ()
    {
        // PLANT & PREY TOGGLE BUTTONS //
        //
        // Set menu selector button parameters
        demButtonFactory.setSize(Screen.width * 0.075f, Screen.height / 14);
        //
        // Create plant menu selector button
        plantMenuButton = demButtonFactory.CreateButton(0, 0, "plantmenu");
        demButtonFactory.SetButtonIcon(plantMenuButton, "DontEatMe/Sprites/plant_icon", plantIconColor, Vector2.left);
        demButtonFactory.SetButtonText(plantMenuButton, "Plant");
        demButtonFactory.SetButtonHighlightedColor(plantMenuButton, plantIconColor);
        plantMenuButton.GetComponent<Button>().onClick.AddListener(() => { SetBuildButtonCategory(0); });
        //
        // Create prey menu selector button
        preyMenuButton = demButtonFactory.CreateButton(demButtonFactory.getXSize() + 7, 0, "preymenu");
        demButtonFactory.SetButtonIcon(preyMenuButton, "DontEatMe/Sprites/prey_icon", preyIconColor, Vector2.left);
        demButtonFactory.SetButtonText(preyMenuButton, "Prey");
        demButtonFactory.SetButtonHighlightedColor(preyMenuButton, preyIconColor);
        preyMenuButton.GetComponent<Button>().onClick.AddListener(() => { SetBuildButtonCategory(1); });
        //
        // Set initial color schemes for plant (active) and prey (inactive) buttons
        plantMenuButton.GetComponentInChildren<Image>().color = selectedColor;
        plantMenuButton.GetComponentsInChildren<Image>()[1].color = plantIconColor;
        demButtonFactory.SetButtonTextColor(plantMenuButton, Color.white);
        //...
        preyMenuButton.GetComponentInChildren<Image>().color = deselectedColor;
        preyMenuButton.GetComponentsInChildren<Image>()[1].color = deselectedColor;
        demButtonFactory.SetButtonTextColor(preyMenuButton, Color.gray);

        // PLANT & PREY BUILD BUTTONS //
        //
        // Creates a buttons for plant/prey menu
        demButtonFactory.setSize(Screen.width * 0.1f, Screen.height / 7);
        plantBuildButtons = new GameObject[6];
        preyBuildButtons = new GameObject[6];

        // Set colors for plant and prey buttons
        ColorBlock plantButtonColors = new ColorBlock();
        plantButtonColors.normalColor = new Color32(122, 255, 171, 255);
        plantButtonColors.highlightedColor = new Color32(183, 255, 173, 255);
        plantButtonColors.pressedColor = new Color32(0, 107, 255, 255);
        ColorBlock preyButtonColors = new ColorBlock();
        preyButtonColors.normalColor = new Color32(255, 179, 150, 255);
        preyButtonColors.highlightedColor = Color.white;
        preyButtonColors.pressedColor = new Color32(150, 66, 135, 255);

        Navigation buttonNavigation = Navigation.defaultNavigation;
        buttonNavigation.mode = Navigation.Mode.None;

        for (int i = 0; i < plantBuildButtons.Length; i++)
        {
            // Create plant button
            GameObject plantButton = demButtonFactory.CreateButton
            (
                0, // x-position (relative to upper left)
                0 - ((Screen.height / 14) + 10 + i * (demButtonFactory.getYSize() - 2)), // y-position (relative to upper left)
                i.ToString() // name
            );
            // Create prey button
            GameObject preyButton = demButtonFactory.CreateButton
            (
                demButtonFactory.getXSize()/3, // x-position (relative to upper left)
                0 - demButtonFactory.getYSize()/3 - ((Screen.height / 14) + 10 + i * (demButtonFactory.getYSize() - 2)), // y-position (relative to upper left)
                i.ToString() // name
            );

            // Set the button images
            demButtonFactory.SetButtonImage(plants[i], plantButton);
            demButtonFactory.SetButtonImage(prey[i], preyButton);

            // Set the button colors
            demButtonFactory.SetButtonNormalColor(plantButton, plantButtonColors.normalColor);
            demButtonFactory.SetButtonHighlightedColor(plantButton, plantButtonColors.highlightedColor);
            demButtonFactory.SetButtonPressedColor(plantButton, plantButtonColors.pressedColor);
            demButtonFactory.SetButtonNormalColor(preyButton, preyButtonColors.normalColor);
            demButtonFactory.SetButtonHighlightedColor(preyButton, preyButtonColors.highlightedColor);
            demButtonFactory.SetButtonPressedColor(preyButton, preyButtonColors.pressedColor);

            // Set navigation to none
            plantButton.GetComponent<Button>().navigation = buttonNavigation;
            preyButton.GetComponent<Button>().navigation = buttonNavigation;

            // Set the images of the plant menu active
            plantButton.transform.Find(plants[i].GetName()).gameObject.SetActive(true);
            preyButton.transform.Find(prey[i].GetName()).gameObject.SetActive(true);

            // Add an onClick listener to detect button clicks
            plantButton.GetComponent<Button>().onClick.AddListener(() => { selectSpecies(plantButton); });
            preyButton.GetComponent<Button>().onClick.AddListener(() => { selectSpecies(preyButton); });
            plantButton.AddComponent<DemButtonFactory>();
            preyButton.AddComponent<DemButtonFactory>();

            plantBuildButtons[i] = plantButton;
            preyBuildButtons[i] = preyButton;
        }
        // Place prey buttons behind plant buttons and lock them
        for (byte i = 0; i < preyBuildButtons.Length; i++)
            preyBuildButtons[i].transform.SetAsFirstSibling();

        LockPreyMenuItems(false);
    }

    /**
        Sets the active build button category (plant or prey).
        If the respective menu is active, the call is ignored.

        @param  type    0 for plant, 1 for prey
    */
    public void SetBuildButtonCategory (int type)
    {
        // Currently active: prey
        if (type == 0 && !plantMenuActive)
        {
            // Swap plant and prey button positions (plants in front)
            for (byte i = 0; i < preyBuildButtons.Length; i++)
            {
                preyBuildButtons[i].transform.SetAsFirstSibling();
                plantBuildButtons[i].transform.SetAsLastSibling();
                Vector3 origPos = plantBuildButtons[i].transform.position;
                plantBuildButtons[i].GetComponent<RectTransform>().position = preyBuildButtons[i].transform.position;
                preyBuildButtons[i].GetComponent<RectTransform>().position = origPos;
            }
            // Toggle plant active boolean to true, set button locks
            plantMenuActive = true;
            UnlockPlantMenuItems();
            LockPreyMenuItems(false);
            // Set color schemes for plant (active) and prey (inactive) buttons
            plantMenuButton.GetComponentInChildren<Image>().color = selectedColor;
            plantMenuButton.GetComponentsInChildren<Image>()[1].color = plantIconColor;
            demButtonFactory.SetButtonTextColor(plantMenuButton, Color.white);

            preyMenuButton.GetComponentInChildren<Image>().color = deselectedColor;
            preyMenuButton.GetComponentsInChildren<Image>()[1].color = deselectedColor;
            demButtonFactory.SetButtonTextColor(preyMenuButton, Color.gray);
        }
        // Currently active: plant
        else if (type == 1 && plantMenuActive)
        {
            // Swap plant and prey button positions (prey in front)
            for (byte i = 0; i < plantBuildButtons.Length; i++)
            {
                plantBuildButtons[i].transform.SetAsFirstSibling();
                preyBuildButtons[i].transform.SetAsLastSibling();
                Vector3 origPos = preyBuildButtons[i].transform.position;
                preyBuildButtons[i].GetComponent<RectTransform>().position = plantBuildButtons[i].transform.position;
                plantBuildButtons[i].GetComponent<RectTransform>().position = origPos;
            }
            // Toggle plant active boolean to false, set button locks
            plantMenuActive = false;
            UnlockPreyMenuItems();
            LockPlantMenuItems(false);
            // Set color schemes for prey (active) and plant (inactive) buttons
            preyMenuButton.GetComponentInChildren<Image>().color = selectedColor;
            preyMenuButton.GetComponentsInChildren<Image>()[1].color = preyIconColor;
            demButtonFactory.SetButtonTextColor(preyMenuButton, Color.white);

            plantMenuButton.GetComponentInChildren<Image>().color = deselectedColor;
            plantMenuButton.GetComponentsInChildren<Image>()[1].color = deselectedColor;
            demButtonFactory.SetButtonTextColor(plantMenuButton, Color.gray);
        }
        UpdateMenuLocks();
    }

    /**
        Skips the player's current turn after verification.
    */
    public void SelectSkip ()
    {
        // Disable skipTurnButton to prevent duplicate cancel/confirm button creation, return if already disabled
        if (!skipTurnButton.GetComponent<Button>().enabled) return;
        skipTurnButton.GetComponent<Button>().enabled = false;
        // Create cancel button
        GameObject cancelButton = demButtonFactory.CreateButton
        (
            (float)(skipTurnButton.transform.localPosition.x + skipTurnButton.GetComponent<RectTransform>().rect.width / 2f),
            (float)(skipTurnButton.transform.localPosition.y - skipTurnButton.GetComponent<RectTransform>().rect.height / 1.5f),
            "cancel"
        );
        // Create confirm button
        GameObject confirmButton = demButtonFactory.CreateButton
        (
            (float)(cancelButton.transform.localPosition.x - cancelButton.GetComponent<RectTransform>().rect.width * 0.75f),
            (float)(skipTurnButton.transform.localPosition.y - skipTurnButton.GetComponent<RectTransform>().rect.height / 1.5f),
            "confirm"
        );
        // Set button attributes
        demButtonFactory.setSize(confirmButton, Screen.width * 0.075f, Screen.height / 20);
        demButtonFactory.setSize(cancelButton, Screen.width * 0.075f, Screen.height / 20);
        demButtonFactory.SetButtonHighlightedColor(confirmButton, new Color32(0, 250, 154, 255));
        demButtonFactory.SetButtonNormalColor(confirmButton, new Color32(107, 142, 35, 255));
        demButtonFactory.SetButtonHighlightedColor(cancelButton, new Color32(255, 20, 147, 255));
        demButtonFactory.SetButtonNormalColor(cancelButton, predatorIconColor);
        demButtonFactory.SetButtonText(confirmButton, "Skip");
        demButtonFactory.SetButtonText(cancelButton, "Not now");
        demButtonFactory.SetButtonTextFontSize(confirmButton, Screen.width / 80);
        demButtonFactory.SetButtonTextFontSize(cancelButton, Screen.width / 80);
        // Define onClick listener for the confirm button
        confirmButton.GetComponent<Button>().onClick.AddListener
        (() => {
            StartCoroutine(GameObject.Find("MainObject").GetComponent<DemTurnSystem>().Skip());
            Destroy(confirmButton);
            Destroy(cancelButton);
        });
        // Define onClick listener for the cancel button
        cancelButton.GetComponent<Button>().onClick.AddListener
        (() => {
            Destroy(confirmButton);
            Destroy(cancelButton);
            skipTurnButton.GetComponent<Button>().enabled = true;
        });
    }

    /**
        Creates the button allowing for the player to skip their current turn.
    */
    void CreateSkipButton ()
    {
        // Set the DemButtonFactory specs, create button
        demButtonFactory.setSize(Screen.width * 0.075f, Screen.height * 0.075f);
        skipTurnButton = demButtonFactory.CreateButton
        (
            preyMenuButton.transform.localPosition.x + Screen.width / 1.5f,
            -10,
            "skipturn" 
        );
        // Set navigation mode to "None" (prevents highlighted state from persisting after clicks)
        Navigation buttonNavigation = Navigation.defaultNavigation;
        buttonNavigation.mode = Navigation.Mode.None;
        skipTurnButton.GetComponent<Button>().navigation = buttonNavigation;

        // Set button attributes, add onClick listener
        skipTurnButton.GetComponent<RectTransform>().pivot = Vector2.up;
        demButtonFactory.SetButtonIcon(skipTurnButton, "DontEatMe/Sprites/skip_turn_icon", predatorIconColor, Vector2.left);
        demButtonFactory.SetButtonText(skipTurnButton, "Skip");
        demButtonFactory.SetButtonHighlightedColor(skipTurnButton, predatorIconColor);
        demButtonFactory.SetButtonTextFontSize(skipTurnButton, Screen.height / 35);
        skipTurnButton.GetComponent<Button>().onClick.AddListener(() => { SelectSkip(); });
    }

    /**
        Updates the menu item locks for active build buttons.
        Menu items are locked if their resource cost exceeds the current respective resource level available.
    */
    public void UpdateMenuLocks ()
    {
        byte i; // Iterator
        // On active PLANT menu button pane
        if (plantMenuActive)
        {
            for (i = 0; i < plants.Length; i++)
            {
                if (SpeciesConstants.Biomass(plants[i].GetName()) > tier1Biomass) LockMenuButton(plantBuildButtons, i);
                else UnlockMenuButton(plantBuildButtons, i);
            }
        }
        // On active PREY menu button pane
        else
        {
            for (i = 0; i < prey.Length; i++)
            {
                if (SpeciesConstants.Biomass(prey[i].GetName()) > tier2Biomass) LockMenuButton(preyBuildButtons, i);
                else UnlockMenuButton(preyBuildButtons, i);
            }
        }
    }

    /*
        Selects the species (plant or prey) associated with a build button, creates the appropriate icon to prepare a
        build, and displays the valid build tiles if such tiles exist.

        @param  button  a GameObject representing a build button (created by an invocation of DemButtonFactory.Create)
    */
    public void selectSpecies(GameObject button)
    {
        // Ignore if button is disabled or turn system is locked
        if (!button.GetComponent<Button>().enabled || turnSystem.turnLock) return;

        int index;
        bool speciesBuild = int.TryParse(button.name, out index);

        if (speciesBuild)
        {
            //Set button array and species type as plants or prey
            DemAnimalFactory[] species = plantMenuActive ? plants : prey;
            short speciesType = plantMenuActive ? (short)0 : (short)1;

            // Play button "click"
            DemAudioManager.audioClick.Play();
            // If a selection is currently in progress...
            if (main.currentSelection)
            {
                // Ignore button click if for the same species
                if (currentAnimalFactory == species[index]) return;
                // Otherwise, destroy the current selection before continuing
                else Destroy(main.currentSelection);
                // Clear buildable tiles
                main.boardController.ClearAvailableTiles();
            }

            // Parse current build species
            currentAnimalFactory = species[index];

            // Define the current prey's initial location relative to the world (i.e. NOT on a screen pixel basis)
            //Vector3 initialBuildPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y));
            //initialBuildPosition.z = -1.5f;

            // Instantiate the current prey, set as last sibling (i.e. on top)
            main.currentSelection = species[index].Create();
            main.currentSelection.transform.SetAsLastSibling();
            //if (speciesType == 1) main.currentSelection.GetComponent<PreyInfo>().speciesType = speciesType;
            if (speciesType == 0) main.currentSelection.GetComponent<BuildInfo>().speciesType = speciesType;


            // Set DemMain's preyOrigin as the center of the button; origin must be in world space (as with 'initialBuildPosition')
            Vector3 buildOrigin = Camera.main.ScreenToWorldPoint(button.gameObject.transform.position);
            // Button justified upper-left, shift half width and height to center origin
            buildOrigin.x += button.gameObject.transform.lossyScale.x / 2;
            buildOrigin.y -= button.gameObject.transform.lossyScale.y / 2;
            main.setBuildOrigin(buildOrigin);

            // Calculate and display available build tiles
            main.boardController.SetAvailableTiles();
        }
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
            demRectUI.setUIText(quitUI, GameState.player.GetName() + "\nAre you sure you want to quit? \nYou currently have "
                + GameState.player.credits);

            //Quit Button on Quit UI
            GameObject yesButton = demButtonFactory.CreateButton(0, 0, "Yes");
            yesButton.transform.SetParent(quitUI.transform);
            yesButton.GetComponent<RectTransform>().anchoredPosition =
                new Vector2(quitUI.GetComponent<RectTransform>().sizeDelta.x / 5.0f,
                            -quitUI.GetComponent<RectTransform>().sizeDelta.y / 5.0f * 3.0f);
            demButtonFactory.SetButtonText(yesButton, "Quit");
            yesButton.GetComponent<Button>().onClick.AddListener(() => { DemAudioManager.audioClick.Play(); Game.SwitchScene("World"); });

            //back button on Quit UI
            GameObject noButton = demButtonFactory.CreateButton(0, 0, "No");
            noButton.transform.SetParent(quitUI.transform);
            noButton.GetComponent<RectTransform>().anchoredPosition =
                new Vector2(quitUI.GetComponent<RectTransform>().sizeDelta.x / 5.0f * 3.0f,
                    -quitUI.GetComponent<RectTransform>().sizeDelta.y / 5.0f * 3.0f);
            demButtonFactory.SetButtonText(noButton, "Back");
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
            GameObject okButton = demButtonFactory.CreateButton(0, 0, "Next");
            okButton.transform.SetParent(instructionUI.transform);
            okButton.GetComponent<RectTransform>().anchoredPosition =
                new Vector2(instructionUI.GetComponent<RectTransform>().sizeDelta.x / 5.0f * 3.4f,
                    - instructionUI.GetComponent<RectTransform>().sizeDelta.y / 5.0f * 3.3f);
            demButtonFactory.SetButtonText(okButton, "Next");
            okButton.GetComponent<Button>().onClick.AddListener(() => { CloseInstructionWindow(); });


            GameObject closeButton = demButtonFactory.CreateButton(0, 0, "Close");
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
    void nextPage(int cPage)
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



    //click on statistic button
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
            t1 = demRectUI.setUIText(statUI, "plant used: " + statistic.getTreeDown(), 0, 0);
            t2 = demRectUI.setUIText(statUI, "prey used: " + statistic.getPreyDown(), 0, 1);
            t3 = demRectUI.setUIText(statUI, "plant destroyed: " + statistic.getTreeDestroy(), 1, 0);
            t4 = demRectUI.setUIText(statUI, "prey eaten: " + statistic.getPreyEaten(), 1, 1);
            t5 = demRectUI.setUIText(statUI, "turns: " + statistic.getTurnCount(), 2, 0);
			t6 = demRectUI.setUIText(statUI, "your total credits: " + GameState.player.credits , 2, 1);

            GameObject backButton = demButtonFactory.CreateButton(0, 0, "back");
            backButton.transform.SetParent(statUI.transform);
            backButton.GetComponent<RectTransform>().anchoredPosition =
                new Vector2(statUI.GetComponent<RectTransform>().sizeDelta.x / 2.0f - backButton.GetComponent<RectTransform>().sizeDelta.x / 2.0f,
                    -statUI.GetComponent<RectTransform>().sizeDelta.y / 5.0f * 3.0f - backButton.GetComponent<RectTransform>().sizeDelta.y);
            demButtonFactory.SetButtonText(backButton, "Back");
            backButton.GetComponent<Button>().onClick.AddListener(() =>
            {
                DemAudioManager.audioClick.Play();
                statUI.SetActive(false);
                mainUIObject.SetActive(true);
            });

            mainUIObject.SetActive(false);

            return;
        }

        t1.GetComponent<Text>().text = "plant used: " + statistic.getTreeDown();
        t2.GetComponent<Text>().text = "prey used: " + statistic.getPreyDown();
        t3.GetComponent<Text>().text = "plant destroyed: " + statistic.getTreeDestroy();
        t4.GetComponent<Text>().text = "prey eaten: " + statistic.getPreyEaten();
        t5.GetComponent<Text>().text = "turns: " + statistic.getTurnCount();
		t6.GetComponent<Text> ().text = "your total credits: " + GameState.player.credits;

        if (!statUI.activeInHierarchy)
        {
            statUI.SetActive(true);
            mainUIObject.SetActive(false);
        }

    }

    // TODO: documentation............
    public void EndGame()
    {
        gameOverUI = demRectUI.createRectUI("quitUI", 0, 0, Screen.width / 2.0f, Screen.height / 2.0f);
        gameOverUI.GetComponent<Image>().sprite = popupBackground;
        demRectUI.setUIText(gameOverUI, "Game Over! Play Again?");

        //Quit Button on Quit UI
        GameObject yesButton = demButtonFactory.CreateButton(0, 0, "Yes");
        yesButton.transform.SetParent(gameOverUI.transform);
        yesButton.GetComponent<RectTransform>().anchoredPosition =
          new Vector2(gameOverUI.GetComponent<RectTransform>().sizeDelta.x / 5.0f,
        -gameOverUI.GetComponent<RectTransform>().sizeDelta.y / 5.0f * 3.0f);
        demButtonFactory.SetButtonText(yesButton, "Yes");
        yesButton.GetComponent<Button>().onClick.AddListener(() => { DemAudioManager.audioClick.Play(); Game.SwitchScene("DontEatMe"); });

        //back button on Quit UI
        GameObject noButton = demButtonFactory.CreateButton(0, 0, "No");
        noButton.transform.SetParent(gameOverUI.transform);
        noButton.GetComponent<RectTransform>().anchoredPosition =
        new Vector2(gameOverUI.GetComponent<RectTransform>().sizeDelta.x / 5.0f * 3.0f,
        -gameOverUI.GetComponent<RectTransform>().sizeDelta.y / 5.0f * 3.0f);
        demButtonFactory.SetButtonText(noButton, "No");
        //noButton.GetComponent<Button> ().onClick.AddListener (()=>{quitUI.SetActive(false);});
        noButton.GetComponent<Button>().onClick.AddListener(() => { DemAudioManager.audioClick.Play(); Game.SwitchScene("World"); });

        mainUIObject.SetActive(false);
    }


    // Updates player's credits
    public void ProcessEndGame (NetworkResponse response)
    {
        ResponsePlayGame args = response as ResponsePlayGame;

        if (args.status == 1)
        {

            GameState.player.credits = args.creditDiff;
            Debug.Log(args.creditDiff);

        }
    }


    // Update is called once per frame
    //void Update()
    //{
        // NOTHING TO DO HERE... //
    //}

    // TODO: documentation....
    public DemAnimalFactory GetCurrentAnimalFactory()
    {
        return currentAnimalFactory;
    }

    // TODO: or not TODO...
    public void SetCurrentAnimalFactory(DemAnimalFactory newAnimalFactory)
    {
        currentAnimalFactory = newAnimalFactory;
    }


    // TODO: bake some cookies
    public void ToggleButtonLocks()
    {
        if (turnSystem.IsTurnLocked())
        {
            turnSystemText.GetComponent<Text>().text = "Your Turn!";
            //UnlockAllMenuItems();
        }
        else
        {
            turnSystemText.GetComponent<Text>().text = "Predator Turn!";
            //LockAllMenuItems();
        }

    }

    // BUTTON LOCKING METHODS
    //
    /**
        Locks a single menu button by disabling interactivity and dimming its color.
        An optional boolean value may be specified to denote whether a button is in the foreground (true by default) or
        background; this will determine the dimmed color values as determined by 'lockedColor' for foreground and
        'inactiveColor' for background; the former is, by default, brighter with 100% alpha while the latter is darker
        with an alpha below 100%.

        @param  menu        a GameObject array comprising menu buttons (plant or prey)
        @param  index       index offset of menu button to lock
        @param  foreground  [optional] true if button is in the foreground, false if in background
    */
    public void LockMenuButton (GameObject[] menu, int index, bool foreground = true)
    {
        menu[index].GetComponent<Button>().interactable = false;
        menu[index].GetComponentsInChildren<Image>()[0].color = foreground ? lockedColor : inactiveColor;
        menu[index].GetComponentsInChildren<Image>()[1].color = foreground ? lockedIconColor : inactiveIconColor;
    }

    /**
        Locks all PLANT menu items (i.e. build buttons).
        Buttons are subsequently non-interactive and dimmed in color.

        @param  foreground  [optional] true if button is in the foreground, false if in background
    */
    public void LockPlantMenuItems (bool foreground = true)
    {
        for (byte i = 0; i < plantBuildButtons.Length; i++)
            LockMenuButton(plantBuildButtons, i, foreground);
    }

    /**
        Locks all PREY menu items (i.e. build buttons).
        Buttons are subsequently non-interactive and dimmed in color.

        @param  foreground  [optional] true if button is in the foreground, false if in background
    */    
    public void LockPreyMenuItems (bool foreground = true)
    {
        for (byte i = 0; i < preyBuildButtons.Length; i++)
            LockMenuButton(preyBuildButtons, i, foreground);
    }

    /**
        Locks all menu items for both plants and prey.

        @param  foreground  [optional] true if button is in the foreground, false if in background

        @see    BuildMenu.LockPlantMenuItems
        @see    BuildMenu.LockPreyMenuItems
    */
    public void LockAllMenuItems (bool foreground = true)
    {
        LockPlantMenuItems(foreground);
        LockPreyMenuItems(foreground);
    }

    // BUTTON UNLOCKING METHODS //
    //
    /**
        Unlocks a single menu button by enabling interactivity and restoring its color.

        @param  menu    a GameObject array comprising menu buttons (plant or prey)
        @param  index   index offset of menu button to unlock
    */    
    public void UnlockMenuButton (GameObject[] menu, int index)
    {
        menu[index].GetComponent<Button>().interactable = true;
        foreach (Image image in menu[index].GetComponentsInChildren<Image>())
            image.color = activeColor;
    }

    /**
        Unlocks all PLANT menu items (i.e. build buttons).
        Buttons are subsequently interactive and undimmed in color.
    */
    public void UnlockPlantMenuItems ()
    {
        for (byte i = 0; i < plantBuildButtons.Length; i++)
            UnlockMenuButton(plantBuildButtons, i);
    }

    /**
        Unlocks all PREY menu items (i.e. build buttons).
        Buttons are subsequently interactive and undimmed in color.
    */
    public void UnlockPreyMenuItems ()
    {
        for (byte i = 0; i < preyBuildButtons.Length; i++)
            UnlockMenuButton(preyBuildButtons, i);
    }

    /**
        Unlocks all menu items for both plants and prey.

        @see    BuildMenu.UnlockPlantMenuItems
        @see    BuildMenu.UnlockPreyMenuItems
    */
    public void UnlockAllMenuItems ()
    {
        UnlockPlantMenuItems();
        UnlockPreyMenuItems();
    }

    /** 
        Checks if plant menu is active (false implies prey menu is active).

        @return true if plant menu is active, false otherwise
    */
    public bool PlantMenuActive ()
    {
        return plantMenuActive;
    }


    // TOOD: documentation, etc., etc., etc.... 


    public void UpdateLives(int lives)
    {

        livesText.GetComponent<Text>().text = lives.ToString();

    }


    public void UpdateCredits(int credits)
    {

        scoreText.GetComponent<Text>().text = credits.ToString();

    }

    public void UpdateTier1Biomass()
    {
        plantBioText.GetComponent<Text>().text = tier1Biomass.ToString();
    }

    public void UpdateTier1Biomass(int biomass)
    {
        tier1Biomass = biomass;
        UpdateTier1Biomass();

    }

    public int GetTier1Biomass()
    {

        return tier1Biomass;

    }

    public int GetTier2Biomass()
    {

        return tier2Biomass;

    }

    public int GetTier3Biomass()
    {

        return tier3Biomass;

    }

    public void UpdateTier2Biomass()
    {

        tier2BioText.GetComponent<Text>().text = tier2Biomass.ToString();

    }

    public void UpdateTier2Biomass(int biomass)
    {

        tier2Biomass = biomass;
        UpdateTier2Biomass();

    }

    public void AddTier2Biomass(int biomass)
    {

        tier2Biomass += biomass;
        UpdateTier2Biomass();

    }


    public void SubtractTier2Biomass(int biomass)
    {

        tier2Biomass -= biomass;
        UpdateTier2Biomass();

    }

    public void UpdateTier3Biomass()
    {

        tier3BioText.GetComponent<Text>().text = tier3Biomass.ToString();

    }

    public void UpdateTier3Biomass(int biomass)
    {

        tier3Biomass = biomass;
        UpdateTier3Biomass();

    }

    public void AddTier3Biomass(int biomass)
    {

        tier3Biomass += biomass;
        UpdateTier3Biomass();

    }

    public void AddTier1Biomass(int biomass)
    {

        tier1Biomass += biomass;
        UpdateTier1Biomass();

    }


    public void SubtractTier3Biomass(int biomass)
    {

        tier3Biomass -= biomass;
        UpdateTier3Biomass();

    }

    public void SubtractTier1Biomass(int biomass)
    {
        tier1Biomass -= biomass;
        UpdateTier1Biomass();
    }

}
