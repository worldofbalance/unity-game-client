using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DemTweenManager {

  private static List<DemTween> tweens = new List<DemTween>();


  public static void AddTween(DemTween tween){
    tweens.Add (tween);
  }

  public static void Update(){
    
    for (int i = tweens.Count - 1; i >= 0; i--) {
      
      tweens [i].Update();

    }
    
  }

}
