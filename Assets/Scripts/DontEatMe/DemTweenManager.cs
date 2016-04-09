using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DemTweenManager : MonoBehaviour{

  private static List<DemTween> tweens = new List<DemTween>();


  public static void AddTween(DemTween tween){
    tweens.Add (tween);
  }

  public static void Update(){
    Debug.Log ("Active Tweens : " + tweens.Count);

    if (tweens.Count == 0) {
      return;
    }
      
    
    for (int i = tweens.Count - 1; i >= 0; i--) {
      
      tweens [i].Update(i);

    }
    
  }

  public static void RemoveTween(int index){
    tweens.RemoveAt (index);
  }




}
