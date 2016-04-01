using UnityEngine;
using System.Collections;

public class DemMain : MonoBehaviour
{

	public static GameObject currentSelection;

  public static GameObject boardGroup;

  public static DemBoard boardController;

	static Vector3 buildOrigin;

  public static DemAnimalFactory[] predators;



    // Use this for initialization
    void Awake()
    {


      //Pick predators

    
    	currentSelection = null;

      predators = new DemAnimalFactory[2];
      predators [0] = new DemAnimalFactory ("Aardvark"); 
      predators [1] =  new DemAnimalFactory ("African Marsh Owl"); 


      




 

        // Setup Play board
        boardGroup = new GameObject("GameBoard");

        //Keep track of our tiles
        boardController = boardGroup.AddComponent<DemBoard> ();

 

        for (int x = 0; x < 9; x++)
        {

            for (int y = 0; y < 5; y++)
            {
                boardController.Add (x, y );

            }

        }
        
        

       


        //Now aligning the background so that it is always in the view port, and scaled as best it can

        GameObject background = GameObject.Find("DemBackGround");
        float bgRight = background.GetComponent<BoxCollider>().bounds.max.x;
        float bgLeft = background.GetComponent<BoxCollider>().bounds.min.x;
        float bgClose = background.GetComponent<BoxCollider>().bounds.min.z;


        float bgDistance = Vector3.Distance(background.transform.position, Camera.main.transform.position);
        Vector3 viewTopMiddleBg = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0f, bgDistance));
        Vector3 viewBottomMiddleBg = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 1f, bgDistance));

        Vector3 viewLeftMiddleBg = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0.5f, bgDistance));
        Vector3 viewRightMiddleBg = Camera.main.ViewportToWorldPoint(new Vector3(1f, 0.5f, bgDistance));


        float bgRatio = 526f / 350f;
        Vector3 scale = viewTopMiddleBg - viewBottomMiddleBg;
        Vector3 scaleX = viewLeftMiddleBg - viewRightMiddleBg;
        //set a min for x scale
        if (-scaleX.x < 14f)
        {
            scaleX.x = -14f;
        }

        scale.z = background.transform.localScale.z;
        scale.x = -scaleX.x;
        scale.y = -scale.y;
        background.transform.localScale = scale;


        //TESTING
        //DemTurnSystem.PredatorTurn();



    }

    // Update is called once per frame
    void Update()
    {
    	// If a species is currently selected for building, update its position to the cursor
    if (BuildMenu.currentAnimalFactory != null) {
    		if (currentSelection) {
          
    			Vector3 world_pos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y));
    			world_pos.z = -1.5f;
    			currentSelection.transform.position = world_pos;
 
    		}

    		// Cancel currently selected species on Escape key press
    		if (Input.GetKeyDown(KeyCode.Escape)) {
          BuildMenu.currentAnimalFactory = null;
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
    public static void setBuildOrigin (Vector3 origin) {
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
