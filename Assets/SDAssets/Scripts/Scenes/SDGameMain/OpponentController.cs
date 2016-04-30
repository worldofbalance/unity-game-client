using UnityEngine;
using System.Collections;

namespace SD {
    public class OpponentController : MonoBehaviour {

        private GameManager sdGameManager;
        private GameController sdGameController;
        private Rigidbody rbOpponent;
        private float xPosition;
        private float yPosition;
        private float xRotation;
        private float yAngle;
        private float turnSpeed = 10f;
        private Vector3 turn;

        // Use this for initialization
        void Start () {
            sdGameController = GameController.getInstance ();
            sdGameManager = GameManager.getInstance ();
            rbOpponent = sdGameController.getOpponent ().GetComponent<Rigidbody> ();
            xPosition = yPosition = 0.0f;
            yAngle = -90;
            turn = new Vector3 (0f, turnSpeed, 0f);
        }

        void FixedUpdate() {
            if (sdGameManager.getIsMultiplayer ()) {
                xPosition = sdGameController.getOpponentPlayer ().xPosition;
                yPosition = sdGameController.getOpponentPlayer ().yPosition;
                rbOpponent.MovePosition (new Vector3(xPosition, yPosition, 0));
                xRotation = sdGameController.getOpponentPlayer ().xRotation;
                yAngle = -90;
                if (xRotation >= -90 && xRotation <= 90) {
                    // invert the angle to avoid upside down movement.
                    xRotation = 180 - xRotation;
                    yAngle = 90;
                }
                rbOpponent.MoveRotation (Quaternion.Euler(xRotation - 180, yAngle, 0));
                if (sdGameController.getOpponentPlayer ().isTurningLeft) {
                    if (rbOpponent.transform.rotation.eulerAngles.y < 270) {
                        rbOpponent.transform.Rotate (-turn);
                    }
                    sdGameController.getOpponentPlayer ().isTurningLeft = false;
                } else if (sdGameController.getOpponentPlayer ().isTurningRight) {
                    if (rbOpponent.transform.rotation.eulerAngles.y > 90) {
                        rbOpponent.transform.Rotate (turn);
                    }
                    sdGameController.getOpponentPlayer ().isTurningRight = false;
                }
            }
        }

        // Update is called once per frame
        void Update () {
            // Update the velocity of the opponent.      
        }
    }
}
