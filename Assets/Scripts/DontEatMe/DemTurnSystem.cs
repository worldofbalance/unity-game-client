using UnityEngine;
using System.Collections;

public class DemTurnSystem : MonoBehaviour {

	

  public static void PredatorTurn(){
    //For Testing
    int random = Random.Range (0, 4);

    int randomPredator = Random.Range (0, DemMain.predators.Length);


    //DemBoard.AddAnimal (0, 2, DemMain.predators[0].Create() );
    DemBoard board = GameObject.Find ("GameBoard").GetComponent<DemBoard>() as DemBoard;
    //Debug.Log ();
    board.Tiles [0, 2].GetComponent<DemTile> ().resident = DemMain.predators [0].Create ();
    //board.Tiles [x, y].GetComponent<DemTile> ().AddAnimal (animal);
  
  }




}
