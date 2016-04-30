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
        private float yRotation;
        private float turnSpeed = 10f;
        private Vector3 turn;

        // Use this for initialization
        void Start () {
            sdGameController = GameController.getInstance ();
            sdGameManager = GameManager.getInstance ();
            rbOpponent = sdGameController.getOpponent ().GetComponent<Rigidbody> ();
            xPosition = yPosition = 0.0f;
            yAngle = yRotation = -90;
            turn = new Vector3 (0f, turnSpeed, 0f);
        }

        void FixedUpdate() {
            if (sdGameManager.getIsMultiplayer ()) {
                xPosition = sdGameController.getOpponentPlayer ().xPosition;
                yPosition = sdGameController.getOpponentPlayer ().yPosition;
                yRotation = sdGameController.getOpponentPlayer ().yRotation;
                rbOpponent.MovePosition (new Vector3(xPosition, yPosition, 0));

               /* if (sdGameController.getOpponentPlayer ().isTurningLeft) {
                    xRotation = sdGameController.getOpponentPlayer ().xRotation;
                    sdGameController.getOpponentPlayer ().yRotation = yRotation = 180;
                    rbOpponent.MoveRotation (Quaternion.Euler (xRotation - 180, yRotation, 0));
                } else if (sdGameController.getOpponentPlayer ().isTurningRight) {
                    xRotation = sdGameController.getOpponentPlayer ().xRotation;
                    sdGameController.getOpponentPlayer ().yRotation = yRotation = -180;
                    rbOpponent.MoveRotation (Quaternion.Euler (xRotation - 180, yRotation, 0));
                } else { */
                    xRotation = sdGameController.getOpponentPlayer ().xRotation;
                    yAngle = -90;
                    if (xRotation >= -90 && xRotation <= 90) {
                        xRotation = 180 - xRotation;
                        yAngle = 90;
                    }
                    rbOpponent.MoveRotation (Quaternion.Euler (xRotation - 180, yAngle, 0));
               // }
            }
        }

        // Update is called once per frame
        void Update () {
            // Update the velocity of the opponent.      
        }
    }
}
