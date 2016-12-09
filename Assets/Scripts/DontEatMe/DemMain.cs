using UnityEngine;
using System.Collections;

/**
    TODO: proper documentation
    TODO: clean up the code formatting:
        - Eliminate superfluous whitespace and newlines
        - Use proper indentation

    The DemMain class is responsible for initializing and controlling all game components and behaviors.
*/
public class DemMain : MonoBehaviour
{
    // Public data members
    public  GameObject mainObject;
    public  GameObject currentSelection;
    public  DemBoard boardController;
    public  DemAnimalFactory[] predators;

    // Private data members
    private GameObject gameBoard;
    private Vector3 buildOrigin;
    private DemTweenManager tweenManager;
    private BuildMenu buildMenu;
    private DemTurnSystem turnSystem;

    // Use this for initialization
    void Awake () {
        mainObject = GameObject.Find ("MainObject");
        tweenManager = mainObject.GetComponent<DemTweenManager> ();
        buildMenu = mainObject.GetComponent<BuildMenu> ();
        // Setup Play board
        //boardGroup = new GameObject("GameBoard");
        gameBoard = GameObject.Find("GameBoard");
        //Keep track of our tiles
        boardController = gameBoard.GetComponent<DemBoard> ();
        // Initialize turn system
        turnSystem = mainObject.GetComponent<DemTurnSystem> ();

        // Initialize predator list
        currentSelection = null;
        predators = new DemAnimalFactory[6];
        SpeciesConstants.SortPredatorsByBiomass();
        for (int i = 0; i < SpeciesConstants.NUM_PREDATORS; i++)
            predators[i] = new DemAnimalFactory(SpeciesConstants.PredatorNames()[i]);

        /*
        predators [0] =  new DemAnimalFactory ("Bat-Eared Fox"); 
        predators [1] =  new DemAnimalFactory ("Black Mamba"); 
        predators [2] =  new DemAnimalFactory ("Serval Cat"); 
        predators [3] =  new DemAnimalFactory ("African Wild Dog"); 
        predators [4] =  new DemAnimalFactory ("Leopard"); 
        predators [5] =  new DemAnimalFactory ("Lion"); 
        */
    }


    void Start ()
    {
        DemAudioManager.audioBg.Play();

        for (int x = 0; x < 9; x++)
            for (int y = 0; y < 5; y++)
                boardController.Add (x, y);
    }

    // Update is called once per frame
    void Update ()
    {
        tweenManager.Update();

        // If a species is currently selected for building, update its position to the cursor
        if (buildMenu.GetCurrentAnimalFactory() != null) 
        {
    		if (currentSelection) 
            {
    			Vector3 world_pos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y));
    			world_pos.z = -1.5f;
    			currentSelection.transform.position = world_pos;
    		}

    		// Cancel currently selected species on Escape key press
    		if (Input.GetKeyDown(KeyCode.Escape)) 
            {
                buildMenu.SetCurrentAnimalFactory (null);
                StartCoroutine(easeReturn());
                boardController.ClearAvailableTiles();
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
    public  void setBuildOrigin (Vector3 origin)
    {
    	buildOrigin = origin;
    }

    /**
    	Eases a cancelled currentlyBuilding object back to its respective button.
        The easing value determines the speed of return.
        A floating point value between 0 (non-inclusive) and 1 (inclusive) represent the portion of the remaining 
        distance to travel on each iteration; for instance, an easing of 0.5f will move the currentlyBuilding object
        half its remaining distance on each iteration.
        Once the object enters a threshold distance, the easing ends and the currentlyBuilding object destroyed.

        @param  easing          the proportion of remaining distance to travel each iteration (default = 0.05f)
        @param  iteration       the length of each easing iteration in seconds (default = 0.01f)
        @param  thresholdFactor sets the threshold distance of easing completion as a proportion of the original distance (default = 0.1f)
    */
    IEnumerator easeReturn (float easing = 0.05f, float iteration = 0.01f, float thresholdFactor = 0.1f)
    {
        // Keep arguments in range
        easing = limitRange(easing, 0.001f, 1f);
        iteration = limitRange(iteration, 0.001f, 1f);
        thresholdFactor = limitRange(thresholdFactor, 0.001f, 1f);

        // One-time calculation of threshold distance at which object is destroyed
		float threshold = Vector3.Distance(buildOrigin, currentSelection.transform.position) * thresholdFactor;
        // Ease until threshold reached
    	while (Vector3.Distance(buildOrigin, currentSelection.transform.position) > threshold)
        {
			currentSelection.transform.position = Vector3.Lerp
			(
				currentSelection.transform.position,
				buildOrigin,
				easing * Vector3.Distance(buildOrigin, currentSelection.transform.position)
			);
			yield return new WaitForSeconds(iteration);
    	}
    	Destroy(currentSelection);
    }

    /**
        Keeps a floating point value within a specified range.

        @param  val the original value to check
        @param  min the minimum allowed value (inclusive)
        @param  max the maximum allowed value (inclusive)

        @return a floating point number guaranteed in the range of [min, max]
    */
    float limitRange (float val, float min, float max)
    {
        return val < min ? min : (val > max ? max : val);
    }
}
