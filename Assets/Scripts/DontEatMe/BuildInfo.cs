using UnityEngine;
using System.Collections;

public class BuildInfo : MonoBehaviour {

	// the menu button sprite
	public Texture previewImage;

	// Species ID
	// NOTE: this is made public for the time being to allow assignment of species ID to GameObject instances
	public int speciesId;

	// the cost to build the item
	public int price;

    //public bool isPlant;
	public short speciesType; //for classify if it is plant =0, prey =1, or preditor =2

  public DemTile tile = null;



	// Current Health
	//[SerializeField]
	//int speciesId = 0;

  	public int GetSpeciesId() {
		return speciesId;
	}

	// Use this for initialization
	void Start () {
    

	}
	
	// Update is called once per frame
  public DemTile GetTile () {
    return tile;
	}

	public bool isPlant(){
		if (speciesType == 0)
			return true;
		return false;
	}

	public bool isPrey(){
		if (speciesType == 1)
			return true;
		return false;
	}

	public bool isPredator(){
		if (speciesType == 2)
			return true;
		return false;
	}

}
