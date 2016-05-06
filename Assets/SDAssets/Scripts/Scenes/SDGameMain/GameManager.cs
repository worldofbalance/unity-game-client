using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

/* Description: Manages client communication with the server during the game.
 * 
 */ 
namespace SD {
    public class GameManager : MonoBehaviour {

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
            persistentObject = SDPersistentData.getInstance ();

            if (SDMain.networkManager != null) {
                SDMain.networkManager.Listen (NetworkCode.SD_END_GAME, ResponseSDEndGame);
                SDMain.networkManager.Listen (NetworkCode.SD_PLAYER_POSITION, ResponseSDPosition);
                SDMain.networkManager.Listen (NetworkCode.SD_KEYBOARD, ResponseSDKeyboard);
                SDMain.networkManager.Listen (NetworkCode.SD_PREY, ResponseSDPrey);
                SDMain.networkManager.Listen (NetworkCode.SD_EAT_PREY, ResponseSDDestroyPrey);
                SDMain.networkManager.Listen (NetworkCode.SD_SCORE, ResponseSDChangeScore);
                isMultiplayer = true;
            } else {
                Debug.LogWarning ("Could not establish a connection to Sea Divided Server. Falling back to offline mode.");
            }
        }

        public static GameManager getInstance() {
            return gameManager;
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
            if (isMultiplayer) {
                SDMain.networkManager.Send (SDEndGameProtocol.Prepare (gameCompleted, (float)finalScore));
            } else {
                SceneManager.LoadScene ("SDGameEnd");
            }
        }

        public void ResponseSDEndGame(NetworkResponse r) {
            ResponseSDEndGame response = r as ResponseSDEndGame;
            persistentObject.setWinningScore ((int)response.winningScore);

            if (response.status == 1) {
                persistentObject.setGameResult (Constants.PLAYER_WIN);
            } else if (response.status == 2) {
                persistentObject.setGameResult (Constants.PLAYER_LOSE);
            } else {
                persistentObject.setGameResult (Constants.PLAYER_DRAW);
            }
            SceneManager.LoadScene ("SDGameEnd");
        }

        // Sends the player's current position to the server.
        public void SetPlayerPositions(float x, float y, float r) {
            if (SDMain.networkManager != null) {
                SDMain.networkManager.Send (SDPlayerPositionProtocol.Prepare (
                    x.ToString (), y.ToString (), r.ToString ()));
            }
        }

        public void ResponseSDPosition(NetworkResponse r) {
            ResponseSDPlayerPosition response = r as ResponseSDPlayerPosition;
            gameController.getOpponentPlayer().xPosition = response.xPosition;
            gameController.getOpponentPlayer ().yPosition = response.yPosition;
            gameController.getOpponentPlayer ().xRotation = response.rotation;
        }

        // Sends the keyboard inputs to the server.
        public void SetKeyboardActions(int keyCode, int keyCombination) {
            if (SDMain.networkManager != null) {
                SDMain.networkManager.Send (SDKeyboardProtocol.Prepare (keyCode, keyCombination));
            }
        }

        public void ResponseSDKeyboard(NetworkResponse r) {

            ResponseSDKeyboard response = r as ResponseSDKeyboard;
            Debug.Log ("Running the keyboard response");
            if (response.keyCode == (int)KeyCode.Space) {
                if (response.keyCombination == 0) {  // Space key down
                    // Speed up the opponent.
                    gameController.getOpponentPlayer().speed = gameController.getOpponentPlayer().speed * gameController.getOpponentPlayer().speedUpFactor;
                }

                if (response.keyCombination == 1) {
                    // Space key up
                    gameController.getOpponentPlayer().speed = gameController.getOpponentPlayer().speed / gameController.getOpponentPlayer().speedUpFactor;
                }
            }

            // Turning right/left with the arrow keys.
            if (response.keyCode == (int)KeyCode.RightArrow) {
                if (response.keyCombination == 0) {
                    gameController.getOpponentPlayer ().isTurningRight = true;
                } else {
                    gameController.getOpponentPlayer ().isTurningRight = false;
                }
            }

            if (response.keyCode == (int)KeyCode.LeftArrow) {
                if (response.keyCombination == 0) {
                    gameController.getOpponentPlayer ().isTurningLeft = true;
                } else {
                    gameController.getOpponentPlayer ().isTurningLeft = false;
                }
            }
        }

        // Get prey position to spawn by ID
        public void FindNPCFishPosition(int id) {
            if (SDMain.networkManager != null) {
                SDMain.networkManager.Send (SDPreyProtocol.Prepare (id));
            }
        }

        public void ResponseSDPrey(NetworkResponse r) {
            ResponseSDPrey response = r as ResponseSDPrey;
            NPCFish fish = gameController.getNpcFishes()[response.preyId];
            if (response.isAlive) {
                // Set the position of the fish and spawn it.
                fish.xPosition = response.xPosition;
                fish.yPosition = response.yPosition;
                fish.isAlive = response.isAlive;
                fish.id = response.preyId;
                gameController.spawnPrey (fish.id, 0);
            } else {
                // Destroy the NPC Fish at the specified position if fish is not alive.
                fish.isAlive = false;
                gameController.destroyPrey (response.preyId);
            }
        }

        public void DestroyNPCFish(int id) {
            if (SDMain.networkManager != null) {
                SDMain.networkManager.Send (SDDestroyPreyProtocol.Prepare (id));
            } else {
                NPCFish fish = gameController.getNpcFishes()[id];
                fish.isAlive = false;
            }
        }

        public void ResponseSDDestroyPrey(NetworkResponse r) {
            ResponseSDDestroyPrey response = r as ResponseSDDestroyPrey;
            NPCFish fish = gameController.getNpcFishes()[response.preyId];
            if (fish.isAlive) {
                // The NPC Fish destroyed on the server is still alive in the client, so destroy it.
                Debug.Log("Opponent consumed prey with ID: " + response.preyId);
                fish.isAlive = false;
                gameController.destroyPrey (response.preyId);
            }
        }

        public void SendScoreToOpponent(int score) {
            if (SDMain.networkManager != null) {
                SDMain.networkManager.Send (SDScoreProtocol.Prepare ((float)score));
                Debug.Log ("Sent the score " + score);
            }
        }

        public void ResponseSDChangeScore(NetworkResponse r) {
            ResponseSDScore response = r as ResponseSDScore;
            gameController.setOpponentScore (response.score);
            Debug.Log ("Received the opponent's score: " + response.score);
        }
    }
}
