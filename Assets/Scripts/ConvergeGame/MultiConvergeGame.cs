using UnityEngine;
//using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

// DH change - all new. Copy new file

public class MultiConvergeGame : MonoBehaviour
{
	private const long SUBMIT_WAIT_MS = 700;
	private int player_id;
	private string player_name;
	// Window Properties
	private int window_id = Constants.CONVERGE_WIN;
    private int host_config_id = Constants.CONVERGE_HOST_CONFIG;
    private int non_host_config_id = Constants.CONVERGE_NONHOST_CONFIG;
	private float left;
	private float top;
	private float width;
	private float height;
    // DH change
    // dimensions for host / non-host windows - both are the same
    private float leftConfig;
    private float topConfig;
    private float widthConfig;
    private float heightConfig;
	// dimensions for round winner windows 
	private float leftWinner;
	private float topWinner;
	private float widthWinner;
	private float heightWinner;

	private float widthGraph;
	private float heightGraph;
	private int bufferBorder = 10;
	private float leftGraph = 10;
	private float topGraph = 45;    // DH change. Was 75
	private float buttonStep = 35;    // Y step for opponent status buttons 
	private float bottomMargin = 40;
	private float heightBG = 300;           // Taken from BarGraph
	private float topBG;
    private float balanceY, sliderY;        // balance msg & species slider Y coordinates
	private float balanceX;                 // balance msg + buttons + species slider X coordinate
	private float entryHeight;              // height for species sliders
	private int sliderBorder = 20;  // DH change. Gives extra for sliders
	private float OppViewWidth;    // DH change. Width of opponent view area
	private Rect windowRect, sliderSRect;
    private Rect windowRectConfig, windowRectWinners;
	// Logic
	private bool gameOver = false;   // Set when game is over 
	private bool isActive = false;   // Not active until host specifies
    private bool isInitial = true;   // helps with GUI focus
	private bool isSetup = false;   // parameters entered in lobby 
    private bool showScores = false;   // used to display end result screen
	private bool alarm10Sec = true;
	private bool alarm5Sec = true;
	private bool haveNames = false; 
	private bool prevActive = false;
	private bool oppGraph = false;
	// DH change
	// eliminate blink. 
	// isProcessing is just used to control access to prior attempts. Matches Converge
	private bool isProcessing = false;
	// private bool blinkOn = true;
	//public Texture background;
	private Texture2D bgTexture;
	private Font font;
	//ecosystems
	private int ecosystem_id;
	private int ecosystem_idx;
	private int new_ecosystem_id;
	private int new_ecosystem_idx;
	private List<ConvergeEcosystem> ecosystemList = null;
	private string[] ecosysDescripts;
	//attempts
	private int referenceAttemptIdx;
	private ConvergeAttempt currAttempt = null;
	private List<ConvergeAttempt> attemptList = null;
	private int attemptCount = 0;
	private String config_field_default;
	//graphs, visualization
	private GraphsCompare graphs = null;
	private ConvergeManager manager = null;
	private BarGraph barGraph = null;
	private Database foodWeb = null;
	//popup messaging
	private bool showPopup = false;
	private bool showPopup2 = false;
	private bool showWinners = false;
	private string popupMessage = "";
	private Rect popupRect;
	private Vector2 popupScrollPosn  = Vector2.zero;
	//hints
	private bool allowHintsConfigured = false;
	private bool allowHintsMaster;
	private int hintCount = 0;
	private Dictionary <int, string> hintDict = new Dictionary <int, string>();
	private List<int> priorHintIdList = new List<int>();
	//reset attempt slider
	private bool isResetSliderInitialized = false;
	private int resetSliderValue = 0;
	private int maxResetSliderValue = 0;
	// DH change
	private string buttonTitle;
	public static int matchID;     // This is the room_id. Set by MultiplayerGames
	private bool host;    // Is this player the host?
	private int timeRemain = 0;   // How many seconds left in round. Could be negative
	private int timeRemainMax = 0;  
	private int timePrevious = 0;
	private int countPrevious = 0;
    private int timeDisplayed = 0;   // Value displayed for time remaining
    private int timeCheck = -4;   // timeRemain value to check for no response
    private int checkCount = 0;   // count of number of CheckPlayers msgs sent
    private int playerDrop = 0;   // Count of frames to display player dropped msg 
	private string remainLabel;
	private int balance, balBeg;    // player money balance, beginning balance 
	private int bet;        // player bet amount 
	private int timeNow;     // present millisecond time component
	private int timeNowNew; 
	private System.DateTime moment;
	private bool results = false;    // Indicates that a round has been done
	private short won;    // 1 -> won, 0 -> lost,  -1 -> did not play
	private int wonAmount;  // amount won (pot) 
	private bool betAccepted = false;
	private bool scoresReady = false;
	private bool windowClosed = false;
	// private bool closedResponseSent = false;
	private IDictionary playerNames;
	private IDictionary betStatusList;
	private int betCount = 0;
	private float buttonWidth;
	private bool simRunning;
	private List<int> scores;   // Obtained from BarGraph
	private List<int> formattedScores;   // Formatted for BetUpdateProtocol
	private int id_otherPlayer;   // id of player to display graph
	private string name_otherPlayer;   // name for other player for graph
	private List<int> otherScores;
    private int pixelPerChar = 15;
	private short numPlayers;
    private short curRound; 
    // Parameters that host specifies
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
    private DateTime tStamp;
    private DateTime tNow;
    private TimeSpan tDiff;
    private bool sendNonHost = true;
	private bool duplSlider = false;
	private string selectedSaved;
    // Initial response message from client
    string ftr1 = "";
    int ftr1P = 0;
	// Slider control for species
	private bool speciesCounted;    // true indicates other values are valid, count cycle complete
	private int speciesColCount;       // number of columns of species in ecosystem
	private int speciesRowCount;       // number of rows of species in ecosystem
	private int speciesColFit;         // number of columns of species that will fit on screen
	private int speciesColIndex;       // index of coloum of species presently starting display. Ranges 0 to speciesCount - speciesFit
	private AudioSource audio;
	private int playerWinner;
	private string playerWinnerName;
	private List<int> winners;         // lists by round the player id of the winner
	private List<int> activePlayers;
	private IDictionary roundsWon;     // Dictionary by player of number of rounds won
	private int[] playerId; 
	private int[] playerWinnings;
	private int[] playerLastImprove;
	private int improveValue = 0;      // This is the players score. Higher is better. Check BarGraph to determine formula
	private string buttonText;
	private long lastSubmit, nowMS; 
	private long finalMS = 0;
	private int finalMSCnt = 0;
	private long namesMS = 0;

	void Awake ()
	{
		width = Screen.width;
		height = Screen.height;
		widthConfig = Screen.width * 0.90f;
		heightConfig = Screen.height * 0.90f;
		widthWinner = Screen.width * 0.70f;
		heightWinner = Screen.height * 0.70f;
        tStamp = DateTime.UtcNow;
        sendNonHost = true;
        DontDestroyOnLoad (gameObject.GetComponent ("Login"));
        player_id = GameState.player.GetID ();
		player_name = GameState.player.name;
		balance = GameState.player.credits;
		balBeg = balance;
		Debug.Log ("Player name: " + player_name + ", credits: " + balance);

        left = (Screen.width - width) / 2;
        top = (Screen.height - height) / 2;
        // DH change
        leftConfig = (Screen.width - widthConfig) / 2;
        topConfig = (Screen.height - heightConfig) / 2;
		leftWinner = (Screen.width - widthWinner) / 2;
		topWinner = (Screen.height - heightWinner) / 2;
        ftr1 = "";
        ftr1P = ftr1.Length * pixelPerChar;

        // DH change
		OppViewWidth = Math.Min((int)(width * 0.25f - 10), 160);
		buttonWidth = OppViewWidth;
		windowRect = new Rect (left, top, width, height);
        windowRectConfig = new Rect (leftConfig, topConfig, widthConfig, heightConfig);
		windowRectWinners = new Rect (leftWinner, topWinner, widthWinner, heightWinner);
		widthGraph = windowRect.width - (bufferBorder * 2) - OppViewWidth;
		popupRect = new Rect ((Screen.width / 2) - 250, (Screen.height / 2) - 125,
		                        500, 200);

		bgTexture = Resources.Load<Texture2D> (Constants.THEME_PATH + Constants.ACTIVE_THEME + "/gui_bg");
		font = Resources.Load<Font> ("Fonts/" + "Chalkboard");
		// SetIsProcessing (true);

		playerNames = new Dictionary<int, string>();
		playerNames.Clear();
		betStatusList = new Dictionary<int, short>();
		betStatusList.Clear();
		roundsWon = new Dictionary<int, int>();
		roundsWon.Clear();

		simRunning = false;
		formattedScores = new List<int>();
		otherScores = new List<int>();
		winners = new List<int> ();
		activePlayers = new List<int> ();
        balanceY = topGraph + buttonStep*4 + 0;     // balance msg Y coordinate
		balanceX = width - buttonWidth - bufferBorder;      // balance, bet msgs, oppo buttons, species slider X coordinate
		sliderY = balanceY + 65;                    // slider Y coordinate 
        curRound = 1;   // Start with Round 1
		topBG = windowRect.height - heightBG - bottomMargin - topGraph;
		heightGraph = Math.Min (windowRect.height * 0.50f, topBG);
		heightGraph = Math.Max (heightGraph, sliderY + 0.0f);
		// Debug.Log ("windowRect.height, topBG, sliderY: " + windowRect.height + " " + topBG + " " + sliderY);
		// last 35 taken to allocate room for multiplayer convergence text 
		entryHeight = height - heightGraph - 30 * 3 - bufferBorder * 2 - 35;
		speciesRowCount = Math.Max((int)((entryHeight-40)/35)+1,1);
		speciesCounted = false;
		speciesColCount = 0;
		speciesColFit = Math.Max ((int)(width / (350 + bufferBorder)), 1);
		// This is the column # where display begins (slider output) range: 0 to SpeciesColCount - speciesColFit
		speciesColIndex = 0;    
		sliderSRect = new Rect (balanceX, sliderY, buttonWidth, 30); 
		// Debug.Log ("=== Awake: sRC/sCF/eH: " + speciesRowCount + " " + speciesColFit + " " + entryHeight);
		audio = gameObject.AddComponent<AudioSource> ();
		playerId = new int[5];
		playerWinnings = new int[5];
		playerLastImprove = new int[5];
	}
	
