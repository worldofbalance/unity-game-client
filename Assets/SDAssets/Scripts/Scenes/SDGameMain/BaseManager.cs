/*
 * File Name: BaseManager.cs
 * Description: Will Handle all events between the player and the base
 *              So far,
 *              1. Player can score their unscored points staying more than 3 second in the base
 *              2. While player is in the base. The stamina stays at 100.
 * 
 */ 


using UnityEngine;
using System.Collections;

namespace SD {
    public class BaseManager : MonoBehaviour {

        private GameController gameController;
        private float time;
        private float secondsToScore = 3f;
        private float staminaRevoverRate = 0.1f;

        // Use this for initialization
        void Start () {
            gameController = GameController.getInstance ();
        }

        // Update is called once per frame
        void Update () {

        }

        // Runs when the player collides the base
        void OnTriggerEnter(Collider other) {
            if (other.tag == "Player") {
                time = secondsToScore;
               
            }
        }

        // Runs while the player is staying in the base
        void OnTriggerStay(Collider other) {
            if (other.tag == "Player") {
                if (gameController)
                    gameController.stamina += staminaRevoverRate;

                // After couplse seconds that is defined by timeToScore, 
                // add unscored points to the actual score
                time -= 1f * Time.deltaTime;
                if (time <= 0) {
                    gameController.Score ();
                    gameController.ResetUnscored ();
                }
            }
        }
    }
}