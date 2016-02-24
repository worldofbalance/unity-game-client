using UnityEngine;
using System.Collections;

public class Predator_Hunger : MonoBehaviour {
	
	// last attack time
	float last = 0;
	
	// the value at which the predator is full
	[SerializeField]
	int fullHungerValue = 100;
	// the current value of the predator's hungry meter
	int hunger = 0;
	// the value that the predator eats each time they eat a prey
	[SerializeField]
	int amountToEatPerTick = 20;

	GameObject gameOverWall = null;
	GameOverWall gameOverWallScript = null;

	Health currentHP;

	public bool isHunger = false;

	public void Eat() {
		
	}
	
	void OnCollisionStay2D(Collision2D collision) {

		// Collided with a plant!? Game over, buddy
		if (collision.gameObject.tag == "Plant") {
			gameOverWall = GameObject.Find ("EndOfGameWall");
			gameOverWallScript = gameOverWall.GetComponent<GameOverWall>();
			gameOverWallScript.LostGame();
		}
		
		// Collided with a prey? Eat it, yo
		if (collision.gameObject.tag == "Herbivore") {
			// Eat once a second
			isHunger = false;
			//GetComponent<Animator>().SetTrigger("isHunger");
			if (Time.time - last >= 1) {
				currentHP = collision.gameObject.GetComponent<Health>();
				currentHP.lowerHealth(amountToEatPerTick);
				if(currentHP.eaten == true){
					isHunger = true;
					transform.position += new Vector3(4,0,0);
				}

				hunger += amountToEatPerTick;
				//Debug.Log("hunger is " + hunger);
				last = Time.time;
				
				if (hunger >= fullHungerValue) {
					BuildMenu.score += fullHungerValue;
					Destroy(gameObject);
				}
			}
		}
	}
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(isHunger == true){
			GetComponent<Animator>().SetTrigger("isHunger");
		}

		if(isHunger == false){
			GetComponent<Animator>().SetTrigger("notThatHunger");
		}
	}
}
