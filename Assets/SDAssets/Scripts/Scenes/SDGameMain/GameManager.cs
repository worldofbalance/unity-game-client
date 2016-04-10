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

        public GameManager() {
        }
 
        void Start() {
            gameManager = this;
            cManager = SDConnectionManager.getInstance ();
            mQueue = SDMessageQueue.getInstance ();
            if (cManager && mQueue) {
                mQueue.AddCallback (Constants.SMSG_SDEND_GAME, ResponseSDEndGame);
            } else {
                Debug.LogWarning ("Could not establish a connection to Sea Divided Server. Falling back to offline mode.");
            }
        }

        public static GameManager getInstance() {
            return gameManager;
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
    }
}
