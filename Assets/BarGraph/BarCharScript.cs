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
	private int[] playerScores;
	private string[] labels;
	private Color[] colors = new Color[]{Color.red,  Color.green, Color.cyan, Color.white};
	public delegate void Callback (String[] topPlayerNames, int[] topPlayerscores);

	void Start () {
		requestTopPlayers ();
		chartHeight = Screen.height + GetComponent<RectTransform>().sizeDelta.y;
		//DisplayGraph (playerScores, labels);

	}

	public void requestTopPlayers(/*Callback callback*/)
	{
		//passedInFunc = callback;

		Game.networkManager.Send(
			TopListProtocol.Prepare(),
			ProcessTopList
		);
	}

	private void ProcessTopList(NetworkResponse response)
	{
		Debug.Log ("PROCESSING TOP LIST");
		ResponseTopList args = response as ResponseTopList;
		labels[0] = args.name1;
		labels[1] = args.name2;
		labels[2] = args.name3;
		playerScores[0] = args.score1;
		playerScores[1] = args.score2;
		playerScores[2] = args.score3;

		Debug.Log("Top Guy: "+labels[0]+ "  Top Score: "+playerScores[0]);

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
