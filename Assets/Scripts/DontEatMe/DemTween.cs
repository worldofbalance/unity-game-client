using UnityEngine;
using System.Collections;
using System;

public class DemTween {
  
  private GameObject tweenObject;
  private Vector3 oldPosition;
  private Vector3 newPosition;
  private int tweenTime;
  private DemTurnSystem.PredatorAdvanceCallback callback;
  private float initialTime;
  private float deltaTime;
  private float progress;
  private Transform originalTransform;
  private Vector3 journeyLength;
  private bool complete;


  public DemTween(GameObject _tweenObject , Vector3 _newPosition, int _tweenTime, DemTurnSystem.PredatorAdvanceCallback _callback){
    
    complete = false;
    progress = 0;
    initialTime = Time.time;
    tweenTime = _tweenTime;
    this.tweenObject = _tweenObject;
    originalTransform = this.tweenObject.GetComponent<Transform> ();
    oldPosition = originalTransform.position;
    this.newPosition = _newPosition;
    callback = _callback;

  }

  public void Update(int id){


    if (complete) {
      //Just in case it gets called after completed
      return;
    }

    deltaTime = (Time.time - initialTime) * 1000;
    progress = deltaTime / tweenTime;

    if (progress >= 1) {
      //Mark for deletion
      complete = true;
      DemTweenManager.RemoveTween (id);
      callback();
      return;
    }

    originalTransform.position = Vector3.Lerp(oldPosition, newPosition, progress);

  }





}
