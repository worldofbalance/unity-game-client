using UnityEngine;
using System;
using System.Collections;

public class MultiplayerGames : MonoBehaviour {

    private GameObject mainObject;

    // Window Properties
    private float width = 500;
    private float height = 300;

    // Other
    private int window_id;
    private string message = "Single Player Game. Click start to play.";
    private Rect windowRect;

    private bool enableRRButton = true;
    private bool enableCWButton = true;
    private bool enableMCButton = true;
    private bool enableSDButton = true;

    private bool quiting = false;
    private bool waiting = false;

    private int room_id = 0;

    void Awake() {
        mainObject = GameObject.Find("MainObject");
        window_id = Constants.GetUniqueID();

        Game.networkManager.Listen (NetworkCode.PAIR, OnPairResult);
        Game.networkManager.Listen (NetworkCode.QUIT_ROOM, OnQuitRoomResult);
        Application.runInBackground = true;
    }

    void OnDestroy () {
        Game.networkManager.Ignore (NetworkCode.PAIR, OnPairResult);
        Game.networkManager.Ignore (NetworkCode.QUIT_ROOM, OnQuitRoomResult);
    }

    // Use this for initialization
    void Start() {
        Game.StartEnterTransition ();
        windowRect = new Rect ((Screen.width - width) / 2, (Screen.height - height) / 2, width, height);

        StartCoroutine(RequestGetRooms(1f));
    }

    void OnGUI() {
        Color newColor = new Color(1,1,1,1.0f);
        GUI.color = newColor;

        windowRect = GUILayout.Window(window_id, windowRect, MakeWindow, "Game Rooms");
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

        GUILayout.Space(30);

        GUI.enabled = enableRRButton;
        if (GUI.Button(new Rect(10, windowRect.height - 80, 140, 30), "Play Running Rhino")) {
            Game.networkManager.Send (PairProtocol.Prepare (Constants.MINIGAME_RUNNING_RHINO, -1));
        }

        GUI.enabled = enableCWButton;
        if (GUI.Button(new Rect(10, windowRect.height - 40, 140, 30), "Play Cards of Wild")) {
            Game.networkManager.Send (PairProtocol.Prepare (Constants.MINIGAME_CARDS_OF_WILD, -1));
        }

        // DH change
        GUI.enabled = enableMCButton;
        if (GUI.Button(new Rect(165, windowRect.height - 80, 160, 30), "Play Multi-Convergence")) {
            Game.networkManager.Send (PairProtocol.Prepare (Constants.MINIGAME_MULTI_CONVERGENCE, -1));
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

            var room = RoomManager.getInstance ().getRoom (args.id);
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
                if (SD.Constants.PLAYER_NUMBER != 1) {
                    SD.SDMain.networkManager.Send (
                        SD.SDPlayInitProtocol.Prepare (SD.Constants.USER_ID, this.room_id), 
                        SDProcessPlayInit
                    );
                } else {
                    Game.SwitchScene("SDReadyScene");
                }
            } else if (args.gameID == Constants.MINIGAME_MULTI_CONVERGENCE) {
                // DH change
                MultiConvergeGame.matchID = args.id;   // game id
                short host = 0;  // Default - not the host
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
            var room = RoomManager.getInstance().addRoom(args.id, args.gameID);
            room.host = GameState.player.GetName();
            room.addPlayer(userID);

            this.enableRRButton = false;
            this.enableCWButton = false;
            this.enableMCButton = false;
            this.enableSDButton = false;
            this.waiting = true;

            if (args.gameID == Constants.MINIGAME_SEA_DIVIDED) {
                SD.Constants.PLAYER_NUMBER = 1; // The host player is player 1
                // The host player has been allocated a room, send that request to the SD server.
                SD.SDMain.networkManager.Send (
                    SD.SDPlayInitProtocol.Prepare (SD.Constants.USER_ID, room.id), 
                    SDProcessPlayInit
                );
            }
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
            Game.SwitchScene ("MultiConverge");
        }
    }

    public RR.RequestRaceInit RR_RequestRaceInit ()
    {
        RR.RequestRaceInit request = new RR.RequestRaceInit ();
        request.Send (RR.Constants.USER_ID, this.room_id);
        return request;
    }

    public void SDProcessPlayInit(NetworkResponse response) {
        SD.ResponseSDPlayInit args = response as SD.ResponseSDPlayInit;
        SD.Constants.PLAYER_NUMBER = args.playerNumber;
        Debug.Log ("The SD player number is " + args.playerNumber);
        if (SD.Constants.PLAYER_NUMBER == 2) {
            Game.SwitchScene("SDReadyScene");
        }
    }

}
