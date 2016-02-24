using UnityEngine;

using System;
using System.Collections;

public class GameResources : MonoBehaviour {
	
	public GUISkin skin;
	
	private int credits;
	private int coins;
	private Boolean showTopListDialog = false;
	private String[] topPlayerNames = new string[3];
	private int[] topPlayerScores = new int[3];
	
	void Awake() {
		skin = Resources.Load("Skins/DefaultSkin") as GUISkin;
	}
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		try {
			//			credits = GameState.world.credits;
			credits = GameState.player.credits;
		} catch (NullReferenceException e) {
		}
	}
	
	void OnGUI() {
		GUI.BeginGroup(new Rect(Screen.width - 150 - 20, 10, 150, 70), GUI.skin.box);
		
		GUIStyle style = new GUIStyle(skin.label);
		style.font = skin.font;
		style.fontSize = 20;
		style.alignment = TextAnchor.UpperRight;
		
		GUIExtended.Label(new Rect(-10, 10, 150, 50), credits.ToString("n0") + " Credits", style, Color.black, Color.green);
		
		style = new GUIStyle(skin.label);
		style.font = skin.font;
		style.fontSize = 20;
		style.alignment = TextAnchor.UpperRight;
		
		GUIExtended.Label(new Rect(-10, 30, 150, 50), coins.ToString("n0") + " Coins", style, Color.black, Color.yellow);
		
		GUI.EndGroup ();
		
		if (GUI.Button (new Rect (Screen.width - 170, 90, 150, 30), "Top 3 players")) {
			showTopListDialog = true;
		}
		
		if (showTopListDialog) {
			GUI.BeginGroup(new Rect(Screen.width/2 - 150, Screen.height/2 - 100, 300, 200), GUI.skin.box);
			
			style.font = skin.font;
			style.fontSize = 20;
			style.alignment = TextAnchor.UpperCenter;
			
			GUIExtended.Label(new Rect(20, 10, 250, 50), " Top players ranking", style, Color.black, Color.white);
			
			style.fontSize = 15;
			style.alignment = TextAnchor.UpperCenter;
			
			getTopPlayers();
			
			GUIExtended.Label(new Rect(10, 50, 250, 50), topPlayerNames[0] + "\t" + topPlayerScores[0], style, Color.black, Color.white);
			GUIExtended.Label(new Rect(10, 100, 250, 50), topPlayerNames[1] + "\t" + topPlayerScores[1], style, Color.black, Color.white);
			GUIExtended.Label(new Rect(10, 150, 250, 50), topPlayerNames[2] + "\t" + topPlayerScores[2], style, Color.black, Color.white);
			
			GUI.EndGroup();
		}
		
		if (Input.GetKeyDown ("return") || Input.GetMouseButtonDown (0)) {
			showTopListDialog = false;
		}
	}
	
	private void getTopPlayers() {
		NetworkManager.Send(
			TopListProtocol.Prepare(),
			ProcessTopList
			);
	}
	
	private void ProcessTopList(NetworkResponse response) {
		ResponseTopList args = response as ResponseTopList;
		topPlayerNames [0] = args.name1;
		topPlayerNames [1] = args.name2;
		topPlayerNames [2] = args.name3;
		topPlayerScores [0] = args.score1;
		topPlayerScores [1] = args.score2;
		topPlayerScores [2] = args.score3;
		//Debug.Log ("rank 1 player: " + args.name1 + " with " + args.score1 + " points.");
		//Debug.Log ("rank 2 player: " + args.name2 + " with " + args.score2 + " points.");
		//Debug.Log ("rank 3 player: " + args.name3 + " with " + args.score3 + " points.");
	}
	
	public void SetCredits(int credits) {
		this.credits = credits;
	}
	
	public void SetCoins(int coins) {
		this.coins = coins;
	}
}
