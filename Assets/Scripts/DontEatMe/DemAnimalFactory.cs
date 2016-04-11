using UnityEngine;
using System.Collections;
//using UnityEditor; // Contains the PrefabUtility class.

public class DemAnimalFactory
{


    // private SpeciesData speciesData;
    private string speciesName;

    private int speciesId;

    private Sprite[] image;

    // For OnGui, remove when OnGui is removed
    private Texture img;

    public DemAnimalFactory(string _speciesName)
    {

        this.speciesId = 0; //When SpeciesData is available then load it
        this.speciesName = _speciesName;
        this.image = Resources.LoadAll<Sprite>("DontEatMe/Sprites/" + this.speciesName);
        this.img = Resources.Load<Texture>("Textures/Species/" + this.speciesName);

    }

    public GameObject Create()
    {

        //GameObject animal = PrefabUtility.CreateEmptyPrefab ("./" + name) as GameObject;
        GameObject animal = new GameObject(this.speciesName);
        BuildInfo info = animal.AddComponent<BuildInfo>();
        info.SetParent(animal);
        info.previewImage = Resources.Load("Textures/Species/" + this.speciesName) as Texture;

        // TO USE OnGUI()
        // Uncomment line 42 and comment out line 43

        SpriteRenderer renderer = animal.AddComponent<SpriteRenderer>();
        //renderer.sprite = Sprite.Create(info.previewImage as Texture2D, new Rect(0f, 0f, 256, 256), new Vector2(0.5f, 0.1f));
        renderer.sprite = this.image[0];
        renderer.transform.parent = animal.transform;
        animal.transform.localScale = new Vector3(0.4f, 0.4f, 0);


        return animal;
    }


    public string GetName()
    {
        return this.speciesName;
    }

    public int GetId()
    {
        return this.speciesId;
    }

    public Sprite GetImage()
    {
        return this.image[0];
    }

    // For OnGui, remove when OnGui is removed
    public Texture GetImageForOnGUI()
    {
        return this.img;
    }

}
