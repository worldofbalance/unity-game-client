using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DemTurnSystem : MonoBehaviour {
  
	private static DemBoard board = GameObject.Find ("GameBoard").GetComponent<DemBoard>();
    public static List<GameObject> activePredators = new List<GameObject>();
	public static List<GameObject> tempPredators = new List<GameObject> ();


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
		if (board.Tiles [x + 1, y].GetComponent<DemTile> ().resident == null) {

			board.Tiles [x + 1, y].GetComponent<DemTile> ().AddAnimal (predator);
			board.Tiles [x, y].GetComponent<DemTile> ().resident = null;
			animal.tile = board.Tiles [x + 1, y].GetComponent<DemTile> ();
			tempPredators.Add (predator);	//add survivied predator to temp list; the predator didn't eat prey

		}		//Check if next Location to left is prey, if it is , eat it
		else if (board.Tiles [x + 1, y].GetComponent<DemTile> ().resident.GetComponent<BuildInfo> ().isPrey ()) {
			board.Tiles [x + 1, y].GetComponent<DemTile> ().RemoveAnimal ();	//remove prey from tile
			board.Tiles [x, y].GetComponent<DemTile> ().RemoveAnimal ();	//remove predator from tile, predator distroyed, but predator reference is still in list

		} else {//leftover case, next location is predator
			tempPredators.Add (predator);	//add survived predator to temp list
		}

    }

	//swap list
		activePredators.Clear();	//remove all the predator, clear the predator reference
		List <GameObject> temp = activePredators;
		activePredators = tempPredators;	//the survived predator set to active
		tempPredators = temp;


    //For Testing
    int random = Random.Range (0, 5);

    int randomPredator = Random.Range (0, DemMain.predators.Length);

    GameObject newPredator = DemMain.predators [randomPredator].Create ();
	newPredator.GetComponent<BuildInfo> ().speciesType = 2;

    activePredators.Add (newPredator);


    board.AddAnimal(0, random, newPredator );


  }




}
