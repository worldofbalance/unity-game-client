using UnityEngine;

using System;
using System.Collections;
using System.IO;
using System.Net.Sockets;

namespace RR
{
    public class Login : MonoBehaviour
    {
        // The scene's main object
        private GameObject mainObject;

        // Window Properties
        private float windowWidth = 280;
        private float windowHeight = 100;

        // Login Credentials
        private string user_id = "";
        private string password = "";

        // Background texture - should be assigned in Unity Editor
        public Texture background;

        // Rectangle which keeps the login window
        private Rect windowRect;

        //
        private bool isHidden;

        // It the user logged in?
        private bool userIsLoggedIn;
    
        /// <summary>
        /// Awake is called only once during the lifetime
        /// of the script instance when the script instance is being loaded.
        /// </summary>
        void Awake ()
        {
            /*
             * Setup components
             */
            mainObject = GameObject.Find ("MainObject");
            mainObject.AddComponent<RRMessageQueue>();
            mainObject.AddComponent<RRConnectionManager>();

            /*
             * Message Queue callbacks 
             */
            RR.RRMessageQueue.getInstance ().AddCallback (RR.Constants.SMSG_AUTH, RRResponseLogin);
//            mainObject.GetComponent<MessageQueue>().AddCallback(Constants.SMSG_SPECIES_LIST, ResponseSpeciesList);
            RR.RRMessageQueue.getInstance ().AddCallback (Constants.SMSG_RACE_INIT, ResponseRaceInit);
        }
        
        /// <summary>
        /// Start is called on the frame when a script is enabled just
        /// before any of the Update methods is called the first time.
        /// </summary>
        void Start ()
        {
            RRConnectionManager cManager = mainObject.GetComponent<RRConnectionManager> ();
            
            if (cManager) {
                // TODO There is probably no reason to call this
                // cManager.Send(new RequestSpeciesList());
            }
        }
        
        /// <summary>
        /// Raises the GU event.
        /// GUI is recreated every frame
        /// </summary>
        void OnGUI ()
        {
            // Background
            GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), background);

            // Client Version Label
            GUI.Label (new Rect (Screen.width - 75, Screen.height - 30, 65, 20), "v" + Constants.CLIENT_VERSION + " Test");

            // Login Interface
            if (!isHidden) {

                // Makes the main login window with all the interface
                windowRect = GUILayout.Window (
                    (int)Constants.GUI_ID.Login,            // window ID
                    new Rect (                              // Screen Rectangle
                        (Screen.width - windowWidth) / 2,   // left
                        Screen.height / 2 - windowHeight,   // top
                        windowWidth,
                        windowHeight
                    ),
                    MakeWindow, // Winndow Creation Method
                    "Login"     // Window Caption
                );

                // Login capability by pressing a return key
                if (Event.current.type == EventType.KeyUp && Event.current.keyCode == KeyCode.Return) {
                    Submit ();
                }

                if (GUI.Button (new Rect (Screen.width * 1 / 3 - 150, Screen.height - 75, 100, 30), "Join Race")) {
                    Join ();
                }

                // TODO Make these buttons active
                // if (GUI.Button (new Rect (Screen.width * 2 / 3 - 150, Screen.height - 75, 100, 30), "Single Player")) {}
                // if (GUI.Button (new Rect (Screen.width * 3 / 3 - 150, Screen.height - 75, 100, 30), "Two Player")) {}
            }
        }

        /// <summary>
        /// Makes the window.
        /// </summary>
        /// <param name="id">Identifier.</param>
        void MakeWindow (int id)
        {
            GUILayout.Label ("User ID");
            GUI.SetNextControlName ("username_field");
            user_id = GUI.TextField (new Rect (10, 45, windowRect.width - 20, 25), user_id, 25);

            GUILayout.Space (30);
            
            GUILayout.Label ("Password");
            GUI.SetNextControlName ("password_field");
            password = GUI.PasswordField (new Rect (10, 100, windowRect.width - 20, 25), password, "*" [0], 25);

            GUILayout.Space (75);

            if (GUI.Button (new Rect (windowRect.width / 2 - 50, 135, 100, 30), "Log In")) {
                Submit ();
            }
        }

        /// <summary>
        /// This method is triggered when login process is about
        /// to take place
        /// </summary>
        public void Submit ()
        {
            user_id = user_id.Trim ();
            password = password.Trim ();

            // Is username field empty?
            if (user_id.Length == 0) {

                Debug.Log ("User ID Required");
                GUI.FocusControl ("username_field");

            } else

            // Is password field empty?
            if (password.Length == 0) {

                Debug.Log ("Password Required");
                GUI.FocusControl ("password_field");

            }

            else {

                //Everything is going well, lets send a login request to the server
                RRConnectionManager rrcm = mainObject.GetComponent<RRConnectionManager> ();

                if (rrcm) {
                    rrcm.Send (RequestLogin (user_id, password));
                }
            }
        }

        /// <summary>
        /// Join makes an attempt to create and or join an online game.
        /// </summary>
        public void Join ()
        {
            RRConnectionManager rrcm = mainObject.GetComponent<RRConnectionManager> ();
    
            if (rrcm) {
                rrcm.Send (RequestRaceInit ());
            }
        }

        /// <summary>
        /// Builds the sendable request to login
        /// </summary>
        /// <returns>RequestLogin</returns>
        /// <param name="username">Username</param>
        /// <param name="password">Password</param>
        public RequestLogin RequestLogin (string username, string password)
        {
            RequestLogin request = new RequestLogin ();
            request.send (username, password);

            return request;
        }

        /// <summary>
        /// Callback that copes with the incoming login response
        /// </summary>
        /// <param name="eventArgs">Event arguments.</param>
        public void RRResponseLogin (ExtendedEventArgs eventArgs)
        {
            ResponseLoginEventArgs args = eventArgs as ResponseLoginEventArgs;

            if (args.status == 0) {
                Constants.USER_ID = args.user_id;
            } else {
                Debug.Log ("Login Failed");
            }
        }

        /// <summary>
        /// The interface window is going to be created.
        /// </summary>
        public void Show ()
        {
            isHidden = false;
        }

        /// <summary>
        /// The interface window is NOT going to be created.
        /// </summary>
        public void Hide ()
        {
            isHidden = true;
        }

        /// <summary>
        /// Update is called once per frame
        /// </summary>
        void Update ()
        {
        }

        /// <summary>
        /// Builds a RaceInit Request
        /// </summary>
        /// <returns>The race init.</returns>
        public RequestRaceInit RequestRaceInit ()
        {
            RequestRaceInit request = new RequestRaceInit ();
            request.Send (Constants.USER_ID, 0);
            return request;
        }

        /// <summary>
        /// User is signed in and server is ready to introduce him to the game
        /// </summary>
        /// <param name="eventArgs">Event arguments.</param>
        public void ResponseRaceInit (ExtendedEventArgs eventArgs)
        {
            ResponseRaceInitEventArgs args = eventArgs as ResponseRaceInitEventArgs;

            if (args.status == 0) {
                Application.LoadLevel ("RRReadyScene");
            } else {
                Join ();
            }
        }
    }
}