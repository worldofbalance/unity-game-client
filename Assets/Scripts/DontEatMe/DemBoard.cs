using UnityEngine;
using System.Collections;

public class DemBoard : MonoBehaviour {
  public GameObject[,] Tiles = new GameObject[9 , 5];

  private float rightEdge ;

  private float bottomEdge;

  private Material grass1;
  private Material grass2;

  private GameObject mainObject;

  private GameObject gameBoard;

  private Color highlightColor;

  private DemMain main;

	// Use this for initialization
	void Awake () {

    grass1 = (Material)Resources.Load("DontEatMe/Materials/tile_1", typeof(Material));
    grass2 = (Material)Resources.Load("DontEatMe/Materials/tile_2", typeof(Material));

    mainObject = GameObject.Find ("MainObject");
    main = mainObject.GetComponent<DemMain> ();

    gameBoard = GameObject.Find("GameBoard");

    // Calculate the right edge of the screen based on the aspect ratio 
    rightEdge = Camera.main.orthographicSize * Screen.width / Screen.height;


    // Calculate the bottom edge of the screen based on the aspect ratio
    bottomEdge = 0 - Camera.main.orthographicSize;


    //We need to pick a better color
    highlightColor = new Color(0.0F, 0.0F, 1.0F, 0.1F);
 
	
	}
	
  public void Add(int x, int y){

    Tiles[x, y] = GameObject.CreatePrimitive(PrimitiveType.Cube);
    //cube.tag = "Tile"; // Add a "Tile" tag to each cube
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

  public void SetAvailableTiles(){
      
    for (int x = 0; x < 9; x++){
      for (int y = 0; y < 5; y++){

		if (main.currentSelection.GetComponent<BuildInfo>().isPlant()) {
          
          if (!Tiles [x, y].GetComponent<DemTile> ().resident) {
            Tiles [x, y].GetComponent<Renderer> ().material.color = highlightColor;
            Tiles [x, y].GetComponent<DemTile> ().currentColor = highlightColor;
            Tiles [x, y].GetComponent<DemTile> ().available = true;
          }

        } else {
          
            if(Tiles [x, y].GetComponent<DemTile> ().hasPlant()){
              
                       if (x - 1 >= 0) {
                
                          if (!Tiles [x - 1, y].GetComponent<DemTile> ().resident) {
                            Tiles [x-1, y].GetComponent<Renderer> ().material.color = highlightColor;
                            Tiles [x-1, y].GetComponent<DemTile> ().currentColor = highlightColor;
                            Tiles [x-1, y].GetComponent<DemTile> ().available = true;


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
        Tiles [x, y].GetComponent<DemTile> ().available = false;

      }

    }
    
  }




  public void AddAnimal(int x , int y, GameObject animal){

    Tiles [x, y].GetComponent<DemTile> ().AddAnimal(animal);

  }



}
