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
        }

        // Update is called once per frame
        void Update()
        {
            networkManager.Update();
        }
    }
}