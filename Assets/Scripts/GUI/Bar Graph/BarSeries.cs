using UnityEngine;

using System.Collections.Generic;

public class BarSeries {

	// Variables
	public int id { get; set; }
	public string label { get; set; }
	public DynamicRect rect;
	public float width { get { return rect.width; } set { rect.width = value; } }
	public float height { get { return rect.height; } }
	public Color color { get; set; }
	public string colorHex { get; set; }
	//public List<float> values { get; set; }
	public int score { get; set; }
	// Other
	public float deltaTime { get; set; }
	// Legend
	public float labelX { get; set; }
	public float labelDT { get; set; }

	public BarSeries(string label, int score, DynamicRect rect, Color color) {  //param 2: List<float> values
		this.label = label;
		//this.values = values;
		this.score = score;
		this.rect = rect;
		this.color = color;

		this.colorHex = Functions.ColorToHex(color);
	}

	public DynamicRect GetRect() {
		return rect;
	}

//	public float GetSum() {
//		float sum = 0;

//		foreach (float value in values) {
//			sum += value;
//		}

//		return sum;
//	}
}
