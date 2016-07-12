using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class ClashNotificationGUI: MonoBehaviour
{
	private GameObject mainObject;
	// Window Properties
	private float width = 800;
	private float height = 500;
	// Other
	private int window_id;
	private string message = "Player Attacked";
	private string message2 = "Match Result";
	private string message3 = "Play Date";
	private Rect windowRect;
	public Vector2 scrollPosition;

	public Dictionary<int, string> playerNames {get; set;}
	public Dictionary<int, string> matchResults {get; set;}
	public Dictionary<int, string> playDates {get; set;}

	void Awake ()
	{
		mainObject = GameObject.Find ("MainObject");
		window_id = Constants.GetUniqueID ();
		playerNames = new Dictionary<int, string>();
		matchResults = new Dictionary<int, string>();
		playDates = new Dictionary<int, string>();
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
		windowRect = GUILayout.Window (window_id, windowRect, MakeWindow, "Update from Your Last Log Out");

		GUILayout.Space (30);

		if (Event.current.type == EventType.KeyUp && Event.current.keyCode == KeyCode.Return) {
			Done ();
		}

		GUIStyle style = new GUIStyle ();
		style.alignment = TextAnchor.UpperLeft;
		style.normal.textColor = Color.white;
		style.fontSize = 20;

		GUILayout.BeginArea(windowRect);
		if (playerNames.Count == 0) {
			GUILayout.Space (30);
			GUILayout.TextArea ("There is No Update Since Your Last Log Out", style);
		} else {
			GUILayout.Space (30);
			GUILayout.BeginHorizontal(); //side by side columns
			GUILayout.Space (30);
			GUILayout.TextArea (message, style);
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
				GUILayout.TextArea (pair.Value, style);
				GUILayout.Space (30);
			}
			GUILayout.EndVertical();

			GUILayout.Space (120);

			GUILayout.BeginVertical(); //side by side columns

			foreach (var pair in matchResults) {
				GUILayout.TextArea (pair.Value, style);
				GUILayout.Space (30);
			}
			GUILayout.EndVertical();

			GUILayout.Space (100);

			GUILayout.BeginVertical(); //side by side columns

			foreach (var pair in playDates) {
				GUILayout.TextArea (pair.Value, style);
				GUILayout.Space (30);
			}
			GUILayout.EndVertical();

			GUILayout.EndHorizontal();
			GUILayout.EndScrollView();
		}
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

		NetworkManagerCOS.getInstance ().Send (ClashNotificationProtocol.Prepare (), (res) => {
			var response = res as ResponseClashNotification;

			playerNames = response.playerNames;
			matchResults = response.matchResults;
			playDates = response.playDates;

		});

	}
}

