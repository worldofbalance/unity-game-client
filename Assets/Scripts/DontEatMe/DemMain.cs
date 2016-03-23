using UnityEngine;
using System.Collections;

public class DemMain : MonoBehaviour {


	// Use this for initialization
	void Start () {


    Material grass1 = (Material)Resources.Load("DontEatMe/Materials/tile_1", typeof(Material));
    Material grass2 = (Material)Resources.Load("DontEatMe/Materials/tile_2", typeof(Material));




    //Setup Play board
    GameObject board = new GameObject("GameBoard");
  

    for(int x = 0; x < 9; x++){
      
      for (int y = 0; y < 5; y++) {

        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.parent = board.transform;
        cube.transform.position = new Vector3(x-2, y-2, -1);
       
        cube.name = x + "," + y;

        if ((x % 2) == (y % 2)) {
          cube.GetComponent<Renderer> ().material = grass1;

        } else {
          cube.GetComponent<Renderer> ().material = grass2;
        }


      }
        
    }


    //Adjusting camera now

    //Get the highest object needed for game
    float bottom = GameObject.Find ("0,0").GetComponent<BoxCollider> ().bounds.min.y;

    //Get lowest object needed for game
    float top = GameObject.Find ("0,4").GetComponent<BoxCollider> ().bounds.max.y;

    //Horizontally allign the camera
    float horizontalCenter = (top + bottom) / 2;
    Vector3 newPosition = Camera.main.transform.position;
    newPosition.y = horizontalCenter;
    Camera.main.transform.position = newPosition;

	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
