using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DemTweenManager : MonoBehaviour{

  private List<DemTween> tweens = new List<DemTween>();


  public void AddTween(DemTween tween){
    tweens.Add (tween);
    tween.Start ();
  }

  public void Update(){
    

    if (tweens.Count == 0) {
      return;
    }
      
    
    for (int i = tweens.Count - 1; i >= 0; i--) {
      
      tweens [i].Update(i);
     
    }
    
  }

  public void RemoveTween(int index){
    tweens.RemoveAt (index);
  }




}
