using UnityEngine;
using System.Collections;
using System;

public class DemTween {
  
  private GameObject tweenObject;
  private Vector3 oldPosition;
  private Vector3 newPosition;
  private int tweenTime;
  private Delegate callback;

  public DemTween(GameObject tweenObject , Vector3 newPosition, int tweenTime, Delegate callback){
    
    this.tweenObject = tweenObject;
    //oldPosition = this.tweenObject.GetComponent<Transform> ();
    this.newPosition = newPosition;
    this.callback = callback;

  }

  public void Update(){
  }


}
