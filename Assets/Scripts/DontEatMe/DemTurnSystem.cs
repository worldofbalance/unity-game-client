using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DemTurnSystem : MonoBehaviour {
  
  private static DemBoard board; //= GameObject.Find ("GameBoard").GetComponent<DemBoard>();
  public static List<GameObject> activePredators = new List<GameObject>();


  DemTile tile;

  void Start(){
    board = GameObject.Find ("GameBoard").GetComponent<DemBoard>();
  }
	

  public static void PredatorTurn(){

    foreach (GameObject predator in activePredators) {

      BuildInfo animal = predator.GetComponent<BuildInfo> ();
      DemTile tile = animal.tile;
      int x = tile.idX;
      int y = tile.idY;

      //Check if next location to left is open
      if( board.Tiles[x+1 , y].GetComponent<DemTile>().resident == null ){

        board.Tiles [x + 1, y].GetComponent<DemTile> ().AddAnimal (predator);
        board.Tiles [x , y].GetComponent<DemTile> ().resident = null;
        animal.tile = board.Tiles [x + 1, y].GetComponent<DemTile> ();

      }

    }

    //For Testing
    int random = Random.Range (0, 4);

    int randomPredator = Random.Range (0, DemMain.predators.Length);

    GameObject newPredator = DemMain.predators [randomPredator].Create ();

    activePredators.Add (newPredator);


    board.AddAnimal(0, random, newPredator );


  }




}
