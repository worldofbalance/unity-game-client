using UnityEngine;
using System.Collections;

public class Prey_Hunger : MonoBehaviour {

	public bool isPreyEating = false;

	// last eat time
	float last = 0;

	// last plant eat time
	int lastPlantEatTime = 0;

	// the value that the herbivore eats every tick
	[SerializeField]
	int amountToEatPerTick = 5;

	// reference to the tile it's placed on
	public GameObject soilWithPlant = null;

	public void Eat() {
		// Eat once a second
		if (isPreyEating) {

			lastPlantEatTime = 0;
			
			//Debug.Log ("Herbivore eating plant");

			GetComponent<Animator>().SetTrigger("hasPlant");

			if (Time.time - last >= 1) {
				soilWithPlant.GetComponent<Soil> ().currentlyPlacedPiece.GetComponent<Health> ().lowerHealth (amountToEatPerTick);
				last = Time.time;

			}
		} else {

			lastPlantEatTime++;
			if (lastPlantEatTime >= 6) {
				//Debug.Log ("herbivore wandered away");
				Destroy (gameObject);
			}

		}	
	}

	// Use this for initialization
	void Start () {
		InvokeRepeating ("Eat", 0, 1);
	
	}
	
	// Update is called once per frame
	void Update () {
		// if the soil contains a plant
		if (soilWithPlant.GetComponent<Soil> ().currentlyPlacedPiece.gameObject != null) {
			// this flag controls whether or not the herbivore trys to eat
			// will only eat when there is a plant to eat on the soil
			if (!isPreyEating) {
				GetComponent<Animator>().SetTrigger("hasPlant");
				//Debug.Log ("is prey eating");

				isPreyEating = true;
			}
		} else {
			isPreyEating = false;
			GetComponent<Animator>().SetTrigger("herbHunger");
		}
	}
}
