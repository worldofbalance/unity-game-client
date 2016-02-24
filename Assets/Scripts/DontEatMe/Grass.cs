using UnityEngine;
using System.Collections;

public class Grass : MonoBehaviour {

	// hold a reference to the game object placed on top of soil
	public GameObject currentlyPlacedPiece = null;

	// the tag with which to find the paired soil gameobject
	public string pairedSoilTag = null;

	// hold a reference to the soil game object to the immediate left
	public GameObject pairedSoil = null;

	void OnMouseUpAsButton() {
		// Is there something to build?
		if (BuildMenu.currentlyBuilding != null) {

			// is it a prey? you can't build plants on the grass
			if (BuildMenu.currentlyBuilding.gameObject.tag != "Herbivore") return;

			// Build it and hold a reference to the newly created gamepiece
			currentlyPlacedPiece = (GameObject) Instantiate(BuildMenu.currentlyBuilding.gameObject, transform.position, Quaternion.identity);

			// reduce the player's resources by the price of the game piece just construsted
			BuildMenu.currentResources -= BuildMenu.currentlyBuilding.price;
			// Add the price of the game piece onto the score
			BuildMenu.score += BuildMenu.currentlyBuilding.price;

			//Calculate coins
			BuildMenu buildMenuScript;
			GameObject mainCamera;

			mainCamera = GameObject.Find("MainCamera");
			buildMenuScript = mainCamera.GetComponent<BuildMenu>();
			buildMenuScript.CalculateCoins();

			// give the currently placed piece a reference to this tile
			if (currentlyPlacedPiece != null && pairedSoil != null) {
				currentlyPlacedPiece.GetComponent<Prey_Hunger>().soilWithPlant = pairedSoil;
			}

			BuildMenu.currentlyBuilding = null;
		}
	}

	// Use this for initialization
	void Start () {
		GameObject foundSoil = GameObject.FindGameObjectWithTag (pairedSoilTag);
		pairedSoil = foundSoil.transform.parent.gameObject;
	}
	
	// Update is called once per frame
	void Update () {
	}

	// highlight the grass tiles with yellow when player mouses over them
	void OnMouseOver () {
		GetComponent<Renderer>().material.color = Color.yellow;
	}
	
	void OnMouseExit() {
		GetComponent<Renderer>().material.color = Color.white;
	}
}
