using UnityEngine;
using System.Collections;

namespace SD {
    public class OpponentController : MonoBehaviour {

        private GameManager sdGameManager;
        private GameController sdGameController;
        private Rigidbody rbOpponent;

        // Use this for initialization
        void Start () {
            GameObject gameControllerObject = GameObject.FindWithTag ("GameController");
            if (gameControllerObject != null) {
                sdGameController = gameControllerObject.GetComponent<GameController> ();
            } else {
                Debug.Log ("Game Controller not found");
            }
            sdGameManager = GameManager.getInstance ();
            rbOpponent = sdGameController.getOpponent ().GetComponent<Rigidbody> ();
        }

        void FixedUpdate() {
            // Update the velocity of the opponent.
            if (sdGameManager.getIsMultiplayer ()) {
                rbOpponent.MovePosition (new Vector3(sdGameController.getOpponentPlayer().xPosition,
                    sdGameController.getOpponentPlayer().yPosition, 0));
                //rbOpponent.velocity = temp * sdGameController.getOpponentPlayer ().speed * sdGameController.getOpponentPlayer ().movementHorizontal;
            }
        }

        // Update is called once per frame
        void Update () {
        
        }
    }
}
