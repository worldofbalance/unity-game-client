﻿using UnityEngine;
using System.Collections;
using System;

public class DemTileTransitionTween :DemTween{

  public DemTileTransitionTween(GameObject _tweenObject , Vector3 _newPosition, int _tweenTime): base(_tweenObject , _tweenTime){

    oldPosition = objectTransform.position;
    newPosition = _newPosition;

  }

  public override void Update(int id){


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

    objectTransform.position = Vector3.Lerp(oldPosition, newPosition, progress);

  }




}