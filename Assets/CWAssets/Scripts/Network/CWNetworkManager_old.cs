using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
namespace CW
{
    public class CWNetworkManager_old : MonoBehaviour
    {
        public delegate void Callback (NetworkResponse response);

        private Dictionary<int, Queue<Callback>> callbackList = new Dictionary<int, Queue<Callback>> ();
        private Dictionary<int, List<Callback>> listenList = new Dictionary<int, List<Callback>> ();
        // Connection
        private ConnectionManager cManager;


        private Queue<NetworkRequest> requests = new Queue<NetworkRequest> ();
        private int counter = 0;
        private int interval = 50;
        static private CWNetworkManager cwNetworkMgrInstance;

       

        public static CWNetworkManager getInstance()
        {
            Debug.Log("inside cw nm getinstance");
            if (cwNetworkMgrInstance != null)
                return cwNetworkMgrInstance;
            else
            {
                cwNetworkMgrInstance = new CWNetworkManager();
                return cwNetworkMgrInstance;
            }
        }

        private CWNetworkManager()
        {
            cManager=new ConnectionManager(Config.REMOTE_HOST,CW.Constants.REMOTE_PORT);

           // StartNW();
        }

    
        // Use this for initialization
        void Start()
        {
            Debug.Log("inside cw nm start");
            if (cManager.Connect() == ConnectionManager.SUCCESS) {
                Debug.Log("CW nw mgr success");
                /*Send (
                ClientProtocol.Prepare (Constants.CLIENT_VERSION, Constants.SESSION_ID),
                ProcessClient
                );*/
            }
        
            //StartCoroutine (Poll (Constants.HEARTBEAT_RATE));
        }
    
        // Update is called once per frame
        public void Update()
        {
            
            if (!cManager.Connected)
            {
                Debug.Log("con manager cow not connected");
                return;
            }
            if(requests.Count>0)
                Debug.Log("request count>0 in update()");
            while (requests.Count > 0)
            {
                Debug.Log("sending the request now...");
                NetworkRequest packet = requests.Peek();

                if (cManager.Send(packet.GetBytes()))
                {
                    requests.Dequeue();

                    if (packet.GetID() != 211)
                    {
                        // commented by Rujoota
                        Debug.Log ("CW: Sent Request No. " + packet.GetID () + " [" + NetworkProtocolTable.Get (packet.GetID ()).ToString () + "]");
                    }

                }
            }

            counter++;
            if (counter == interval) {
                //Debug.Log ("checking response buffer... (+50)");
                counter = 0;
            }
            foreach (NetworkResponse args in cManager.Read()) {
                Debug.Log("Checking response");
                bool status = false;

                int protocol_id = args.GetID ();

                // One-Time
                if (callbackList.ContainsKey (protocol_id)) {
                    if (callbackList [protocol_id].Count > 0) {
                        callbackList [protocol_id].Dequeue () (args);


                        foreach (Callback callback in callbackList[protocol_id]) {
                            callback (args);
                        }

                        status = true;
                    }
                }
                // Listen
                if (listenList.ContainsKey (protocol_id)) {
                    if (listenList [protocol_id].Count > 0) {
                        foreach (Callback callback in listenList[protocol_id]) {
                            callback (args);
                        }

                        status = true;
                    }
                }

                if (args.GetID () != 211)
                    Debug.Log ((status ? "Processed" : "Ignored") + " Response No. " + args.GetID () + " [" + NetworkProtocolTable.Get (args.GetID ()).ToString () + "]");
            }
        }

        public void Send(NetworkRequest packet)
        {
            requests.Enqueue (packet);
            Debug.Log("inside cw nm send(pkt). requests.count="+requests.Count);
            //UpdateNW();
        }
    
        public void Send(NetworkRequest packet, Callback callback)
        {
            Debug.Log("inside cw nm Send(pkt,callback)");
            //Send (packet);
            requests.Enqueue (packet);
            int protocol_id = packet.GetID ();

            if (!callbackList.ContainsKey (protocol_id)) {
                callbackList [protocol_id] = new Queue<Callback> ();
            }

            callbackList [protocol_id].Enqueue (callback);
            Debug.Log("inside cw nm send(pkt,callback). requests.count="+requests.Count);
        }

        public void Listen(int protocol_id, Callback callback)
        {
            if (!listenList.ContainsKey (protocol_id)) {
                listenList [protocol_id] = new List<Callback> ();
            }

            listenList [protocol_id].Add (callback);
        }

        public void Ignore(int protocol_id, Callback callback)
        {
            if (listenList.ContainsKey (protocol_id) && listenList [protocol_id].Contains (callback)) {
                while (listenList[protocol_id].Contains(callback)) {
                    listenList [protocol_id].Remove (callback);
                }
            } else {
                Debug.LogError ("Callback for Protocol [" + protocol_id + "] Does Not Exist");
            }
        }

        public void Clear ()
        {
            callbackList.Clear ();
            listenList.Clear ();
            requests.Clear ();
        }
    
        private IEnumerator Poll (float time)
        {
            while (true) {
                if (cManager.Connected) {
                    cManager.Send (HeartbeatProtocol.Prepare ().GetBytes ());
                    yield return new WaitForSeconds (time);
                } else {
                    cManager.Reconnect ();
                    yield return new WaitForSeconds (3.0f);
                }
            }
        }
    
        public void ProcessClient (NetworkResponse response)
        {
            ResponseClient args = response as ResponseClient;
            Constants.SESSION_ID = args.session_id;
        }
    }
}