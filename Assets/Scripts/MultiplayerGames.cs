using UnityEngine;
using System;
using System.Collections;
using System.IO;
using System.Text;

public class MultiplayerGames : MonoBehaviour {
	
	private GameObject mainObject;

	// Window Properties
	private float width = 500;
	private float height = 300;
	private float heightOffset = 50;    // DH

	// Multiplayer Convergence Window Properties    DH
	private float widthMC = 500;
	private float heightMC;       // Set based upon left over space
	private float heightMCSpace = 50;  

	// DH  Other additions
	private bool enableHostEntry = false;
	private float widthConfig;
	private float heightConfig;
	private float topConfig;
	private float leftConfig;
	private int pixelPerChar = 15;
	private Texture2D bgTexture;
	// Parameters that host specifies
	private short numPlayers = 0;
	private string numPlayersS = "";
	private short numRounds = 0;
	private string numRoundsS = "";
	private short timeWindow = 0;
	private string timeWindowS = "";
	private short betAmount = 0;
	private string betAmountS = "";
	private short ecoCount = 0;
	private short ecoNumber = 0;
	private string ecoNumberS = "";
	private short allowSliders = 0;    // 1 = display *** above sliders
	private string allowSlidersS = "";
	private bool isInitial = true;
	private int host_config_id = Constants.CONVERGE_HOST_CONFIG;
	private Font font;
	private short host;

	// Other
	private int window_id, window_idMC;
	private string message = "Single Player Game. Click start to play.";
	private Rect windowRect, windowRectMC, windowRectConfig;

	private bool enableRRButton = true;
	private bool enableCWButton = true;
	private bool enableMCButton = true;
    private bool enableSDButton = true;

	private bool quiting = false;
	private bool waiting = false;

	private int room_id = 0;
	private Room room;

	void Awake() {
		mainObject = GameObject.Find("MainObject");
		window_id = Constants.GetUniqueID();
		window_idMC = Constants.GetUniqueID();

		Game.networkManager.Listen (NetworkCode.PAIR, OnPairResult);
		Game.networkManager.Listen (NetworkCode.QUIT_ROOM, OnQuitRoomResult);

		// DH other values 
		widthConfig = Screen.width * 0.80f;
		heightConfig = Screen.height * 0.80f;
		leftConfig = (Screen.width - widthConfig) / 2;
		topConfig = (Screen.height - heightConfig) / 2;
		bgTexture = Resources.Load<Texture2D> (Constants.THEME_PATH + Constants.ACTIVE_THEME + "/gui_bg");
		font = Resources.Load<Font> ("Fonts/" + "Chalkboard");
		windowRectConfig = new Rect (leftConfig, topConfig, widthConfig, heightConfig);
	}

	void OnDestroy () {
		Game.networkManager.Ignore (NetworkCode.PAIR, OnPairResult);
		Game.networkManager.Ignore (NetworkCode.QUIT_ROOM, OnQuitRoomResult);
	}

	// Use this for initialization
	void Start() {
		Game.StartEnterTransition ();
		// DH
		ReadConvergeEcosystemsFileCount();
		windowRect = new Rect ((Screen.width - width) / 2, Screen.height - height - heightOffset, width, height);

		heightMC = Screen.height - height - heightOffset - heightMCSpace;
		windowRectMC = new Rect ((Screen.width - widthMC) / 2, heightMCSpace/2, widthMC, heightMC);

		StartCoroutine(RequestGetRooms(1f));
	}

	void OnGUI() {

		if (enableHostEntry) {
			windowRectConfig = GUI.Window (host_config_id, windowRectConfig, MakeWindowHost, "Host Configuration Entry", GUIStyle.none);
		} else {
			Color newColor = new Color(1,1,1,1.0f);
			GUI.color = newColor;
			windowRect = GUILayout.Window(window_id, windowRect, MakeWindow, "Game Rooms");
			windowRectMC = GUILayout.Window(window_idMC, windowRectMC, MakeWindowMC, "Multiplayer Convergence Game Rooms");
		}
	}
	
