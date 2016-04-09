using UnityEngine;
using System.Collections;
using UnityEditor; // Contains the PrefabUtility class.

public class DemAnimalFactory{
  

  //private SpeciesData speciesData;
  private string speciesName;

  private int speciesId;

  private Texture image;

  public DemAnimalFactory(string _speciesName){
    
    this.speciesId = 0; //When SpeciesData is available then load it
    this.speciesName = _speciesName;
    this.image = Resources.Load ("Textures/Species/" + this.speciesName) as Texture;
    
  }

  public GameObject Create(){

    //GameObject animal = PrefabUtility.CreateEmptyPrefab ("./" + name) as GameObject;
    GameObject animal = new GameObject (this.speciesName);
    BuildInfo info = animal.AddComponent<BuildInfo> ();
    info.SetParent (animal);
    info.previewImage = Resources.Load ("Textures/Species/" + this.speciesName) as Texture;


    SpriteRenderer renderer = animal.AddComponent<SpriteRenderer> ();
    renderer.sprite = Sprite.Create( info.previewImage as Texture2D, new Rect(0f, 0f, 256, 256), new Vector2(0.5f, 0.1f));
    renderer.transform.parent = animal.transform;
    animal.transform.localScale = new Vector3(0.4F, 0.4f, 0);


    return animal;
  }


  public string GetName(){
    return this.speciesName;
  }

  public int GetId(){
    return this.speciesId;
  }

  public Texture GetImage(){
    return this.image;
  }

}
