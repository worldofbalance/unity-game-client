using UnityEngine;
using System.Collections;

public class DemTile : MonoBehaviour {

  	int idX; // X-coord for DemTile
  	int idY; // Y-coord for DemTile

  	GameObject resident; // Resident object (HerbivoreObject or PlantObject) placed on tile

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
		if (!resident)
			this.GetComponent<Renderer>().material.color = Color.cyan;
		else
			this.GetComponent<Renderer>().material.color = Color.red;
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
		NOTE: this is an experimental implementation for placing game objects.
	*/
	void OnMouseDown () {
		Vector3 center = this.GetComponent<Renderer>().bounds.center; // Get center coords of tile

		Debug.Log("Tile (" + idX + ", " + idY + ") clicked, center @ (" + center.x + ", " + center.y + ", " + center.z + ")");

		// Test spawn a resident
		// NOTE: for testing purposes, the resident is hard coded to a TreeMouse @ it's normal size
		// TODO: spawn resident based on species ID, resize resident appropriately
		BuildInfo currCreature = BuildMenu.currentlyBuilding;
		if (!resident) { // If there is no current resident on tile
			if (currCreature) {
				Debug.Log("Placing " + currCreature.name + ", speciesId = " + currCreature.GetSpeciesId());

				// Create Herbivore GameObject from currentlyBuilding type
				GameObject herbivore = (GameObject) Resources.Load("Prefabs/Herbivores/" + currCreature.name);

				// NOTE: the Prey_Hunger.cs script must be disabled for the moment to prevent compiler errors
				herbivore.GetComponent<Prey_Hunger>().enabled = false;


				// Calculate the exact offset needed for herbivore placement on the tile
				float z_offset = 0.5f * Mathf.Abs(center.z + herbivore.transform.position.z);

				// Instantiate the resident
				resident = (GameObject) Instantiate(herbivore, new Vector3(center.x, center.y, center.z - z_offset), Quaternion.identity);
				// Assign species ID
				// NOTE: all the species ID's for the prefabs are set to zero, so for now I will simply set the ID for each instance.
				resident.GetComponent<BuildInfo>().speciesId = SpeciesConstants.SpeciesIdByName(currCreature.name);

				Vector3 tilesize = transform.lossyScale;

				Debug.Log("tile size: " + tilesize.ToString());

				// Define new scale for resident object
				tilesize.Scale
				(
					new Vector3
					(
						SpeciesConstants.ScaleBySpeciesId(resident.GetComponent<BuildInfo>().GetSpeciesId()),
						SpeciesConstants.ScaleBySpeciesId(resident.GetComponent<BuildInfo>().GetSpeciesId()),
						1.0f
					)
				);
				// Apply the new scale
				resident.transform.localScale = tilesize;

				Debug.Log("resident size: " + resident.transform.localScale.ToString());

				if (resident != null)
					Debug.Log("Placed " + resident.name + " @ " + resident.GetComponent<Transform>().position);
				}
		} 
		else {
			Debug.Log("Tile already taken!");
		}
	}
}
