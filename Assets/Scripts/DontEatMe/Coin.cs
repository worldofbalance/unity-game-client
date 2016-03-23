using UnityEngine;
using System.Collections;

/**
	Defines coin rules.
	NOTE: this class may not be in the final game.
*/
public class Coin : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionStay2D(Collision2D collision) {
		//Debug.Log ("coin collided");
		Destroy (gameObject);
	}
}
