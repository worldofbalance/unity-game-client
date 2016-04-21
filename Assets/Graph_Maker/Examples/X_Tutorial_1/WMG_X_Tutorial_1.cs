using UnityEngine;
using System.Collections.Generic;

public class WMG_X_Tutorial_1 : MonoBehaviour {

	public GameObject emptyGraphPrefab;

	public WMG_Axis_Graph graph;

	public WMG_Series series1;

	public List<Vector2> series1Data;
	public bool useData2;
	public List<string> series1Data2;

	// Use this for initialization
	void Start () {
		GameObject graphGO = GameObject.Instantiate(emptyGraphPrefab);
		graphGO.transform.SetParent(this.transform, false);
		graph = graphGO.GetComponent<WMG_Axis_Graph>();

		series1 = graph.addSeries();
		graph.xAxis.AxisMaxValue = 5;

		if (useData2) {
			List<string> groups = new List<string>();
			List<Vector2> data = new List<Vector2>();
			for (int i = 0; i < series1Data2.Count; i++) {
				string[] row = series1Data2[i].Split(',');
				groups.Add(row[0]);
				if (!string.IsNullOrEmpty(row[1])) {
					float y = float.Parse(row[1]);
					data.Add(new Vector2(i+1, y));
				}
			}

			graph.groups.SetList(groups);
			graph.useGroups = true;

			graph.xAxis.LabelType = WMG_Axis.labelTypes.groups;
			graph.xAxis.AxisNumTicks = groups.Count;

			series1.seriesName = "Fruit Data";

			series1.UseXDistBetweenToSpace = true;

			series1.pointValues.SetList(data);
		}
		else {
			series1.pointValues.SetList(series1Data);
		}
	}



}
