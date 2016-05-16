using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;


public class BarCharScript : MonoBehaviour {
	
	public Bar barPrefab;
	List<Bar> bars = new List<Bar>();
	float chartHeight;
	public int[] playerScores;
	public string[] labels;
	private Color[] colors = new Color[]{Color.red,  Color.green, Color.cyan, Color.white};

	void Start () {
		chartHeight = Screen.height + GetComponent<RectTransform>().sizeDelta.y;
		DisplayGraph (playerScores);

	}
	
	void DisplayGraph(int[] vals){
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
