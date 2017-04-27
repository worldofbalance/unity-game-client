using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting;
using System;
using UnityEngine.UI;

public class DemBoard : MonoBehaviour {
  public GameObject[,] Tiles;
  /** Number of tile rows on board */
  public int numRows;
  /** Number of tile columns on board */
  public int numColumns;

  private float rightEdge ;

  private float bottomEdge;

  private Material grass1;
  private Material grass2;

  private GameObject mainObject;

  public GameObject gameBoard;

  private Color highlightColor1, highlightColor2; // Highlight colors for available tile pulse

  private DemMain main;

    private DemTile hoveredTile; // Tile with current OnMouseEnter capture
    /** Public modifiers for hoveredTile */
    public DemTile HoveredTile
    { 
        get { return hoveredTile; }
        set { hoveredTile = value; }
    }

	// Use this for initialization
	void Awake () {
    numRows = 5;
    numColumns = 9;
    Tiles = new GameObject[numColumns, numRows];

    grass1 = (Material)Resources.Load("DontEatMe/Materials/tile_1_border", typeof(Material));
    grass2 = (Material)Resources.Load("DontEatMe/Materials/tile_2_border", typeof(Material));

    mainObject = GameObject.Find ("MainObject");
    main = mainObject.GetComponent<DemMain> ();

    gameBoard = GameObject.Find("GameBoard");
    Debug.Log (gameBoard.transform);

    // Calculate the right edge of the screen based on the aspect ratio 
    rightEdge = Camera.main.orthographicSize * Screen.width / Screen.height;


    // Calculate the bottom edge of the screen based on the aspect ratio
    bottomEdge = 0 - Camera.main.orthographicSize;

    // Define highlight colors
    highlightColor1 = Color.white;
    highlightColor2 = Color.Lerp(Color.blue, Color.white, 0.25f);

    hoveredTile = null;

	}
	
    public void Add (int x, int y)
    {
        Tiles[x,y] = GameObject.CreatePrimitive(PrimitiveType.Quad);

        Tiles[x, y].transform.parent = gameBoard.transform;

        Tiles[x, y].transform.position = new Vector3(rightEdge - 1 - x, bottomEdge + 1 + y, -1);

        Tiles[x, y].name = x + "," + y;

        if ((x % 2) == (y % 2))
        {
            Tiles[x, y].GetComponent<Renderer>().material = grass1;

        }
        else
        {
            Tiles[x, y].GetComponent<Renderer>().material = grass2;
        }

        Tiles[x, y].AddComponent<DemTile>(); // Add the DemTile script to each cube
    }

    /**
        Determines which tiles are available for building.
    */
    public void SetAvailableTiles ()
    {
        // Determine next tile
        /*
            FORMAT: 5 rows (y-axis), 9 columns (x-axis):

              8 7 6 5 4 3 2 1 0
            4 . . . . . . . . . 4
            3 . . . . . . . . . 3
            2 . . . . . . . . . 2
            1 . . . . . . . . . 1
            0 . . . . . . . . . 0
              8 7 6 5 4 3 2 1 0 
        */
        for (int x = 1; x < 9; x++)
        {
            for (int y = 0; y < 5; y++)
            {
                // Define DemTile component for simple access
                DemTile tile = Tiles[x,y].GetComponent<DemTile>();

                // If building a plant
                if (main.currentSelection.GetComponent<BuildInfo>().isPlant())
                {
                    // If tile is clear
                    if (!tile.resident)
                    {   
                        // Trigger highlight pulse and set as available
                        tile.SetPulse(highlightColor1, highlightColor2);
                        tile.SetRestorePulse(highlightColor1, highlightColor2);
                        tile.SignalPulse(true);
                        tile.available = true;
                    }
                }

                // If building a prey
                else
                {
                    if (tile.hasPlant())
                    {
                        // Get range data for the plant and iterate for available tiles
                        int[][] range = SpeciesConstants.Range(tile.resident.GetComponent<BuildInfo>().name);

                        foreach (int[] coord in range)
                        {                        
                            // Range must not extend past grid bounds
                            if (x + coord[0] < 0 || x + coord[0] > 8 || y + coord[1] < 0 || y + coord[1] > 4)
                                continue;

                            // Define tile
                            tile = Tiles[x + coord[0], y + coord[1]].GetComponent<DemTile>();

                            // Tile must be free
                            if (!tile.resident)
                            {
                                // Set the pulse for each tile
                                tile.SetPulse(highlightColor1, highlightColor2);
                                tile.SetRestorePulse(highlightColor1, highlightColor2);
                                tile.SignalPulse(true);

                                // Set availability
                                tile.available = true;
                            }
                        }
                    }
                }
            }
        }
    }

  public void ClearAvailableTiles(){
    for (int x = 0; x < 9; x++){

      for (int y = 0; y < 5; y++){
        Tiles [x, y].GetComponent<Renderer> ().material.color = Color.white;
        Tiles [x, y].GetComponent<DemTile> ().currentColor = Color.white;
        DemTile tile = Tiles [x, y].GetComponent<DemTile>();
        tile.SignalPulse(false);
        tile.available = false;
      }

    }
    
  }



  public void AddAnimal(int x , int y, GameObject animal){

    Tiles [x, y].GetComponent<DemTile> ().AddAnimal(animal);

  }


  public void AddNewPredator(int x , int y, GameObject animal){

    Tiles [x, y].GetComponent<DemTile> ().AddNewPredator(animal);

  }

    public Dictionary<int, GameObject> GetPredators()
    {
        Dictionary<int, GameObject> activePredators = new Dictionary<int, GameObject>();
        for (int x = numColumns - 1; x >= 0; x--)
        {
            for (int y = 0; y < numRows; y++)
            {
                if (Tiles[x, y].GetComponent<DemTile>().ResidentIsPredator())
                {
                    try
                    {
                        activePredators.Add (Tiles [x, y].GetComponent<DemTile> ().resident.GetInstanceID (), Tiles [x, y].GetComponent<DemTile> ().resident);
                    }
                    catch (NullReferenceException e) { Debug.Log("resident is NULL @ GetPredators()"); }
                }
            }
        }
        return activePredators;
    }
}
