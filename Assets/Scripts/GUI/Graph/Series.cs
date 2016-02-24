using UnityEngine;

using System.Collections.Generic;

public class Series {

	// Variables
	public string label { get; set; }
	private Rect rect;
	public float width { get { return rect.width; } set { rect.width = value; } }
	public float height { get { return rect.height; } }
	public Color color { get; set; }
	public string colorHex { get; set; }
	public List<float> values { get; set; }
	// Other
	public float deltaTime { get; set; }
	// Legend
	public float labelX { get; set; }
	public float labelDT { get; set; }

	public Series(string label, Rect rect, Color color) {
		this.label = label;
		this.rect = rect;
		this.color = color;
		this.values = new List<float>();

		this.colorHex = Functions.ColorToHex(color);
	}

	public Rect GetRect() {
		return rect;
	}
}
