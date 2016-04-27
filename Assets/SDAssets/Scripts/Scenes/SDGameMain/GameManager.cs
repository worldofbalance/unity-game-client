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
        private static GameController gameController;
        private static SDPersistentData persistentObject;
        private bool isMultiplayer = false;
        private string resultText;

        public GameManager() {
        }
 
        void Awake() {
            gameManager = this;
        }

        void Start() {
            gameController = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController>();
            cManager = SDConnectionManager.getInstance ();
            mQueue = SDMessageQueue.getInstance ();
            persistentObject = SDPersistentData.getInstance ();

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
                if (!mQueue.callbackList.ContainsKey (Constants.SMSG_EAT_PREY))
                    mQueue.AddCallback (Constants.SMSG_EAT_PREY, ResponseSDDestroyPrey);
                if (!mQueue.callbackList.ContainsKey (Constants.SMSG_SCORE))
                    mQueue.AddCallback (Constants.SMSG_SCORE, ResponseSDChangeScore);
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
            if (persistentObject) {
                persistentObject.setPlayerFinalScore (finalScore);
            }
            if (cManager) {
                RequestSDEndGame request = new RequestSDEndGame ();
                request.Send (gameCompleted, (float)finalScore);
                cManager.Send (request);
            } else {
                SceneManager.LoadScene ("SDGameEnd");
            }
        }

        public void ResponseSDEndGame(ExtendedEventArgs eventArgs) {
            ResponseSDEndGameEventArgs args = eventArgs as ResponseSDEndGameEventArgs;
            persistentObject.setWinningScore ((int)args.winningScore);

            if (args.status == 1) {
                persistentObject.setGameResult (Constants.PLAYER_WIN);
            } else if (args.status == 2) {
                persistentObject.setGameResult (Constants.PLAYER_LOSE);
            } else {
                persistentObject.setGameResult (Constants.PLAYER_DRAW);                
            }
            SceneManager.LoadScene ("SDGameEnd");
        }

        // Sends the player's current position to the server.
        public void SetPlayerPositions(float x, float y, float r) {
            if (cManager) {
                RequestSDPosition request = new RequestSDPosition ();
                request.Send (x.ToString (), y.ToString (), r.ToString());
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
                // Destroy the NPC Fish at the specified position if fish is not alive.
                fish.isAlive = false;
                gameController.destroyPrey (args.prey_id);
            }
        }

        public void DestroyNPCFish(int id) {
            if (cManager) {
                RequestSDDestroyPrey request = new RequestSDDestroyPrey ();
                request.Send (id);
                cManager.Send (request);
            } else {
                NPCFish fish = gameController.getNpcFishes()[id];
                fish.isAlive = false;
            }
        }

        public void ResponseSDDestroyPrey(ExtendedEventArgs eventArgs) {
            ResponseSDDestroyPreyEventArgs args = eventArgs as ResponseSDDestroyPreyEventArgs;
            NPCFish fish = gameController.getNpcFishes()[args.prey_id];
            if (fish.isAlive) {
                // The NPC Fish destroyed on the server is still alive in the client, so destroy it.
                Debug.Log("Opponent consumed prey with ID: " + args.prey_id);
                fish.isAlive = false;
                gameController.destroyPrey (args.prey_id);
            }
        }

        public void SendScoreToOpponent(int score) {
            if (cManager) {
                RequestSDScore request = new RequestSDScore ();
                request.Send ((float)score);
                cManager.Send (request);
                Debug.Log ("Sent the score " + score);
            }
        }

        public void ResponseSDChangeScore(ExtendedEventArgs eventArgs) {
            ResponseSDScoreEventArgs args = eventArgs as ResponseSDScoreEventArgs;
            gameController.setOpponentScore (args.score);
            Debug.Log ("Received the opponent's score: " + args.score);
        }
    }
}
