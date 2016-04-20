using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace SD {
    public class SDReadySceneManager : MonoBehaviour {

        private SDConnectionManager cManager;
        private SDMessageQueue mQueue;
        private static SDReadySceneManager sceneManager;
        private bool _isPlayerReady, _isOpponentReady;
        private bool isPlayerReady { 
            get {
                return this._isPlayerReady;
            }
            set {
                this._isPlayerReady = value;
                if (this._isPlayerReady && this.isOpponentReady) {
                    Debug.Log ("Loading the Game Scene..");
                    SceneManager.LoadScene ("SDGameMain");
                }
            }
        }
        private bool isOpponentReady {
            get {
                return this._isOpponentReady;
            }
            set {
                this._isOpponentReady = value;
                if (this._isPlayerReady && this._isOpponentReady) {
                    Debug.Log ("Loading the Game Scene..");
                    SceneManager.LoadScene ("SDGameMain");
                }
            }
        }

        public static SDReadySceneManager getInstance() {
            return sceneManager;
        }

        // Use this for initialization
        void Start () {
            sceneManager = this;
            cManager = SDConnectionManager.getInstance ();
            mQueue = SDMessageQueue.getInstance ();
            isPlayerReady = false;
            isOpponentReady = false;

            if (cManager && mQueue) {
                if (!mQueue.callbackList.ContainsKey(Constants.SMSG_SDSTART_GAME))
                    mQueue.AddCallback (Constants.SMSG_SDSTART_GAME, ResponseSDStartGame);
                if (mQueue.callbackList.ContainsKey(Constants.SMSG_POSITION))  // Remove when we create a new protocol.
                    mQueue.RemoveCallback (Constants.SMSG_POSITION);
                if (mQueue.callbackList.ContainsKey (Constants.SMSG_POSITION))
                    mQueue.RemoveCallback (Constants.SMSG_POSITION);
                if (!mQueue.callbackList.ContainsKey (Constants.SMSG_POSITION))
                    mQueue.AddCallback (Constants.SMSG_POSITION, ResponseSDStartSync);
            } else {
                Debug.LogWarning ("Could not obtain a connection to Sea Divided Server. Falling back to offline mode.");
            }
        }

        public void StartGame() {
            if (cManager) {
                RequestSDStartGame request = new RequestSDStartGame ();
                request.Send (Constants.USER_ID);
                cManager.Send (request);
                isPlayerReady = true;
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
                // Send a request to the opponent indicating that this client is ready to play.
                RequestSDPosition request = new RequestSDPosition();
                request.Send (0.ToString (), 0.ToString ());
                cManager.Send (request);
                Debug.Log ("Waiting for opponent to respond..");
            } else {
                Debug.Log ("Encountered an error in starting the game.");
            }
        }

        public void ResponseSDStartSync(ExtendedEventArgs eventArgs) {
            Debug.Log ("The opponent is ready to play, loading the game scene.");
            this.isOpponentReady = true;
            //StartCoroutine (WaitToStartGame());
        }

        public IEnumerator WaitToStartGame() {
            while (!(isPlayerReady && isOpponentReady)) {
                yield return new WaitForSeconds (0.001f); 
            }
            SceneManager.LoadScene ("SDGameMain");
        }
    }
}
