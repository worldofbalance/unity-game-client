using UnityEngine;

using System.Collections;
using System.Collections.Generic;
namespace CW{
public class Chat : MonoBehaviour {

	// Window Properties
	private float width = 400;
	private float height = 200;
	// Other
	private Rect windowRect;
	private Rect buttonRect;
	private Vector2 scrollViewVector;
	private string message = "";
	private Vector2 lastQuadrant;
	private Vector2 lastPosition;
	private bool isHidden;
	public GUISkin skin;
	private Texture2D bgTexture;
	private Font font;
	private List<string> msgList = new List<string>();
	
	void Awake() {
		bgTexture = Resources.Load<Texture2D>(Constants.THEME_PATH + Constants.ACTIVE_THEME + "/gui_bg");
		font = Resources.Load<Font>("Fonts/" + "Chalkboard");

		NetworkManager.Listen(
			NetworkCode.MESSAGE,
			ProcessMessage
		);
	}
	
	// Use this for initialization
	void Start() {
		if (!isHidden) {
			windowRect = new Rect(10, Screen.height - height - 10, width, height);
		}

		scrollViewVector = Vector2.zero;
	}
	
	// Update is called once per frame
	void Update() {
	}

	void OnDestroy() {
		NetworkManager.Ignore(
			NetworkCode.MESSAGE,
			ProcessMessage
		);
	}

	void OnGUI() {
		GUI.skin = skin;
		windowRect = GUI.Window(Constants.CHAT_WIN, windowRect, MakeWindow, "", GUIStyle.none);
		
		if (Event.current.type == EventType.KeyUp && Event.current.keyCode == KeyCode.Return) {
			SendMessage();
		}
	}
	
	public void MakeWindow(int id) {
		GUI.color = new Color(1, 1, 1, 0.8f);
		Functions.DrawBackground(new Rect(0, 0, width, height), bgTexture);
		GUI.color = Color.white;
		
		GUIStyle style = new GUIStyle(GUI.skin.label);
		style.alignment = TextAnchor.UpperCenter;
		style.font = font;
		style.fontSize = 16;
		
		GUI.Label(new Rect((windowRect.width - 100) / 2, 0, 100, 30), "Chat", style);

		Rect innerRect = new Rect(25, 40, width - 50, height * 0.5f);

		style = new GUIStyle(GUI.skin.label);
		style.alignment = TextAnchor.UpperLeft;

		float rectHeight = 0;
		for (int i = 0; i < msgList.Count; i++) {
			rectHeight += style.CalcHeight(new GUIContent(msgList[i]), innerRect.width - 25);
		}
		
		GUI.Box(new Rect(innerRect.x - 10, innerRect.y - 10, innerRect.width + 20, innerRect.height + 20), "");
		scrollViewVector = GUI.BeginScrollView(innerRect, scrollViewVector, new Rect(0, 0, innerRect.width - 25, rectHeight));
			float yStart = 0;
			for (int i = 0; i < msgList.Count; i++) {
				float msgHeight = style.CalcHeight(new GUIContent(msgList[i]), innerRect.width - 25);
				GUI.Label(new Rect(0, yStart, innerRect.width - 25, msgHeight), msgList[i], style);
				yStart += msgHeight;
			}
		GUI.EndScrollView();
		
		GUI.SetNextControlName("chat_field");
		message = GUI.TextField(new Rect(innerRect.x - 10, innerRect.y + innerRect.height + 20, 300, 20), message, 100);

		style = new GUIStyle(GUI.skin.button);
		style.alignment = TextAnchor.UpperCenter;

		buttonRect = new Rect(320, innerRect.y + innerRect.height + 20, 60, 20);
		if (GUI.Button(buttonRect, "Send", style)) {
			SendMessage();
		}

		if (Input.GetKeyUp(KeyCode.Return)) {
			if (GUI.GetNameOfFocusedControl() != "chat_field") {
				GUI.FocusControl("chat_field");
			}
		}
	}

	public void SendMessage() {
		if (message == "") {
//			GUI.FocusControl("");
		} else {
			NetworkManager.Send(
				MessageProtocol.Prepare(0, message)
			);

			SetMessage("[" + GameState.player.name + "] says: " + message);
			message = "";
		}
	}

	public void SetMessage(string message) {
		msgList.Add(message);
		scrollViewVector = new Vector2(0, 9999);
	}
	
	public void ProcessMessage(NetworkResponse response) {
		ResponseMessage args = response as ResponseMessage;

		if (args.status == 0) {
			string message = "";
			
			if (args.type == 0) {
				if (args.username.Equals(GameState.player.name)) {
					return;
				}

				message += "[" + args.username + "] says: ";
			}
			
			message += args.message;
			SetMessage(message);
		}
	}
}
}