	// Use this for initialization
	void Start ()
	{
		// DH change
		// Get room that player is in
		// Debug.Log ("Screen width/height " + Screen.width + " " + Screen.height);
		var room = RoomManager.getInstance().getRoom(matchID);
		Debug.Log("MC: room id / host name / player_id: " + matchID + " " + room.host + " " + player_id);
		numPlayers = (short) room.numPlayers ();
		Debug.Log("MC: Number of players: " + numPlayers);
		// Read parameters from Room object
		numRounds = room.numRounds;
		bet = room.betAmt;
		ecoNumber = room.ecoNum;
		allowSliders = room.helps;
		Debug.Log("ROOM: numRounds/bet/eco#/sliders: " + numRounds + " " + bet + " " + ecoNumber + " " + allowSliders);

		if (GameState.player.GetName () == room.host) {
			host = true;
			Debug.Log ("This player is the host");
		} else {
			host = false;
			Debug.Log ("This player is not the host");
		}

		Game.StartEnterTransition ();
		//to generate converge-ecosystem.txt, remove comments and let protocol run;
		//server will generate txt from sql table
		//NetworkManager.Send(ConvergeEcosystemsProtocol.Prepare(),ProcessConvergeEcosystems);

		//get list of ecosystems
		ReadConvergeEcosystemsFile ();
		//set default ecosystem values
		new_ecosystem_id = Constants.ID_NOT_SET;

		// ecosystem_idx = 0;     // Initially set to first ecosystem
		// ecosystem_id = GetEcosystemId (ecosystem_idx);
		//get player's most recent prior attempts 
		// DH change - start everyone at beginning to make equal
		// GetPriorAttempts ();
		// Replacement for GetPriorAttempts()
		/*
        if (!isSetup) {
            NoPriorAttempts();
            InitializeBarGraph();
        }
        */

		ecosystem_idx = ecoNumber;  // implement after ecosystems read
		ecosystem_id = GetEcosystemId (ecosystem_idx);
		NoPriorAttempts();
		InitializeBarGraph();
		isActive = true;

		//create array of ecosystem descriptions
        /*
		if (ecosystemList != null) {
			ecosysDescripts = ConvergeEcosystem.GetDescriptions (ecosystemList);
		}
        */      
		// GetHints ();
		Debug.Log ("Now in MultiConverge.cs");

		moment = DateTime.Now;
		timeNow = moment.Millisecond;
		Debug.Log ("Time: " + timeNow);
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
	
	void OnGUI ()
	{
		// Background
		//GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), background, ScaleMode.ScaleAndCrop);
		
		// Client Version Label
		GUI.Label (new Rect (Screen.width - 75, Screen.height - 30, 65, 20), "v" + Constants.CLIENT_VERSION + " Beta");

		if ((!simRunning) && isActive && (!gameOver)) {
			moment = DateTime.Now;
			timeNowNew = moment.Millisecond;
			int delta = timeNowNew - timeNow;
			// Debug.Log ("New Time/Delta = " + timeNowNew + " " + delta);
			// check if more than 300 msec have passed 
			if ((delta < 0) || (delta > 400)) {
				timeNow = timeNowNew;
				GetTime();  // Update bet time 

				if (timeRemain > timeRemainMax) {
					timeRemainMax = timeRemain;
				}

				if (!haveNames) {
					if (timeRemainMax > timeRemain + 10) {
						haveNames = true;   // After 10 seconds assume missing person dropped 
						GetNames ();  
					} else if (((DateTime.Now.Ticks - namesMS) / TimeSpan.TicksPerMillisecond) > 1000) {
						namesMS = DateTime.Now.Ticks;
						GetNames ();  
					}
				}

				if (timeRemain > timePrevious) {
					timePrevious = timeRemain;
					countPrevious = 5;    // cycles to play the round
				}
				timePrevious = timeRemain;
				countPrevious--;
				if (countPrevious == 0) {
					audio.PlayOneShot ((AudioClip)Resources.Load ("Audio/startRound"));
				}

				if ((timeRemain > 15) && !betAccepted) {
					alarm10Sec = false;
					alarm5Sec = false;
				}

				if ((timeRemain <= 11) && !alarm10Sec && !betAccepted) {
					alarm10Sec = true;
					audio.PlayOneShot ((AudioClip)Resources.Load ("Audio/alarm_10sec"));
				}

				if ((timeRemain <= 6) && !alarm5Sec && !betAccepted) {
					alarm5Sec = true;
					audio.PlayOneShot ((AudioClip)Resources.Load ("Audio/alarm_5sec"));
				}
				// On the multiples of 4 seconds, get the active players, eliminating those that dropped
                if ((timeRemain % 4) == 0) {
					GetActiveNames();
				}
			}
		}

		/*
		if ((timeRemain < timeCheck) && (checkCount < 2) && isActive && (!gameOver)) {  // waiting too long for some client(s). Check Server
            checkCount++;
            CheckPlayers();
        }
        */

		// Check if betting window closed and no bet entered
		nowMS = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
		if (!betAccepted && windowClosed && isActive && !gameOver && ((nowMS - lastSubmit) > SUBMIT_WAIT_MS)) {
			lastSubmit = nowMS;
			short betEntered = 0;	
			// improveValue = 0;    // Use previous value
			// closedResponseSent = true;
			if (!scoresReady) {
				ObtainScores();
				scoresReady = true;
			}
			Debug.Log ("MC: Submitting windowClosed timeout ConvergeBetUpdateProtocol");
			Debug.Log ("nowMS, lastSubmit, difference: " + nowMS + " " + lastSubmit + " " + (nowMS - lastSubmit));
			Game.networkManager.Send (
				ConvergeBetUpdateProtocol.Prepare (
					betEntered, 
					curRound,
					improveValue,
					formattedScores[0],
					formattedScores[1],
					formattedScores[2],
					formattedScores[3],
					formattedScores[4]
				),
				ProcessConvergeBetUpdate
			);
		}			
            
		// Converge Game Interface
		if (isActive) {
			windowRect = GUI.Window (window_id, windowRect, MakeWindow, "Multiplayer Converge", GUIStyle.none);
			
			//if (Event.current.type == EventType.KeyUp && Event.current.keyCode == KeyCode.Return) {
			//	Submit();
			//}
		}
		GUI.skin.label.fontSize = 12;  //for legend series labels

		if (showPopup) {
			GUI.Window (Constants.CONVERGE_POPUP_WIN, popupRect, ShowPopup, "Error", GUIStyle.none);
		}

		if (showPopup2) {
			GUI.Window (Constants.CONVERGE_POPUP_WIN2, popupRect, ShowPopup2, "Error", GUIStyle.none);
		}

		if (showWinners) {
			GUI.Window (Constants.CONVERGE_SHOW_WINNERS, windowRectWinners, makeWindowWinners, "Round Winners", GUIStyle.none);
			GUI.BringWindowToFront(Constants.CONVERGE_SHOW_WINNERS);
		}

		if (showScores) {    // Final score screen screen
			windowRectConfig = GUI.Window (host_config_id, windowRectConfig, MakeWindowDone, "Game Over", GUIStyle.none);
			GUI.BringWindowToFront(host_config_id);
		}

		if ((finalMSCnt < 20) && (finalMS > 0) && (((DateTime.Now.Ticks - finalMS) / TimeSpan.TicksPerMillisecond) > 500)) {
			finalMS = DateTime.Now.Ticks;
			finalMSCnt++;
			Debug.Log ("ProcessConvergeGetFinalScores submitted again");
			Game.networkManager.Send (
				ConvergeGetFinalScoresProtocol.Prepare (
				),
				ProcessConvergeGetFinalScores
			);
		}
	}
	
	void MakeWindow (int id)
	{
		if ((barGraph != null) && !oppGraph) {
			if (prevActive != barGraph.GetActive()) {
				prevActive = barGraph.GetActive ();
				speciesCounted = false;
				speciesColCount = 0;
				speciesColIndex = 0;
				if (prevActive) {
					speciesColFit = Math.Max ((int)(barGraph.getGraphLeft() / (350 + bufferBorder)), 1);
					// Debug.Log ("MCBG: getGraphLeft, speciesColFit: " + barGraph.getGraphLeft () + " " + speciesColFit);
				} else {
					speciesColFit = Math.Max ((int)(width / (350 + bufferBorder)), 1);
				}
			}
		}

		Functions.DrawBackground (new Rect (0, 0, width, height), bgTexture);
		
		GUIStyle style = new GUIStyle (GUI.skin.label);
		style.alignment = TextAnchor.UpperCenter;
		style.font = font;
		style.fontSize = 16;

		GUI.Label (new Rect ((windowRect.width - 200) / 2, 0, 200, 30), "Multiplayer Convergence", style);
		if (gameOver)
			buttonText = "Return to Lobby";
		else
			buttonText = "Surrender";
		if (GUI.Button (new Rect (windowRect.width - 100 - bufferBorder, 0, 100, 30), buttonText)) {
			if (!gameOver) {
				// Update database with new balance
				Game.networkManager.Send (
					EndGameProtocol.Prepare (
						(short) 5,
						balance
					),
					ProcessEndGame
				);
				GameState.player.credits = balance;    // Update player object as well
				audio.PlayOneShot ((AudioClip)Resources.Load ("Audio/gameOver"));
			}
			Destroy (this);
			Destroy (foodWeb);
			// GameState gs = GameObject.Find ("Global Object").GetComponent<GameState> ();
			// Species[] s = gs.GetComponents<Species>();
			// foreach (Species sp in s) Destroy (sp); //destroy the "species" objects
			Game.SwitchScene("World");
		}

		if (graphs != null) {
			graphs.DrawGraphs ();
		}

		GUIStyle styleEdit = new GUIStyle (GUI.skin.textArea);
		styleEdit.wordWrap = true;

		//data entry fields
		DrawParameterFields (style);

		/* DH change. Not using ecosystem selection by player
		if (isInitial && GUI.GetNameOfFocusedControl () == "") {
			GUI.FocusControl ("ecosystem_idx_field");
			isInitial = false;
		}
		*/

		// DH change
		// Add in time remaining label
        timeDisplayed = (timeRemain < 0) ? 0 : timeRemain;
		if (betAccepted) {
			remainLabel = "Submission Accepted";
		} else if ((windowClosed) || (timeDisplayed == 0)) {
			remainLabel = "Submission Window Closed";
		} else {
			remainLabel = "Submission Time Remaining: " + timeDisplayed + " seconds";
		}
		if (gameOver) {
			remainLabel = "Game Over";
		}
		GUI.Label (new Rect (bufferBorder, height - 75 - bufferBorder, 400, 30), remainLabel, style);

		// Add in money balance and bid amount
		if (!gameOver) {
			GUI.Label (new Rect (balanceX, balanceY, 200, 30), "Balance: $" + balance, style);
			GUI.Label (new Rect (balanceX, balanceY + 25, 200, 30), "Bet:      $" + bet, style);
			GUI.Label (new Rect (bufferBorder + 320, height - 75 - bufferBorder, 200, 30), "Round " + curRound + " of " + numRounds, style);

			if (betAccepted) {
				GUI.Label (new Rect (bufferBorder + 450, height - 75 - bufferBorder, 300, 30), "Please wait for round results.", style);
			} else if (results) {
				if (won == 1) {
					GUI.Label (new Rect (bufferBorder + 450, height - 75 - bufferBorder, 300, 30), 
						"Congratulations - you won round " + (curRound-1), style);
				} else if (won == 0) {
					// GUI.Label (new Rect (bufferBorder + 450, height - 75 - bufferBorder, 300, 30), "Sorry - you lost.", style);
					GUI.Label (new Rect (bufferBorder + 450, height - 75 - bufferBorder, 300, 30), 
						playerWinnerName + " won round " + (curRound-1), style);
				} else {
					GUI.Label (new Rect (bufferBorder + 450, height - 75 - bufferBorder, 300, 30), 
						"You did not play round " + (curRound-1), style);
				}
			}
		}			

		/*  Hard to find space in GUI for this message 
        if (playerDrop > 0) {
            GUI.Label (new Rect (bufferBorder + 450, height - 75 - bufferBorder + 20, 300, 30), "A player left the game.", style);
            playerDrop--;

        }
        */

		if (betAccepted) {
			buttonTitle = "Entered";
			nowMS = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
			if (((nowMS - lastSubmit) > SUBMIT_WAIT_MS) && scoresReady) {
				lastSubmit = nowMS;
				short betEntered = 1;	
				Debug.Log ("MC: Submitting betAccepted ConvergeBetUpdateProtocol");
				Debug.Log ("bet: nowMS, lastSubmit, difference: " + nowMS + " " + lastSubmit + " " + (nowMS - lastSubmit));
				Game.networkManager.Send (
					ConvergeBetUpdateProtocol.Prepare (
						betEntered, 
						curRound,
						improveValue,
						formattedScores[0],
						formattedScores[1],
						formattedScores[2],
						formattedScores[3],
						formattedScores[4]
					),
					ProcessConvergeBetUpdate
				);
			}
		} else {
			buttonTitle = windowClosed ? "Closed" : "Accept";
		}
		if (!gameOver) {
			if (GUI.Button (new Rect (bufferBorder, height - 30 - bufferBorder, 70, 30), buttonTitle) &&
				!betAccepted && !windowClosed) {
				audio.Stop ();
				alarm10Sec = true;
				alarm5Sec = true;
				audio.PlayOneShot ((AudioClip)Resources.Load ("Audio/gong"));
				//make sure new config is distinct from prior attempts and initial value
				currAttempt.UpdateConfig ();  //update config based on user data entry changes
				ConvergeAttempt prior = attemptList.Find (entry => entry.config == currAttempt.config);
				nowMS = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
				betAccepted = true;
				Submit ();
				isProcessing = true;  // Block access to prior attempt buttons during simulation


				/*
				if (prior == null && currAttempt.ParamsUpdated ()) {
					nowMS = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
					betAccepted = true;
					Submit ();
					isProcessing = true;  // Block access to prior attempt buttons during simulation
					// SetIsProcessing (true);
				} else if (!showPopup && !gameOver) {
					int prior_idx = attemptList.FindIndex (entry => entry.config == currAttempt.config);
					if (prior_idx == Constants.ID_NOT_SET) {
						popupMessage = "This attempt matches the initial ecosystem. Each attempt must be unique. ";
						popupMessage +=  "Please change one slider and press 'Accept' again.";
					} else {
						popupMessage = "This attempt matches prior attempt #" 
							+ (prior_idx + 1) + ". Each attempt must be unique. Please change one slider and press 'Accept' again.";
					}
					//Debug.Log (popupMessage);
					showPopup = true;
				}
				*/



			}
		}

		if (gameOver) {
			buttonTitle = "Scores";
			if (GUI.Button (new Rect (bufferBorder, height - 30 - bufferBorder, 70, 30), buttonTitle)) {
				showScores = !showScores;
			}

		}

		if (GUI.Button (new Rect (bufferBorder + 80, height - 30 - bufferBorder, 70, 30), "Preview")) {
			// Runs simulation & updates graph, but does not submit bet or do anything else
			// This allows the player to see the impact of a potential bet
			if (!gameOver) {
				currAttempt.UpdateConfig ();  //update config based on user data entry changes
				Preview ();
			}
		}
			
		if (GUI.Button (new Rect (bufferBorder + 160, height - 30 - bufferBorder, 70, 30), "Progress")) {
			GenerateBarGraph ();
		}

		if (GUI.Button (new Rect (bufferBorder + 240, height - 30 - bufferBorder, 70, 30), "Winners")) {
			Debug.Log ("showWinners set to true");
			showWinners = !showWinners;
		}
			
		// Display buttons with opponent bet status 
		Color savedColor2 = GUI.color;
		float topLeft = topGraph;

		// style.fontSize = 12;
		// Debug.Log ("Other player button routine");
		GUIStyle customButton = new GUIStyle("button");
		customButton.fontSize = 12;
		foreach (DictionaryEntry entry in playerNames) {
			// do something with entry.Value or entry.Key
			id_otherPlayer = (int) entry.Key;
			if (!activePlayers.Contains (id_otherPlayer))
				continue;
			name_otherPlayer = (string)entry.Value + " (won " + roundsWon [id_otherPlayer] + ") ";
			if ((id_otherPlayer > 0) && (betStatusList.Contains (id_otherPlayer))) {
				GUI.color = Color.red;
				buttonText = name_otherPlayer;
				if (!gameOver) {
					if (((short)betStatusList [id_otherPlayer]) == 1) {  // bet placed
						GUI.color = Color.green; 
						buttonText += " Made Bet";
					} else {  // bet not placed
						buttonText += " No Bet";
					}
				}
				if (GUI.Button (new Rect (balanceX, topLeft, buttonWidth, 25), buttonText, customButton)) {
					barGraph.setOppName (name_otherPlayer);
					displayOtherGraph ();
				}
				topLeft += buttonStep;
			}
		}
		GUI.color = savedColor2;

			
		int screenOffset = 0;
		/* Not doing hints in multiplayer game - initially
		if (currAttempt != null && currAttempt.allow_hints) {
			screenOffset += bufferBorder + 110;
			if (GUI.Button (new Rect (bufferBorder * 2 + 110 * 2, height - 30 - bufferBorder, 110, 30), "Get Hint")) {
				//only give new hint if one hasn't been provided during this session.
				if (currAttempt.hint_id == Constants.ID_NOT_SET) {
					int hintId = GetRandomHint (true);
					if (hintId == Constants.ID_NOT_SET) {
						popupMessage = "Sorry, no new hints are available.";
					} else {
						popupMessage = hintDict[hintId];
						currAttempt.hint_id = hintId;
					}
				} else {
					if (hintDict.ContainsKey (currAttempt.hint_id)) {
						popupMessage = hintDict[currAttempt.hint_id] + "\n[Only one hint is provided per attempt.]";
					} else {
						Debug.LogError ("Invalid hint for current attempt, ID = " + currAttempt.hint_id);
						popupMessage = "Error, hint not available.";
					}
				}
				if (priorHintIdList.Count > 0) {
					popupMessage += "\n\nPrior Hints:";
				}
				for (int i = 0; i < priorHintIdList.Count; i++) {
					popupMessage += "\n\n" + hintDict[priorHintIdList[i]];
				}
				//Debug.Log (popupMessage);
				showPopup = true;
			}
		}
		*/

		DrawResetButtons (screenOffset, style);
	}

	void makeWindowWinners(int id) {
		Functions.DrawBackground(new Rect(0, 0, widthWinner, heightWinner), bgTexture);
		GUIStyle style = new GUIStyle(GUI.skin.label);
		style.alignment = TextAnchor.UpperCenter;
		style.font = font;
		style.fontSize = 14;

		string msg;
		string banner = "Round Winners";
		string hdrMsg = "Round   Winner ";
		int pixelPerChar2 = 10;
		int xStart = 10;
		int xStep = hdrMsg.Length * pixelPerChar2;
		int xMax = (int) ((widthWinner - 2 * xStart) / xStep);
		int yStep = 25;
		int yStart = 75;
		int yHeader = 45;
		int yMax = (int) ((heightWinner - 55 - yStart - yStep) / yStep);
		int rounds = winners.Count;
		int xHdrCnt = Math.Min (xMax, (int)((rounds - 1) / yMax) + 1);
		int startRound = Math.Max (1, rounds + 1 - xMax * yMax);
		int xIdx = 1;
		int yIdx = 1;
		GUI.Label(new Rect((widthWinner-banner.Length*pixelPerChar2)/2, 10, banner.Length*pixelPerChar2, yStep), banner, style);
		for (int i = 0; i < xHdrCnt; i++) {
			GUI.Label(new Rect(xStart + i*xStep, yHeader, xStep, yStep), hdrMsg, style);
		}
		for (int i = startRound; i <= rounds; i++) {
			msg = " " + i + "      ";
			if (i < 10) {
				msg = " " + msg;
			}
			if (playerNames.Contains (winners [i - 1])) {
				msg += (playerNames [winners [i - 1]] as string);
			} else if (winners [i - 1] == player_id) {
				msg += player_name;
			} else {
				msg += "tie/none";
			}
			msg = (msg + "          ").Substring (0, 18);
			GUI.Label(new Rect(xStart + (xIdx - 1) * xStep, yStart + (yIdx - 1) * yStep, xStep, yStep), msg, style);
			yIdx++;
			if (yIdx > yMax) {
				yIdx = 1;
				xIdx++;
			}
		}
			
		if (GUI.Button(new Rect(25,  windowRectWinners.height - 55, 80, 30), "Close")) {
			Debug.Log ("showWinners set to false");
			showWinners = false;
		}
	}
		
    void MakeWindowDone(int id) {
        Functions.DrawBackground(new Rect(0, 0, widthConfig, heightConfig), bgTexture);
        string hdr1 = "Multiplayer Convergence";
        int hdr1P = hdr1.Length * pixelPerChar;
        string hdr2 = "Game Over. Thank you for playing";
        int hdr2P = hdr2.Length * pixelPerChar;
		string hdr3 = "Your Beginning balance was: " + balBeg;
		int hdr3P = hdr3.Length * pixelPerChar;
        string hdr4 = "Your Final balance is: " + balance;
        int hdr4P = hdr4.Length * pixelPerChar;
        string hdr5 = "The Database will be updated with your new Balance";
        int hdr5P = hdr5.Length * pixelPerChar;
		string hdr6 = "Player Final Results are listed below in Ranked Order";
		int hdr6P = hdr6.Length * pixelPerChar;

        string retButton = "Close";
        int retButtonP = 110;

        GUIStyle style = new GUIStyle(GUI.skin.label);
        style.alignment = TextAnchor.UpperCenter;
        style.font = font;
        style.fontSize = 16;

        GUI.Label(new Rect((windowRectConfig.width - hdr1P) / 2, windowRectConfig.height * 0.10f, hdr1P, 30), hdr1, style);
        GUI.Label(new Rect((windowRectConfig.width - hdr2P) / 2, windowRectConfig.height * 0.15f, hdr2P, 30), hdr2, style);
        GUI.Label(new Rect((windowRectConfig.width - hdr3P) / 2, windowRectConfig.height * 0.20f, hdr3P, 30), hdr3, style);
        GUI.Label(new Rect((windowRectConfig.width - hdr4P) / 2, windowRectConfig.height * 0.25f, hdr4P, 30), hdr4, style);
		GUI.Label(new Rect((windowRectConfig.width - hdr5P) / 2, windowRectConfig.height * 0.30f, hdr5P, 30), hdr5, style);
		GUI.Label(new Rect((windowRectConfig.width - hdr6P) / 2, windowRectConfig.height * 0.40f, hdr6P, 30), hdr6, style);

		GUI.Label(new Rect(windowRectConfig.width * 0.10f, windowRectConfig.height * 0.45f, 100, 30), "Player", style);
		GUI.Label(new Rect(windowRectConfig.width * 0.25f, windowRectConfig.height * 0.45f, 150, 30), "Rounds Won", style);
		GUI.Label(new Rect(windowRectConfig.width * 0.45f, windowRectConfig.height * 0.45f, 120, 30), "Winnings", style);
		GUI.Label(new Rect(windowRectConfig.width * 0.60f, windowRectConfig.height * 0.45f, 250, 30), "Final Delta to Target", style);

		int[] tPlayerId = new int[5];
		int[] tPlayerWinnings = new int[5];
		int[] tPlayerLastImprove = new int[5];
		for (int i = 0; i < 5; i++) {
			tPlayerId [i] = playerId [i];
			tPlayerWinnings [i] = playerWinnings [i];
			tPlayerLastImprove [i] = playerLastImprove [i];
		}

		int idx = 0;
		int bestWinnings;
		int besti = 0;
		Boolean done = false; 
		String displayName;
		while (!done) {
			bestWinnings = -1000000;
			done = true;
			for (int i = 0; i < 5; i++) {
				if ((tPlayerId [i] != -1) && (tPlayerWinnings [i] > bestWinnings)) {
					bestWinnings = tPlayerWinnings [i];
					besti = i;
					done = false;
				}
			}
			if (!done) {
				if (playerId [besti] == player_id) {
					displayName = player_name;
				} else {
					displayName = (string) playerNames [playerId [besti]];
				}
				GUI.Label(new Rect(windowRectConfig.width * 0.10f, windowRectConfig.height * (0.49f + idx * 0.04f), 100, 30), 
					" " + displayName, style);
				GUI.Label(new Rect(windowRectConfig.width * 0.27f, windowRectConfig.height * (0.49f + idx * 0.04f), 100, 30), 
					" " + roundsWon[playerId[besti]], style);				
				GUI.Label(new Rect(windowRectConfig.width * 0.47f, windowRectConfig.height * (0.49f + idx * 0.04f), 120, 30), 
					" " + playerWinnings[besti], style);
				GUI.Label(new Rect(windowRectConfig.width * 0.67f, windowRectConfig.height * (0.49f + idx * 0.04f), 150, 30), 
					" " + (-playerLastImprove[besti]), style);
				idx++;
				tPlayerId [besti] = -1;
			}
		}
			
        if (GUI.Button (new Rect ((windowRectConfig.width - retButtonP) / 2, windowRectConfig.height * 0.85f, retButtonP, 30), retButton)) {
			showScores = false;
        }
    }
		
	private void DrawParameterFields (GUIStyle style)
	{
		style.alignment = TextAnchor.UpperRight;
		style.font = font;
		style.fontSize = 14;
		Color savedColor = GUI.color;
		Color savedBkgdColor = GUI.backgroundColor;

		if (currAttempt != null && currAttempt.seriesParams.Count > 0) {
			int paramCnt = currAttempt.seriesParams.Count;
			int row = 0;
			int col = 0;
            float highRange = 0f;
            float lowRange = 0f;
            int startRange = 0;
            int spanRange = 0;

			GUI.BeginGroup (new Rect (bufferBorder, topGraph + heightGraph + bufferBorder, width, entryHeight));

            GUIStyle styleMR = new GUIStyle(GUI.skin.label);
            styleMR.alignment = TextAnchor.UpperLeft;
            styleMR.font = font;
            styleMR.fontSize = 12;
			int sCnt = 0;
			//use seriesNodes to force order
			foreach (int nodeId in manager.seriesNodes) {
				//look for all possible parameter types for each node
				foreach (char paramId in new char[]{'K', 'R', 'X'}) {
					string strId = paramId.ToString ();
					ConvergeParam param;
					string nodeIdParamId = ConvergeParam.NodeIdParamId (nodeId, strId);
					if (currAttempt.seriesParams.ContainsKey (nodeIdParamId)) {
						param = currAttempt.seriesParams [nodeIdParamId];
					} else {
						continue;
					}
					sCnt++;
					if (speciesCounted && (speciesColCount > speciesColFit)) {
						if ((speciesColIndex * speciesRowCount) >= sCnt) {
							continue;
						}
					}	
					if (!speciesCounted) {
						speciesColCount = col+1;
					}
					if (col < speciesColFit) {
						float min = 0f;
						float max = 1f;
						switch (paramId) {
						case 'K':
							min = 1000f;
							max = 15000f;
							break;
						case 'R':
							min = 0f;
							max = 3f;
							break;
						default:
							break;
						}

						Rect labelRect, TFRect;
						//draw name, paramId
						labelRect = new Rect (col * (350 + bufferBorder), row * 35 + sliderBorder, 200, 30);
						TFRect = new Rect (col * (350 + bufferBorder) + 215, row * 35 + sliderBorder, 30, 25);
						if (labelRect.Contains (Event.current.mousePosition) || TFRect.Contains(Event.current.mousePosition)) {
							manager.mouseOverLabels.Add (param.name);
							manager.selected = param.name;
							manager.lastSeriesToDraw = param.name;
						}
						GUI.color = (param.name.Equals (manager.selected)) ? 
							manager.seriesColors [param.name] : Color.white;
						String pStr = param.name;
						int pSIdx = pStr.IndexOf ("[");
						if (pSIdx != -1) {
							pStr = pStr.Substring (0, pSIdx);
						}
						// GUI.Label (labelRect, param.name + " - " + param.paramId, style);
						GUI.Label (labelRect, pStr, style);

						int pVInInt = (int) (99.5 * (param.value - min) / (max - min));
						String pVInStr = "" + pVInInt;
						if (pVInInt == 0) {
							pVInStr = "";
						} 
						String pVOutStr = GUI.TextField(TFRect, pVInStr, 2);
						if (!pVInStr.Equals (pVOutStr)) {
							int pVOutInt;
							if (!Int32.TryParse(pVOutStr, out pVOutInt)) {
								pVOutInt = 0;
							}
							param.value = min + (pVOutInt + 0.5f) * (max - min) / 99.5f;
						}

						//if player clicks on species, set as selected and activate foodWeb
						if (GUI.Button (labelRect, "", GUIStyle.none)) {
							foodWeb.selected = SpeciesTable.GetSpeciesName (param.name);
							foodWeb.SetActive (true, foodWeb.selected);
						}				
						//draw slider with underlying colored bar showing original value
						Rect sliderRect = new Rect (labelRect.x + 250 + bufferBorder, labelRect.y + 5, 100, 20);

						if (sliderRect.Contains (Event.current.mousePosition) || TFRect.Contains(Event.current.mousePosition)) {
							manager.mouseOverLabels.Add (param.name);
							manager.selected = param.name;
							manager.lastSeriesToDraw = param.name;
						}
						GUI.color = manager.seriesColors [param.name];
						float origValWidth = 
							ConvergeParam.NormParam (param.origVal, min, max, sliderRect.width);
						//float origValWidth = (param.origVal / (max - min)) * sliderRect.width;
						Color slTexture= new Color (0.85f, 0.85f, 0.85f);
						GUI.DrawTexture (new Rect (sliderRect.x, sliderRect.y, origValWidth, 10), //sliderRect.height),
							Functions.CreateTexture2D (slTexture));
						//					GUI.color = savedColor;

						// DH change
						// Add slider markers. First read slider range values
						highRange = param.highRange;
						lowRange = param.lowRange;
						if ((allowSliders == 1) && param.markerEnabled && ((highRange != -1) || (lowRange != -1))) {
							if (lowRange == -1) {
								startRange = 0;
								lowRange = min;
							} else {
								startRange = (int) (100.0f * (lowRange - min) / (max - min));
							}
							if (highRange == -1) {
								spanRange = 100 - startRange;
							} else {
								spanRange = (int)(100.0f * (highRange - lowRange) / (max - min));
								spanRange = spanRange > 9 ? spanRange : 10;
							}                      
							GUI.Label (new Rect (labelRect.x + 250 + bufferBorder + startRange, row * 35 + sliderBorder - 20 + 5, spanRange, 20), 
								"******************", styleMR);
						}

						//draw slider for parameter value manipulation
						GUI.backgroundColor = manager.seriesColors [param.name];
						param.value = GUI.HorizontalSlider (
							sliderRect, 
							param.value, 
							min, 
							max
						);

						//show normalized value for parameter
						if (param.name.Equals (manager.selected)) {
							string valLabel = String.Format (
								"{0}",
								ConvergeParam.NormParam (param.value, min, max));
							if (param.value != param.origVal) {
								valLabel = valLabel + String.Format (
									" [{0}]", 
									ConvergeParam.NormParam (param.origVal, min, max));
								// DH change
								// Since slider moved, reset all other sliders
								duplSlider = false;
								foreach(KeyValuePair<string, ConvergeParam> entry in currAttempt.seriesParams)
								{
									// do something with entry.Value or entry.Key
									if (!entry.Value.name.Equals(manager.selected)) {
										if (entry.Value.value != entry.Value.origVal) {
											duplSlider = true;
											selectedSaved = manager.selected;
										}
									}
								}
								if (duplSlider && !gameOver) {
									popupMessage = "You are only allowed to change one slider per round. "
										+ "Press 'OK' to reset previous slider and continue. "
										+ "Press 'Cancel' to retain previous slider setting and undo current change. ";
									showPopup2 = true;
								}
							}
							style.alignment = TextAnchor.UpperLeft;
							float xPosn = 
								sliderRect.x + 
								(param.value / (max - min)) * 
								sliderRect.width +
								bufferBorder;
							Rect valRect = new Rect(xPosn, labelRect.y, 70, labelRect.height - 5);

							GUI.Box (valRect, valLabel);
							style.alignment = TextAnchor.UpperRight;
						}
					}

					if ((row + 1) * 35 + 40 > entryHeight) {
						col++;
						row = 0;
					} else {
						row++;
					}

					GUI.color = savedColor;
					GUI.backgroundColor = savedBkgdColor;
				}
			}
			GUI.EndGroup ();
			if (speciesCounted && (speciesColCount > speciesColFit)) {
				speciesColIndex = Mathf.RoundToInt (GUI.HorizontalSlider (
					sliderSRect, speciesColIndex, 0, speciesColCount - speciesColFit));
			}
			// Debug.Log ("=== sCC/sCF/r/c/sC: " + speciesColCount + " " + speciesColFit + " " + row + " " + col + " " + sCnt);
			speciesCounted = true;
		}
		style.alignment = TextAnchor.UpperLeft;
		style.font = font;
		style.fontSize = 16;
	}

	void DrawResetButtons (int screenOffset, GUIStyle style)
	{
		GUI.Label (new Rect (bufferBorder + 270 + screenOffset + 50, height - 30 - bufferBorder, 110, 30), "Reset to:", style);
		Rect initial = new Rect (bufferBorder * 2 + 330 + screenOffset + 50, height - 30 - bufferBorder, 50, 30);
		if (GUI.Button (initial, "Initial") && !isProcessing) {
			ResetCurrAttempt (Constants.ID_NOT_SET);
		}
		//use slider to accommodate more reset attempt buttons that fit on the screen
		int widthPer = 45;
		int sliderWidth = 100;
		if (!isResetSliderInitialized) {
			InitializeResetSlider (width - (initial.x + initial.width + sliderWidth + bufferBorder), widthPer);
			isResetSliderInitialized = !isProcessing;
		}
		int maxVal = attemptList.Count - maxResetSliderValue + resetSliderValue;
		for (int i = resetSliderValue; i < maxVal; i++) {
			int slideNo = i - resetSliderValue;
			if (GUI.Button (
				new Rect (bufferBorder * 2 + 390 + (slideNo * widthPer) + screenOffset + 50, height - 30 - bufferBorder, 
			          widthPer - 5, 
			          30
			          ), 
			    String.Format ("#{0}", i + 1)
				)
			    && !isProcessing) {
				ResetCurrAttempt (i);
			}
		}

		if (maxResetSliderValue > 0) {
			Rect sliderRect = new Rect (width - sliderWidth - bufferBorder, initial.y, sliderWidth, 30); 
			resetSliderValue = Mathf.RoundToInt (GUI.HorizontalSlider (
				sliderRect, 
				resetSliderValue, 
				0, 
				maxResetSliderValue
			));
		}

	}

	void InitializeResetSlider (float availWidth, int widthPer)
	{
		int perPage = Mathf.FloorToInt (availWidth / widthPer);
		// Adjust init slider val
		maxResetSliderValue = Mathf.Max (0, attemptList.Count - perPage);
		resetSliderValue = maxResetSliderValue;
	}

	void ShowPopup (int windowID)
	{
		GUIStyle style = new GUIStyle (GUI.skin.label);
		style.alignment = TextAnchor.UpperCenter;
		style.font = font;
		style.fontSize = 16;
		
		Functions.DrawBackground (new Rect (0, 0, popupRect.width, popupRect.height), bgTexture);
		GUI.BringWindowToFront (windowID);
		Rect outerRect = new Rect (
			bufferBorder, 
			bufferBorder, 
			popupRect.width - bufferBorder, 
			popupRect.height - 30 - bufferBorder * 2
			);
		float msgHeight = Mathf.Max (1, popupMessage.Length / 150) * outerRect.height;
		popupScrollPosn = GUI.BeginScrollView (
			outerRect,
			popupScrollPosn, 
			new Rect (0, 0, outerRect.width - 32, msgHeight),
			null, 
		    GUI.skin.verticalScrollbar
			);
		GUI.Label (
			new Rect (0, 0, outerRect.width - 32, msgHeight), 
			popupMessage, 
			style
			);
		GUI.EndScrollView ();

		if (GUI.Button (new Rect ((popupRect.width - 80) / 2, popupRect.height - 30 - bufferBorder, 80, 30), "OK")) {
			showPopup = false;
		}
		
	}

	void ShowPopup2 (int windowID)
	{
		GUIStyle style = new GUIStyle (GUI.skin.label);
		style.alignment = TextAnchor.UpperCenter;
		style.font = font;
		style.fontSize = 16;

		Functions.DrawBackground (new Rect (0, 0, popupRect.width, popupRect.height), bgTexture);
		GUI.BringWindowToFront (windowID);
		Rect outerRect = new Rect (
			bufferBorder, 
			bufferBorder, 
			popupRect.width - bufferBorder, 
			popupRect.height - 30 - bufferBorder * 2
		);
		float msgHeight = Mathf.Max (1, popupMessage.Length / 150) * outerRect.height;
		popupScrollPosn = GUI.BeginScrollView (
			outerRect,
			popupScrollPosn, 
			new Rect (0, 0, outerRect.width - 32, msgHeight),
			null, 
			GUI.skin.verticalScrollbar
		);
		GUI.Label (
			new Rect (0, 0, outerRect.width - 32, msgHeight), 
			popupMessage, 
			style
		);
		GUI.EndScrollView ();

		if (GUI.Button (new Rect (40, popupRect.height - 30 - bufferBorder, 80, 30), "OK")) {
			showPopup2 = false;
			foreach(KeyValuePair<string, ConvergeParam> entry in currAttempt.seriesParams)
			{
				// do something with entry.Value or entry.Key
				if (!entry.Value.name.Equals(selectedSaved)) {
					entry.Value.value = entry.Value.origVal;
				}
			}
		}

		if (GUI.Button (new Rect (popupRect.width - 80 - 40, popupRect.height - 30 - bufferBorder, 80, 30), "Cancel")) {
			showPopup2 = false;
			foreach(KeyValuePair<string, ConvergeParam> entry in currAttempt.seriesParams)
			{
				// do something with entry.Value or entry.Key
				if (entry.Value.name.Equals(selectedSaved)) {
					entry.Value.value = entry.Value.origVal;
				}
			}
		}
	}

	public void Preview () {
		simRunning = true;
		Game.networkManager.Send (
			ConvergeNewAttemptProtocol.Prepare (
				player_id, 
				ecosystem_id + 1000,    // Offset by 1000 to seperate from single player game. This is only for DB
				currAttempt.attempt_id,
				currAttempt.allow_hints,
				currAttempt.hint_id,
				ecosystemList [ecosystem_idx].timesteps,
				currAttempt.config),
			ProcessConvergeNewAttemptPreview
		);
	}




	public void ProcessConvergeNewAttemptPreview (NetworkResponse response)
	{
		Debug.Log ("ProcessConvergeNewAttemptPreview");
		ConvergeAttempt attempt;
		ResponseConvergeNewAttempt args = response as ResponseConvergeNewAttempt;
		attempt = args.attempt;

		//if the submission resulted in a valid attempt, add to attempt list and reinitialize 
		//currAttempt for next attempt.  Otherwise, keep current attempt
		if (attempt != null && attempt.attempt_id != Constants.ID_NOT_SET) {
			currAttempt.attempt_id = attempt.attempt_id;
			currAttempt.SetCSV (attempt.csv_string);
			attemptList.Add (currAttempt);
			attemptCount = attemptList.Count;

			//calculate score and send back to server.
			CSVObject target = ecosystemList[ecosystem_idx].csv_target_object;
			int score = currAttempt.csv_object.CalculateScore (target);
			Game.networkManager.Send (
				ConvergeNewAttemptScoreProtocol.Prepare (
					player_id, 
					ecosystem_id + 1000, 
					attempt.attempt_id, 
					score
				),
				ProcessConvergeNewAttemptScore
			);

			//update pertinent variables with new data
			if (currAttempt.hint_id != Constants.ID_NOT_SET) {
				priorHintIdList.Add (currAttempt.hint_id);
			}
			//need to recalc reset slider config due to additional attempt
			isResetSliderInitialized = false;

			if (barGraph != null) {
				barGraph.InputToCSVObject (currAttempt.csv_string, manager);
			}

			currAttempt = new ConvergeAttempt (
				player_id, 
				ecosystem_id,     // Don't add 1000 here. This is not updating the DB
				attempt.attempt_id + 1,
				allowHintsMaster,
				Constants.ID_NOT_SET,
				attempt.config,
				null,
				ecosystemList[ecosystem_idx].sliderRanges,
				ecosystemList[ecosystem_idx].markerEnabled
				//manager
			);

			FinalizeAttemptUpdate (attemptCount - 1, false);

		} else {
			Debug.LogError ("Submission of new attempt failed to produce results.");
			// betAccepted = false;
			isProcessing = false;
			// SetIsProcessing (false);
		}
	}









		
	public void Submit ()
	{
		simRunning = true;
        Game.networkManager.Send (
			ConvergeNewAttemptProtocol.Prepare (
			player_id, 
			ecosystem_id + 1000,    // Offset by 1000 to seperate from single player game. This is only for DB
			currAttempt.attempt_id,
			currAttempt.allow_hints,
			currAttempt.hint_id,
			ecosystemList [ecosystem_idx].timesteps,
			currAttempt.config),
			ProcessConvergeNewAttempt
		);
		//Debug.Log ("Submit RequestConvergeNewAttempt");
	}

	// Used for multiplayer Convergence so that all players start at same point 
	private void NoPriorAttempts() {
		attemptList = new List<ConvergeAttempt> ();
		attemptCount = 0;
		isResetSliderInitialized = false;
		FinalizeLoadPriorAttempts ();  // set for no prior attempts
	}
		
	private void GetPriorAttempts ()
	{
		//get attempts from server based on specified ecosystem
		attemptList = new List<ConvergeAttempt> ();
		attemptCount = 0;

        Game.networkManager.Send (
			ConvergePriorAttemptCountProtocol.Prepare (player_id, new_ecosystem_id),
			ProcessConvergePriorAttemptCount
		);
		//Debug.Log ("Send RequestConvergePriorAttemptCount, new_ecosystem_id = " + new_ecosystem_id);

	}
	
	public void ProcessConvergeEcosystems (NetworkResponse response)
	{
		ResponseConvergeEcosystems args = response as ResponseConvergeEcosystems;
		ecosystemList = args.ecosystemList;
	}

	public void ProcessConvergeNewAttemptScore (NetworkResponse response)
	{
		ResponseConvergeNewAttemptScore args = response as ResponseConvergeNewAttemptScore;
		int status = args.status;
		simRunning = false;
		//Debug.Log ("Processed ReponseConvergeNewAttemptScore, status = " + status);
	}
	
	public void ProcessConvergeNewAttempt (NetworkResponse response)
	{
		Debug.Log ("ProcessConvergeNewAttempt");
		ConvergeAttempt attempt;
		ResponseConvergeNewAttempt args = response as ResponseConvergeNewAttempt;
		attempt = args.attempt;

		//if the submission resulted in a valid attempt, add to attempt list and reinitialize 
		//currAttempt for next attempt.  Otherwise, keep current attempt
		if (attempt != null && attempt.attempt_id != Constants.ID_NOT_SET) {
			currAttempt.attempt_id = attempt.attempt_id;
			currAttempt.SetCSV (attempt.csv_string);
			attemptList.Add (currAttempt);
			attemptCount = attemptList.Count;

			//calculate score and send back to server.
			CSVObject target = ecosystemList[ecosystem_idx].csv_target_object;
			int score = currAttempt.csv_object.CalculateScore (target);
            Game.networkManager.Send (
				ConvergeNewAttemptScoreProtocol.Prepare (
				player_id, 
				ecosystem_id + 1000, 
				attempt.attempt_id, 
				score
				),
				ProcessConvergeNewAttemptScore
				);

			//update pertinent variables with new data
			if (currAttempt.hint_id != Constants.ID_NOT_SET) {
				priorHintIdList.Add (currAttempt.hint_id);
			}
			//need to recalc reset slider config due to additional attempt
			isResetSliderInitialized = false;

			if (barGraph != null) {
				barGraph.InputToCSVObject (currAttempt.csv_string, manager);
			}

			currAttempt = new ConvergeAttempt (
				player_id, 
			    ecosystem_id,     // Don't add 1000 here. This is not updating the DB
			    attempt.attempt_id + 1,
			    allowHintsMaster,
			    Constants.ID_NOT_SET,
			    attempt.config,
			    null,
                ecosystemList[ecosystem_idx].sliderRanges,
                ecosystemList[ecosystem_idx].markerEnabled
			    //manager
			);

			FinalizeAttemptUpdate (attemptCount - 1, false);

			// DH change
			// Send response server with client improvement
			// See BarGarph.cs to determine how that value is calculated
			improveValue = barGraph.Improvement(); 
			Debug.Log ("MC send improvement: " + improveValue);

			// ConvergeBetUpdate:
			// short - 1 = bet entered, 0 = no bet entered
			// integer = improveValue, improvement for this round; 0 if no bet

			short betEntered = 1;	
			ObtainScores ();
			scoresReady = true;

			Debug.Log ("MC: Submitting initial submit() ConvergeBetUpdateProtocol");
			lastSubmit = nowMS;

            Game.networkManager.Send (
				ConvergeBetUpdateProtocol.Prepare (
					betEntered, 
					curRound,
					improveValue,
					formattedScores[0],
					formattedScores[1],
					formattedScores[2],
					formattedScores[3],
					formattedScores[4]
				),
				ProcessConvergeBetUpdate
			);
			lastSubmit = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
		} else {
			Debug.LogError ("Submission of new attempt failed to produce results.");
			// betAccepted = false;
			isProcessing = false;
			// SetIsProcessing (false);
		}
	}
		
	public void ProcessConvergeBetUpdate (NetworkResponse response)
	{
		ResponseConvergeBetUpdate args = response as ResponseConvergeBetUpdate;
		Debug.Log ("In responseconvergetbetupdate");
		int roundComplete = args.roundComplete;
		int round = args.round;
		Debug.Log ("roundComplete, round, curRound = " + roundComplete + " " + round + " " + curRound);
		if ((roundComplete != 1) || (round != curRound)) {
			return;
		}
		Debug.Log ("#1 Winning player id: " + playerWinner);
		curRound++;
		won = args.winStatus;
		wonAmount = args.wonAmount; 
		playerWinner = args.playerWinner;
		if (playerNames.Contains (playerWinner)) {
			playerWinnerName = playerNames [playerWinner] as string;
		} else {
			playerWinnerName = "";
		}
		Debug.Log ("#2 Winning player id: " + playerWinner);
		winners.Add (playerWinner);   // Add winner to the list as winner of this round
		if ((playerWinner != 0) && (roundsWon.Count > 0)) {   // If there is actually a winner (At least one played)			
			// Increment count of rounds won 
			roundsWon[playerWinner] = ((int) roundsWon[playerWinner]) + 1;
		}

		Debug.Log ("Round winner id: " + playerWinner);
		Debug.Log ("won/wonamount: " + won + " " + wonAmount);
		if (betAccepted) {
			Debug.Log ("Bet accepted");
			if (won == 1) {
				Debug.Log ("you won");
				audio.PlayOneShot ((AudioClip)Resources.Load ("Audio/wonRound"));
				balance = balance + wonAmount - bet;
			} else {
				Debug.Log ("you lost");
				audio.PlayOneShot ((AudioClip)Resources.Load ("Audio/lostRound"));
				balance -= bet;
			}
		} else {  // this person did not play
			Debug.Log("You did not play this round");
			won = -1;   // signals he did not play
		}
		results = true;
		betAccepted = false; 
		windowClosed = false;
		scoresReady = false;
		// closedResponseSent = false;           
        if (curRound > numRounds) {
			// Update database with new player balance
			Debug.Log ("Submit EndGameProtocol with new balance: " + balance);
			// Update database with new balance
			Game.networkManager.Send (
				EndGameProtocol.Prepare (
					(short) 5,
					balance
				),
				ProcessEndGame
			);
			GameState.player.credits = balance;    // Update player object as well
			// Game over - switch to game over display 
            // isActive = false;    // We keep active to allow player to check things 
			gameOver = true;
			finalMS = DateTime.Now.Ticks;
			Debug.Log ("ProcessConvergeGetFinalScores submitted");
			Game.networkManager.Send (
				ConvergeGetFinalScoresProtocol.Prepare (
				),
				ProcessConvergeGetFinalScores
			);
			audio.PlayOneShot ((AudioClip)Resources.Load ("Audio/gameOver"));
			Debug.Log ("Game over");
        }
		Debug.Log ("new balance: " + balance);
	}
		
	public void ProcessEndGame (NetworkResponse response)
	{
		ResponseEndGame args = response as ResponseEndGame;
		Debug.Log ("==== ResponseEndGame");
		Debug.Log ("Credit difference: " + args.creditDiff);
	}

	public void ProcessConvergeGetFinalScores (NetworkResponse response)
	{
		ResponseConvergeGetFinalScores args = response as ResponseConvergeGetFinalScores;
		Debug.Log ("Inside ProcessConvergeGetFinalScores");
		if (args.status == 1) {
			Debug.Log ("Scores are final");
			// finalMS = 0;
		}
		playerId = args.playerId;
		playerWinnings = args.playerWinnings;
		playerLastImprove = args.playerLastImprove;
		showScores = true;
		showPopup = false;
		showPopup2 = false; 
		Debug.Log ("ProcessConvergeGetFinalScores");
		Debug.Log ("playerId/winnings/lastImprove");
		for (int i = 0; i < 5; i++) {
			Debug.Log (playerId [i] + " " + playerWinnings [i] + " " + playerLastImprove [i]);
		}
	}
		
	// Not used in MC 	
	public void ProcessConvergePriorAttemptCount (NetworkResponse response)
	{
		ResponseConvergePriorAttemptCount args = response as ResponseConvergePriorAttemptCount;
		new_ecosystem_id = args.ecosystem_id;
		attemptCount = args.count;
		priorHintIdList = new List<int>();
		isResetSliderInitialized = false;

		//once count of attempts has been received, send requests for individual attempt's data
		for (int attemptOffset = 0; attemptOffset < attemptCount; attemptOffset++) {
            Game.networkManager.Send (
				ConvergePriorAttemptProtocol.Prepare (player_id, new_ecosystem_id, attemptOffset),
				ProcessConvergePriorAttempt
			);
			//Debug.Log ("Send RequestConvergePriorAttempt");
		}

		UpdateEcosystemIds ();

		//if no prior attempts found create new curAttempt based on default config
		//of curr ecosystem
		if (attemptCount == 0) {
			FinalizeLoadPriorAttempts ();
		}
	}

	public void ProcessConvergePriorAttempt (NetworkResponse response)
	{

		ResponseConvergePriorAttempt args = response as ResponseConvergePriorAttempt;
		ConvergeAttempt attempt = args.attempt;

		//add new attempt to list if response includes an attempt
		if (attempt.attempt_id == Constants.ID_NOT_SET) {
			Debug.LogError ("attempt_id not valid in ProcessConvergePriorAttempt");
		} else {
			attemptList.Add (attempt);
			if (attempt.hint_id != Constants.ID_NOT_SET) {
				priorHintIdList.Add (attempt.hint_id);
			}
		}

		//Once all attempts have been processed, finalize.
		if (attemptList.Count == attemptCount) {
			FinalizeLoadPriorAttempts ();
		}
	}

	private void UpdateEcosystemIds ()
	{
		//If ecosystem_id is not set, use default (ecosystem 0)
		if (new_ecosystem_id == Constants.ID_NOT_SET) {
			new_ecosystem_idx = 0;
			new_ecosystem_id = GetEcosystemId (new_ecosystem_idx);
		} else {
			//otherwise, set new index based on selected ecosystem id
			new_ecosystem_idx = ecosystemList.FindIndex (entry => entry.ecosystem_id == new_ecosystem_id);
		}

		//update current ecosystem info 
		ecosystem_id = new_ecosystem_id;
		ecosystem_idx = new_ecosystem_idx;
	}

	//after all prior attempts have been retrieved from server, finalize
	private void FinalizeLoadPriorAttempts ()
	{
		//if this is the first load, whether or not to allow hints for this player
		//has not been established; determine randomly if no prior, o/w use prior
		//attempt.
		if (!allowHintsConfigured) {
			if (attemptCount == 0) {
				allowHintsMaster = Functions.RandomBoolean ();
			} else {
				allowHintsMaster = attemptList [attemptCount - 1].allow_hints;
			}
			allowHintsConfigured = true;
		}

		//if count is 0, base next attempt info on ecosystem default
		if (attemptCount == 0) {
			int attempt_id = 0;
			currAttempt = new ConvergeAttempt (
				player_id,
				ecosystem_id, 
			    attempt_id,
			    allowHintsMaster,
			    Constants.ID_NOT_SET,
			    ecosystemList [ecosystem_idx].config_default,
			    ecosystemList [ecosystem_idx].csv_default_string,
			    //manager
                ecosystemList [ecosystem_idx].sliderRanges,
                ecosystemList [ecosystem_idx].markerEnabled
			);
			//otherwise, base next attempt info on immediate prior attempt
		} else {
			int attempt_id = attemptList [attemptCount - 1].attempt_id + 1;
			currAttempt = new ConvergeAttempt (
				player_id, 
			    ecosystem_id, 
			    attempt_id,
			    allowHintsMaster,
			    Constants.ID_NOT_SET,
			    attemptList [attemptCount - 1].config,
			    attemptList [attemptCount - 1].csv_string,
			    //manager
                ecosystemList[ecosystem_idx].sliderRanges,
                ecosystemList[ecosystem_idx].markerEnabled
			);
		}


		FinalizeAttemptUpdate (attemptCount - 1, true);
	}

	//reset currattempt based on requested attempt (or default)
	private void ResetCurrAttempt (int attemptIdx)
	{
		//if count is 0, base next attempt info on ecosystem default
		int attempt_id = currAttempt.attempt_id;
		int hint_id = currAttempt.hint_id;  //if player received a hint, retain history.
//		if (attemptCount > 0) {
//			attempt_id = attemptList [attemptCount - 1].attempt_id + 1;
//		} else {
//			attempt_id = 0;
//		}
		if (attemptIdx == Constants.ID_NOT_SET) {
			currAttempt = new ConvergeAttempt (
				player_id, 
		        ecosystem_id, 
		        attempt_id,
		        allowHintsMaster,
		        hint_id,
		        ecosystemList [ecosystem_idx].config_default,
			    ecosystemList [ecosystem_idx].csv_default_string,
			    //manager
                ecosystemList[ecosystem_idx].sliderRanges,
                ecosystemList[ecosystem_idx].markerEnabled
			);
		} else {
			currAttempt = new ConvergeAttempt (
				player_id, 
		        ecosystem_id, 
		        attempt_id,
		        allowHintsMaster,
			    hint_id,
			    attemptList [attemptIdx].config,
			    attemptList [attemptIdx].csv_string,
			    //manager
                ecosystemList[ecosystem_idx].sliderRanges,
                ecosystemList[ecosystem_idx].markerEnabled
			);
		}
		FinalizeAttemptUpdate (attemptIdx, false);
	}

	//update variables and graph configuration as necessary with updated attempt info
	private void FinalizeAttemptUpdate (int attemptIdx, bool newEcosystem)
	{
		referenceAttemptIdx = attemptIdx;

		if (newEcosystem) {
			manager = new ConvergeManager ();
		}
			
		//refresh or replace graphs as appropriate
		GenerateGraphs (newEcosystem);

		//extract data entry field data 
		//note: has dependency on manager info generated in graphs, 
		//so has to appear following GenerateGraphs
		currAttempt.ParseConfig (manager);

		// betAccepted = false;
		isProcessing = false;
		// SetIsProcessing (false);
	}
	
	public void SetActive (bool active)
	{
		this.isActive = active;
	}

	private void ReadConvergeEcosystemsFile ()
	{
		ResponseConvergeEcosystems response = new ResponseConvergeEcosystems ();
		
		// string filename = "converge-ecosystems-Ben";  // Problem with file 
		string filename = "converge-ecosystems";

        // string filenameR = "converge-ecosystems-Ben-sliders.txt";
		string filenameR = "";

		ecosystemList = new List<ConvergeEcosystem> ();
        Debug.Log("ecosystem files: " + filename + " " + filenameR);
		
		if ((filenameR.Length == 0) || !File.Exists (filenameR)) {
            Debug.Log("Sliders hint file: " + filenameR + " not found.");
			allowSliders = 0;
        }

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

					for (int i = 0; i < ecosystemCnt; i++) {
						int ecosystem_id = br.ReadInt32 ();
						Debug.Log("**** ecosystem_id: " + ecosystem_id);

						ConvergeEcosystem ecosystem = new ConvergeEcosystem (ecosystem_id);
						int fldSize = br.ReadInt16 ();
                        // Debug.Log("description_fldSize: " + fldSize);
						ecosystem.description = System.Text.Encoding.UTF8.GetString (br.ReadBytes (fldSize));
                        // Debug.Log(ecosystem.description);
						ecosystem.timesteps = br.ReadInt32 ();
						fldSize = br.ReadInt16 ();
                        // Debug.Log("config_default_fldSize: " + fldSize);
						ecosystem.config_default = System.Text.Encoding.UTF8.GetString (br.ReadBytes (fldSize));
                        // Debug.Log(ecosystem.config_default);
						fldSize = br.ReadInt16 ();
                        // Debug.Log("config_target_fldSize: " + fldSize);
						ecosystem.config_target = System.Text.Encoding.UTF8.GetString (br.ReadBytes (fldSize));
                        Debug.Log(ecosystem.config_target);
						fldSize = br.ReadInt16 ();
                        // Harjit's 32 bit length string
                        // fldSize = br.ReadInt32 ();
                        // Debug.Log("Harjit: csv_default_string_fldSize: " + fldSize);
						ecosystem.csv_default_string = System.Text.Encoding.UTF8.GetString (br.ReadBytes (fldSize));
						Debug.Log("ecosystem.csv_default_string next");
                        Debug.Log(ecosystem.csv_default_string);
						fldSize = br.ReadInt16 ();
                        // Harjit's 32 bit length string
                        // fldSize = br.ReadInt32 ();
                        // Debug.Log("Harjit: csv_target_string_fldSize: " + fldSize);
						ecosystem.csv_target_string = System.Text.Encoding.UTF8.GetString (br.ReadBytes (fldSize));
                        // Debug.Log(ecosystem.csv_target_string);
                        ecosystem.sliderRanges = "";
                        ecosystem.markerEnabled = false;
						
						ecosystemList.Add (ecosystem);
					}
					
					//set initial ecosystem id
					if (ecosystemList.Count == 0) {
						Debug.LogError ("No converge ecosystems found.");
					}

					response.ecosystemList = ecosystemList;
					
				}
			}
            String line;
            int rIndex = 0;

