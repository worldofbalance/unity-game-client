using UnityEngine;

using System;
using System.Collections;
using System.Text.RegularExpressions;
using System.Collections.Generic;

public class Register : MonoBehaviour {
	
	private GameObject mainObject;
	// Window Properties
	private float width = 300;
	private float height = 100;
	// Other
	private string fname = "";
	private string lname = "";
	private string name = "";
	private string email = "";
	private string password = "";
	private string confirm = "";
	private short color = 1;
	private Rect windowRect;
	private bool isInitial = true;

	public Dictionary<int, Color32> PlayerColorList {get; private set;}

	void Awake() {
		mainObject = GameObject.Find("MainObject");
//		gameObject.AddComponent("RegisterColorPanel");

		PlayerColorList = new Dictionary<int, Color32>();
//		PlayerColorList = GameObject.Find("Map").GetComponent<Map>().playerColorList;
	}
	
	// Use this for initialization
	void Start() {
		windowRect = new Rect((Screen.width - width) / 2, 100, width, height);
	}
	
	void OnGUI() {
		windowRect = GUILayout.Window(Constants.REGISTER_WIN, windowRect, MakeWindow, "Register");
		if (Event.current.type == EventType.KeyUp && Event.current.keyCode == KeyCode.Return) Submit();
	}
	
	void MakeWindow(int id) {
		GUILayout.Label("First Name");
		GUI.SetNextControlName("fname_field");
		fname = GUILayout.TextField(fname, 25);

		GUILayout.Label("Last Name");
		GUI.SetNextControlName("lname_field");
		lname = GUILayout.TextField(lname, 25);
		
		GUILayout.Label("Display Name");
		GUI.SetNextControlName("name_field");
		name = GUILayout.TextField(name, 25);

		GUILayout.Label("Email Address");
		GUI.SetNextControlName("email_field");
		email = GUILayout.TextField(email, 25);
		
		GUILayout.Label("Password");
		GUI.SetNextControlName("password_field");
		password = GUILayout.PasswordField(password, "*"[0], 25);
		
		GUILayout.Label("Confirm Password");
		GUI.SetNextControlName("confirm_field");
		confirm = GUILayout.PasswordField(confirm, "*"[0], 25);

		if (isInitial) { // && GUI.GetNameOfFocusedControl() == "") {
			GUI.FocusControl("fname_field");
			isInitial = false;
		}
	
//		GUILayout.Label("Pick Color");
//		GUI.SetNextControlName("color_field");

//		var boxStyle = new GUIStyle(GUI.skin.box);
//		boxStyle.normal.textColor = PlayerColorList[color];
//		if (GUILayout.Button ("PLAYER COLOR", boxStyle, GUILayout.ExpandWidth (true))) {
//			gameObject.GetComponent<RegisterColorPanel>().Show ();
//		}


		GUILayout.Space(40);

		if (GUI.Button(new Rect(windowRect.width / 2 - 110, windowRect.height - 40, 100, 30), "Submit")) 
			Submit();
		if (GUI.Button(new Rect(windowRect.width / 2 + 10, windowRect.height - 40, 100, 30), "Cancel")) 
			SwitchToLogin();
	}
	
	public bool CheckEmail(string email) {
		return Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z");
	}

	public void setColor(short color){
		this.color = color;
	}

	public void Submit() {
		name = name.Trim();
		email = email.Trim();
		password = password.Trim();
		confirm = confirm.Trim();

		if (name.Length == 0) {
//			mainObject.GetComponent<Main>().CreateMessageBox("Display Name Required");
			GUI.FocusControl("name_field");
		} else if (email.Length == 0) {
//			mainObject.GetComponent<Main>().CreateMessageBox("Email Required");
			GUI.FocusControl("email_field");
		} else if (!CheckEmail(email)) {
//			mainObject.GetComponent<Main>().CreateMessageBox("Invalid Email");
			GUI.FocusControl("email_field");
		} else if (password.Length == 0) {
//			mainObject.GetComponent<Main>().CreateMessageBox("Password Required");
			GUI.FocusControl("password_field");
		} else if (!password.Equals(confirm)) {
//			mainObject.GetComponent<Main>().CreateMessageBox("Passwords do not match");
			GUI.FocusControl("confirm_field");
		} else {
			NetworkManager.Send(
				RegisterProtocol.Prepare(fname, lname, email, password, name, color),
				ProcessRegister
			);
		}
	}
	
	public void ProcessRegister(NetworkResponse response) {
		ResponseRegister args = response as ResponseRegister;

		switch (args.status) {
			case 0:
//				NetworkManager.Send(
//					SpeciesActionProtocol.Prepare(0, 0),
//					ProcessSpeciesAction
//				);

				SwitchToLogin();
				break;
			case 1:
//				mainObject.GetComponent<Main>().CreateMessageBox("Email Taken");
				break;
			case 2:
//				mainObject.GetComponent<Main>().CreateMessageBox("Display Name Taken");
				break;
		}
	}

	public void SwitchToLogin() {
//		Destroy (gameObject.GetComponent<RegisterColorPanel>());
		Destroy(this);
		gameObject.GetComponent<Login>().SetActive(true);
	}
	// Update is called once per frame
	void Update() {
	}

	public void ProcessSpeciesAction(NetworkResponse response) {
		ResponseSpeciesAction args = response as ResponseSpeciesAction;
		
		if (args.action == 0) {
			Dictionary<int, int> speciesList = new Dictionary<int, int>();
			
			foreach (string item in args.selectionList.Split(',')) {
				string[] pair = item.Split(':');
				int species_id = int.Parse(pair[0]);
				int biomass = int.Parse(pair[1]);
				
				speciesList.Add(species_id, biomass);
				Debug.Log(species_id + " " + biomass);
			}

			NetworkManager.Send(
				SpeciesActionProtocol.Prepare(1, speciesList),
				ProcessSpeciesAction
			);
		}
	}
}
