using UnityEngine;
using System.Collections;

public class BuildInfo : MonoBehaviour {

	// the menu button sprite
	public Texture previewImage;
    public GameObject parent;

	// Species ID
	// NOTE: this is made public for the time being to allow assignment of species ID to GameObject instances
	public int speciesId;
	public int price; // Build price for an object
	public short speciesType; // Classifies species type: plant =0, prey =1, or preditor =2
    public DemTile tile = null;
    public DemTile nextTile = null;

	// Current Health
	//[SerializeField]
	//int speciesId = 0;

    /**
        Returns the species ID.

        @return an integer
    */
    public int GetSpeciesId () {
		return speciesId;
	}

	// Use this for initialization
	void Start () {
        //parent = GetComponent<Transform> ();
	}
	
	/**
        Returns the resident's tile.

        @return a DemTile instance
    */
    public DemTile GetTile () {
        return tile;
	}

    /**
        Sets the resident's tile.

        @param  newTile a DemTile instance
    */
    public void SetTile (DemTile newTile) {
        tile = newTile;
    }

    /**
        Returns the resident's next tile.

        @return a DemTile instance
    */
    public DemTile GetNextTile () {
        return nextTile;
    }

    /**
        Sets the next tile in the object's movement path.

        @param  newNextTile a DemTile instance
    */
    public void SetNextTile (DemTile newNextTile) {
        nextTile = newNextTile;
    }

    /**
        Advances the animal object to its next tile.
    */
    public void AdvanceTile () {
        if (tile) {
            DemTile oldTile = tile;
            nextTile.AddAnimal (parent);
            oldTile.SetResident (null);
            tile = nextTile;
            nextTile = null;
        } else {
            nextTile.AddAnimal (parent);
            tile = nextTile;
            nextTile = null;  
        }
    }

    /**
        Checks if the associated animal is a plant.

        @return true if a plant, false otherwise
    */
	public bool isPlant () {
		return speciesType == 0;
	}

    /**
        Checks if the associated animal is a prey.

        @return true if a prey, false otherwise
    */
	public bool isPrey () {
		return speciesType == 1;
	}

    /**
        Checks if the associated animal is a predator.

        @return true if a predator, false otherwise
    */
	public bool isPredator () {

		return speciesType == 2;

	}

    /**
        Sets the parent game object.

        @param  _parent a GameObject instance
    */
    public void SetParent (GameObject _parent) {
        parent = _parent;
    }
}