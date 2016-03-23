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
        //board = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //DemTile tile = DemTile.Create(x,y,0);
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.parent = board.transform;
        cube.transform.position = new Vector3(x, y, -1);

        if ((x % 2) == (y % 2)) {
          cube.GetComponent<Renderer> ().material = grass1;

        } else {
          cube.GetComponent<Renderer> ().material = grass2;
        }


      }
        
    }
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
