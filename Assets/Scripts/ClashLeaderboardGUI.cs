using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class ClashLeaderboardGUI: MonoBehaviour
{
	private GameObject mainObject;
	// Window Properties
	private float width = 800;
	private float height = 500;
	// Other
	private int window_id;
	private string message = "Ranking";
	private string message1 = "Player Name";
	private string message2 = "Wins";
	private string message3 = "Loses";
	private Rect windowRect;
	public Vector2 scrollPosition;

	public Dictionary<int, string> playerNames {get; set;}
	public Dictionary<int, string> rankings {get; set;}
	public Dictionary<int, string> playerWins {get; set;}
	public Dictionary<int, string> playerLoses {get; set;}

	void Awake ()
	{
		mainObject = GameObject.Find ("MainObject");
		window_id = Constants.GetUniqueID ();
		playerNames = new Dictionary<int, string>();
		playerWins = new Dictionary<int, string>();
		playerLoses = new Dictionary<int, string>();
	}

	// Use this for initialization
	void Start ()
	{
		Game.StartEnterTransition ();
		windowRect = new Rect ((Screen.width - (width - 400)) / 2, (Screen.height - height) / 2, width, height);
		tableContent ();
	}

	void OnGUI ()
	{
		windowRect = GUILayout.Window (window_id, windowRect, MakeWindow, "Leaderboard");

		GUILayout.Space (30);

		if (Event.current.type == EventType.KeyUp && Event.current.keyCode == KeyCode.Return) {
			Done ();
		}

		GUIStyle style = new GUIStyle ();
		style.alignment = TextAnchor.UpperLeft;
		style.normal.textColor = Color.white;
		style.fontSize = 20;

		GUILayout.BeginArea(windowRect);
		GUILayout.Space (30);
		GUILayout.BeginHorizontal(); //side by side columns
		GUILayout.Space (30);
		GUILayout.TextArea (message, style);
		GUILayout.Space (30);
		GUILayout.TextArea (message1, style);
		GUILayout.Space (30);
		GUILayout.TextArea (message2, style);
		GUILayout.Space (30);
		GUILayout.TextArea (message3, style);
		GUILayout.Space (30);
		GUILayout.EndHorizontal();
		GUILayout.Space (50);

		scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(800), GUILayout.Height(500));
		GUILayout.BeginHorizontal(); //side by side columns
		GUILayout.Space (50);
		GUILayout.BeginVertical(); //side by side columns
		foreach (var pair in playerNames) {
			GUILayout.TextArea (pair.Key.ToString(), style);
			GUILayout.Space (30);
		}
		GUILayout.EndVertical();


		GUILayout.Space (100);

		GUILayout.BeginVertical(); //side by side columns
		foreach (var pair in playerNames) {
			GUILayout.TextArea (pair.Value, style);
			GUILayout.Space (30);
		}
		GUILayout.EndVertical();

		GUILayout.Space (120);

		GUILayout.BeginVertical(); //side by side columns

		foreach (var pair in playerWins) {
			GUILayout.TextArea (pair.Value, style);
			GUILayout.Space (30);
		}
		GUILayout.EndVertical();

		GUILayout.Space (100);

		GUILayout.BeginVertical(); //side by side columns

		foreach (var pair in playerLoses) {
			GUILayout.TextArea (pair.Value, style);
			GUILayout.Space (30);
		}
		GUILayout.EndVertical();

		GUILayout.EndHorizontal();
		GUILayout.EndScrollView();
		GUILayout.EndArea();
	}

	void MakeWindow (int id)
	{
		GUILayout.Space (10);

		GUILayout.Space (30);

		if (GUI.Button (new Rect (windowRect.width / 2 - 30, windowRect.height - 40, 100, 30), "Done")) {
			Debug.Log ("Done Pressed");
			Done ();
		}

		GUI.BringWindowToFront (window_id);
		GUI.DragWindow ();
	}

	public void setMessage (string message)
	{
		this.message = message;
	}

	public void Done ()
	{
		Destroy (this);
	}


	// Update is called once per frame
	void Update ()
	{
	}

	void tableContent (){

		NetworkManagerCOS.getInstance ().Send (ClashLeaderboardProtocol.Prepare (), (res) => {
			var response = res as ResponseClashLeaderboard;

			playerNames = response.playerNames;
			playerWins = response.playerWinsNo;
			playerLoses = response.playerLosesNo;

		});

	}


	public void ProcessPlayGame (NetworkResponse response)
	{
		ResponsePlayGame args = response as ResponsePlayGame;

		if (args.status == 1) {
			GameState.player.credits -= 10;
			//Destroy (this);
			Game.SwitchScene ("ClashSplash");
		} else {
			Debug.Log ("Not enough credits");
		}
	}
}

