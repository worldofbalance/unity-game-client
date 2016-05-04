using UnityEngine;

using System;
using System.Collections;

public class SDLogin : MonoBehaviour {

    private int window_id = 1;
    // Window Properties
    private float left;
    private float top;
    private float width = 280;
    private float height = 190;
    // Other
    public Texture background;
    private Font font;
    private string user_id = "";
    private string password = "";
    private Rect windowRect;
    private bool isActive = true;
    private bool isInitial = true;

    void Awake() {
        //DontDestroyOnLoad(gameObject);

        left = (Screen.width - width) / 2;
        top = (Screen.height - height) / 2;

        windowRect = new Rect(left, top, width, height);
        font = Resources.Load<Font>("Fonts/" + "Chalkboard");
        /*SD.SDMessageQueue.getInstance().AddCallback (SD.Constants.SMSG_AUTH, SD_ResponseLogin);
        SD.SDMessageQueue.getInstance ().AddCallback (SD.Constants.SMSG_RACE_INIT, SD_ResponsePlayInit);*/
        SD.SDMain.networkManager.Listen (NetworkCode.SD_GAME_LOGIN, SD_ResponseLogin);
        SD.SDMain.networkManager.Listen (NetworkCode.SD_PLAY_INIT, SDProcessPlayInit);
    }

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    void OnDestroy() {
        /*SD.SDMessageQueue.getInstance().RemoveCallback (SD.Constants.SMSG_AUTH);
        SD.SDMessageQueue.getInstance ().RemoveCallback (SD.Constants.SMSG_RACE_INIT);*/
        SD.SDMain.networkManager.Ignore (NetworkCode.SD_GAME_LOGIN, SD_ResponseLogin);
        SD.SDMain.networkManager.Ignore(NetworkCode.SD_PLAY_INIT, SDProcessPlayInit);
    }

    void OnGUI() {
        // Background
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), background, ScaleMode.ScaleAndCrop);

        // Login Interface
        if (isActive) {
            windowRect = GUI.Window(window_id, windowRect, MakeWindow, "Login");

            if (Event.current.type == EventType.KeyUp && Event.current.keyCode == KeyCode.Return) {
                Submit();
            }
        }
    }

    void MakeWindow(int id) {
        //Functions.DrawBackground(new Rect(0, 0, width, height), bgTexture);

        GUIStyle style = new GUIStyle(GUI.skin.label);
        style.alignment = TextAnchor.UpperCenter;
        style.font = font;
        style.fontSize = 16;


        GUI.BeginGroup(new Rect(10, 25, 300, 100));
        {
            style.alignment = TextAnchor.UpperLeft;
            style.fontSize = 14;
            GUI.Label(new Rect(0, 0, 300, 30), "User ID (Display Name or Email)", style);
            GUI.SetNextControlName("username_field");
            user_id = GUI.TextField(new Rect(0, 25, windowRect.width - 20, 25), user_id, 25);
        }
        GUI.EndGroup();

        GUI.BeginGroup(new Rect(10, 80, 300, 100));
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = 14;
        GUI.Label(new Rect(0, 0, 100, 30), "Password", style);
        GUI.SetNextControlName("password_field");
        password = GUI.PasswordField(new Rect(0, 25, windowRect.width - 20, 25), password, "*"[0], 25);
        GUI.EndGroup();

        if (isInitial) {  // && GUI.GetNameOfFocusedControl() == "") {
            GUI.FocusControl("username_field");
            isInitial = false;
        }

        if (GUI.Button(new Rect(width / 2 - 110, 145, 100, 30), "Log In")) {
            Submit();
        }

        if (GUI.Button(new Rect(width / 2 + 10, 145, 100, 30), "Register")) {
            SwitchToRegister();
        }
    }

    public void Submit() {
        user_id = user_id.Trim();
        password = password.Trim();

        if (user_id.Length == 0) {
            //          mainObject.GetComponent<Main>().CreateMessageBox("User ID Required");
            GUI.FocusControl("username_field");
        } else if (password.Length == 0) {
            //          mainObject.GetComponent<Main>().CreateMessageBox("Password Required");
            GUI.FocusControl("password_field");
        } else {
            // Send a request to login.
            SD.SDMain.networkManager.Send (SD.SDLoginProtocol.Prepare (user_id, password), SD_ResponseLogin);
        }
    }
        
        



    IEnumerator wait(Boolean condition)
    {
        if (condition) {
            yield return new WaitForSeconds (1f);
        }
        Game.SwitchScene("World"); //"World");
    }

    public void SwitchToRegister() {
        isActive = false;
        gameObject.AddComponent<Register>();
    }

    public void SetActive(bool active) {
        this.isActive = active;
        //reset GUI focus if reactivating login.
        this.isInitial = this.isInitial || this.isActive;
    }

    public void SD_ResponseLogin(NetworkResponse r)
    {
        SD.ResponseSDLogin response = r as SD.ResponseSDLogin;
        if (response.status == 0)
        {
            Debug.Log ("Logged in ");
            SD.Constants.USER_ID = response.userId;
            // Send the playInit request.
            SD.SDMain.networkManager.Send (SD.SDPlayInitProtocol.Prepare (
                SD.Constants.USER_ID, SD.Constants.TEMP_ROOM_ID), SDProcessPlayInit);
            Debug.Log ("Sent the play init request");
        }
        else {
            Debug.Log("SD: Login Failed");
        }
    }

    /*
    public void SD_ResponseLogin(SD.ExtendedEventArgs eventArgs)
    {
        SD.ResponseLoginEventArgs args = eventArgs as SD.ResponseLoginEventArgs;

        if (args.status == 0)
        {
            SD.Constants.USER_ID = args.user_id;
            // send the request to initialize the game.
            SD.SDConnectionManager sManager = SD.SDConnectionManager.getInstance();
            sManager.Send(SD_RequestPlayInit());
        }
        else {
            Debug.Log("SD: Login Failed");
        }
    }

    public SD.RequestPlayInit SD_RequestPlayInit()
    {
        SD.RequestPlayInit request = new SD.RequestPlayInit();
        request.Send(SD.Constants.USER_ID, SD.Constants.TEMP_ROOM_ID);
        return request;
    }
    public void SD_ResponsePlayInit(SD.ExtendedEventArgs eventArgs) {
        SD.ResponsePlayInitEventArgs args = eventArgs as SD.ResponsePlayInitEventArgs;
        SD.Constants.PLAYER_NUMBER = args.playerNumber;
        Debug.Log ("The player number is " + args.playerNumber);
        Game.SwitchScene ("SDReadyScene");
    }*/

    public void SDProcessPlayInit(NetworkResponse response) {
        SD.ResponseSDPlayInit args = response as SD.ResponseSDPlayInit;
        SD.Constants.PLAYER_NUMBER = args.playerNumber;
        Debug.Log ("The player number is " + args.playerNumber);
        Game.SwitchScene ("SDReadyScene");
    }
}

