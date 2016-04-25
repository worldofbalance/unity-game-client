using UnityEngine;

using System;
using System.Collections;

namespace CW
{
	public class Login : MonoBehaviour
	{
	
		private int window_id = Constants.LOGIN_WIN;
		// Window Properties
		private float left;
		private float top;
		private float width = 280;
		private float height = 280;
		// private float height = 190;
		// Cards of wild : true if test being run
		bool testing = false;

		// Other
		public Texture background;
		private Texture2D bgTexture;
		private Font font;
		private string user_id = "";
		private string password = "";
		private Rect windowRect;
		private bool isActive = true;
		private bool isInitial = true;
		// Tests
		//private Tests tests = new Tests();
		// Hardcded player2 for match initialization in built version
		private int playerID2 = Constants.PLAYER2ID;
		// Hardcded player2 for match initialization in built version

		void Awake ()
		{
			//DontDestroyOnLoad(gameObject);
		
			left = (Screen.width - width) / 2;
			top = (Screen.height - height) / 2;
		
			windowRect = new Rect (left, top, width, height);
		
			bgTexture = Resources.Load<Texture2D> (Constants.THEME_PATH + Constants.ACTIVE_THEME + "/gui_bg");
			font = Resources.Load<Font> ("Fonts/" + "Chalkboard");
		}
	
		// Use this for initialization
		void Start ()
		{
			//StartCoroutine("AutoLogin");
		}
	
		// Update is called once per frame
		void Update ()
		{
		
		}
	
		void OnDestroy ()
		{
		
		}
	
		void OnGUI ()
		{
			// Background
			GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), background, ScaleMode.ScaleAndCrop);
		
			// Client Version Label
			GUI.Label (new Rect (Screen.width - 75, Screen.height - 30, 65, 20), "v" + Constants.CLIENT_VERSION + " Beta");
		
			// Login Interface
			if (isActive) {
				windowRect = GUI.Window (window_id, windowRect, MakeWindow, "Login", GUIStyle.none);
			
				if (Event.current.type == EventType.KeyUp && Event.current.keyCode == KeyCode.Return) {
					Submit ();
				}
			}
		}
	
		void MakeWindow (int id)
		{
			Functions.DrawBackground (new Rect (0, 0, width, height), bgTexture);
		
			GUIStyle style = new GUIStyle (GUI.skin.label);
			style.alignment = TextAnchor.UpperCenter;
			style.font = font;
			style.fontSize = 16;
		
			GUI.Label (new Rect ((windowRect.width - 100) / 2, 0, 100, 30), "Login", style);
		
			GUI.BeginGroup (new Rect (10, 25, 300, 100));
			{
				style.alignment = TextAnchor.UpperLeft;
				style.fontSize = 14;
				GUI.Label (new Rect (0, 0, 300, 30), "User ID (Display Name or Email)", style);
				GUI.SetNextControlName ("username_field");
				user_id = GUI.TextField (new Rect (0, 25, windowRect.width - 20, 25), user_id, 25);
			}
			GUI.EndGroup ();
		
			GUI.BeginGroup (new Rect (10, 80, 300, 100));
			style.alignment = TextAnchor.UpperLeft;
			style.fontSize = 14;
			GUI.Label (new Rect (0, 0, 100, 30), "Password", style);
			GUI.SetNextControlName ("password_field");
			password = GUI.PasswordField (new Rect (0, 25, windowRect.width - 20, 25), password, "*" [0], 25);
			GUI.EndGroup ();
		
			if (isInitial) {  // && GUI.GetNameOfFocusedControl() == "") {
				GUI.FocusControl ("username_field");
				isInitial = false;
			}
		
			if (GUI.Button (new Rect (width / 2 - 110, 145, 100, 30), "Log In")) {
				Debug.Log ("Login script logging in");
				Submit ();
			}
		
			if (GUI.Button (new Rect (width / 2 + 10, 145, 100, 30), "Register")) {
				SwitchToRegister ();
			}
			//	 Test Button
			if (GUI.Button (new Rect (width / 2 + 10, 185, 100, 30), "Initialize Match")) {
				InitMatch ();
			}
			if (GUI.Button (new Rect (width / 2 - 110, 185, 100, 30), "Enter Battle")) {
				EnterBattle ();
			}
		}

		public void EnterBattle ()
		{

			Game.SwitchScene ("Battle");
		}
	
		public void  InitMatch ()
		{
			int playerID1 = GameState.player.GetID ();
			CWGame.networkManager.Send (
					MatchInitProtocol.Prepare (GameState.player.GetID (), playerID2), ProcessMatchInit
			);
			Debug.Log ("Init for player: " + playerID1);
		}
	
		public void ProcessMatchInit (NetworkResponse response)
		{
			ResponseMatchInit args = response as ResponseMatchInit;

			if (args.status == 0) {
				GameState.matchID = args.matchID;
				Debug.Log ("MatchID set to: " + args.matchID);
			}
		}

		public void Submit ()
		{
			user_id = user_id.Trim ();
			password = password.Trim ();
		
			if (user_id.Length == 0) {
				//			mainObject.GetComponent<Main>().CreateMessageBox("User ID Required");
				GUI.FocusControl ("username_field");
			} else if (password.Length == 0) {
				//			mainObject.GetComponent<Main>().CreateMessageBox("Password Required");
				GUI.FocusControl ("password_field");
			} else {
				CWGame.networkManager.Send (
				LoginProtocol.Prepare (user_id, password),
				ProcessLogin
				);
			}
		}
	
		public IEnumerator AutoLogin ()
		{
			while (true) {
				user_id = "1";
				password = "1";
				Submit ();
			
				yield return new WaitForSeconds (1.0f);
			}
		}
	
		public void ProcessLogin (NetworkResponse response)
		{
			/*
			ResponseLogin args = response as ResponseLogin;
		
			if (args.status == 0) {
				GameState.account = args.account;
			
				CWGame.networkManager.Send (
				PlayerSelectProtocol.Prepare (0),
				ProcessPlayerSelect
				);
			
			} else {
				Debug.Log ("login failed, server message = " + args.status);
				//mainObject.GetComponent<Main>().CreateMessageBox("Login Failed");
			}
			*/
		}
	
		public void ProcessPlayerSelect (NetworkResponse response)
		{
			/*
			ResponsePlayerSelect args = response as ResponsePlayerSelect;

			//TODO: scene is null 
			if (args.status == 0) {
				GameState.player = args.player;
				//Application.LoadLevel("Battle");
				//Game.SwitchScene("BattleMainMenu"); //"World");
				Debug.Log ("Switching to CWBattleMainMenu");
			 
			}
			*/
		}
	
		public void SwitchToRegister ()
		{
			isActive = false;
			gameObject.AddComponent <Register>();
		}
	
		public void SetActive (bool active)
		{
			this.isActive = active;
			//reset GUI focus if reactivating login.
			this.isInitial = this.isInitial || this.isActive;
		}
	}
}