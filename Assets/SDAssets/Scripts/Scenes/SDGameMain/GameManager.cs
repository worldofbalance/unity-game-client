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
        public void EndGame(bool gameCompleted, string finalScore) {
            if (!gameCompleted) {
                Debug.Log ("The player has surrendered - the game was unfinished. ");
            }
            if (cManager) {
                RequestSDEndGame request = new RequestSDEndGame();
                request.Send (gameCompleted, finalScore);
                cManager.Send (request);
            }
        }
        public void ResponseSDEndGame(ExtendedEventArgs eventArgs) {
            ResponseSDEndGameEventArgs args = eventArgs as ResponseSDEndGameEventArgs;
            resultText = " Your final score is " + args.finalScore + ".";
            if (args.isWinner) {
                resultText += "Congratulations !";
            } else {
                resultText += "Sorry, you lost ! Better luck next time !";
            }
            Debug.Log(resultText);
            //Rect windowRect = new Rect(Screen.width/2 - 50, Screen.height/2 - 30, 100, 60);
            //GUI.ModalWindow(0, windowRect, ShowResultDialog, "Game Result");
        }

        //public void ShowResultDialog(int windowId) {
       //     if (GUI.Button (Screen.width / 2 - 50, Screen.height / 2 + 30, 20, 20)) {
        //        
        //    }
       // }

    }
}
