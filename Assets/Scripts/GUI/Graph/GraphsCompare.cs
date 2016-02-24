using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using System.Globalization;

//version of "Graph" that gets csv data from static 2 static sources for comparison, 
//rather than single source from GameState
//for use with ConvergeGame
public class GraphsCompare
{

	// Window Properties
	private float left;
	private float top;
	private float height;
	// Other
	private Rect legendRect;
	private Rect legendScrollRect;
	private Vector2 legendScrollPos = Vector2.zero;

	public GraphUnit graph1 { get; set; }

	private GraphUnit graph2;
	private float widthGraph;
	private float widthLegend = 200;
	private float bufferGraph = 10;
	private float bufferBorder = 10;
	private float heightSeries = 20;
	private ConvergeManager manager;
	private Database foodWeb;
	private bool isActiveLegend = false;  //disabled permanently

	public GraphsCompare (
		CSVObject csv1, 
		CSVObject csv2, 
		float left, 
		float top,
		float pageWidth,
		float height,
		string title1,
		string title2,
		int xNumMarkers,
		Database foodWeb,
		ConvergeManager manager
	)
	{
		this.left = left;
		this.top = top;
		this.height = height;
		if (isActiveLegend) {
			widthGraph = (pageWidth - widthLegend - (bufferGraph * 2)) / 2;
		} else {
			widthGraph = (pageWidth - (bufferGraph * 2)) / 2;;
		}
		this.foodWeb = foodWeb;

		//object for handling common graph control info
		this.manager = manager;

		graph1 = new GraphUnit (
			csv1,
			left, 
			top, 
			widthGraph, 
			height, 
			title1,
			xNumMarkers,
			manager,
			true
		);
		graph2 = new GraphUnit (
			csv2,
			left + widthGraph + bufferGraph, 
			top, 
			widthGraph, 
			height, 
			title2,
			xNumMarkers,
			manager,
			false
		);

		NormalizeYScale ();

	}

	//prev "MakeWindow()", legend separated
	public void DrawGraphs ()
	{
		graph1.DrawGraph ();
		graph2.DrawGraph ();

		if (isActiveLegend) {
			DrawLegend ();
		}
	}

	public void DrawLegend ()
	{
		legendRect = new Rect (left + (2 * widthGraph) + (2 * bufferGraph), top, widthLegend, height);
		legendScrollRect = new Rect (
			legendRect.width * 0.05f, 
			bufferBorder, 
			legendRect.width - bufferBorder * 2, 
			legendRect.height - bufferBorder * 2
		);

		GUIStyle style = new GUIStyle (GUI.skin.label);
		style.richText = true;

		Texture2D markerTex = Functions.CreateTexture2D (new Color (0.85f, 0.85f, 0.85f));
		Texture2D markerHoverTex = Functions.CreateTexture2D (Color.white);

		Color temp = GUI.color;

		GUI.BeginGroup (legendRect, GUI.skin.box);
		style.alignment = TextAnchor.UpperCenter;
		//GUI.color = Color.yellow;
		//GUI.Label (new Rect ((legendRect.width - 200) / 2, 10, 200, 30), "Legend", style);
		//GUI.color = temp;

		Rect scrollRect = new Rect (0, 0, legendScrollRect.width - 20, manager.seriesLabels.Count * 30);
		legendScrollPos = GUI.BeginScrollView (legendScrollRect, legendScrollPos, scrollRect);
		style.alignment = TextAnchor.UpperLeft;
		for (int i = 0; i < manager.seriesLabels.Count; i++) {
			Series series = graph1.seriesList [manager.seriesLabels [i]];
			string label = series.label;

			Rect itemRect = new Rect (10, i * heightSeries, legendScrollRect.width * 0.8f, heightSeries);

			if (itemRect.Contains (Event.current.mousePosition)) {
				manager.selected = label;
				manager.lastSeriesToDraw = label;
			}
			// Marquee Effect
			if (label == manager.selected) {
				float textWidth = GUI.skin.label.CalcSize (new GUIContent (label)).x;
				float maxViewWidth = itemRect.width - 50;

				if (textWidth > maxViewWidth) {
					series.labelX = Mathf.Lerp (0, maxViewWidth - textWidth, series.labelDT += Time.deltaTime * 0.75f);
				}
			} else {
				series.labelX = 0;
				series.labelDT = 0;
			}

			GUI.BeginGroup (itemRect);
			GUI.color = manager.excludeList.Contains (label) ? Color.gray : manager.seriesColors [series.label];
			GUI.DrawTexture (new Rect (5, 10, 30, 15), (label == manager.selected) ? markerHoverTex : markerTex);
			GUI.color = temp;
			
			GUI.color = manager.excludeList.Contains (label) ? Color.gray : temp;
			style.normal.textColor = (label == manager.selected) ? manager.seriesColors [series.label] : Color.white;

			{
				GUI.BeginGroup (new Rect (50, 5, 200, heightSeries));
				GUI.Label (new Rect (series.labelX, 0, 200, heightSeries), label, style);
				GUI.EndGroup ();
			}

			GUI.color = temp;

			//if player clicks on species, set as selected and activate foodWeb
			if (GUI.Button(new Rect(0, 0, itemRect.width, itemRect.height), "", GUIStyle.none)) {
				foodWeb.selected = SpeciesTable.GetSpeciesName (label);
				foodWeb.SetActive (true, foodWeb.selected);
			}

			GUI.EndGroup ();
		}
		GUI.EndScrollView ();

		GUI.EndGroup ();
	}

	public void UpdateGraph1Data (CSVObject graph1CSV, string graph1Title)
	{
		graph1.csv = graph1CSV;
		graph1.UpdateData ();
		graph1.title = graph1Title;
		NormalizeYScale ();
	}

	private void NormalizeYScale ()
	{
		//force graphs to have consistent scale for y axis
		if (graph1.yRange > graph2.yRangeActual) {
			graph2.yRange = graph1.yRange;
		} else {
			graph1.yRange = graph2.yRangeActual;
		}
	}
	
}
