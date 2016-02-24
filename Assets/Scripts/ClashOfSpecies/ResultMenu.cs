using UnityEngine;
using System.Collections;

public class ResultMenu : MonoBehaviour 
{
	public GUISkin myskin;
	
	private Rect windowRect;
	private bool paused = true;
	
	private void Start()
	{
		windowRect = new Rect(Screen.width / 2 - 100, Screen.height / 2 - 100, 200, 200);
	}
	
	private void waiting()
	{

	}
	
	private void Update()
	{

	}
	
	private void OnGUI()
	{
		if (paused)

			windowRect = GUI.Window(0, windowRect, windowFunc, "You Win !  ");
	}
	
	private void windowFunc(int id)
	{

		if (GUILayout.Button("Quit"))
		{
			Application.LoadLevel("Main Menu");
		}
	}
}
