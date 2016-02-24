using UnityEngine;

using System.Collections.Generic;

public class BarLegend
{

	private BarGraph graph;
	private Rect rect;
	private Rect legendScrollRect;
	private Vector2 legendScrollPos = Vector2.zero;
	private bool isLegendContentHidden = false;
	private Texture2D markerTex;
	private Texture2D markerHoverTex;

	public BarLegend (Rect rect, BarGraph graph)
	{
		this.rect = rect;
		this.graph = graph;

		legendScrollRect = new Rect (rect.width * 0.05f, 40, rect.width * 0.9f, rect.height * 0.75f);

		markerTex = Functions.CreateTexture2D (new Color (0.85f, 0.85f, 0.85f));
		markerHoverTex = Functions.CreateTexture2D (Color.white);
	}

	public void Draw ()
	{
		GUIStyle style = new GUIStyle (GUI.skin.label);
		style.richText = true;
		
		Color temp = GUI.color;
		
		GUI.BeginGroup (rect, GUI.skin.box);
		style.alignment = TextAnchor.UpperCenter;
		GUI.color = Color.yellow;
		GUI.Label (new Rect ((rect.width - 200) / 2, 10, 200, 30), Strings.LEGEND, style);
		GUI.color = temp;
			
		legendScrollPos = GUI.BeginScrollView (legendScrollRect, legendScrollPos, new Rect (0, 0, legendScrollRect.width - 20, graph.seriesSets.Count * 30));
		style.alignment = TextAnchor.UpperLeft;

		string selected = graph.GetSelectedSeries ();

		List<string> sortedLabels = graph.convergeManager.seriesLabels;
		//List<string> tempList = new List<string> (graph.colorList.Keys);
		//for (int i = 0; i < sortedLabels.Count; i++) {
			//Color color = graph.colorList [tempList [i]];
			//string label = tempList [i];
		int offset = 0;
		foreach (string label in sortedLabels) {
			Color color = graph.convergeManager.seriesColors [label];
					
			Rect itemRect = new Rect (10, offset * 30, legendScrollRect.width * 0.8f, 30);
					
			if (itemRect.Contains (Event.current.mousePosition)) {
				graph.SelectSeries (label);
			}
			// Marquee Effect
			if (label == selected) {
				float textWidth = GUI.skin.label.CalcSize (new GUIContent (label)).x;
				float maxViewWidth = itemRect.width - 50;
						
				if (textWidth > maxViewWidth) {
//							seriesSet.labelX = Mathf.Lerp(0, maxViewWidth - textWidth, seriesSet.labelDT += Time.deltaTime * 0.75f);
				}
			} else {
//						seriesSet.labelX = 0;
//						seriesSet.labelDT = 0;
			}
					
			GUI.BeginGroup (itemRect);
//						GUI.color = seriesSet.isActive ? seriesSet.color : Color.gray;
			GUI.color = color;
			GUI.DrawTexture (new Rect (5, 10, 30, 15), (label == selected) ? markerHoverTex : markerTex);
			GUI.color = temp;
						
//						GUI.color = seriesSet.isActive ? temp : Color.gray;
			GUI.color = temp;
//						style.normal.textColor = (label == selected) ? seriesSet.color : Color.white;
						
			GUI.BeginGroup (new Rect (50, 7, 200, 30));
//							GUI.Label(new Rect(seriesSet.labelX, 0, 200, 30), label, style);
			GUI.Label (new Rect (0, 0, 200, 30), label, style);
			GUI.EndGroup ();
						
			GUI.color = temp;
						
			if (GUI.Button (new Rect (0, 0, itemRect.width, itemRect.height), "", GUIStyle.none)) {
//							graph.SetSeriesActive(1, excludeList.Contains(label));
			}
			GUI.EndGroup ();
			offset++;
		}
		GUI.EndScrollView ();
			
//			if (GUI.Button(new Rect((rect.width - 85) / 2, rect.height - 33, 85, 25), isLegendContentHidden ? Strings.SHOW_ITEMS : Strings.HIDE_ITEMS)) {
//				isLegendContentHidden = !isLegendContentHidden;
//
//				foreach (SeriesSet seriesSet in graph.seriesSets) {
//					seriesSet.SetActive(isLegendContentHidden);
//				}
//			}
		GUI.EndGroup ();
	}
}
