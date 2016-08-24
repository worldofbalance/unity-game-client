using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class ClashHintsGUI: MonoBehaviour
{
	private GameObject mainObject;
	// Window Properties
	private float width = 800;
	private float height = 500;
	// Other
	private int window_id;
	private Rect windowRect;
	public Vector2 scrollPosition;
	public static int clicks = 0;
	public ClashMainMenu clashMainMenu;


	void Awake ()
	{
		mainObject = GameObject.Find ("MainObject");
		Debug.Log (mainObject);
		window_id = Constants.GetUniqueID ();
		clashMainMenu = GetComponent<ClashMainMenu>();
	}

	// Use this for initialization
	void Start ()
	{
		Game.StartEnterTransition ();
		windowRect = new Rect ((Screen.width - (width - 400)) / 2, (Screen.height - height) / 2, width, height);
//		tableContent ();
	}

	void OnGUI ()
	{
		windowRect = GUILayout.Window (window_id, windowRect, MakeWindow, "Hints");

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

		scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(800), GUILayout.Height(500));
		Debug.Log (clashMainMenu);
		Debug.Log (clashMainMenu.carnivore);
		if (clashMainMenu.carnivore.Count == 0 && clashMainMenu.herbivore.Count == 0 && clashMainMenu.omnivore.Count == 0 && clashMainMenu.plant.Count == 0) {
			GUILayout.Space (30);
			GUILayout.TextArea ("First Select The Player", style);
		} else {
			if (clicks >= 0) {
				GUILayout.Space (30);
				GUILayout.BeginHorizontal (); //side by side columns
				GUILayout.Space (30);
				GUILayout.TextArea ("Carnivore", style);
				GUILayout.Space (30);
				GUILayout.TextArea (clashMainMenu.carnivore.Count.ToString (), style);
				GUILayout.Space (30);
				GUILayout.EndHorizontal ();
				GUILayout.Space (50);
				GUILayout.BeginHorizontal (); //side by side columns
				GUILayout.Space (30);
				GUILayout.TextArea ("Omnivore", style);
				GUILayout.Space (30);
				GUILayout.TextArea (clashMainMenu.omnivore.Count.ToString (), style);
				GUILayout.Space (30);
				GUILayout.EndHorizontal ();
				GUILayout.Space (50);
				GUILayout.BeginHorizontal (); //side by side columns
				GUILayout.Space (30);
				GUILayout.TextArea ("Herbivore", style);
				GUILayout.Space (30);
				GUILayout.TextArea (clashMainMenu.herbivore.Count.ToString (), style);
				GUILayout.Space (30);
				GUILayout.EndHorizontal ();
				GUILayout.Space (50);
				GUILayout.BeginHorizontal (); //side by side columns
				GUILayout.Space (30);
				GUILayout.TextArea ("Plant", style);
				GUILayout.Space (35);
				GUILayout.TextArea (clashMainMenu.plant.Count.ToString (), style);
				GUILayout.Space (30);
				GUILayout.EndHorizontal ();
				GUILayout.Space (50);


			}

			if (clicks >= 1) {
				GUILayout.Space (30);
				GUILayout.TextArea ("");
				GUILayout.BeginHorizontal (); //side by side columns
				GUILayout.Space (30);
				GUILayout.TextArea ("Carnivore", style);
				GUILayout.Space (30);
				GUILayout.TextArea ("Omnivore", style);
				GUILayout.Space (30);
				GUILayout.TextArea ("Herbivore", style);
				GUILayout.Space (30);
				GUILayout.TextArea ("Plant", style);
				GUILayout.Space (30);
				GUILayout.EndHorizontal ();
				GUILayout.Space (50);

				GUILayout.BeginHorizontal(); //side by side columns
				GUILayout.Space (30);
				GUILayout.BeginVertical(); //side by side columns
				foreach (var specie in clashMainMenu.carnivore) {
					GUILayout.TextArea (specie, style);
					GUILayout.Space (30);
				}
				GUILayout.EndVertical();

				GUILayout.Space (30);

				GUILayout.BeginVertical(); //side by side columns

				foreach (var specie in clashMainMenu.omnivore) {
					GUILayout.TextArea (specie, style);
					GUILayout.Space (30);
				}
				GUILayout.EndVertical();

				GUILayout.Space (30);

				GUILayout.BeginVertical(); //side by side columns

				foreach (var specie in clashMainMenu.herbivore) {
					GUILayout.TextArea (specie, style);
					GUILayout.Space (30);
				}
				GUILayout.EndVertical();
				GUILayout.Space (30);

				GUILayout.BeginVertical(); //side by side columns

				foreach (var specie in clashMainMenu.plant) {
					GUILayout.TextArea (specie, style);
					GUILayout.Space (30);
				}
				GUILayout.EndVertical();
				GUILayout.Space (30);


				GUILayout.EndHorizontal();
				GUILayout.Space (80);

			}
		}
		GUILayout.EndScrollView();

		GUILayout.EndArea();

	}

	void MakeWindow (int id)
	{
		GUILayout.Space (10);

		GUILayout.Space (30);

		if (GUI.Button (new Rect (windowRect.width / 2 - 30, windowRect.height - 40, 100, 30), "Done")) {
			Debug.Log ("Done Pressed");
			if (clashMainMenu.carnivore.Count != 0 || clashMainMenu.herbivore.Count != 0 || clashMainMenu.omnivore.Count != 0 || clashMainMenu.plant.Count != 0) {
				if (clicks == 1) {
					clicks = 0;
				} else {
					clicks = 1;
				}
			}
			Done ();
		}

		GUI.BringWindowToFront (window_id);
		GUI.DragWindow ();
	}
		

	public void Done ()
	{
		Destroy (this);
	}


	// Update is called once per frame
	void Update ()
	{
	}

//	void tableContent (){
//
//		NetworkManagerCOS.getInstance ().Send (ClashNotificationProtocol.Prepare (), (res) => {
//			var response = res as ResponseClashNotification;
//
//			playerNames = response.playerNames;
//			matchResults = response.matchResults;
//			playDates = response.playDates;
//
//		});
//
//	}

	public void ProcessPlayGame (NetworkResponse response)
	{
		ResponsePlayGame args = response as ResponsePlayGame;

		if (args.status == 1) {
			GameState.player.credits -= 10;
			//Destroy (this);
		} else {
			Debug.Log ("Not enough credits");
		}
	}
}