	void MakeWindow(int id) {
		Color newColor = new Color(1,1,1,1.0f);
		GUI.color = newColor;

		GUILayout.Space(10);

		GUIStyle style = new GUIStyle();
		style.alignment = TextAnchor.MiddleCenter;
		style.normal.textColor = Color.white;

		GUILayout.BeginHorizontal();
		GUILayout.Label(new GUIContent("#"));
		GUILayout.Label(new GUIContent("         Game"));
		GUILayout.Label(new GUIContent("         Status"));
		GUILayout.Label(new GUIContent("    Host"));
		GUILayout.Label(new GUIContent(""), GUILayout.Width(100));
		GUILayout.EndHorizontal();

		foreach(var item in RoomManager.getInstance().getRooms()) {
			if (item.Value.game_id != Constants.MINIGAME_MULTI_CONVERGENCE) {
				GUILayout.BeginHorizontal();
				GUILayout.Label(new GUIContent("" + item.Key));
				GUILayout.Label(new GUIContent(Room.getGameName(item.Value.game_id)));
				GUILayout.Label(new GUIContent(item.Value.status()));
				GUILayout.Label(new GUIContent(item.Value.host));

				if (item.Value.containsPlayer(GameState.account.account_id)) {
					if(GUILayout.Button(new GUIContent("Quit"), GUILayout.Width(100))) {
						Game.networkManager.Send (QuitRoomProtocol.Prepare ());
					}
				} else {
					GUI.enabled = !this.waiting;
					if(GUILayout.Button(new GUIContent("Join"), GUILayout.Width(100))) {
						Game.networkManager.Send (PairProtocol.Prepare (item.Value.game_id, item.Value.id));
					}
					GUI.enabled = true;
				}
				GUILayout.EndHorizontal();
			}
		}

		GUILayout.Space(30);

		GUI.enabled = enableRRButton;
		if (GUI.Button(new Rect(10, windowRect.height - 80, 140, 30), "Play Running Rhino")) {
			Game.networkManager.Send (PairProtocol.Prepare (Constants.MINIGAME_RUNNING_RHINO, -1));
		}

		GUI.enabled = enableCWButton;
		if (GUI.Button(new Rect(10, windowRect.height - 40, 140, 30), "Play Cards of Wild")) {
			Game.networkManager.Send (PairProtocol.Prepare (Constants.MINIGAME_CARDS_OF_WILD, -1));
		}
            
        GUI.enabled = enableSDButton;
        if (GUI.Button(new Rect(165, windowRect.height - 40, 160, 30), "Play Sea Divided")) {
            Game.networkManager.Send (PairProtocol.Prepare (Constants.MINIGAME_SEA_DIVIDED, -1));
        }

		GUI.enabled = true;
		if (GUI.Button(new Rect(windowRect.width - 110, windowRect.height - 40, 100, 30), "Quit")) {
			Quit();
		}

		GUI.BringWindowToFront(window_id);
		GUI.DragWindow();
	}


