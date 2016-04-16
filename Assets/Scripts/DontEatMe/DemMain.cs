using UnityEngine;
using System.Collections;

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

    
    	currentSelection = null;

      predators = new DemAnimalFactory[2];
      predators [0] = new DemAnimalFactory ("Aardvark"); 
      predators [1] =  new DemAnimalFactory ("African Marsh Owl"); 
        

 


        
        

  


    }


  void Start()
  {
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
    		if (Input.GetKeyDown(KeyCode.Escape)) {
          //BuildMenu.currentAnimalFactory = null;
          buildMenu.SetCurrentAnimalFactory (null);

    			//Destroy(currentSelection);
    			// DEBUG MESSAGE
    			//Debug.Log("currentlyBuilding reset to 'null', returning object to (" + buildOrigin.x + ", " + buildOrigin.y + ")");

    			// Start easing animation
    			StartCoroutine(easeReturn(0.05f));
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
    	Sets the current prey's origin, i.e. the corresponding button
    */
    public  void setBuildOrigin (Vector3 origin) {
    
    	buildOrigin = origin;

    }

    /**
    	Eases a cancelled currentlyBuilding object back to its respective button.
    */
    IEnumerator easeReturn (float easing) {
    
		float startDistance = Vector3.Distance(buildOrigin, currentSelection.transform.position);
    	while (Vector3.Distance(buildOrigin, currentSelection.transform.position) > startDistance/10) {

			currentSelection.transform.position = Vector3.Lerp
			(
				currentSelection.transform.position,
				buildOrigin,
				easing * Vector3.Distance(buildOrigin, currentSelection.transform.position)
			);
			yield return new WaitForSeconds(0.01f);
    	}
    	Destroy(currentSelection);

    }

}
