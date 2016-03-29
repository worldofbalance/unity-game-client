using UnityEngine;
using System.Collections;

public class DemAnimalFactory{

  public static GameObject Create(string name, int speciesId, int price){
    
    GameObject animal = new GameObject (name);
    BuildInfo info = animal.AddComponent<BuildInfo> ();
    info.speciesId = speciesId;
    info.price = price;

    return animal;
  }
}