	void MakeWindowMC(int id) {
		Color newColor = new Color(1,1,1,1.0f);
		GUI.color = newColor;

		GUILayout.Space(10);

		GUIStyle style = new GUIStyle();
		style.alignment = TextAnchor.MiddleCenter;
		style.normal.textColor = Color.white;

		GUILayout.BeginHorizontal();
		GUILayout.Label(new GUIContent(" "));
		GUILayout.Label(new GUIContent("    Total "));
		GUILayout.Label(new GUIContent("     #  "));
		GUILayout.Label(new GUIContent("      #  "));
		GUILayout.Label(new GUIContent("    Bet"));
		GUILayout.Label(new GUIContent(" Eco"));
		GUILayout.Label(new GUIContent("        "));
		GUILayout.Label(new GUIContent("        "));
		GUILayout.Label(new GUIContent(""), GUILayout.Width(40));
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		GUILayout.Label(new GUIContent("#"));
		GUILayout.Label(new GUIContent("   Players"));
		GUILayout.Label(new GUIContent("  Joined"));
		GUILayout.Label(new GUIContent("  Rounds"));
		GUILayout.Label(new GUIContent("  Amt"));
		GUILayout.Label(new GUIContent("  #  "));
		GUILayout.Label(new GUIContent(" Helps?"));
		GUILayout.Label(new GUIContent(" Host"));
		GUILayout.Label(new GUIContent(""), GUILayout.Width(40));
		GUILayout.EndHorizontal();

		foreach(var item in RoomManager.getInstance().getRooms()) {
			if (item.Value.game_id == Constants.MINIGAME_MULTI_CONVERGENCE) {
				GUILayout.BeginHorizontal();
				GUILayout.Label(new GUIContent("" + item.Key));
				GUILayout.Label(new GUIContent("     " + item.Value.totalPlayers));
				GUILayout.Label(new GUIContent("        " + item.Value.players.Count));
				GUILayout.Label(new GUIContent("       " + item.Value.numRounds));
				GUILayout.Label(new GUIContent("       " + item.Value.betAmt));
				GUILayout.Label(new GUIContent("  " + item.Value.ecoNum));
				GUILayout.Label(new GUIContent("    " + (item.Value.helps == 1 ? "Y" : "N") + "   "));
				GUILayout.Label(new GUIContent(item.Value.host));

				if (item.Value.containsPlayer(GameState.account.account_id)) {
					if(GUILayout.Button(new GUIContent("Quit"), GUILayout.Width(40))) {
						Game.networkManager.Send (QuitRoomProtocol.Prepare ());
					}
				} else {
					GUI.enabled = !this.waiting;
					if(GUILayout.Button(new GUIContent("Join"), GUILayout.Width(40))) {
						Game.networkManager.Send (PairProtocol.Prepare (item.Value.game_id, item.Value.id));
					}
					GUI.enabled = true;
				}
				GUILayout.EndHorizontal();

			}
		}

		GUILayout.Space(30);

		// DH change
		GUI.enabled = enableMCButton;
		if (GUI.Button(new Rect(10, windowRectMC.height - 40, 160, 30), "Play Multi-Convergence")) {
			isInitial = true;
			enableHostEntry = true;
		}

		GUI.enabled = true;
		if (GUI.Button(new Rect(windowRectMC.width - 110, windowRectMC.height - 40, 100, 30), "Quit")) {
			Quit();
		}

		GUI.BringWindowToFront(window_id);
		GUI.DragWindow();
	}		



