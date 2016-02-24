using UnityEngine;

using System.Collections.Generic;

public class SeriesSet {

	public string label { get; set; }
	public DynamicRect rect { get; set; }
	public List<BarSeries> seriesList { get; set; }
	public bool isActive { get; set; }
	public Color color { get; set; }
	public string colorHex { get; set; }
	// Legend
	public float labelX { get; set; }
	public float labelDT { get; set; }

	public SeriesSet(string label, DynamicRect rect, bool isActive = true) {
		this.label = label;
		this.rect = rect;
		this.isActive = isActive;
		this.color = new Color(Random.Range(0.4f, 0.7f), Random.Range(0.4f, 0.7f), Random.Range(0.4f, 0.7f));
		
		this.colorHex = Functions.ColorToHex(color);

		seriesList = new List<BarSeries>();
	}

	public void Add(BarSeries series) {
		seriesList.Add(series);
	}

	public void SetActive(bool active) {
		isActive = active;

		if (active) {
			rect.width = 0;
			rect.deltaTime = 0;
		}
	}

	public void Sort(List<string> sortedLabels)
	{
		List<BarSeries> sortedList = new List<BarSeries> ();
		//use insert to reverse order
		foreach (string name in sortedLabels) {
			sortedList.Insert (0, seriesList.Find (series => series.label == name));
		}
		seriesList = sortedList;
	}

}
