using UnityEngine;

using System.Collections;
using System.Collections.Generic;

public class RegisterColorPanel : MonoBehaviour {

	private GameObject mainObject;
	private Rect windowRect;

	private Register register;
	private Dictionary<int, Color32> PlayerColorList = new Dictionary<int, Color32> ();
	public Texture colorImage = (Texture)Resources.Load(Constants.IMAGE_RESOURCES_PATH + "Color-Select");

	private bool isBeHidden { get; set; }
	// Use this for initialization
	void Start () {
		mainObject = GameObject.Find("MainObject");
		register = GameObject.Find("Global Object").GetComponent<Register>();
		//register = GameObject.Find("Local Object").GetComponent<Register>();
		PlayerColorList = register.PlayerColorList;
		Hide();
	}
	
	// Update is called once per frame
	void Update () {
	}

	void OnGUI() {
				if (!isBeHidden) {
						int width = 40 * PlayerColorList.Count + PlayerColorList.Count;

						GUI.Box (new Rect (((Screen.width - width) / 2) , 520, width, 50), "");
						for (int i = 1; i <= PlayerColorList.Count; i++) {

							GUI.BeginGroup(new Rect(((Screen.width - width) / 2) + (i-1) * 40, 525, 40, 40));
								if (GUI.Button(new Rect(4, 0, 40, 40), "")) {
									register.setColor((short) i);
								}

								GUI.color  = PlayerColorList[i];
								GUI.DrawTexture(new Rect(4, 0, 40, 40), colorImage);
							GUI.EndGroup();
						}
				}
		
		}
	public void Show() {
		isBeHidden = false;
	}
	
	public void Hide() {
		isBeHidden = true;
	}
}
