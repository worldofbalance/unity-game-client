using UnityEngine;
using System.Collections.Generic;

public class EcoGraphScript : MonoBehaviour {
	public GameObject ecoGraph;


	public List<Vector2> player1Data;

	public List<Vector2> player2Data;

	public List<Vector2> player3Data;

	public List<Vector2> youData;
	public MenuScript statusPanel;
	private bool isAlive = false;
	private GameObject graphGO;

	void Start () {

	}

	void update(){

	}

	public void createGraph(){
		/*
		graphGO = GameObject.Instantiate (ecoGraph);
		graphGO.transform.SetParent (this.transform, false);
		graph = graphGO.GetComponent<WMG_Axis_Graph> ();

		player3 = graph.addSeries ();
		player3.pointValues.SetList (player3Data);
        player3.lineColor= Color.green;
		player3.lineScale = 2;

		player2 = graph.addSeries ();
		player2.pointValues.SetList (player2Data);
		player2.lineColor = Color.cyan;
		player2.lineScale = 2;

		player1 = graph.addSeries ();
		player1.pointValues.SetList (player1Data);
		player1.lineColor = Color.red;
		player1.lineScale = 2;

		you = graph.addSeries ();
		you.pointValues.SetList (youData);
		you.lineColor = Color.white;
		you.lineScale = 2;*/
	}

	public void destroyGraph(){
		GameObject.Destroy (graphGO);
	}
}
