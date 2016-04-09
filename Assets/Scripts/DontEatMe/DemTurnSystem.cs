using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DemTurnSystem : MonoBehaviour {
  
	private static DemBoard board = GameObject.Find ("GameBoard").GetComponent<DemBoard>();
  public static List<GameObject> activePredators = new List<GameObject>();
  private static DemTile nextTile;
  private static DemTile currentTile;
  public static bool turnLock = false;
  private static GameObject predator;


  DemTile tile;

  void Start(){
    board = GameObject.Find ("GameBoard").GetComponent<DemBoard>();
  }
	

  public static void PredatorTurn(){

    turnLock = true;
    for(int i = activePredators.Count - 1; i >=0 ; i--){
      
    //foreach (GameObject predator in activePredators) {
      predator = activePredators[i];

      BuildInfo animal = predator.GetComponent<BuildInfo> ();
      int x = animal.GetTile ().GetIdX ();
      int y = animal.GetTile ().GetIdY ();
	
      if(x+1 == 9){
          //Do the bounds checking first instead of last
          currentTile.RemoveAnimal (); //remove predator
          Debug.Log("you lost, or whatever, bla bla bla...");
          continue;
      }

      nextTile = board.Tiles [x + 1, y].GetComponent<DemTile> ();
      currentTile = board.Tiles [x, y].GetComponent<DemTile> ();

		  //Check if next location to left is open
      if (nextTile.GetResident() == null) {

				nextTile.AddAnimal (predator);
        currentTile.SetResident (null);
        animal.SetTile (nextTile);


			}		//Check if next Location to left is plant, if it is , destroy it
			else if (nextTile.resident.GetComponent<BuildInfo> ().isPlant ()) {
				nextTile.RemoveAnimal ();	//remove plant from tile
				nextTile.AddAnimal (predator);
        currentTile.SetResident (null);
        animal.SetTile (nextTile);

			}		//Check if next Location to left is prey, if it is , eat it
			else if (nextTile.resident.GetComponent<BuildInfo> ().isPrey ()) {
				nextTile.RemoveAnimal ();	//remove prey from tile
				currentTile.RemoveAnimal ();	//remove predator from tile, predator distroyed, but predator reference is still in list
        activePredators.RemoveAt(i);


			} 
		
			
    }


    //For Testing
    int random = Random.Range (0, 5);

    int randomPredator = Random.Range (0, DemMain.predators.Length);

    GameObject newPredator = DemMain.predators [randomPredator].Create ();
	  newPredator.GetComponent<BuildInfo> ().speciesType = 2;

    activePredators.Add (newPredator);


    board.AddAnimal(0, random, newPredator );

    turnLock = false;


  }




}
