using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace SD {
    public class SDGameEnd : MonoBehaviour {

        private SDPersistentData persistentObject;
        private Text txtResult;
        private Text txtScore;
        private Text txtOpponentScore;

        // Use this for initialization
        void Start () {
            txtResult = GameObject.Find ("TxtResult").GetComponent<Text> ();
            txtScore = GameObject.Find ("TxtScore").GetComponent<Text> ();
            txtOpponentScore = GameObject.Find ("TxtOpponentScore").GetComponent<Text> ();
            displayResult ();
        }

        void displayResult() {
            persistentObject = SDPersistentData.getInstance ();
            int finalScore = 0;
            int opponentScore = GameController.getInstance().getOpponentScore();
            int winningScore = 0;
            int gameResult = 0;
            string result = null;

            if (persistentObject) {
                finalScore = GameController.getInstance ().getPlayerScore ();
                winningScore = persistentObject.getWinningScore ();
                gameResult = persistentObject.getGameResult ();
            } else {
                if (GameController.getInstance ()) {
                    finalScore = GameController.getInstance ().getPlayerScore ();
                }
            }

            if (gameResult == Constants.PLAYER_WIN) {
                result = "Congratulations ! You won this round !";
            } else if (gameResult == Constants.PLAYER_LOSE) {
                result = "Sorry ! You lost this round. ";
                if (GameController.getInstance ().getHasSurrendered ()) // TODO: Keeping it generic for now.
                   result = "Your gamed ended before time. " + result;
            } else if (gameResult == Constants.PLAYER_DRAW) {
                result = "A draw ! Your score is the same as your opponent's.";
            } else { 
                result = "Game result undefined in single player mode. ";
            }

            txtScore.text = "Your score: " + finalScore;
            txtOpponentScore.text = "Your opponent's score: " + opponentScore;
            txtResult.text = result;
        }
    }
}