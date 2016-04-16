﻿using UnityEngine;
using System.Collections;

namespace SD {
    public class OpponentController : MonoBehaviour {

        private GameManager sdGameManager;
        private GameController sdGameController;
        private Rigidbody rbOpponent;
        float xPosition;
        float yPosition;

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
            xPosition = yPosition = 0.0f;
        }

        void FixedUpdate() {
            if (sdGameManager.getIsMultiplayer ()) {
                xPosition = sdGameController.getOpponentPlayer ().xPosition;
                yPosition = sdGameController.getOpponentPlayer ().yPosition;
                rbOpponent.MovePosition (new Vector3(xPosition, yPosition, 0));
            }
        }

        // Update is called once per frame
        void Update () {
            // Update the velocity of the opponent.      
        }
    }
}