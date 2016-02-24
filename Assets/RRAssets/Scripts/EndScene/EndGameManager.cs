using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace RR
{
	public class EndGameManager : MonoBehaviour
	{
		public Canvas canvas;
		public EventSystem sys;
		public bool called = true;
		private string text = " ";
		private bool show = false;
		private GameObject obj;

		// Use this for initialization
		void Start ()
		{
		}

		void OnGUI ()
		{
			GUIStyle myStyle = new GUIStyle ();
			myStyle.normal.textColor = Color.blue;
			myStyle.fontSize = 50;
			myStyle.alignment = TextAnchor.UpperCenter;

			if (show) {
				GUI.Label (new Rect (Screen.width/2- 50, Screen.height / 2-50, 200, 100), text, myStyle);
				myStyle.normal.textColor = Color.red;
				myStyle.fontSize = 20;
				myStyle.alignment = TextAnchor.UpperCenter;
				GUI.Label (new Rect (Screen.width/2, Screen.height/ 2, 200, 100),"Winning Time is: " + PlayerPrefs.GetString ("Winning Time") , myStyle);
			}
		}
		
		// Update is called once per frame
		void Update ()
		{
			if (PlayerPrefs.GetInt ("Win") >= 0 && called) {
				Debug.Log ("Winning Time is: " + PlayerPrefs.GetString ("Winning Time"));
				if (PlayerPrefs.GetInt ("Win") == 1) {
					show = true;
					text = "YOU WIN!";
					Debug.Log ("YOU WIN!");
				} else {
					Debug.Log ("YOU LOSE!");
					show = true;
					called = false;
					text = "YOU LOSE";
				}
			}
		}
	
		public void backButton ()
		{
			Game.SwitchScene ("World");
		}
	}
}