using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

/* Description: Manages client communication with the server during the game.
 * 
 */ 
namespace SD {
    public class GameManager : MonoBehaviour {

        private SDConnectionManager cManager;
        private SDMessageQueue mQueue;
        private static GameManager gameManager;
        private string resultText;
        private static GameController gameController;

        public GameManager() {
        }
 
        void Start() {
            gameManager = this;
            gameController = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController>();
            cManager = SDConnectionManager.getInstance ();
            mQueue = SDMessageQueue.getInstance ();
            if (cManager && mQueue) {
                if (!mQueue.callbackList.ContainsKey(Constants.SMSG_SDEND_GAME))
                    mQueue.AddCallback (Constants.SMSG_SDEND_GAME, ResponseSDEndGame);
                if (!mQueue.callbackList.ContainsKey (Constants.SMSG_POSITION))
                    mQueue.AddCallback (Constants.SMSG_POSITION, ResponseSDPosition);
                if (!mQueue.callbackList.ContainsKey (Constants.SMSG_KEYBOARD))
                    mQueue.AddCallback (Constants.SMSG_KEYBOARD, ResponseSDKeyboard);
            } else {
                Debug.LogWarning ("Could not establish a connection to Sea Divided Server. Falling back to offline mode.");
            }
        }

        public static GameManager getInstance() {
            return gameManager;
        }

        public SDConnectionManager getConnectionManager() {
            return cManager;
        }
        // Ends the current game
        public void EndGame(bool gameCompleted, int finalScore) {
            if (!gameCompleted) {
                Debug.Log ("The player has surrendered - the game was unfinished. ");
            }
            if (cManager) {
                RequestSDEndGame request = new RequestSDEndGame();
                request.Send (gameCompleted, finalScore.ToString()); // TODO: This should be changed to int in the server.
                cManager.Send (request);
            }
            SceneManager.LoadScene ("SDReadyScene");  // TODO: Remove once the DB and Server work.
        }

        public void ResponseSDEndGame(ExtendedEventArgs eventArgs) {
            ResponseSDEndGameEventArgs args = eventArgs as ResponseSDEndGameEventArgs;
            resultText = " Your final score is " + args.finalScore + ".";
            if (args.isWinner) {
                resultText += "Congratulations ! You win !";
            } else {
                resultText += "Sorry, you lost ! Better luck next time !";
            }
            Debug.Log(resultText);
        }
        // Sends the player's current position to the server.
        public void SetPlayerPositions(float x, float y) {
            if (cManager) {
                RequestSDPosition request = new RequestSDPosition ();
                request.Send (x.ToString(), y.ToString());
                cManager.Send (request);
            } else {
                Debug.LogWarning ("Could not send the player's position to the server.");
            }
        }

        public void ResponseSDPosition(ExtendedEventArgs eventArgs) {
            ResponseSDPositionEventArgs args = eventArgs as ResponseSDPositionEventArgs;
            Debug.Log ("The position of the other player is ("+args.xPosition + "," + args.yPosition + ")");
            gameController.getOpponentPlayer().movementHorizontal = args.xPosition;
            gameController.getOpponentPlayer ().movementVertical = args.yPosition;
        }

        // Sends the keyboard inputs to the server.
        public void SetKeyboardActions(int keyCode, int keyCombination) {
            if (cManager) {
                RequestSDKeyboard request = new RequestSDKeyboard ();
                request.Send (keyCode, keyCombination);
                cManager.Send (request);
                Debug.Log ("Sent a request for " + keyCode);
            } else {
                Debug.LogWarning ("Could not send the player's keyboard input to the server.");
            }
        }

        public void ResponseSDKeyboard(ExtendedEventArgs eventArgs) {
            ResponseSDKeyboardEventArgs args = eventArgs as ResponseSDKeyboardEventArgs;
            if (args.keyCode == (int)KeyCode.Space) {
                if (args.keyCombination == 0) {  // Space key down
                    // Speed up the opponent.
                    gameController.getOpponentPlayer().speed = gameController.getOpponentPlayer().speed * gameController.getOpponentPlayer().speedUpFactor;
                }

                if (args.keyCombination == 1) {
                    // Space key up
                    gameController.getOpponentPlayer().speed = gameController.getOpponentPlayer().speed / gameController.getOpponentPlayer().speedUpFactor;
                }
            }
            Debug.Log ("The keyboard input of the other player is keycode=" + args.keyCode + ", keyCombination=" + args.keyCombination);
            // TODO: update the keyboard input of the opponent.
        }
    }
}
