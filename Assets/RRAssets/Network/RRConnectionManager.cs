using UnityEngine;
using System.Collections;

using System;
using System.IO;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;

namespace RR
{
    /// <summary>
    /// Running Rhino client to server connection manager.
    /// </summary>
    public class RRConnectionManager : MonoBehaviour
    {

        // The scene's main object
        private GameObject mainObject;

        // Socket between server and client
        private TcpClient mySocket;

        // Data stream
        private NetworkStream theStream;

        // Is the socket already connected or not?
        private bool socketReady = false;

        // Instance of singleton
        static private RRConnectionManager sInstance;

        /// <summary>
        /// Gets the instance of singleton RRConnectionManager.
        /// </summary>
        /// <returns>RRConnectionManager</returns>
        public static RRConnectionManager getInstance() {
            return sInstance;
        }

        /// <summary>
        /// Awake is called only once during the lifetime
        /// of the script instance when the script instance is being loaded.
        /// </summary>
        void Awake ()
        {
            sInstance = this;
            mainObject = GameObject.Find ("MainObject");
        }

        /// <summary>
        /// Start is called on the frame when a script is enabled just
        /// before any of the Update methods is called the first time.
        /// </summary>
        void Start ()
        {
            //SetupSocket ();
        }

        /// <summary>
        /// Setups the socket.
        /// </summary>
        public void SetupSocket ()
        {
            // Is the socket already connected?
            if (socketReady) {
                Debug.Log ("Already Connected");
                return;
            }

            try {
                mySocket = new TcpClient (RR.Constants.REMOTE_HOST, RR.Constants.REMOTE_PORT);
                theStream = mySocket.GetStream ();
                socketReady = true;

                // TODO debug, report
                Debug.Log ("Connecting to host \"" + RR.Constants.REMOTE_HOST + "\" on port " + RR.Constants.REMOTE_PORT);

                // TODO What is this???
                // RequestLogin login = new RequestLogin();
                // //Hardcoded login only for testing purposes.
                // Send(login.send("1","1"));

            } catch (Exception e) {
                Debug.Log ("Socket error: " + e);
            }
        }

        /// <summary>
        /// Reads the input from the socket's data stream.
        /// This method should be called on every frame.
        /// </summary>
        public void ReadSocket ()
        {
            if (!socketReady) {
                // Socket is not set yet
                return;
            }

            if (!theStream.DataAvailable) {
                // There is no available data in the stream
                // This is about to happen in most frames
                return;
            }

            // What size should the buffer allocate for the response?
            byte[] buffer = new byte[2];
            theStream.Read (buffer, 0, 2);
            short bufferSize = BitConverter.ToInt16 (buffer, 0);

            // Make a memory stream from the buffer
            buffer = new byte[bufferSize];
            theStream.Read (buffer, 0, bufferSize);
            MemoryStream dataStream = new MemoryStream (buffer);

            // Get the response from the stream
            short response_id = DataReader.ReadShort (dataStream);
            NetworkResponse response = RR.NetworkResponseTable.get (response_id);

            // TODO debug, report
            Debug.Log ("New response with id " + response_id + " has been received");

            if (response != null) {
                response.dataStream = dataStream;

                // Process the response data
                response.parse ();
                ExtendedEventArgs args = response.process ();

                // Add these data to the response queue
                if (args != null) {
                    RR.RRMessageQueue msgQueue = RR.RRMessageQueue.getInstance ();
                    msgQueue.AddMessage (args.event_id, args);
                }
            }
        }

        /// <summary>
        /// Closes the socket.
        /// </summary>
        public void CloseSocket ()
        {
            if (!socketReady) {
                return;
            }
            mySocket.Close ();
            socketReady = false;
        }

        /// <summary>
        /// Send the specified request.
        /// </summary>
        /// <param name="request">NetworkRequest</param>
        public void Send (NetworkRequest request)
        {
            if (!socketReady) {
                return;
            }

            GamePacket packet = request.packet;

            byte[] bytes = packet.getBytes ();
            theStream.Write (bytes, 0, bytes.Length);

            // TODO debug, report
            if (request.request_id != RR.Constants.CMSG_HEARTBEAT) {
                Debug.Log ("Sent Request #: " + request.request_id + " [" + request.ToString () + "]");
            }
        }

        /// <summary>
        /// Update is called once per frame
        /// </summary>
        void Update ()
        {
            ReadSocket ();
        }
    }
}