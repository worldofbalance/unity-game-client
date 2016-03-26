using UnityEngine;
using System.Collections;

public class DemTile : MonoBehaviour {

  int idX; // X-coord for DemTile
  int idY; // Y-coord for DemTile

  GameObject resident;

	// Use this for initialization
  	void Start () {
  		// Parse X and Y coords from name
  		// Name format = "X,Y", so X is stored @ name[0] and Y @ name[2]
  		// The char value for '0' starts at 0x30, subtract this to parse numeric value
  		// NOTE: this assumes that X and Y values remain within the range [0,9]
		idX = this.name[0] - 0x30; 
		idY = this.name[2] - 0x30;
		//Debug.Log("Cube at (" + idX + ", " + idY + ")");

		// Set resident to null
		resident = null;
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

	/**
		Activates on mouse click
	*/
	void OnMouseDown () {
		Vector3 center = this.GetComponent<Renderer>().bounds.center;
		Debug.Log("Tile (" + idX + ", " + idY + ") clicked, center @ (" + center.x + ", " + center.y + ", " + center.z + ")");
		// Test spawn a resident
		// NOTE: for testing purposes, the resident is hard coded to a TreeMouse @ it's normal size
		// TODO: spawn resident based on species ID, resize resident appropriately
		BuildInfo currCreature = BuildMenu.currentlyBuilding;
		if (!resident && currCreature) {
			Debug.Log("Placing " + currCreature.name + ", speciesId = " + currCreature.GetSpeciesId());
			GameObject treemouse = (GameObject) Resources.Load("Prefabs/Herbivores/TreeMouse");
			treemouse.GetComponent<Prey_Hunger>().enabled = false;
			resident = (GameObject) Instantiate(treemouse, new Vector3(center.x, center.y, (center.z - 0.5f)), Quaternion.identity);
			if (resident != null)
				Debug.Log("Placed " + resident.name + " @ " + resident.GetComponent<Transform>().position);
		} 
		else {
			Debug.Log("Tile already taken!");
		}
	}
}
