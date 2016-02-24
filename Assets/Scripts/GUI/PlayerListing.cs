using UnityEngine;

using System.Collections;
using System.Collections.Generic;

public class PlayerListing : MonoBehaviour {

	public GUISkin skin;
	
	public Dictionary<int, string> playerList = new Dictionary<int, string>();

	// Use this for initialization
	void Start () {
		playerList.Add(1, "Unknown");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI() {
		GUIStyle style = new GUIStyle(skin.label);
		style.font = skin.font;
		style.fontSize = 18;

		GUIExtended.Label(new Rect(130, 10, 200, 50), playerList.Count + " Player(s) Online", style, Color.black, Color.white);
	}
}
