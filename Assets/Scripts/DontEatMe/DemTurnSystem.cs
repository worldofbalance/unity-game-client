using UnityEngine;
using System.Collections;

public class DemTurnSystem : MonoBehaviour {
  private static DemBoard board; //= GameObject.Find ("GameBoard").GetComponent<DemBoard>();
  DemTile tile;

  void Start(){
    board = GameObject.Find ("GameBoard").GetComponent<DemBoard>();
  }
	

  public static void PredatorTurn(){
    //For Testing
    int random = Random.Range (0, 4);

    int randomPredator = Random.Range (0, DemMain.predators.Length);


    board.AddAnimal(0, 0, DemMain.predators[0].Create() );


  }




}
