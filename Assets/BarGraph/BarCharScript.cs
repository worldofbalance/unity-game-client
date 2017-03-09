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
	private int[] cPlayerScores = new int[4];
	private string[] labels = new string[4];
	private string[] cLabels = new string[4];
	private Color[] colors = new Color[]{Color.red,  Color.green, Color.cyan, Color.white};
	public delegate void Callback (String[] topPlayerNames, int[] topPlayerscores);
	public Text header, playerName, playerScore, playerCredits, playerSpecies, playerDays, playerRank;
	public Button Button_Toggle;
	private int valsCount;
	private int speciesCount;
	private bool showHighest;
	private string name;
	private int cDay, fDay, lDay, dayCount;


	void Start () {
		speciesCount = 0;
		cDay = fDay = lDay = 0;
		dayCount = 0;
		ReloadData ();
		chartHeight = Screen.height + GetComponent<RectTransform> ().sizeDelta.y - 320;
		Button_Toggle.GetComponentInChildren<Text> ().fontSize = 16;
		Button_Toggle.GetComponentInChildren<Text> ().alignment = TextAnchor.UpperCenter;
	}

	public void requestTopPlayers(/*Callback callback*/)
	{
		//passedInFunc = callback;
		Game.networkManager.Send(TopListProtocol.Prepare(), ProcessTopList);
	}

	public void requestSpeciesCount()
	{
		Game.networkManager.Send(SpeciesActionProtocol.Prepare((short) 5), processSpeciesCount);
		Debug.Log ("BarCharScript: SpeciesActionProtocol 5 sent");
	}
		
	public void processSpeciesCount(NetworkResponse response)
	{
		ResponseSpeciesAction args = response as ResponseSpeciesAction;
		speciesCount = args.count;
		Debug.Log ("BarCharScript: speciesCount = " + speciesCount);
		playerSpecies.text = "Number Species: " + speciesCount;
	}
		
	public void requestDayInfo()
	{
		Game.networkManager.Send(SpeciesActionProtocol.Prepare((short) 6), processDayInfo);
		Debug.Log ("BarCharScript: SpeciesActionProtocol 6 sent");
	}

	public void processDayInfo(NetworkResponse response)
	{
		ResponseSpeciesAction args = response as ResponseSpeciesAction;
		cDay = args.cDay;
		fDay = args.fDay;
		lDay = args.lDay;
		Debug.Log ("BarCharScript: c,f,lDay = " + cDay + " " + fDay + " " + lDay);
		dayCount = Math.Max (cDay, lDay) - fDay + 1; 
		playerDays.text = "Days playing: " + dayCount;
	}
		

	public void ToggleScores() {
		Debug.Log ("Toggle scores pressed: inside BarCharScript");
		showHighest = !showHighest;
		if (showHighest) {
			Button_Toggle.GetComponentInChildren<Text>().text = "Show Current Scores";
			header.text = "Top Ranking Players - Highest Scores";
			DisplayGraph (playerScores, labels);
		} else {
			Button_Toggle.GetComponentInChildren<Text>().text = "Show Highest Scores";
			header.text = "Top Ranking Players - Current Scores";
			DisplayGraph (cPlayerScores, cLabels);
		}
	}

	public void ReloadData() {		
		showHighest = true;
		requestTopPlayers ();
		requestSpeciesCount ();
		requestDayInfo ();
	}
		
	private void ProcessTopList(NetworkResponse response)
	{		
		ResponseTopList args = response as ResponseTopList;
	
		labels[0] = args.name1;
		labels[1] = args.name2;
		labels[2] = args.name3;
		playerScores[0] = args.score1;
		playerScores[1] = args.score2;
		playerScores[2] = args.score3;
		playerScores[3] = GameObject.Find ("Global Object").GetComponent<EcosystemScore> ().highScore;

		cLabels[0] = args.cName1;
		cLabels[1] = args.cName2;
		cLabels[2] = args.cName3;
		cPlayerScores[0] = args.cScore1;
		cPlayerScores[1] = args.cScore2;
		cPlayerScores[2] = args.cScore3;
		cPlayerScores[3] = GameObject.Find ("Global Object").GetComponent<EcosystemScore> ().score;

		if (GameState.player != null) {
			cLabels [3] = GameState.player.name;
			labels[3] = cLabels[3];
		} else {
			cLabels [3] = "you";
			labels[3] = cLabels[3];
		}

		//These are the text fields to the right of the chart
		playerName.text = "Welcome " + labels [3]+"!";
		playerScore.text = "Current Score: " + cPlayerScores[3];
		playerCredits.text = "Credit Balance: " + GameState.player.credits;
		playerSpecies.text = "Number Species: " + speciesCount;
		playerDays.text = "Days playing: " + dayCount;
		// Debug.Log ("BarCharScript: playerScores [3] = " + playerScores [3]);
		// Debug.Log ("BarCharScript: GameState.player.credits = " + GameState.player.credits);
		Button_Toggle.GetComponentInChildren<Text>().text = "Show Current Scores";
		header.text = "Top Ranking Players - Highest Scores";
		DisplayGraph (playerScores, labels);
	}


	void DisplayGraph(int[] vals, string[]labels){
		int maxValue = vals.Max();
		valsCount = 4;
		playerRank.text = "";
		for (int i = 0; i < 3; i++) {
			if (labels[3].Equals(labels[i], StringComparison.Ordinal)) {
				playerRank.text = "You are player rank #" + (i + 1) + "!";
			}
		}

		for (int i = 0; i < valsCount; i++) {
			Bar newBar;
			if (bars.Count > i) {
				newBar = bars [i];
			} else {
				newBar = Instantiate(barPrefab) as Bar;
				bars.Add (newBar);
			}
			newBar.transform.SetParent(transform);

			//size bar
			RectTransform rt = newBar.bar.GetComponent<RectTransform> ();
			//normalize score based on highest score
			float normalizedValue = (float)vals [i] / (float)maxValue * 1.00f;
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
			newBar.barValue.text = vals [i].ToString ();
			//if height is too small, move value label to top of bar
			if (rt.sizeDelta.y < 30f) {
				newBar.barValue.color = Color.white;
				newBar.barValue.GetComponent<RectTransform> ().pivot = new Vector2 (0.5f, 0f);
				newBar.barValue.GetComponent<RectTransform> ().anchoredPosition = Vector2.zero;
			}
		}
	}
}
