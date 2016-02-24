using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour
{
		public GUISkin myskin;
		public Rect windowRect;
		public bool paused = false, waited = true;

		public void Start ()
		{
				windowRect = new Rect (Screen.width / 2 - 100, Screen.height / 2 - 100, 200, 200);
		}

		public void waiting ()
		{
				waited = true;
		}

		public void ClickPause ()
		{
				paused = true;
		}

		public void Update ()
		{
				if (waited) {
						if (Input.GetKey (KeyCode.Escape)) {
								if (paused)
										paused = false;
								else
										paused = true;

								waited = false;
								Invoke ("waiting", 0.3f);
						}
				}
				Time.timeScale = paused ? 0f : 1f;
		}

		public void OnGUI ()
		{
				if (paused)
						windowRect = GUI.Window (0, windowRect, windowFunc, "Pause Menu");
		}

		public void windowFunc (int id)
		{
				if (GUILayout.Button ("Resume")) {
						paused = false;
				}

				if (GUILayout.Button ("Quit")) {
						Application.LoadLevel ("ClashMain");
				}
		}


}
