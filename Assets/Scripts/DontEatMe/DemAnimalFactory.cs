using UnityEngine;
using System.Collections;
using UnityEditor; // Contains the PrefabUtility class.
using System;

/**
	
*/
public class DemAnimalFactory
{
    // private SpeciesData speciesData;
    private string speciesName; // Species name
    private int speciesId; // Unique species ID (consistent with database)
    private short speciesType; // Species type (NOT consistent with database)

    private Sprite[] image;

    // For OnGui, remove when OnGui is removed
    private Texture img;

    /**
    	Default constructor.
    	Any additional data can be automatically parsed using the _speciesName parameter.
    */
    public DemAnimalFactory(string _speciesName)
    {
        this.speciesName = _speciesName;
        this.speciesId = SpeciesConstants.SpeciesID(this.speciesName);
        this.speciesType = SpeciesConstants.SpeciesType(this.speciesId);
        this.image = Resources.LoadAll<Sprite>("DontEatMe/Sprites/" + this.speciesName);
        this.img = Resources.Load<Texture>("Textures/Species/" + this.speciesName);
    }

    public bool isPlant(){
      return speciesType == 0;
    }

    public bool isPrey(){
      return speciesType == 1;
    }

    public bool isPredator(){

      return speciesType == 2;

    }


    /**
    	Returns a GameObject instance as defined by this factory.
    */
    public GameObject Create()
    {
        //GameObject animal = PrefabUtility.CreateEmptyPrefab ("./" + name) as GameObject;
        GameObject animal = new GameObject(this.speciesName);

        // Add appropriate BuildInfo component type
        BuildInfo info;
        switch (this.speciesType)
        {
        	// Prey
        case 1:
				  info = animal.AddComponent<PreyInfo>().Initialize(this.speciesId);
				  break;
        	// Predator
        case 2:
        		info = animal.AddComponent<PredatorInfo>().Initialize(this.speciesId);
        		break;
        	// Plant --> case 0 --> may need a PlantInfo class later (?)
        	// Omnivore --> case ? --> some prey may also be predators, so an OmnivoreInfo class would accommodate this (?)
        	// Default
        	default:
        		info = animal.AddComponent<BuildInfo>();
        		break;
        }
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

    /**
    	Returns the current species name.
    */
    public string GetName()
    {
        return this.speciesName;
    }

    /**
    	Returns the current species ID.
    */
    public int GetId()
    {
        return this.speciesId;;
    }

    /**
    	Returns the current species type.
    	1 = prey, 2 = predator.
    */
    public short GetType ()
    {
    	return this.speciesType;
    }

    /**
    	Returns the current Sprite object.
    */
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
