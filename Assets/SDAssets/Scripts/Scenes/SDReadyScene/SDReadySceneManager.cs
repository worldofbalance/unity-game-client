using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace SD {
    public class SDReadySceneManager : MonoBehaviour {

        private SDConnectionManager cManager;
        // Use this for initialization
        void Start () {
            cManager = SDConnectionManager.getInstance ();
            SDMessageQueue.getInstance ().AddCallback (Constants.SMSG_SDSTART_GAME, ResponseSDStartGame);
        }

        public static void StartGame() {
            RequestSDStartGame request = new RequestSDStartGame ();
            request.Send (Constants.USER_ID);
            cManager.Send (request);
        }

        public void ResponseSDStartGame (ExtendedEventArgs eventArgs)
        {
            Debug.Log ("ResponseSDStartGame called.");
            ResponseSDStartGameEventArgs args = eventArgs as ResponseSDStartGameEventArgs;

            if (args.status == 0) {
                SceneManager.LoadScene ("SDGameMain");
            } else {
                Debug.Log ("Encountered an error in starting the game.");
            }
        }

    }
}
