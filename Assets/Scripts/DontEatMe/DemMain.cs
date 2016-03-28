using UnityEngine;
using System.Collections;

public class DemMain : MonoBehaviour
{

	public static GameObject currentSelection;

	static Vector3 preyOrigin;


    // Use this for initialization
    void Start()
    {
    	currentSelection = null;


        Material grass1 = (Material)Resources.Load("DontEatMe/Materials/tile_1", typeof(Material));
        Material grass2 = (Material)Resources.Load("DontEatMe/Materials/tile_2", typeof(Material));




        // Setup Play board
        GameObject board = new GameObject("GameBoard");

        // Calculate the right edge of the screen based on the aspect ratio 
        float rightEdge = Camera.main.orthographicSize * Screen.width / Screen.height;

        // Calculate the bottom edge of the screen based on the aspect ratio
        float bottomEdge = 0 - Camera.main.orthographicSize;

        for (int x = 0; x < 9; x++)
        {

            for (int y = 0; y < 5; y++)
            {

                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                //cube.tag = "Tile"; // Add a "Tile" tag to each cube
                cube.transform.parent = board.transform;
                cube.transform.position = new Vector3(rightEdge - 1 - x, bottomEdge + 1 + y, -1);

                cube.name = x + "," + y;
   
                if ((x % 2) == (y % 2))
                {
                    cube.GetComponent<Renderer>().material = grass1;

                }
                else
                {
                    cube.GetComponent<Renderer>().material = grass2;
                }

                cube.AddComponent<DemTile>(); // Add the DemTile script to each cube
            }

        }


        //// Adjusting camera now
        ////Had to put back in for now  

        //// Get the highest object needed for game
        //float bottom = GameObject.Find("0,0").GetComponent<BoxCollider>().bounds.min.y;

        //// Get lowest object needed for game
        //float top = GameObject.Find("0,4").GetComponent<BoxCollider>().bounds.max.y;

        //// Vertically align the camera
        //float verticalCenter = (top + bottom) / 2;
        //Vector3 newPosition = Camera.main.transform.position;
        //newPosition.y = verticalCenter;
        //Camera.main.transform.position = newPosition;





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



    }

    // Update is called once per frame
    void Update()
    {
    	// If a species is currently selected for building, update its position to the cursor
    	if (BuildMenu.currentlyBuilding) {
    		if (currentSelection) {
    			Vector3 world_pos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y));
    			world_pos.z = -1.5f;
    			currentSelection.transform.position = world_pos;
 
    		}

    		// Cancel currently selected species on Escape key press
    		if (Input.GetKeyDown(KeyCode.Escape)) {
    			BuildMenu.currentlyBuilding = null;
    			//Destroy(currentSelection);
    			// DEBUG MESSAGE
    			//Debug.Log("currentlyBuilding reset to 'null', returning object to (" + preyOrigin.x + ", " + preyOrigin.y + ")");

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
    public static void setPreyOrigin (Vector3 origin) {
    	preyOrigin = origin;
    }

    /**
    	Eases a cancelled currentlyBuilding object back to its respective button.
    */
    IEnumerator easeReturn (float easing) {
		float startDistance = Vector3.Distance(preyOrigin, currentSelection.transform.position);
    	while (Vector3.Distance(preyOrigin, currentSelection.transform.position) > startDistance/10) {

			currentSelection.transform.position = Vector3.Lerp
			(
				currentSelection.transform.position,
				preyOrigin,
				easing * Vector3.Distance(preyOrigin, currentSelection.transform.position)
			);
			yield return new WaitForSeconds(0.01f);
    	}
    	Destroy(currentSelection);
    }

}
