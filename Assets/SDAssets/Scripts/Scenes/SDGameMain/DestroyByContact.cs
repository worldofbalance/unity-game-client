/* 
 * File Name: DestroyByContact.cs
 * Description: Destroys attached objects if they collide with the player.
 */

using UnityEngine;
using System.Collections;

public class DestroyByContact : MonoBehaviour {

    public GameController gameController;
    private const int newScoreValue = 10; // Score to be recieved by eating prey


    // Use this for initialization
    void Start () {
        GameObject gameControllerObject = GameObject.FindWithTag ("GameController");
        if (gameControllerObject != null) {
            gameController = gameControllerObject.GetComponent<GameController> ();
        } else {
            Debug.Log ("Game controller not found");
        }
    }
    
    // Update is called once per frame
    void Update () {
    
    }

    // Destroys the attached object upon a collison with the player
    void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            Destroy (gameObject);
            gameController.AddUnscoredPoint (newScoreValue);
        }
    }
}
