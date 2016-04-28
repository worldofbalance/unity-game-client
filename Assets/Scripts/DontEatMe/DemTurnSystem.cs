
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemTurnSystem : MonoBehaviour {
  
  private  DemBoard board;
  private Dictionary<int, GameObject> activePredators;// = new Dictionary<int, GameObject>();
  private  Queue<DemTween> tweenList = new Queue<DemTween>();
  private  DemTile nextTile;
  private  DemTile currentTile;
  public  bool turnLock = false;
  //private  GameObject predator;
  private DemMain main;
  private GameObject mainObject;
  private DemTweenManager tweenManager;
  DemTile tile;
  private int lives;
  private BuildMenu buildMenu;


  void Awake()
  {
    
    mainObject = GameObject.Find ("MainObject");
    board = GameObject.Find ("GameBoard").GetComponent<DemBoard>();
    main = mainObject.GetComponent<DemMain> ();
    tweenManager = mainObject.GetComponent<DemTweenManager> ();
    buildMenu = mainObject.GetComponent<BuildMenu> ();
    lives = 3;
    buildMenu.UpdateLives (lives);

  }
	

  public  void PredatorTurn()
  {

    turnLock = true;
    buildMenu.ToggleButtonLocks ();
    activePredators = board.GetPredators ();
    Debug.Log ("Total predators :" + activePredators.Count);
    foreach(KeyValuePair<int, GameObject> predator in activePredators)
    {

      BuildInfo animal = predator.Value.GetComponent<BuildInfo> ();
      int x = animal.GetTile ().GetIdX ();
      int y = animal.GetTile ().GetIdY ();
	    
      currentTile = board.Tiles [x, y].GetComponent<DemTile> ();

      if (x + 1 == 9) {
        //Do the bounds checking first instead of last
        //currentTile.RemoveAnimal (); //remove predator
          
        //Debug.Log ("you lost, or whatever, bla bla bla...");
        Debug.Log("arrived at final tile");
        tweenList.Enqueue (new DemPredatorExitTween (predator.Value,  500));
        //continue;

      } else {
        

        nextTile = board.Tiles [x + 1, y].GetComponent<DemTile> ();


        animal.SetNextTile (nextTile);
        tweenList.Enqueue (new DemTileTransitionTween (predator.Value, nextTile.GetCenter (), 500));
      }

    }

    GenerateNewPredators ();
    ProcessTweens ();

  }

  public  bool IsTurnLocked(){
    
    return !turnLock;

  }


  public  void PredatorFinishedMove (GameObject finishedPredator)
  {
    
    BuildInfo predator = finishedPredator.GetComponent<BuildInfo> ();
    Boolean markForDeletion = false;

    if( predator.GetNextTile().resident  != null){
      
      BuildInfo nextAnimal = predator.GetNextTile ().resident.GetComponent<BuildInfo> ();

      if(nextAnimal.isPrey() || nextAnimal.isPlant()){
        predator.GetNextTile ().RemoveAnimal();
      }

      if(nextAnimal.isPrey()){
        markForDeletion = true;
      }

    }



    predator.AdvanceTile ();

    if(markForDeletion){

      DemTile tempTile = predator.GetTile ();
      activePredators.Remove(finishedPredator.GetInstanceID());
      tempTile.RemoveAnimal ();

    }
      
    ProcessTweens ();


  }


  public void PredatorExit(GameObject finishedPredator){

    BuildInfo predator = finishedPredator.GetComponent<BuildInfo> ();
    predator.GetTile ().RemoveAnimal ();
    lives--;
    buildMenu.UpdateLives (lives);

    if (lives == 0) {
      
      GameOver ();
    
    } else {

      activePredators.Remove(finishedPredator.GetInstanceID());
      ProcessTweens ();

    }

  }

  public  void GenerateNewPredators()
  {

    //For Testing
    int random = UnityEngine.Random.Range (0, 5);

    int randomPredator = UnityEngine.Random.Range (0, main.predators.Length);

    GameObject newPredator = main.predators [randomPredator].Create ();
    newPredator.GetComponent<BuildInfo> ().speciesType = 2;

    //activePredators.Add (newPredator.GetInstanceID() , newPredator);

    board.AddNewPredator(0, random, newPredator );

    tweenList.Enqueue(new DemPredatorEnterTween (newPredator, 700));

    //tweenList.Clear ();

    //turnLock = false;

  }

  public void ProcessTweens()
  {
    
    if (tweenList.Count > 0) {
      tweenManager.AddTween (tweenList.Dequeue ());
    } else {
      //GenerateNewPredators ();
      tweenList.Clear();
      turnLock = false;
      buildMenu.ToggleButtonLocks ();
    }

  }

  public int GetLives(){
    return lives;
  }



  public void GameOver(){
    turnLock = true;
    Debug.Log ("game Over");
  }


}
