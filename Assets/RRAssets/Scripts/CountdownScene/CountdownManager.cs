using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace RR {
	public class CountdownManager : MonoBehaviour {

		public Canvas canvas;
		private Image chart;

		void Start()
		{
			Debug.Log("Started");
			drawChart();
		}

		// Set chart.sprite to the given chart sprite wanted
		void drawChart()
		{
			Image chart = canvas.GetComponentInChildren<Image>();
			 
			chart.preserveAspect = true;
			chart.sprite = Resources.Load<Sprite>("Prefabs/UI/feedingChart");
		}

		private void endScene()
		{
			Application.LoadLevel("RRRunnerScene");
		}
	}
}