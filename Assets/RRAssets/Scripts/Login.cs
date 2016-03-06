using UnityEngine;

using System;
using System.Collections;
using System.IO;
using System.Net.Sockets;

namespace RR
{
	public class Login : MonoBehaviour
	{
		private GameObject mainObject;
		// Window Properties
		private float width = 280;
		private float height = 100;
		// Other
		public Texture background;
		private string user_id = "";
		private string password = "";
		private Rect windowRect;
		private bool isHidden;
		
		void Awake ()
		{
			mainObject = GameObject.Find ("MainObject");
			
			mainObject.AddComponent<RRMessageQueue>();
			mainObject.AddComponent<RRConnectionManager>();
			mainObject.GetComponent<RRMessageQueue> ().AddCallback (Constants.SMSG_AUTH, ResponseLogin);
			//		mainObject.GetComponent<MessageQueue>().AddCallback(Constants.SMSG_SPECIES_LIST, ResponseSpeciesList);
			mainObject.GetComponent<RRMessageQueue> ().AddCallback (Constants.SMSG_RACE_INIT, ResponseRaceInit);
		}
		
		// Use this for initialization
		void Start ()
		{
			RRConnectionManager cManager = mainObject.GetComponent<RRConnectionManager> ();
			
			if (cManager) {
				//			cManager.Send(new RequestSpeciesList());
			}
		}
		
		void OnGUI ()
		{
			// Background
			GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), background);
			
			// Client Version Label
			GUI.Label (new Rect (Screen.width - 75, Screen.height - 30, 65, 20), "v" + Constants.CLIENT_VERSION + " Test");
			
			// Login Interface
			if (!isHidden) {
				windowRect = new Rect ((Screen.width - width) / 2, Screen.height / 2 - height, width, height);
				windowRect = GUILayout.Window ((int)Constants.GUI_ID.Login, windowRect, MakeWindow, "Login");
			
				if (Event.current.type == EventType.KeyUp && Event.current.keyCode == KeyCode.Return) {
					Submit ();
				}
			}
	
			if (GUI.Button (new Rect (Screen.width * 1 / 3 - 150, Screen.height - 75, 100, 30), "Join Race")) {
				Join ();
			}
	
			if (GUI.Button (new Rect (Screen.width * 2 / 3 - 150, Screen.height - 75, 100, 30), "single player")) {
	
			}
	
			if (GUI.Button (new Rect (Screen.width * 3 / 3 - 150, Screen.height - 75, 100, 30), "two player")) {
	
			}
			
		}
		
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
		
		public void Submit ()
		{
			user_id = user_id.Trim ();
			password = password.Trim ();
			
			if (user_id.Length == 0) {
				Debug.Log ("User ID Required");
				GUI.FocusControl ("username_field");
			} else if (password.Length == 0) {
				Debug.Log ("Password Required");
				GUI.FocusControl ("password_field");
			} else {
				//			Debug.Log(mainObject.GetInstanceID());
				RRConnectionManager cManager = mainObject.GetComponent<RRConnectionManager> ();
				
				if (cManager) {
					cManager.Send (RequestLogin (user_id, password));
				}
			}
		}
	
		// Join makes an attempt to create and or join an online game.
		public void Join ()
		{
			RRConnectionManager cManager = mainObject.GetComponent<RRConnectionManager> ();
	
			if (cManager) {
				cManager.Send (RequestRaceInit ());
			}
		}
	
		public RequestRaceInit RequestRaceInit ()
		{
			RequestRaceInit request = new RequestRaceInit ();
			request.Send (Constants.USER_ID, 0);
			return request;
		}
	
		public void ResponseRaceInit (ExtendedEventArgs eventArgs)
		{
			ResponseRaceInitEventArgs args = eventArgs as ResponseRaceInitEventArgs;
	
			if (args.status == 0) {
				Application.LoadLevel ("RRReadyScene");
			} else {
				Join ();
			}
		}
		
		public RequestLogin RequestLogin (string username, string password)
		{
			RequestLogin request = new RequestLogin ();
			request.send (username, password);
			
			return request;
		}
	
		public void ResponseLogin (ExtendedEventArgs eventArgs)
		{
			ResponseLoginEventArgs args = eventArgs as ResponseLoginEventArgs;
			
			if (args.status == 0) {
				Constants.USER_ID = args.user_id;
			} else {
				Debug.Log ("Login Failed");
			}
		}
		
		public void Show ()
		{
			isHidden = false;
		}
		
		public void Hide ()
		{
			isHidden = true;
		}
		
		// Update is called once per frame
		void Update ()
		{
		}
	}
}