	void MakeWindowHost(int id) {
		Functions.DrawBackground(new Rect(0, 0, widthConfig, heightConfig), bgTexture);
		string hdr1 = "Multiplayer Convergence";
		int hdr1P = hdr1.Length * pixelPerChar;
		string hdr2 = "You are the host of this game";
		int hdr2P = hdr2.Length * pixelPerChar;
		string hdr3 = "Please enter the information below and click 'Submit'";
		int hdr3P = hdr3.Length * pixelPerChar;

		GUIStyle style = new GUIStyle(GUI.skin.label);
		style.alignment = TextAnchor.UpperCenter;
		style.font = font;
		style.fontSize = 16;

		GUI.Label(new Rect((windowRectConfig.width - hdr1P) / 2, windowRectConfig.height * 0.02f, hdr1P, 45), hdr1, style);
		GUI.Label(new Rect((windowRectConfig.width - hdr2P) / 2, windowRectConfig.height * 0.07f, hdr2P, 45), hdr2, style);
		GUI.Label(new Rect((windowRectConfig.width - hdr3P) / 2, windowRectConfig.height * 0.12f, hdr3P, 45), hdr3, style);

		GUI.BeginGroup(new Rect(10, windowRectConfig.height * 0.18f, windowRectConfig.width * 0.80f, windowRectConfig.height * 0.30f));
		{
			style.alignment = TextAnchor.UpperLeft;
			style.fontSize = 14;
			GUI.Label(new Rect(0, 0, windowRectConfig.width * 0.85f, 30), "Enter Number of Players (2 to 5)", style);
			GUI.SetNextControlName("number_of_players");
			numPlayersS = GUI.TextField(new Rect(0, 25, windowRectConfig.width * 0.80f, 22), numPlayersS, 22);
		}
		GUI.EndGroup();

		GUI.BeginGroup(new Rect(10, windowRectConfig.height * 0.30f, windowRectConfig.width * 0.80f, windowRectConfig.height * 0.30f));
		{
			style.alignment = TextAnchor.UpperLeft;
			style.fontSize = 14;
			GUI.Label(new Rect(0, 0, windowRectConfig.width * 0.85f, 30), "Enter Number of Rounds (5 to 50)", style);
			GUI.SetNextControlName("number_of_rounds");
			numRoundsS = GUI.TextField(new Rect(0, 25, windowRectConfig.width * 0.80f, 22), numRoundsS, 22);
		}
		GUI.EndGroup();

		GUI.BeginGroup(new Rect(10, windowRectConfig.height * 0.42f, windowRectConfig.width * 0.80f, windowRectConfig.height * 0.30f));
		{
			style.alignment = TextAnchor.UpperLeft;
			style.fontSize = 14;
			GUI.Label(new Rect(0, 0, windowRectConfig.width * 0.85f, 30), "Enter Round Play Time in seconds (30 to 180)", style);
			GUI.SetNextControlName("bet_time");
			timeWindowS = GUI.TextField(new Rect(0, 25, windowRectConfig.width * 0.80f, 22), timeWindowS, 22);
		}
		GUI.EndGroup();

		GUI.BeginGroup(new Rect(10, windowRectConfig.height * 0.54f, windowRectConfig.width * 0.80f, windowRectConfig.height * 0.30f));
		{
			style.alignment = TextAnchor.UpperLeft;
			style.fontSize = 14;
			GUI.Label(new Rect(0, 0, windowRectConfig.width * 0.85f, 30), "Enter Bet Amount (20 to 200)", style);
			GUI.SetNextControlName("bet_amount");
			betAmountS = GUI.TextField(new Rect(0, 25, windowRectConfig.width * 0.80f, 22), betAmountS, 22);
		}
		GUI.EndGroup();

		GUI.BeginGroup(new Rect(10, windowRectConfig.height * 0.66f, windowRectConfig.width * 0.80f, windowRectConfig.height * 0.30f));
		{
			style.alignment = TextAnchor.UpperLeft;
			style.fontSize = 14;
			GUI.Label(new Rect(0, 0, windowRectConfig.width * 0.85f, 30), "Enter EcoSystem Number (0 to " + (ecoCount-1) + ")", style);
			GUI.SetNextControlName("ecosystem_number");
			ecoNumberS = GUI.TextField(new Rect(0, 25, windowRectConfig.width * 0.80f, 22), ecoNumberS, 22);
		}
		GUI.EndGroup();

		GUI.BeginGroup(new Rect(10, windowRectConfig.height * 0.78f, windowRectConfig.width * 0.80f, windowRectConfig.height * 0.30f));
		{
			style.alignment = TextAnchor.UpperLeft;
			style.fontSize = 14;
			GUI.Label(new Rect(0, 0, windowRectConfig.width * 0.85f, 30), "Enter 'Y' to see Slider Help *'s", style);
			GUI.SetNextControlName("allow_slider");
			allowSlidersS = GUI.TextField(new Rect(0, 25, windowRectConfig.width * 0.80f, 22), allowSlidersS, 22);
		}
		GUI.EndGroup();

		if (isInitial) {  // && GUI.GetNameOfFocusedControl() == "") {
			GUI.FocusControl("number_of_players");
			isInitial = false;
		}

		float butY = Math.Max (windowRectConfig.height * 0.90f, windowRectConfig.height * 0.76f + 25 + 22 + 10);
		if (GUI.Button(new Rect((windowRectConfig.width - 100) / 2,  butY, 100, 25), "Submit")) {
			SubmitHostConfig();
		}
	}

