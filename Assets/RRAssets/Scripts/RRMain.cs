using UnityEngine;

using System.Collections;
using System.Collections.Generic;

namespace RR {

    /// <summary>
    /// Main class of the game, the whole code
    /// should be running from this.
    /// </summary>
    public class RRMain : MonoBehaviour {

        /// <summary>
        /// Awake is called only once during the lifetime
        /// of the script instance when the script instance is being loaded.
        /// </summary>
        void Awake() {
            DontDestroyOnLoad(gameObject);
            
            gameObject.AddComponent<RRMessageQueue>();    
            gameObject.AddComponent<RRConnectionManager>();
    
            NetworkRequestTable.init();
            NetworkResponseTable.init();
        }
        
        /// <summary>
        /// Start is called on the frame when a script is enabled just
        /// before any of the Update methods is called the first time.
        /// </summary>
        void Start () {
            Application.LoadLevel("RRLogin");

            RRConnectionManager cManager = gameObject.GetComponent<RRConnectionManager>();
            if (cManager) {
                StartCoroutine(RequestHeartbeat(1f));
            }
        }
        
        /// <summary>
        /// Update is called once per frame
        /// </summary>
        void Update () {
        }

        /// <summary>
        /// Requests the heartbeat.
        /// </summary>
        /// <returns>The heartbeat.</returns>
        /// <param name="time">Time.</param>
        public IEnumerator RequestHeartbeat(float time) {
            yield return new WaitForSeconds(time);
            
            RRConnectionManager cManager = gameObject.GetComponent<RRConnectionManager>();
            
            if (cManager) {
                RequestHeartbeat request = new RequestHeartbeat();
                request.Send();
                
                cManager.Send(request);
            }
            
            // Originially 1f changed to .1f
            StartCoroutine(RequestHeartbeat(1f));
        }
    }
}