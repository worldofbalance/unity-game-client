using UnityEngine;
using System.Collections;

namespace SD {
    public class OpponentController : MonoBehaviour {

        private GameManager sdGameManager;
        private GameController sdGameController;

        // Use this for initialization
        void Start () {
            GameObject gameControllerObject = GameObject.FindWithTag ("GameController");
            if (gameControllerObject != null) {
                sdGameController = gameControllerObject.GetComponent<GameController> ();
            } else {
                Debug.Log ("Game Controller not found");
            }
        }

        void FixedUpdate() {
            // Update the velocity of the opponent.
            if (sdGameManager.getIsMultiplayer ()) {
                Vector3 opponentMovement = new Vector3 (
                    sdGameController.getOpponentPlayer ().movementHorizontal,
                    sdGameController.getOpponentPlayer ().movementVertical,
                    0.0f);
                sdGameController.getOpponent ().GetComponent<Rigidbody> ().velocity = opponentMovement * sdGameController.getOpponentPlayer ().speed;
                Debug.Log ("Opponents velocity is " + sdGameController.getOpponent ().GetComponent<Rigidbody> ().velocity);
            }
        }

        // Update is called once per frame
        void Update () {
        
        }
    }
}
