using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {
	
	private bool isHidden;
	private Rect windowRect;

	// Use this for initialization
	void Start () {
		isHidden = true;
	}
	
	// Update is called once per frame
	void Update () {
		windowRect = new Rect(0, 50, Screen.width, Screen.height);
	}
	
	void OnGUI() {
		if (GUI.Button(new Rect(10, 10, 100, 30), "Menu")) {
			isHidden = !isHidden;
		}
		
		if (!isHidden) {
			windowRect = GUI.Window(Constants.MENU_WIN, windowRect, MakeWindow, "Menu");
		}
	}
	
	public void MakeWindow(int id) {
	}
}
