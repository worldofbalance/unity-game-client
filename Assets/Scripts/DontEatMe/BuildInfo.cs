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
	void Update () {
	
	}


}
