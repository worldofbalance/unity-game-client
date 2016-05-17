using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;
using System;

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

	//statiUI
	private GameObject statUI;
	public Statistic statistic;
	private GameObject t1;	//static text 1 to 6
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
    public GameObject[] menuButtons;

    private GameObject mainObject;

    private DemTurnSystem turnSystem;

    private DemMain main;

    private int plantBiomass; 

    private int tier2Biomass; 

    private int tier3Biomass; 

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

    // this method increases score every 2s
    void increaseResources()
    {
        currentResources += 50;
    }


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
        mainUIObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        mainUIObject.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
        mainUIObject.GetComponent<RectTransform>().anchorMax = new Vector2(1, 1);
        mainUIObject.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
        mainUIObject.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);

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
		statistic = new Statistic ();
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
        gameObject.AddComponent<DemButton>();
        demButton = gameObject.GetComponent<DemButton>();

        // building the RectUI
        gameObject.AddComponent<DemRectUI>();
        demRectUI = gameObject.GetComponent<DemRectUI>();


        // Toggle button to switch between plant and prey menu
        demButton.setSize(Screen.width * 0.1f, Screen.height / 14);
        GameObject toggleButton = demButton.CreateButton(0, 0, "Toggle");
        demButton.SetButtonText(toggleButton, "Plants");


        // Creates a buttons for plant/prey menu
        demButton.setSize(Screen.width * 0.1f, Screen.height / 7);
        menuButtons = new GameObject[6];
        for (int i = 0; i < 6; i++)
        {

            GameObject button = demButton.CreateButton(0, 0 - ((Screen.height / 14) + 10 + i * (demButton.getYSize() - 2)), i.ToString());

            // Set the button images
            demButton.SetButtonImage(plants[i], button);
            demButton.SetButtonImage(prey[i], button);

            // Set the images of the untoggled menu to inactive
            button.transform.Find(prey[i].GetName()).gameObject.SetActive(false);

            // Add an onClick listener to detect button clicks
            button.GetComponent<Button>().onClick.AddListener(() => { selectSpecies(button); });
            button.AddComponent<DemButton>();

            menuButtons[i] = button;
        }

        // Add an onClick listener to dectect button clicks
        toggleButton.GetComponent<Button>().onClick.AddListener(() => { selectMenu(toggleButton, menuButtons); });

        //quit button 
        float qBX = Screen.width / 10.0f;
        float qBY = Screen.height / 10.0f;
        demButton.setSize(qBX, qBY);
        GameObject quitButton = demButton.CreateButton(Screen.width - qBX, 0, "Quit");
        demButton.SetButtonText(quitButton, "Quit");
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
		float sBY = Screen.height / 10.0f;
		demButton.setSize(sBX, sBY);
		GameObject statButton = demButton.CreateButton(Screen.width - qBX*2, 0, "statistic");
		demButton.SetButtonText(statButton, "Statistic");
		statButton.GetComponent<Button>().onClick.AddListener(() => { selectStatistic(); });
		// quitButton.transform.SetParent(menuPanel.transform); 

		statButton.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
		statButton.GetComponent<RectTransform>().anchorMin = new Vector2(1, 1);
		statButton.GetComponent<RectTransform>().anchorMax = new Vector2(1, 1);

		statButton.GetComponent<RectTransform>().offsetMax = new Vector2(-70, -22*3);
		statButton.GetComponent<RectTransform>().offsetMin = new Vector2(-70, -22*3);

		statButton.GetComponent<RectTransform>().sizeDelta = new Vector2(70, 30);


        // Instructions button
        float iBX = Screen.width / 5.0f;
        float iBY = Screen.height / 10.0f;
        demButton.setSize(iBX, iBY);
        GameObject instructionButton = demButton.CreateButton(Screen.width - iBX, 10, "How to Play");
        demButton.SetButtonText(instructionButton, "?");

        instructionButton.GetComponent<Image>().sprite = Resources.Load<Sprite>("DontEatMe/Sprites/buttonBackgroundRound");
        instructionButton.GetComponent<Button>().onClick.AddListener(() => { selectInstruction(); });

        instructionButton.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
        instructionButton.GetComponent<RectTransform>().anchorMin = new Vector2(1, 1);
        instructionButton.GetComponent<RectTransform>().anchorMax = new Vector2(1, 1);

        instructionButton.GetComponent<RectTransform>().offsetMax = new Vector2(-154, -22);
        instructionButton.GetComponent<RectTransform>().offsetMin = new Vector2(-145, -22);

        instructionButton.GetComponent<RectTransform>().sizeDelta = new Vector2(30, 30);

        // Reset sizes for quit popup buttons
        demButton.setSize(qBX, qBY);
        UpdatePlantBiomass (); 
        UpdateBuildableAnimals ();
    }


    // Toggle between plant and prey menu when the toggle button is clicked
    void selectMenu(GameObject tButton, GameObject[] mButtons)
    {
        Debug.Log("Clicked " + tButton.name);

    toggleCount = (toggleCount + 1) % 2;

        if (toggleCount == 0)
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

      UpdateMenuLocks ();
    }


    /**
     * Use this method To set animals that go over the resource limit to not be buildable
     */

  public void UpdateMenuLocks(){
    UnlockAllMenuItems ();
    UpdateBuildableAnimals ();
  }

    public void UpdateBuildableAnimals()
    {
      for (int i = 0; i < 6; i++)
      {
      if (toggleCount == 0) {
        if(SpeciesConstants.Biomass (plants [i].GetName ()) > plantBiomass) {
          LockMenuButton (i);
        }

        } else {
          if(SpeciesConstants.Biomass (prey [i].GetName ()) > tier2Biomass) {
          LockMenuButton (i);
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

        if (toggleCount % 2 == 0)
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
            demRectUI.setUIText(quitUI, GameState.player.GetName() + "\nAre you sure you want to quit? \nYou currently have " 
                + GameState.player.credits);

            //Quit Button on Quit UI
            GameObject yesButton = demButton.CreateButton(0, 0, "Yes");
            yesButton.transform.SetParent(quitUI.transform);
            yesButton.GetComponent<RectTransform>().anchoredPosition =
                new Vector2(quitUI.GetComponent<RectTransform>().sizeDelta.x / 5.0f,
                            -quitUI.GetComponent<RectTransform>().sizeDelta.y / 5.0f * 3.0f);
            demButton.SetButtonText(yesButton, "Quit");
            yesButton.GetComponent<Button>().onClick.AddListener(() => { DemAudioManager.audioClick.Play();  Game.SwitchScene("World"); });

            //back button on Quit UI
            GameObject noButton = demButton.CreateButton(0, 0, "No");
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

    void selectInstruction()
    {
        DemAudioManager.audioClick.Play();

        if (main.currentSelection)
        {
            Destroy(main.currentSelection);
            main.boardController.ClearAvailableTiles();
        }

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

    }


    public void CloseInstructionWindow(){
      DemAudioManager.audioClick.Play(); 
      instructionUI.SetActive(false); 
      mainUIObject.SetActive(true);
    }

    public void SetNextInstructionSlide(){
    }



	//click on statistic button
	public void selectStatistic(){
		DemAudioManager.audioClick.Play();

		if (main.currentSelection) {
			Destroy(main.currentSelection);
			main.boardController.ClearAvailableTiles();
		}

		if (statUI == null) {
			statUI = demRectUI.createRectUI ("statUI", 0, 0, Screen.width / 1.2f, Screen.height / 1.2f);
			statUI.GetComponent<Image> ().sprite = popupBackground;
			t1 = demRectUI.setUIText (statUI, "plant used: "+statistic.getTreeDown(),0,0);
			t2 = demRectUI.setUIText (statUI, "prey used: "+statistic.getPreyDown(),0,1);
			t3 = demRectUI.setUIText (statUI, "plant destroyed: "+ statistic.getTreeDestroy(),1,0);
			t4 = demRectUI.setUIText (statUI, "prey eaten: "+ statistic.getPreyEaten(),1,1);
			t5 = demRectUI.setUIText (statUI, "turns: "+ statistic.getTurnCount (),2,0);
			t6 = demRectUI.setUIText (statUI, "",2,1);

			GameObject backButton = demButton.CreateButton (0, 0, "back");
			backButton.transform.SetParent (statUI.transform);
			backButton.GetComponent<RectTransform> ().anchoredPosition = 
				new Vector2 (statUI.GetComponent<RectTransform> ().sizeDelta.x/2.0f- backButton.GetComponent<RectTransform>().sizeDelta.x/2.0f,
					-statUI.GetComponent<RectTransform> ().sizeDelta.y/5.0f*3.0f - backButton.GetComponent<RectTransform>().sizeDelta.y);
			demButton.SetButtonText (backButton, "Back");
			backButton.GetComponent<Button> ().onClick.AddListener (()=>{
				DemAudioManager.audioClick.Play(); 
				statUI.SetActive(false); 
				mainUIObject.SetActive(true);
			});

			mainUIObject.SetActive (false);

			return;
		}

		t1.GetComponent<Text> ().text = "plant used: " + statistic.getTreeDown ();
		t2.GetComponent<Text> ().text = "prey used: " + statistic.getPreyDown ();
		t3.GetComponent<Text> ().text = "plant destroyed: " + statistic.getTreeDestroy();
		t4.GetComponent<Text> ().text = "prey eaten: " + statistic.getPreyEaten ();
		t5.GetComponent<Text> ().text = "turns: " + statistic.getTurnCount ();

		if (!statUI.activeInHierarchy) {
			statUI.SetActive (true);
			mainUIObject.SetActive (false);
		}

	}


    public void EndGame()
    {
        gameOverUI = demRectUI.createRectUI("quitUI", 0, 0, Screen.width / 2.0f, Screen.height / 2.0f);
        gameOverUI.GetComponent<Image>().sprite = popupBackground;
        demRectUI.setUIText(gameOverUI, "Game Over! Play Again?");

        //Quit Button on Quit UI
        GameObject yesButton = demButton.CreateButton(0, 0, "Yes");
        yesButton.transform.SetParent(gameOverUI.transform);
        yesButton.GetComponent<RectTransform>().anchoredPosition =
          new Vector2(gameOverUI.GetComponent<RectTransform>().sizeDelta.x / 5.0f,
        -gameOverUI.GetComponent<RectTransform>().sizeDelta.y / 5.0f * 3.0f);
        demButton.SetButtonText(yesButton, "Yes");
        yesButton.GetComponent<Button>().onClick.AddListener(() => { DemAudioManager.audioClick.Play(); Game.SwitchScene("DontEatMe"); });

        //back button on Quit UI
        GameObject noButton = demButton.CreateButton(0, 0, "No");
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

    public DemAnimalFactory GetCurrentAnimalFactory()
    {
        return currentAnimalFactory;
    }

    public void SetCurrentAnimalFactory(DemAnimalFactory newAnimalFactory)
    {
        currentAnimalFactory = newAnimalFactory;
    }

    public void ToggleButtonLocks()
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

    public void UnlockAllMenuItems()
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


  public void LockMenuButton(int buttonNum)
  {
    menuButtons[buttonNum].GetComponent<Button>().interactable = false;
    foreach (Image image in menuButtons[buttonNum].GetComponentsInChildren<Image>())
    {
      image.color = new Color(1.0F, 1.0F, 1.0F, 0.8F);
    }
  }


    public void UpdateLives(int lives)
    {

        livesText.GetComponent<Text>().text = lives.ToString();

    }


    public void UpdateCredits(int credits)
    {

        scoreText.GetComponent<Text>().text = credits.ToString();

    }

    public void UpdatePlantBiomass()
    {
        plantBioText.GetComponent<Text> ().text = plantBiomass.ToString ();
    }

    public void UpdatePlantBiomass(int biomass)
    {
        plantBiomass = biomass;
        UpdatePlantBiomass ();

    }

    public int getPlantBiomass()
    {
    
        return plantBiomass;

    }

    public int GetTier2Biomass()
    {
    
    return tier2Biomass;

    }

    public int GetTier3Biomass(){

    return tier3Biomass;
      
    }

    public void UpdateTier2Biomass(){

      tier2BioText.GetComponent<Text> ().text = tier2Biomass.ToString (); 

    }

    public void UpdateTier2Biomass(int biomass)
    {

      tier2Biomass = biomass;
      UpdateTier2Biomass ();
      
    }

    public void AddTier2Biomass(int biomass)
    {

      tier2Biomass += biomass;
      UpdateTier2Biomass ();

    }


    public void SubtractTier2Biomass(int biomass)
    {

      tier2Biomass -= biomass;
      UpdateTier2Biomass ();

    }

    public void UpdateTier3Biomass(){

      tier3BioText.GetComponent<Text> ().text = tier3Biomass.ToString (); 

    }

    public void UpdateTier3Biomass(int biomass)
    {

     tier3Biomass = biomass;
     UpdateTier3Biomass ();

    }

    public void AddTier3Biomass(int biomass)
    {

      tier3Biomass += biomass;
      UpdateTier3Biomass ();

    }

  public void AddPlantBiomass(int biomass)
  {

    plantBiomass += biomass;
    UpdatePlantBiomass ();

  }


    public void SubtractTier3Biomass(int biomass)
    {

      tier3Biomass -= biomass;
      UpdateTier3Biomass ();

    }

  public void SubtractPlantBiomass(int biomass){
    plantBiomass -= biomass;
    UpdatePlantBiomass ();
  }

}
