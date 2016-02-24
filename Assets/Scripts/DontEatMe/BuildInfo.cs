using UnityEngine;
using System.Collections;

public class BuildInfo : MonoBehaviour {

	// the menu button sprite
	public Texture previewImage;

	// Current Health
	[SerializeField]
	int speciesId = 0;


  public int GetSpeciesId() {
		return speciesId;
	}

	// the cost to build the item
	public int price;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


}
