using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace SD {
    public class SDGameEnd : MonoBehaviour {

        private SDPersistentData persistentObject;
        private Text txtResult;
        private Text txtScore;

        // Use this for initialization
        void Start () {
            txtResult = GameObject.Find ("TxtResult").GetComponent<Text> ();
            txtScore = GameObject.Find ("TxtScore").GetComponent<Text> ();
            displayResult ();
        }

        void displayResult() {
            persistentObject = SDPersistentData.getInstance ();
            int finalScore = 0;
            int winningScore = 0;
            int gameResult = 0;
            string result = null;

            if (persistentObject) {
                finalScore = persistentObject.getPlayerFinalScore ();
                winningScore = persistentObject.getWinningScore ();
            } else {
                finalScore = GameController.getInstance ().getPlayerScore ();
            }

            if (gameResult == Constants.PLAYER_WIN)
                result = "Congratulations ! You won this round !";
            else if (gameResult == Constants.PLAYER_LOSE)
                result = "Sorry ! You lost this round. ";
            else if (gameResult == Constants.PLAYER_DRAW)
                result = "A draw ! Your score is the same as your opponent's.";
            else 
                result = "Game result undefined in single player mode. ";

            txtScore.text = "Your score: " + finalScore;
            txtResult.text = result;
        }
    }
}