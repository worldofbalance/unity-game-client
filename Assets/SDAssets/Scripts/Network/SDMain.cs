using UnityEngine;

using System.Collections;
using System.Collections.Generic;

namespace SD
{
    public class SDMain : MonoBehaviour
    {
        public static NetworkManager networkManager;

        void Awake()
        {
            networkManager = new NetworkManager(
                this,
                new ConnectionManager(Config.GetHost(), SD.Constants.REMOTE_PORT), 
                false
            );

            DontDestroyOnLoad(gameObject);

        }

        // Use this for initialization
        void Start()
        {
            StartCoroutine (RequestInGameHeartbeat()); 
        }

        // Update is called once per frame
        void Update()
        {
            networkManager.Update();
        }

        void OnDestroy() {
            StopCoroutine (RequestInGameHeartbeat());
        }
 
        IEnumerator RequestInGameHeartbeat()
        {
            
            while(true) {
                // Send a heartbeat request only if we are actually in a game.
                if (SDPersistentData.getInstance () != null && networkManager != null) {
                    networkManager.Send (SDHeartbeatProtocol.Prepare ());
                }
                yield return new WaitForSeconds(Constants.HEARTBEAT_SECONDS);
            }
        }

    }
}