	private void SubmitHostConfig() {
		Debug.Log ("SubmitHostConfig called");

		numPlayersS = numPlayersS.Trim();
		numRoundsS = numRoundsS.Trim();
		timeWindowS = timeWindowS.Trim();
		betAmountS = betAmountS.Trim();
		ecoNumberS = ecoNumberS.Trim();

		short tempConvert;
		short neg1 = -1;
		numPlayers = Int16.TryParse (numPlayersS, out tempConvert) ? tempConvert : neg1;
		numRounds = Int16.TryParse (numRoundsS, out tempConvert) ? tempConvert : neg1;
		timeWindow = Int16.TryParse (timeWindowS, out tempConvert) ? tempConvert : neg1;
		betAmount = Int16.TryParse (betAmountS, out tempConvert) ? tempConvert : neg1;
		ecoNumber = Int16.TryParse (ecoNumberS, out tempConvert) ? tempConvert : neg1;
		allowSliders = (short) (((allowSlidersS.Length > 0) && (allowSlidersS.ToUpper().Substring(0, 1)) == "Y") ? 1 : 0);

		if ((numPlayers < 2) || (numPlayers > 5)) {
			GUI.FocusControl ("number_of_players");
		} else if ((numRounds < 5) || (numRounds > 50)) {
			GUI.FocusControl ("number_of_rounds");
		} else if ((timeWindow < 30) || (timeWindow > 180)) {
			GUI.FocusControl ("bet_time");
		} else if ((betAmount < 20) || (betAmount > 200)) {
			GUI.FocusControl ("bet_amount");
		} else if ((ecoNumber < 0) || (ecoNumber >= ecoCount)) {
			GUI.FocusControl ("ecosystem_number");
		} else {
			enableHostEntry = false;
			Game.networkManager.Send (MCSetupProtocol.Prepare (Constants.MINIGAME_MULTI_CONVERGENCE, numPlayers, numRounds,
				timeWindow, betAmount, ecoNumber, allowSliders));
		}
	}

	public void ProcessConvergeHostConfig (NetworkResponse response)
	{
		ResponseConvergeHostConfig args = response as ResponseConvergeHostConfig;
		Debug.Log ("In MultiplayerGames, responseconvergehostconfg");
		short status = args.status;
		Debug.Log ("status: " + status);
		Game.SwitchScene ("MultiConverge");
	}



	public void setMessage(string message) {
		this.message = message;
	}

	public void Quit() {
        if (!this.enableRRButton || !this.enableCWButton || !this.enableSDButton || !this.enableMCButton) {
			Game.networkManager.Send (QuitRoomProtocol.Prepare ());
			quiting = true;
		} else {
			Destroy (this);
		}
	}

	public void OnQuitRoomResult (NetworkResponse response) {
		// DH change
		this.enableMCButton = true;
		this.enableRRButton = true;
		this.enableCWButton = true;
        this.enableSDButton = true;
		this.waiting = false;

		RoomManager.getInstance().removePlayer(GameState.account.account_id);
		if (quiting) {
			Destroy (this);
		}
	}

	public void OnPairResult (NetworkResponse response) {
		ResponsePair args = response as ResponsePair;
		int userID = GameState.account.account_id;
		
		if (args.status == 0) {
			Debug.Log ("All players are ready to play [room id=" + args.id + "]");
			this.room_id = args.id;

			this.enableRRButton = true;
			this.enableCWButton = true;
			this.enableMCButton = true;
            this.enableSDButton = true;

			room = RoomManager.getInstance ().getRoom (args.id);
			if (!room.containsPlayer (userID)) {
				room.addPlayer (userID);
			}

			// switch scene
			if (args.gameID == Constants.MINIGAME_RUNNING_RHINO) {
				RR.RRConnectionManager cManager = RR.RRConnectionManager.getInstance ();
				cManager.Send (RR_RequestRaceInit ());

				Game.SwitchScene ("RRReadyScene");
			} else if (args.gameID == Constants.MINIGAME_CARDS_OF_WILD) {
				CW.GameManager.matchID = args.id;
                gameObject.AddComponent <CWGame>();
                Quit();
                CWGame.networkManager.Send (CW.MatchInitProtocol.Prepare 
                    (GameState.player.GetID(), args.id), 
                    ProcessMatchInit);
            } else if (args.gameID == Constants.MINIGAME_SEA_DIVIDED) {
                SD.SDConnectionManager sManager = SD.SDConnectionManager.getInstance();
                sManager.Send(SD_RequestPlayInit());
                Game.SwitchScene ("SDReadyScene");
            } else if (args.gameID == Constants.MINIGAME_MULTI_CONVERGENCE) {
				// DH change
				MultiConvergeGame.matchID = args.id;   // game id
				host = 0;  // Default - not the host
                string playerName = GameState.player.GetName ();
				if (playerName == room.host) {
					host = 1;  // this is the host
				}
				Game.networkManager.Send (MCMatchInitProtocol.Prepare 
					(GameState.player.GetID (), args.id, host, playerName), 
					MCProcessMatchInit);
				Debug.Log("MC notice sent to server(game id, player id): " + args.id + " " + playerName);
				Debug.Log ("Player Name: " + playerName);
                Debug.Log ("userID: " + userID);
				Debug.Log ("This player host value is: " + host);
			}
		} else {
			Debug.Log("New room allocated [room id=" + args.id + "]");
			room = RoomManager.getInstance().addRoom(args.id, args.gameID);
			room.host = GameState.player.GetName();
			room.addPlayer(userID);

			this.enableRRButton = false;
			this.enableCWButton = false;
			this.enableMCButton = false;
            this.enableSDButton = false;
			this.waiting = true;
		}
	}

