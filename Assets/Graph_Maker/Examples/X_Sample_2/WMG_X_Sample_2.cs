using UnityEngine;
using System.Collections;

public class WMG_X_Sample_2 : MonoBehaviour {

	public Object pieGraphPrefab;
	WMG_Pie_Graph graph;

	// Use this for initialization
	void Start () {
		GameObject graphGO = GameObject.Instantiate(pieGraphPrefab) as GameObject;
		graphGO.transform.SetParent(this.transform, false);
		graph = graphGO.GetComponent<WMG_Pie_Graph>();

		graph.Init(); // Call Init() to ensure things are initialized

		// lets create an interactive doughnut chart
		graph.interactivityEnabled = true;
		graph.useDoughnut = true;
		graph.doughnutPercentage = 0.5f;
		graph.explodeLength = 0;
		graph.WMG_Pie_Slice_Click += myPieSliceClickFunction;
		graph.WMG_Pie_Slice_MouseEnter += myPieSliceHoverFunction;
	}

	void myPieSliceClickFunction(WMG_Pie_Graph pieGraph, WMG_Pie_Graph_Slice aSlice) {
		//Debug.Log("Pie Slice Clicked: " + pieGraph.sliceLabels[aSlice.sliceIndex]);
	}
	
	void myPieSliceHoverFunction(WMG_Pie_Graph pieGraph, WMG_Pie_Graph_Slice aSlice, bool hover) {
		//Debug.Log("Pie Slice Hover: " + pieGraph.sliceLabels[aSlice.sliceIndex]);
		if (hover) {
			Vector3 newPos = graph.getPositionFromExplode(aSlice, 30);
			WMG_Anim.animPosition(aSlice.gameObject, 1, DG.Tweening.Ease.OutQuad, newPos);
		}
		else {
			Vector3 newPos = graph.getPositionFromExplode(aSlice, 0);
			WMG_Anim.animPosition(aSlice.gameObject, 1, DG.Tweening.Ease.OutQuad, newPos);
		}
	}
}
