using UnityEngine;

using System.Collections;
using System.Collections.Generic;

namespace SD
{
    public class SDMain : MonoBehaviour
    {

        void Awake()
        {
            DontDestroyOnLoad(gameObject);

            gameObject.AddComponent<SDMessageQueue>();
            gameObject.AddComponent<SDConnectionManager>();

            NetworkRequestTable.init();
            NetworkResponseTable.init();
        }

        // Use this for initialization
        void Start()
        {
            //Application.LoadLevel("RRLogin");
            SDConnectionManager sManager = gameObject.GetComponent<SDConnectionManager>();
            //if (sManager)
            //{
            //    StartCoroutine(RequestHeartbeat(1f));
            //}
        }

        // Update is called once per frame
        void Update()
        {
        }

        public IEnumerator RequestHeartbeat(float time)
        {
            yield return new WaitForSeconds(time);

            SDConnectionManager sManager = gameObject.GetComponent<SDConnectionManager>();

            if (sManager)
            {
                //RequestHeartbeat request = new RequestHeartbeat();
                //request.Send();

               // cManager.Send(request);
            }

            //originially 1f changed to .1f
            StartCoroutine(RequestHeartbeat(1f));
        }
    }
}