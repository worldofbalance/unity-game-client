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

    private bool easeEnd; // True if easing ended
    private KeyCode[] hotkeys; // Numeric hotkey KeyCodes


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
    	currentSelection = null;

        predators = new DemAnimalFactory[6];
        predators [0] = new DemAnimalFactory ("Bat-Eared Fox"); 
        predators [1] =  new DemAnimalFactory ("Black Mamba"); 
        predators [2] =  new DemAnimalFactory ("Serval Cat"); 
        predators [3] =  new DemAnimalFactory ("African Wild Dog"); 
        predators [4] =  new DemAnimalFactory ("Leopard"); 
        predators [5] =  new DemAnimalFactory ("Lion");

        easeEnd = true;
        hotkeys = new KeyCode[6]
        {
            KeyCode.Alpha1,
            KeyCode.Alpha2,
            KeyCode.Alpha3,
            KeyCode.Alpha4,
            KeyCode.Alpha5,
            KeyCode.Alpha6
        };
        
        // "Trust me guys, we need all these empty lines."
 


        
        

  


    }


  void Start()

  {
    DemAudioManager.audioBg.Play();

    for (int x = 0; x < boardController.numColumns; x++)
    {

      for (int y = 0; y < boardController.numRows; y++)
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
        if (buildMenu.GetCurrentAnimalFactory() != null)
        {
    		//if (currentSelection && easeEnd)
            if (easeEnd)
            {
                Vector3 world_pos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y));
                world_pos.z = -1.5f;
                currentSelection.transform.position = world_pos;
    		}

    		// Cancel currently selected species on Escape key press
    		if (Input.GetKeyDown(KeyCode.Escape))
            {
                buildMenu.SetCurrentAnimalFactory(null);         
                // Start easing animation
                StartCoroutine(EaseReturn());
                boardController.ClearAvailableTiles();
    		}
		}

        // Hotkeys for build menus //
        for (byte i = 0; i < hotkeys.Length; i++)
        {
            if (Input.GetKeyDown(hotkeys[i]))
            {
                Debug.Log("plantBuildButtons.Length = " + buildMenu.plantBuildButtons.Length);
                if (easeEnd)
                {
                    // Plant
                    if (buildMenu.PlantMenuActive())
                    {
                        if (buildMenu.plantBuildButtons[i].GetComponent<Button>().interactable)
                        {
                            // If currently building, return currentSelection and reset board
                            if (buildMenu.GetCurrentAnimalFactory() != null)
                            {
                                buildMenu.SetCurrentAnimalFactory(null);
                                byte k = i;
                                StartCoroutine(EaseReturn(0.05f, () => { buildMenu.selectSpecies(buildMenu.plantBuildButtons[k]); }));
                                boardController.ClearAvailableTiles();

                            }
                            // Else set build immediately
                            else buildMenu.selectSpecies(buildMenu.plantBuildButtons[i]);
                        }
                    }
                    // Prey
                    else if (buildMenu.preyBuildButtons[i].GetComponent<Button>().interactable)
                    {
                        // If currently building, return currentSelection and reset board
                        if (buildMenu.GetCurrentAnimalFactory() != null)
                        {
                                buildMenu.SetCurrentAnimalFactory(null);
                                byte k = i;
                                StartCoroutine(EaseReturn(0.05f, () => { buildMenu.selectSpecies(buildMenu.plantBuildButtons[k]); }));
                                boardController.ClearAvailableTiles();
                        }
                        // Set build
                        else buildMenu.selectSpecies(buildMenu.preyBuildButtons[i]);
                    }
                }
            }
        }

		// Toggle build button categories
		if (Input.GetKeyDown(KeyCode.Space))
            buildMenu.SetBuildButtonCategory(buildMenu.PlantMenuActive() ? 1 : 0);

        // Invoke skip function (CAUTION! executes without confirm/cancel prompt!)
        if (Input.GetKeyDown(KeyCode.S))
        {
            buildMenu.SelectSkip(false, 0.25f);
        }
    }

    /**
    	Sets the current prey's origin, i.e. the corresponding button.

        @param  origin  a Vector3 object
    */
    public void setBuildOrigin (Vector3 origin)
    {
    	buildOrigin = origin;
    }

    /**
    	Eases a cancelled currentlyBuilding object back to its respective button.
        An easing coefficient may be specified, which represents the ratio of the remaining distance to travel each time
        segment.

        @param  easing      a floating point value in the range (0, 1]
        @param  onComplete  an Action<bool> to execute on completion
    */
    IEnumerator EaseReturn (float easing = 0.05f, Action onComplete = null)
    {
        // Set easeEnd to false
        easeEnd = false;
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
        // Destroy current selection upon arrival, flag easeEnd as true
        Destroy(currentSelection);
        easeEnd = true;
        if (onComplete != null) onComplete();
    }
}
