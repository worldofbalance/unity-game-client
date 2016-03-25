using UnityEngine;
using System.Collections;

public class DemTile : MonoBehaviour {

  int idX; // X-coord for DemTile
  int idY; // Y-coord for DemTile

  GameObject tile;

	// Use this for initialization
  	void Start () {
  		// Parse X and Y coords from name
  		// Name format = "X,Y", so X is stored @ name[0] and Y @ name[2]
  		// The char value for '0' starts at 0x30, subtract this to parse numeric value
  		// NOTE: this assumes that X and Y values remain within the range [0,9]
		idX = this.name[0] - 0x30; 
		idY = this.name[2] - 0x30;
		//Debug.Log("Cube at (" + idX + ", " + idY + ")");
	}


	
	// Update is called once per frame
	void Update () {
	
	}

	/**
		Activates on mouse enter
	*/
	void OnMouseEnter () {
		// Set highlight color
		// TODO: change highlight color based on a tile's legality
		this.GetComponent<Renderer>().material.color = Color.cyan;
	}

	/**
		Activates on mouse exit
	*/
	void OnMouseExit () {
		// Reset highlight color
		this.GetComponent<Renderer>().material.color = Color.white;
	}
}
