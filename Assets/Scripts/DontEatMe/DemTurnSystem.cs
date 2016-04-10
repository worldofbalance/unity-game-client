
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemTurnSystem : MonoBehaviour {
  
  private  DemBoard board;
  private Dictionary<int, GameObject> activePredators = new Dictionary<int, GameObject>();
  private  Stack<DemTween> tweenList = new Stack<DemTween>();
  private  DemTile nextTile;
  private  DemTile currentTile;
  public  bool turnLock = false;
  private  GameObject predator;
  private DemMain main;
  private GameObject mainObject;
  private DemTweenManager tweenManager;
  DemTile tile;

  void Awake()
  {
    
    mainObject = GameObject.Find ("MainObject");
    board = GameObject.Find ("GameBoard").GetComponent<DemBoard>();
    main = mainObject.GetComponent<DemMain> ();
    tweenManager = mainObject.GetComponent<DemTweenManager> ();

  }
	

  public  void PredatorTurn()
  {

    turnLock = true;
    foreach(KeyValuePair<int, GameObject> predator in activePredators)
    {

      BuildInfo animal = predator.Value.GetComponent<BuildInfo> ();
      int x = animal.GetTile ().GetIdX ();
      int y = animal.GetTile ().GetIdY ();
	    
      currentTile = board.Tiles [x, y].GetComponent<DemTile> ();

      if(x+1 == 9){
          //Do the bounds checking first instead of last
          currentTile.RemoveAnimal (); //remove predator
          Debug.Log("you lost, or whatever, bla bla bla...");
          continue;
      }

      nextTile = board.Tiles [x + 1, y].GetComponent<DemTile> ();


      animal.SetNextTile (nextTile);
      tweenList.Push(new DemTween (predator.Value, nextTile.GetCenter (), 700));

    }

    ProcessTweens ();

  }

  public  bool IsTurnLocked(){
    
    return !turnLock;

  }


  public  void PredatorFinishedMove (GameObject finishedPredator)
  {
    
    BuildInfo animal = finishedPredator.GetComponent<BuildInfo> ();

    if( animal.GetNextTile().resident  != null){
      animal.GetNextTile ().RemoveAnimal();
    }

    animal.AdvanceTile ();
    ProcessTweens ();


  }

  public  void GenerateNewPredators()
  {

    //For Testing
    int random = UnityEngine.Random.Range (0, 5);

    int randomPredator = UnityEngine.Random.Range (0, main.predators.Length);

    GameObject newPredator = main.predators [randomPredator].Create ();
    newPredator.GetComponent<BuildInfo> ().speciesType = 2;

    activePredators.Add (newPredator.GetInstanceID() , newPredator);


    board.AddAnimal(0, random, newPredator );

    tweenList.Clear ();

    turnLock = false;

  }

  void ProcessTweens()
  {
    
    if (tweenList.Count > 0) {
      tweenManager.AddTween (tweenList.Pop ());
    } else {
      GenerateNewPredators ();
    }

  }






}
