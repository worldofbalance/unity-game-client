using UnityEngine;
using System.Collections;
using System;

public class DemTween :Component{
  
  private GameObject tweenObject;
  private Vector3 oldPosition;
  private Vector3 newPosition;
  private int tweenTime;
  private float initialTime;
  private float deltaTime;
  private float progress;
  private Transform originalTransform;
  private Vector3 journeyLength;
  private bool complete;
  public bool running;
  private DemTurnSystem turnSystem;
  private  GameObject mainObject;
  private DemMain main;
  private DemTweenManager tweenManager;

  public DemTween(GameObject _tweenObject , Vector3 _newPosition, int _tweenTime){

    mainObject = GameObject.Find ("MainObject");
    main = mainObject.GetComponent<DemMain> ();
    tweenManager = mainObject.GetComponent<DemTweenManager> ();
    turnSystem = mainObject.GetComponent<DemTurnSystem> ();

    running = false;
    complete = false;
    progress = 0;
    tweenTime = _tweenTime;
    this.tweenObject = _tweenObject;
    originalTransform = this.tweenObject.GetComponent<Transform> ();
    oldPosition = originalTransform.position;
    this.newPosition = _newPosition;

  }

  public void Start(){
    
    initialTime = Time.time;
    running = true;


  }

  public void Update(int id){


    if (complete || !running) {
      //Just in case it gets called after completed
      return;
    }

    deltaTime = (Time.time - initialTime) * 1000;
    progress = deltaTime / tweenTime;

    if (progress >= 1) {
      //Mark for deletion
      complete = true;
      tweenManager.RemoveTween (id);
      turnSystem.PredatorFinishedMove (tweenObject);
      return;
    }

    originalTransform.position = Vector3.Lerp(oldPosition, newPosition, progress);

  }





}
