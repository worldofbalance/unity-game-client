using UnityEngine;
using System.Collections;

public class DemAnimalFactory{

  public static GameObject Create(string name, int speciesId, int price){
    
    GameObject animal = new GameObject (name);
    BuildInfo info = animal.AddComponent<BuildInfo> ();
    info.speciesId = speciesId;
    info.price = price;
    info.previewImage = Resources.Load ("Textures/Species/" + name) as Texture;

    SpriteRenderer renderer = animal.AddComponent<SpriteRenderer> ();
    renderer.sprite = Resources.Load ("Sprites/Plants/" + name.Trim()) as Sprite;

   

    return animal;
  }
}
