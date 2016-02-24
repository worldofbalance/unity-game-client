using UnityEngine;
using System.Collections;

public class HighScores : MonoBehaviour {

	public GUISkin skin;

	private short mode;
	private int[] highScoreList = new int[3];
	private int[] totalScoreList = new int[3];
	private int[] currentScoreList = new int[3];

	// Use this for initialization
	void Start () {
		highScoreList = new int[]{3000, 2000, 1000};
		totalScoreList = new int[]{2000, 1000, 0};
		currentScoreList = new int[]{1000, 0, 0};

		StartCoroutine(SwapScores(10f));
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI() {
		GUIStyle style = new GUIStyle(skin.label);
		style.font = skin.font;
		style.fontSize = 22;
		style.alignment = TextAnchor.UpperRight;

		string title = mode == 0 ? "Your Scores" : "Top Scores";
		
		GUI.BeginGroup(new Rect(Screen.width - 210, 10, 200, 300));

			GUIExtended.Label(new Rect(0, 0, 200, 50), title, style, Color.black, Color.white);
			
			if (mode == 0) {
				ShowYourScores();
			} else {
				ShowTopScores();
			}

		GUI.EndGroup();
	}
	
	public void SetHighScoreList(int[] highScoreList) {
	}
	
	public void SetTotalScoreList(int[] totalScoreList) {
	}
	
	public void SetCurrentScoreList(int[] currentScoreList) {
	}
	
	public IEnumerator SwapScores(float time) {
		yield return new WaitForSeconds(time);
		
		if (mode == 0) {
			mode = 1;
		} else {
			mode = 0;
		}
		
		StartCoroutine(SwapScores(10f));
	}

	public void ShowYourScores() {
		GUIStyle style = new GUIStyle(skin.label);
		style.font = skin.font;
		style.fontSize = 20;
		style.alignment = TextAnchor.UpperRight;

		GUIStyle scoreStyle = new GUIStyle(skin.label);
		scoreStyle.font = skin.font;
		scoreStyle.fontSize = 18;
		scoreStyle.alignment = TextAnchor.UpperRight;

		{
			Rect rect = new Rect(0, 30, 200, 50);
			GUIExtended.Label(rect, "High Score", style, Color.black, new Color(1.0f, 0.93f, 0.73f, 1.0f));
			
			string scoreText = highScoreList[0] == 0 ? "-" : highScoreList[0].ToString("n0");
			GUIExtended.Label(new Rect(rect.x, rect.y + 20, 200, 50), scoreText, scoreStyle, Color.black, Color.white);
		}
		
		{
			Rect rect = new Rect(0, 80, 200, 50);
			GUIExtended.Label(rect, "Total Score", style, Color.black, new Color(1.0f, 0.93f, 0.73f, 1.0f));

			string scoreText = totalScoreList[0] == 0 ? "-" : totalScoreList[0].ToString("n0");
			GUIExtended.Label(new Rect(rect.x, rect.y + 20 , 200, 50), scoreText, scoreStyle, Color.black, Color.white);
		}
		
		{
			Rect rect = new Rect(0, 130, 200, 50);
			GUIExtended.Label(rect, "Current Score", style, Color.black, new Color(1.0f, 0.93f, 0.73f, 1.0f));
			
			string scoreText = currentScoreList[0] == 0 ? "-" : currentScoreList[0].ToString("n0");
			GUIExtended.Label(new Rect(rect.x, rect.y + 20, 200, 50), scoreText, scoreStyle, Color.black, Color.white);
		}
	}
	
	public void ShowTopScores() {
		GUIStyle style = new GUIStyle(skin.label);
		style.font = skin.font;
		style.fontSize = 20;
		style.alignment = TextAnchor.UpperRight;

		GUIStyle scoreStyle = new GUIStyle(skin.label);
		scoreStyle.font = skin.font;
		scoreStyle.fontSize = 18;
		scoreStyle.alignment = TextAnchor.UpperRight;
		
		{
			Rect rect = new Rect(0, 30, 200, 50);
			GUIExtended.Label(rect, "High Score", style, Color.black, new Color(1.0f, 0.93f, 0.73f, 1.0f));
			
			for (int i = 0; i < highScoreList.Length; i++) {
				string scoreText = highScoreList[i] == 0 ? "-" : highScoreList[i].ToString("n0");
				GUIExtended.Label(new Rect(rect.x, rect.y + 20 + i * 20, 200, 50), scoreText, scoreStyle, Color.black, Color.white);
			}
		}

		{
			Rect rect = new Rect(0, 120, 200, 50);
			GUIExtended.Label(rect, "Total Score", style, Color.black, new Color(1.0f, 0.93f, 0.73f, 1.0f));
	
			for (int i = 0; i < totalScoreList.Length; i++) {
				string scoreText = totalScoreList[i] == 0 ? "-" : totalScoreList[i].ToString("n0");
				GUIExtended.Label(new Rect(rect.x, rect.y + 20 + i * 20, 200, 50), scoreText, scoreStyle, Color.black, Color.white);
			}
		}

		{
			Rect rect = new Rect(0, 200, 200, 50);
			GUIExtended.Label(rect, "Current Score", style, Color.black, new Color(1.0f, 0.93f, 0.73f, 1.0f));
			
			for (int i = 0; i < currentScoreList.Length; i++) {
				string scoreText = currentScoreList[i] == 0 ? "-" : currentScoreList[i].ToString("n0");
				GUIExtended.Label(new Rect(rect.x, rect.y + 20 + i * 20, 200, 50), scoreText, scoreStyle, Color.black, Color.white);
			}
		}
	}
}