			if (allowSliders == 1) {
				try {
					using (StreamReader sr = new StreamReader(filenameR)) {
						for (rIndex = 0; rIndex < ecosystemCnt; rIndex++) {
							line = sr.ReadLine();
							Debug.Log("slider ranges, eco # " + rIndex + " is " + line);
							ecosystemList[rIndex].sliderRanges = line;
							if (line.Length > 0) {
								ecosystemList[rIndex].markerEnabled = true;     
								Debug.Log("markerEnabled = true");
							}
						}
					}
				}
				catch (Exception e)
				{
					Debug.Log("Exception exit of reading slider range file: " + filenameR );
					Debug.Log("ecosystemCnt/rIndex: " + ecosystemCnt + " " + rIndex);
					Debug.Log(e.Message);
				}					
			}
		}
	}

	private void GenerateGraphs (bool newEcosystem)
	{
		string title = (
			referenceAttemptIdx == Constants.ID_NOT_SET ? 
			"Initial config" : 
			String.Format ("Attempt #{0}", referenceAttemptIdx + 1)
			);  //hide 0 start from user

		CSVObject graph1CSV;

		graph1CSV = (referenceAttemptIdx == Constants.ID_NOT_SET) ? 
			ecosystemList [ecosystem_idx].csv_default_object :   
			attemptList [referenceAttemptIdx].csv_object;
				
		if (newEcosystem) {
			//destroy prior bargraph if it exists
			//Note: barGraph is regenerated when requested
			if (barGraph != null) {
				Destroy (barGraph);
				barGraph = null;
			}

			GenerateFoodWeb ();
			
			graphs = new GraphsCompare (
				graph1CSV, 
				ecosystemList [ecosystem_idx].csv_target_object, 
				leftGraph, 
				topGraph, 
				widthGraph,
				heightGraph,
				title,
				"Target Graph",
				ecosystemList [ecosystem_idx].timesteps,
				foodWeb,
				manager
			);

		} else {
			graphs.UpdateGraph1Data (graph1CSV, title);			
		}
		
	}

	private int GetEcosystemId (int idx)
	{
		return ecosystemList [idx].ecosystem_id;

	}

	private void InitializeBarGraph () {
		if (barGraph == null) {
			barGraph = gameObject.AddComponent<BarGraph> ().GetComponent<BarGraph> ();
			barGraph.setOppGraph (false);

			//first object must be target, then default 
			barGraph.InputToCSVObject (ecosystemList [ecosystem_idx].csv_target_string, manager);
			barGraph.InputToCSVObject (ecosystemList [ecosystem_idx].csv_default_string, manager);
		}
		barGraph.setOppGraph (false);
		oppGraph = false;
	}
		
	private void GenerateBarGraph ()
	{
		if (barGraph == null) {
			barGraph = gameObject.AddComponent<BarGraph> ().GetComponent<BarGraph> ();
			barGraph.setOppGraph (false);

			//first object must be target, then default 
			barGraph.InputToCSVObject (ecosystemList [ecosystem_idx].csv_target_string, manager);
			barGraph.InputToCSVObject (ecosystemList [ecosystem_idx].csv_default_string, manager);

			//followed by all of the player's prior attempts
			for (int i = 0; i < attemptCount; i++) {
				barGraph.InputToCSVObject (attemptList [i].csv_string, manager);
			}
		}
		barGraph.setOppGraph (false);   // This graph is for the player
		oppGraph = false;
		barGraph.SetActive (true);
	}

	private void GenerateFoodWeb ()
	{
		//reset gamestate for new ecosystem
		GameState gs = GameObject.Find ("Global Object").GetComponent<GameState> ();
		gs.speciesList = new Dictionary<int, Species> ();

		//loop through species in ecosystem and add to gamestate species list
		List <string> ecosystemSpecies = new List<string> (currAttempt.csv_object.csvList.Keys);
		foreach (string name in ecosystemSpecies) {
			//find name in species table
			SpeciesData species = SpeciesTable.GetSpecies (name);
			if (species == null) {
				Debug.LogError ("Failed to create Species '" + name + "'");
				continue;
			}

			gs.CreateSpecies (Constants.ID_NOT_SET, species.biomass, species.name, species);
		}

		//generate foodWeb if not present
		if (foodWeb == null) {
			foodWeb = Database.NewDatabase (
				GameObject.Find ("Global Object"), 
		        Constants.MODE_CONVERGE_GAME,
				manager
			);
		} else {
			foodWeb.manager = manager;
		}

		
	}
	/* DH change 
	 * Remove blinking and set processing 
	private void SetIsProcessing (bool isProc)
	{
		this.isProcessing = isProc;
		blinkOn = true;
		if (isProc) {
			StartCoroutine ("BlinkProcessing");
		} else {
			StopCoroutine ("BlinkProcessing");
		}
	}

	//function to blink the text
	private IEnumerator BlinkProcessing ()
	{
		while (true) {
			//set the Text's text to blank
			blinkOn = true;
			//display blank text for 0.5 seconds
			yield return new WaitForSeconds (.5f);
			//display “I AM FLASHING TEXT” for the next 0.5 seconds
			blinkOn = false;
			yield return new WaitForSeconds (.5f);
		}
	}
	*/
	
	public void ProcessLogout (NetworkResponse response)
	{
		ResponseLogout args = response as ResponseLogout;
		//act on logout regardless of response
		Application.Quit ();
		Game.SwitchScene ("Login");
	}

	void OnDestroy ()
	{
	}
	
	public void GetHints ()
	{
		hintCount = 0;
		
        Game.networkManager.Send (
			ConvergeHintCountProtocol.Prepare (),
			ProcessConvergeHintCount
			);
		//Debug.Log ("Send RequestConvergeHintCount");
	}
	
	public void ProcessConvergeHintCount (NetworkResponse response)
	{
		ResponseConvergeHintCount args = response as ResponseConvergeHintCount;
		hintCount = args.count;
		
		//once count of hints has been received, send requests for individual hint's data
		for (int hintOffset = 0; hintOffset < hintCount; hintOffset++) {
            Game.networkManager.Send (
				ConvergeHintProtocol.Prepare (hintOffset),
				ProcessConvergeHint
				);
			//Debug.Log ("Send RequestConvergeHint");
		}
	}

	
	public void ProcessConvergeHint (NetworkResponse response)
	{
		
		ResponseConvergeHint args = response as ResponseConvergeHint;
		ConvergeHint hint = args.hint;
		
		if (hint == null) {
			Debug.LogError ("Returned hint not valid in ProcessConvergeHint");
		} else {
			hintDict.Add (hint.hintId, hint.text);
			//Debug.Log ("adding hint: " + hint.text);
		}
	}
	
	public int GetRandomHint (bool excludePrior) {
		int hintIdx;
		int isPrior;
		List <int> available = new List<int> ();
		
		//create set of available ids
		foreach (int id in hintDict.Keys) {
			int found = priorHintIdList.Find (entry => entry.Equals (id));
			if (!excludePrior || priorHintIdList.IndexOf (id) == Constants.ID_NOT_SET) {
				available.Add (id);
			}
		}
		
		//return -1 if no available hints found
		if (available.Count == 0) {
			return Constants.ID_NOT_SET;
		}
		//select hint randomly
		hintIdx = UnityEngine.Random.Range (0, available.Count - 1);
		
		return available [hintIdx];
	}

	public void GetTime ()
	{
		Debug.Log ("Get time request sent");
        Game.networkManager.Send (
			ConvergeGetTimeProtocol.Prepare (curRound),
			ProcessGetTime
		);
	}

	public void ProcessGetTime (NetworkResponse response)
	{
		int newBetCount = 0;
		ResponseConvergeGetTime args = response as ResponseConvergeGetTime;
		Debug.Log ("ResponseConvergeGetTime received. Bet time = " + args.betTime);
		timeRemain = args.betTime;
        if (timeRemain > 0) {
            checkCount = 0;
        }
		betStatusList.Clear ();
		betStatusList.Add(args.player1ID, args.betStatus1);
		betStatusList.Add(args.player2ID, args.betStatus2);
		betStatusList.Add(args.player3ID, args.betStatus3);
		betStatusList.Add(args.player4ID, args.betStatus4);
		newBetCount = args.betStatus1 + args.betStatus2 + args.betStatus3 + args.betStatus4;
		if (newBetCount > betCount) {
			audio.PlayOneShot ((AudioClip)Resources.Load ("Audio/gong"));
		}
		betCount = newBetCount;

		int id;
		short val;
		foreach (DictionaryEntry entry in betStatusList) {
			// do something with entry.Value or entry.Key
			id = (int) entry.Key;
			val = (short) entry.Value;
			Debug.Log ("ResponseConvergeGetTime, id, betstatus: " + id + " " + val);
			if ((id <= 0) || (id == player_id)) {
				// betStatusList.Remove (entry.Key);
			} else {
				// Debug.Log ("ResponseConvergeGetTime, id, betstatus: " + id + " " + val);
			}
		}

		if (timeRemain <= 0)
			windowClosed = true;
	}

    public void CheckPlayers() {
        Debug.Log ("Check Players request sent");
        Game.networkManager.Send (
            ConvergeCheckPlayersProtocol.Prepare (),
            ProcessCheckPlayers
        );
    }

    public void ProcessCheckPlayers (NetworkResponse response)
    {
        ResponseConvergeCheckPlayers args = response as ResponseConvergeCheckPlayers;
        Debug.Log ("ResponseConvergeCheckPlayers received. Status = " + args.status);
        if (args.status == 1 )  {   // means a player was dropped
            playerDrop = 600;   // Display player dropped a while 
        }
    }

	public void GetNames() {
		Debug.Log ("Get names request sent");
        Game.networkManager.Send (
			ConvergeGetNamesProtocol.Prepare (),
			ProcessGetNames
		);
	}

	public void ProcessGetNames (NetworkResponse response)
	{
		ResponseConvergeGetNames args = response as ResponseConvergeGetNames;
		// Debug.Log ("ResponseConvergeGetNames received");
		if (!roundsWon.Contains(player_id)) {
			roundsWon.Add (player_id, 0);
		}
		if (!roundsWon.Contains (args.player1ID) && (args.player1ID > 0)) {
			playerNames.Add (args.player1ID, args.player1Name);
			roundsWon.Add (args.player1ID, 0);
		}
		if (!roundsWon.Contains (args.player2ID) && (args.player2ID > 0)) {
			playerNames.Add (args.player2ID, args.player2Name);
			roundsWon.Add (args.player2ID, 0);
		}
		if (!roundsWon.Contains (args.player3ID) && (args.player3ID > 0)) {
			playerNames.Add (args.player3ID, args.player3Name);
			roundsWon.Add (args.player3ID, 0);
		}
		if (!roundsWon.Contains (args.player4ID) && (args.player4ID > 0)) {
			playerNames.Add (args.player4ID, args.player4Name);
			roundsWon.Add (args.player4ID, 0);
		}
		if (roundsWon.Count == numPlayers) {
			haveNames = true;
		}

		Debug.Log ("Other player id / name"); 

		int id;
		foreach (DictionaryEntry entry in playerNames) {
			// do something with entry.Value or entry.Key
			id = (int) entry.Key;
			Debug.Log (" " + id + " " + entry.Value);
			if ((id <= 0 ) || (id == player_id)) {
				// playerNames.Remove (entry.Key);
			} else {
				// Debug.Log (" " + id + " " + entry.Value);
			}
		}
	}

	public void GetActiveNames() {
		Debug.Log ("Get names request sent - for active names");
		Game.networkManager.Send (
			ConvergeGetNamesProtocol.Prepare (),
			ProcessGetActiveNames
		);
	}

	public void ProcessGetActiveNames (NetworkResponse response)
	{
		ResponseConvergeGetNames args = response as ResponseConvergeGetNames;
		Debug.Log ("ResponseConvergeGetNames received - active names");
		activePlayers.Clear ();
		activePlayers.Add(args.player1ID);
		activePlayers.Add(args.player2ID);
		activePlayers.Add(args.player3ID);
		activePlayers.Add(args.player4ID);
	}

	void ObtainScores() {
		int i, j;
		scores = barGraph.getScores ();
		formattedScores.Clear();
		for (i = 0; i < 5; i++) {
			formattedScores.Add (-1);
		}
		int numScores = scores.Count;
		i = (numScores < 5) ? 0 : numScores - 5;
		j = 0;
		while (i < numScores) {
			formattedScores [j] = scores [i];
			i++;
			j++;
		}
		Debug.Log ("Obtain Scores: Sending these scores to server:");
		for (i = 0; i < 5; i++) {
			Debug.Log (" " + i + " " + formattedScores [i]);
		}
	}

	// DH change
	// Displays score of other player in id_otherPlayer
	private void displayOtherGraph() {
        Debug.Log ("MC: DisplayOtherGraph");
        Game.networkManager.Send (
			ConvergeGetOtherScoreProtocol.Prepare (id_otherPlayer),
			ProcessConvergeGetOtherScore
		);

	}

	public void ProcessConvergeGetOtherScore (NetworkResponse response)
	{
		ResponseConvergeGetOtherScore args = response as ResponseConvergeGetOtherScore;
		Debug.Log ("MC: ResponseConvergeGetOtherScore received");
		otherScores.Clear ();
		otherScores.Add (args.score0);
		otherScores.Add (args.score1);
		otherScores.Add (args.score2);
		otherScores.Add (args.score3);
		otherScores.Add (args.score4);

		for (int i = 0; i < 5; i++ ) {
			Debug.Log (" " + i + " " + otherScores [i]);
		}

		if (args.score0 != -1) {  // Only display graph if some values
			// Give name and indicate that graph for other player 
            Debug.Log("MC: Display other player graph");
			barGraph.setOppScores(otherScores);
			barGraph.setOppGraph (true);
			oppGraph = true;
			barGraph.SetActive (true);
		}
	}
}