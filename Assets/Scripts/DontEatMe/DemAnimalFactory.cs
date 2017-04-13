using UnityEngine;
using System.Collections;

//using UnityEditor; // Contains the PrefabUtility class.
using System;

/**
	TODO: documentation
*/
public class DemAnimalFactory
{
    // private SpeciesData speciesData;
    private string speciesName;
    // Species name
    private int speciesId;
    // Unique species ID (consistent with database)
    private short speciesType;
    // Species type (NOT consistent with database)

    private Sprite[] image;

    // For OnGui, remove when OnGui is removed
    private Texture img;

    /**
    	Default constructor.
    	Any additional data can be automatically parsed using the _speciesName parameter.

        @param  _speciesName    a string containing a species name (must match with database!)
    */
    public DemAnimalFactory (string _speciesName)
    {
        this.speciesName = _speciesName;
        this.speciesId = SpeciesConstants.SpeciesID(this.speciesName);
        this.speciesType = SpeciesConstants.SpeciesType(this.speciesId);
        this.image = Resources.LoadAll<Sprite>("DontEatMe/Sprites/" + this.speciesName);
        this.img = Resources.Load<Texture>("Textures/Species/" + this.speciesName);
    }

    public bool isPlant()
    {
        return speciesType == 0;
    }

    public bool isPrey()
    {
        return speciesType == 1;
    }

    public bool isPredator()
    {

        return speciesType == 2;

    }


    /**
    	Returns a GameObject instance as defined by this factory.
     The GameObject represents a species instance of one of the following types, each attached a relevant BuildInfo script:
     - Plant (BuildInfo)
     - Prey (PreyInfo)
     - Predator (PredatorInfo)
                

     @return a GameObject representing a species instance      
    */
    public GameObject Create ()
    {
        // Create a new animal using the factory's current settings
        GameObject animal = new GameObject(this.speciesName);

        // Add appropriate BuildInfo component type
        switch (this.speciesType)
        {
        	// Prey
            case 1:
                PreyInfo preyinfo = animal.AddComponent<PreyInfo>().Initialize(this.speciesId);
                preyinfo.SetParent(animal);
                preyinfo.previewImage = Resources.Load("Textures/Species/" + this.speciesName) as Texture;
				break;
        	// Predator
		    case 2:
                PredatorInfo predinfo = animal.AddComponent<PredatorInfo>().Initialize(this.speciesId);
                predinfo.SetParent(animal);
                predinfo.previewImage = Resources.Load("Textures/Species/" + this.speciesName) as Texture;
        		break;
        	// Plant --> case 0 --> may need a PlantInfo class later (?)
        	// Omnivore --> case ? --> some prey may also be predators, so an OmnivoreInfo class would accommodate this (?)
        	// Default
        	default:
                BuildInfo info = animal.AddComponent<BuildInfo>();
                info.SetParent(animal);
                info.previewImage = Resources.Load("Textures/Species/" + this.speciesName) as Texture;
        		break;
        }

        // TO USE OnGUI()
        // Uncomment line 42 and comment out line 43

        // Attach a SpriteRenderer and define appropriate settings
        SpriteRenderer renderer = animal.AddComponent<SpriteRenderer>();
        renderer.sprite = this.image[0];
        renderer.transform.parent = animal.transform;
        animal.transform.localScale = new Vector3(0.4f, 0.4f, 0);

		//predator indicator
        // TODO: implement something useful
		//start
		if (this.speciesType == 2) { // if predator
            /*
			Sprite predatorRage = Resources.Load<Sprite>("DontEatMe/PredatorRage");
			GameObject predatorIndicate = new GameObject ("predatorRage");
			predatorIndicate.AddComponent<SpriteRenderer> ();
			predatorIndicate.GetComponent<SpriteRenderer> ().sprite = predatorRage;
			predatorIndicate.transform.SetParent(animal.transform);
			predatorIndicate.transform.localScale = new Vector3(1.6f, 1.6f, 0);
			predatorIndicate.transform.localPosition = new Vector3 (-0.8f, 0.8f, 0);
			predatorIndicate.GetComponent<SpriteRenderer>().sortingOrder = 1;
			Debug.Log (predatorRage);
            */
		}
		//end
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
        return this.speciesId;
        ;
    }

    /**
    	Returns the current species type.
    	1 = prey, 2 = predator.
    */
    public short GetType()
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