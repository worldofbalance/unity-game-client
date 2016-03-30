using UnityEngine;
using System.Collections;
using UnityEditor; // Contains the PrefabUtility class.

public class DemAnimalFactory{

  public static GameObject Create(string name, int speciesId, int price){

    //GameObject animal = PrefabUtility.CreateEmptyPrefab ("./" + name) as GameObject;
    GameObject animal = new GameObject (name);
    BuildInfo info = animal.AddComponent<BuildInfo> ();
    info.speciesId = speciesId;
    info.price = price; 
    info.previewImage = Resources.Load ("Textures/Species/" + name) as Texture;


    SpriteRenderer renderer = animal.AddComponent<SpriteRenderer> ();
    renderer.sprite = Sprite.Create( info.previewImage as Texture2D, new Rect(0f, 0f, 256, 256), new Vector2(0.5f, 0.1f));
    renderer.transform.parent = animal.transform;
    animal.transform.localScale = new Vector3(0.4F, 0.4f, 0);

    //renderer.sprite.pivot = SpriteAlignment.Center;
    //renderer.bounds.size.x = 1;
    //renderer.bounds.size.y = 1;

 

   

    return animal;
  }
}