	public IEnumerator RequestGetRooms(float time) {
		yield return new WaitForSeconds(time);
		
		Game.networkManager.Send (GetRoomsProtocol.Prepare ());
		
		StartCoroutine(RequestGetRooms(1f));
	}

	public void ProcessMatchInit(NetworkResponse response) {
		CW.ResponseMatchInit args = response as CW.ResponseMatchInit;
		
		if (args.status == 0) {
			Debug.Log("MatchID set to: " + args.matchID);
			Game.SwitchScene ("CWBattle");
		}
	}

	// DH change
	public void MCProcessMatchInit(NetworkResponse response) {
		ResponseMCMatchInit args = response as ResponseMCMatchInit;

		if (args.status == 0) {
			Debug.Log("MC MatchID set to: " + args.matchID + " Player id is: " + GameState.player.GetID ());
			Debug.Log("numRounds, timeWindow, betAmount, ecoNumber, allowSlliders");
			Debug.Log (numRounds + " " + timeWindow + " " + betAmount + " " + ecoNumber + " " + allowSliders);
			if (host == 1) {
				Game.networkManager.Send (
					ConvergeHostConfigProtocol.Prepare (numRounds, timeWindow, betAmount, ecoNumber, allowSliders), 
					ProcessConvergeHostConfig);
			} else {
				MCWaitForParams ();
			}

		}
	}
		
	public void MCWaitForParams() {
		Game.networkManager.Send (
			ConvergeNonHostConfigProtocol.Prepare (), ProcessConvergeNonHostConfig);
	}

	public void ProcessConvergeNonHostConfig (NetworkResponse response)
	{
		ResponseConvergeNonHostConfig args = response as ResponseConvergeNonHostConfig;
		numRounds = args.numRounds;
		Debug.Log ("In responseconvergenonhostconfg - received values. NumRounds = " + numRounds);
		if (numRounds > 0) {  // Host has loaded the values 
			Game.SwitchScene ("MultiConverge");
		} else {
			DateTime tStamp, tNow;
			TimeSpan tDiff;
			tStamp = DateTime.UtcNow;
			do {
				tNow = DateTime.UtcNow;
				tDiff = tNow.Subtract(tStamp);
			} while (tDiff.TotalSeconds < 0.2);
			MCWaitForParams ();
		}
	}

	public RR.RequestRaceInit RR_RequestRaceInit ()
	{
		RR.RequestRaceInit request = new RR.RequestRaceInit ();
		request.Send (RR.Constants.USER_ID, this.room_id);
		return request;
	}
    public SD.RequestPlayInit SD_RequestPlayInit()
    {
        SD.RequestPlayInit request = new SD.RequestPlayInit();
        request.Send(SD.Constants.USER_ID, this.room_id);
        return request;
    }
		
	private void ReadConvergeEcosystemsFileCount ()
	{
		string filename = "converge-ecosystems-Ben";
		int ecosystemCnt;

		if (!File.Exists (filename)) {
			Debug.LogError (filename + " not found.");
		} else {
			using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read)) {
				using (BinaryReader br = new BinaryReader(fs, Encoding.UTF8)) {
					int size = br.ReadInt16 ();
					int responseId = br.ReadInt16 ();
					ecosystemCnt = br.ReadInt16 ();
					ecoCount = (short) ecosystemCnt;
				}
			}
		}
	}
}
