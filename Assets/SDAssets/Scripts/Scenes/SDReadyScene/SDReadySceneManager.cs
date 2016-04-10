using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace SD {
    public class SDReadySceneManager : MonoBehaviour {

        private SDConnectionManager cManager;
        private SDMessageQueue mQueue;
        private static SDReadySceneManager sceneManager;

        public void SDReadySceneMananger() {
        }

        public static SDReadySceneManager getInstance() {
            return sceneManager;
        }

        // Use this for initialization
        void Start () {
            sceneManager = this;
            cManager = SDConnectionManager.getInstance ();
            mQueue = SDMessageQueue.getInstance ();
            if (cManager && mQueue) {
                mQueue.AddCallback (Constants.SMSG_SDSTART_GAME, ResponseSDStartGame);
            } else {
                Debug.LogWarning ("Could not obtain a connection to Sea Divided Server. Falling back to offline mode.");
            }
        }

        public void StartGame() {
            if (cManager) {
                RequestSDStartGame request = new RequestSDStartGame ();
                request.Send (Constants.USER_ID);
                cManager.Send (request);
            } else {
                Debug.LogWarning ("Starting game without server component.");
                SceneManager.LoadScene ("SDGameMain");
            }
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
