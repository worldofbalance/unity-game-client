using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
using System;


public class BarCharScript : MonoBehaviour {
	
	public Bar barPrefab;
	List<Bar> bars = new List<Bar>();
	float chartHeight;
	private int[] playerScores = new int[4];
	private string[] labels = new string[4];
	private Color[] colors = new Color[]{Color.red,  Color.green, Color.cyan, Color.white};
	public delegate void Callback (String[] topPlayerNames, int[] topPlayerscores);
	public Text player1Name, player2Name, player3Name, player4Name;


	void Start () {
		
		requestTopPlayers ();
		chartHeight = Screen.height + GetComponent<RectTransform>().sizeDelta.y;



	}

	public void requestTopPlayers(/*Callback callback*/)
	{
		//passedInFunc = callback;

		Game.networkManager.Send(TopListProtocol.Prepare(), ProcessTopList);
	}

	private void ProcessTopList(NetworkResponse response)
	{
		
		ResponseTopList args = response as ResponseTopList;
	
		labels[0] = args.name1;
		labels[1] = args.name2;
		labels[2] = args.name3;
		labels [3] = "You";
		playerScores[0] = args.score1;
		playerScores[1] = args.score2;
		playerScores[2] = args.score3;
		playerScores [3] = 1800;
		player1Name.text = "1) " + labels [0] + ": " + playerScores [0];
		player2Name.text = "2) " + labels [1] + ": " + playerScores [1];
		player3Name.text = "3) " + labels [2] + ": " + playerScores [2];
		player4Name.text = "4) " + labels [3] + ": " + playerScores [3];
		DisplayGraph (playerScores, labels);
	}

	void DisplayGraph(int[] vals, string[]labels){
		//find largest value

		int maxValue = vals.Max();

		for (int i = 0; i < vals.Length; i++){
			Bar newBar = Instantiate(barPrefab) as Bar;
			newBar.transform.SetParent(transform);

			//size bar
			RectTransform rt = newBar.bar.GetComponent<RectTransform> ();
			//normalize score based on highest score
			float normalizedValue = (float)vals [i] / (float)maxValue * 0.85f;
			rt.sizeDelta = new Vector2 (rt.sizeDelta.x, chartHeight * normalizedValue);
			newBar.bar.color = colors [i % colors.Length];
			//newBar.bar.color = Color.cyan;

			//Bar label
			if (labels.Length <= i) {
				newBar.label.text = "UNDEFINED";
			} else {
				newBar.label.text = labels [i];
			}
			//Bar value 
			newBar.barValue.text = playerScores [i].ToString ();
			//if height is too small, move value label to top of bar
			if (rt.sizeDelta.y < 30f) {
				newBar.barValue.GetComponent<RectTransform> ().pivot = new Vector2 (0.5f, 0f);
				newBar.barValue.GetComponent<RectTransform> ().anchoredPosition = Vector2.zero;
			}
		}
	}
}
