using UnityEngine;

using System.Collections;
using System.Collections.Generic;

public class PlayerList : MonoBehaviour {

	private GameObject mainObject;

	// Window Properties
	private float width = 280;
	private float height = 55;
	// Other
	private Rect windowRect;
	private Rect[] buttonRectList;
	private bool isHidden { get; set; }
	
	void Awake() {
		buttonRectList = new Rect[3];
		mainObject = GameObject.Find("MainObject");
		isHidden = true;

		NetworkManager.Send(
			PlayersProtocol.Prepare()
		);
	}

	// Use this for initialization
	void Start () {
		//mainObject = GameObject.Find("MainObject");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI() {
		if (!isHidden) {
						windowRect = new Rect(25, 10, 100, height);
						windowRect = GUI.Window(Constants.PLIST_WIN, windowRect, MakeWindow, "PlayerList");
				}

	}
	
	void MakeWindow(int id) {
		if (GUI.Button(new Rect(10, 20, 80, 30), "PlayerList")) {
			mainObject.AddComponent<PlayerListPanel>();
			mainObject.GetComponent<PlayerListPanel>().enabled = true;
			mainObject.GetComponent<PlayerListPanel>().Show();
			Hide();
	}
	}

	public void Show() {
		isHidden = false;
	}
	
	public void Hide() {
		isHidden = true;
	}
}
