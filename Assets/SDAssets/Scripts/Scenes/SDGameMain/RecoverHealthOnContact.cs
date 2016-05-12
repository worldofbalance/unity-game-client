/* 
 * File Name: RecoverHealthOnContact.cs
 * Description: Recovers the player's health when the player consumes prey.
 */

using UnityEngine;
using System.Collections;

namespace SD {
    public class RecoverHealthOnContact : MonoBehaviour {

        private GameController gameController;
        private const int damage = 10; // Score to be recieved by eating prey

        // Use this for initialization
        void Start () {
            gameController = GameController.getInstance ();
        }


        // Update is called once per frame
        void Update () {

        }
            
        void OnTriggerEnter(Collider other) {
            if (other.tag == "Player") {
                if (gameController.GetHealth() < 100) {
                    gameController.UpdateHealth (damage);
                }
            }
        }
    }
}
