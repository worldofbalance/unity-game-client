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
        private bool isMultiplayer = false;

        public GameManager() {
        }
 
        void Awake() {
            gameManager = this;
        }

        void Start() {
            gameController = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController>();
            cManager = SDConnectionManager.getInstance ();
            mQueue = SDMessageQueue.getInstance ();
            if (cManager && mQueue) {
                if (!mQueue.callbackList.ContainsKey(Constants.SMSG_SDEND_GAME))
                    mQueue.AddCallback (Constants.SMSG_SDEND_GAME, ResponseSDEndGame);
                if (mQueue.callbackList.ContainsKey (Constants.SMSG_POSITION))
                    mQueue.RemoveCallback (Constants.SMSG_POSITION);
                if (!mQueue.callbackList.ContainsKey (Constants.SMSG_POSITION))
                    mQueue.AddCallback (Constants.SMSG_POSITION, ResponseSDPosition);
                if (!mQueue.callbackList.ContainsKey (Constants.SMSG_KEYBOARD))
                    mQueue.AddCallback (Constants.SMSG_KEYBOARD, ResponseSDKeyboard);
                if (!mQueue.callbackList.ContainsKey (Constants.SMSG_PREY))
                    mQueue.AddCallback (Constants.SMSG_PREY, ResponseSDPrey);
                isMultiplayer = true;
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

        public bool getIsMultiplayer() {
            return isMultiplayer;
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
                request.Send (x.ToString (), y.ToString ());
                cManager.Send (request);
            }
        }

        public void ResponseSDPosition(ExtendedEventArgs eventArgs) {
            ResponseSDPositionEventArgs args = eventArgs as ResponseSDPositionEventArgs;
            gameController.getOpponentPlayer().xPosition = args.xPosition;
            gameController.getOpponentPlayer ().yPosition = args.yPosition;
        }

        // Sends the keyboard inputs to the server.
        public void SetKeyboardActions(int keyCode, int keyCombination) {
            if (cManager) {
                RequestSDKeyboard request = new RequestSDKeyboard ();
                request.Send (keyCode, keyCombination);
                cManager.Send (request);
                Debug.Log ("Sent a request for " + keyCode);
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
        }

        // Get prey position to spawn by ID
        public void FindNPCFishPosition(int id) {
            if (cManager) {
                RequestSDPrey request = new RequestSDPrey ();
                request.Send (id);
                cManager.Send (request);
            }
        }

        public void ResponseSDPrey(ExtendedEventArgs eventArgs) {
            ResponseSDPreyEventArgs args = eventArgs as ResponseSDPreyEventArgs;
            NPCFish fish = gameController.getNpcFishes()[args.prey_id];
            if (args.isAlive) {
                // Set the position of the fish and spawn it.
                fish.xPosition = args.xPosition;
                fish.yPosition = args.yPosition;
                fish.isAlive = args.isAlive;
                fish.id = args.prey_id;
                gameController.spawnPrey (fish.id);
            } else {
                // Destroy the NPC Fish at the specified position at the opponent's side.
                fish.isAlive = false;
                gameController.destroyPrey (args.prey_id);
            }
        }
    }
}
