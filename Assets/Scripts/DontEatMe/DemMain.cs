using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class DemMain : MonoBehaviour
{
  public  GameObject mainObject;

	public  GameObject currentSelection;

  private  GameObject gameBoard;

  public  DemBoard boardController;

	private Vector3 buildOrigin;

  public  DemAnimalFactory[] predators;

  private DemTweenManager tweenManager;

  private BuildMenu buildMenu;

  private DemTurnSystem turnSystem;




    // Use this for initialization
    void Awake()
    {

      mainObject = GameObject.Find ("MainObject");
      tweenManager = mainObject.GetComponent<DemTweenManager> ();
      buildMenu = mainObject.GetComponent<BuildMenu> ();
      // Setup Play board
      //boardGroup = new GameObject("GameBoard");
      gameBoard = GameObject.Find("GameBoard");
      //Keep track of our tiles
      boardController = gameBoard.GetComponent<DemBoard> ();

      turnSystem = mainObject.GetComponent<DemTurnSystem> ();


      //Pick predators
      // FIXME: Swap hard-coded predators for dynamically-generated;
      // also, Bat-Eared Fox is in both the predator and the prey selections, so there's some cannibalism going on
      // (Yes, it's been tested and verified: the "predator" version eats the "prey" version... no bueno.)
    
    	currentSelection = null;

      predators = new DemAnimalFactory[6];
      predators [0] = new DemAnimalFactory ("Bat-Eared Fox"); 
      predators [1] =  new DemAnimalFactory ("Black Mamba"); 
      predators [2] =  new DemAnimalFactory ("Serval Cat"); 
      predators [3] =  new DemAnimalFactory ("African Wild Dog"); 
      predators [4] =  new DemAnimalFactory ("Leopard"); 
      predators [5] =  new DemAnimalFactory ("Lion"); 
        
        // "Trust me guys, we need all these empty lines."
 


        
        

  


    }


  void Start()

  {
    DemAudioManager.audioBg.Play();

    for (int x = 0; x < 9; x++)
    {

      for (int y = 0; y < 5; y++)
      {
        boardController.Add (x, y );

      }

    }
  }

    // Update is called once per frame
    void Update()
    {
        tweenManager.Update ();
    	// If a species is currently selected for building, update its position to the cursor
        if (buildMenu.GetCurrentAnimalFactory() != null) {
    		if (currentSelection) {
          
    			Vector3 world_pos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y));
    			world_pos.z = -1.5f;
    			currentSelection.transform.position = world_pos;
 
    		}

    		// Cancel currently selected species on Escape key press
    		if (Input.GetKeyDown(KeyCode.Escape))
            {
                //BuildMenu.currentAnimalFactory = null;
                buildMenu.SetCurrentAnimalFactory (null);         
    			// Start easing animation
    			StartCoroutine(easeReturn());
                boardController.ClearAvailableTiles();
    		}
		}
        else
        {
            // Hotkeys for build menus //
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                if (buildMenu.PlantMenuActive())
                {
                    if (buildMenu.plantBuildButtons[0].GetComponent<Button>().interactable)
                        buildMenu.selectSpecies(buildMenu.plantBuildButtons[0]);
                }
                else if (buildMenu.preyBuildButtons[0].GetComponent<Button>().interactable)
                    buildMenu.selectSpecies(buildMenu.preyBuildButtons[0]);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                if (buildMenu.PlantMenuActive())
                {
                    if (buildMenu.plantBuildButtons[1].GetComponent<Button>().interactable)
                        buildMenu.selectSpecies(buildMenu.plantBuildButtons[1]);
                }
                else if (buildMenu.preyBuildButtons[1].GetComponent<Button>().interactable)
                    buildMenu.selectSpecies(buildMenu.preyBuildButtons[1]);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                if (buildMenu.PlantMenuActive())
                {
                    if (buildMenu.plantBuildButtons[2].GetComponent<Button>().interactable)
                        buildMenu.selectSpecies(buildMenu.plantBuildButtons[2]);
                }
                else if (buildMenu.preyBuildButtons[2].GetComponent<Button>().interactable)
                    buildMenu.selectSpecies(buildMenu.preyBuildButtons[2]);
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                if (buildMenu.PlantMenuActive())
                {
                    if (buildMenu.plantBuildButtons[3].GetComponent<Button>().interactable)
                        buildMenu.selectSpecies(buildMenu.plantBuildButtons[3]);
                }
                else if (buildMenu.preyBuildButtons[3].GetComponent<Button>().interactable)
                    buildMenu.selectSpecies(buildMenu.preyBuildButtons[3]);
            }
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                if (buildMenu.PlantMenuActive())
                {
                    if (buildMenu.plantBuildButtons[4].GetComponent<Button>().interactable)
                        buildMenu.selectSpecies(buildMenu.plantBuildButtons[4]);
                }
                else if (buildMenu.preyBuildButtons[4].GetComponent<Button>().interactable)
                    buildMenu.selectSpecies(buildMenu.preyBuildButtons[4]);
            }
            if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                if (buildMenu.PlantMenuActive())
                {
                    if (buildMenu.plantBuildButtons[5].GetComponent<Button>().interactable)
                        buildMenu.selectSpecies(buildMenu.plantBuildButtons[5]);
                }
                else if (buildMenu.preyBuildButtons[5].GetComponent<Button>().interactable)
                    buildMenu.selectSpecies(buildMenu.preyBuildButtons[5]);
            }
        }

		// DEBUGGING STUFF
		if (Input.GetKeyDown(KeyCode.Space)) {
			Vector3 wp = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -1.5f));
			Debug.Log("Cursor at (" + wp.x + ", " + wp.y + ")");
			if (currentSelection != null)
				Debug.Log("currentSelection at (" + currentSelection.transform.position.x + ", " + currentSelection.transform.position.y + ")");
		}
    }

    /**
    	Sets the current prey's origin, i.e. the corresponding button.

        @param  origin  a Vector3 object
    */
    public  void setBuildOrigin (Vector3 origin)
    {
    	buildOrigin = origin;
    }

    /**
    	Eases a cancelled currentlyBuilding object back to its respective button.
        An easing coefficient may be specified, which represents the ratio of the remaining distance to travel each time
        segment.

        @param  easing  a floating point value in the range (0, 1]
    */
    IEnumerator easeReturn (float easing = 0.05f)
    {
        // Keep easing coefficient in range, define starting distance
        easing = (float)Math.Min(Math.Max(1e-12, easing), 1.0f);
		float startDistance = Vector3.Distance(buildOrigin, currentSelection.transform.position);
        // Ease until within one tenth the starting distance
    	while (Vector3.Distance(buildOrigin, currentSelection.transform.position) > startDistance / 10)
        {
			currentSelection.transform.position = Vector3.Lerp
			(
				currentSelection.transform.position,
				buildOrigin,
				easing * Vector3.Distance(buildOrigin, currentSelection.transform.position)
			);
			yield return new WaitForSeconds(0.01f);
    	}
        // Destroy current selection upon arrival
    	Destroy(currentSelection);
    }
}
