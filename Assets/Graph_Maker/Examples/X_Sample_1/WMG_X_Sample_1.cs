using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class WMG_X_Sample_1 : MonoBehaviour {

	public Object emptyGraphPrefab;
	public bool plotOnStart;
	public bool plottingData { get {return _plottingData;} 
		set {
			if (_plottingData != value) {
				_plottingData = value;
				plottingDataC.Changed();
			}
		}
	}
	[SerializeField] private bool _plottingData;

	public float plotIntervalSeconds;
	public float plotAnimationSeconds;
	Ease plotEaseType = Ease.OutQuad;
	public float xInterval;
	public bool useAreaShading;
	public bool blinkCurrentPoint;
	public float blinkAnimDuration;
	float blinkScale = 2;
	public bool moveXaxisMinimum;
	public Object indicatorPrefab;
	public int indicatorNumDecimals;

	private List<WMG_Change_Obj> changeObjs = new List<WMG_Change_Obj>();
	private WMG_Change_Obj plottingDataC = new WMG_Change_Obj();

	WMG_Axis_Graph graph;
	WMG_Series series1;
	GameObject graphOverlay;
	GameObject indicatorGO;

	System.Globalization.NumberFormatInfo tooltipNumberFormatInfo = new System.Globalization.CultureInfo( "en-US", false ).NumberFormat;
	System.Globalization.NumberFormatInfo yAxisNumberFormatInfo = new System.Globalization.CultureInfo( "en-US", false ).NumberFormat;
	System.Globalization.NumberFormatInfo seriesDataLabelsNumberFormatInfo = new System.Globalization.CultureInfo( "en-US", false ).NumberFormat;
	System.Globalization.NumberFormatInfo indicatorLabelNumberFormatInfo = new System.Globalization.CultureInfo( "en-US", false ).NumberFormat;

	float addPointAnimTimeline;
	Tween blinkingTween;

	// Use this for initialization
	void Start () {
		changeObjs.Add(plottingDataC);

		GameObject graphGO = GameObject.Instantiate(emptyGraphPrefab) as GameObject;
		graphGO.transform.SetParent(this.transform, false);
		graph = graphGO.GetComponent<WMG_Axis_Graph>();
		graph.legend.hideLegend = true;
		graph.stretchToParent(graphGO);
		graphOverlay = new GameObject();
		graphOverlay.AddComponent<RectTransform>();
		graphOverlay.name = "Graph Overlay";
		graphOverlay.transform.SetParent(graphGO.transform, false);
		indicatorGO = GameObject.Instantiate(indicatorPrefab) as GameObject;
		indicatorGO.transform.SetParent(graphOverlay.transform, false);
		indicatorGO.SetActive(false);
		graph.GraphBackgroundChanged += UpdateIndicatorSize;
		graph.paddingLeftRight = new Vector2(65, 60);
		graph.paddingTopBottom = new Vector2(40, 40);
		graph.xAxis.LabelType = WMG_Axis.labelTypes.ticks;
		graph.xAxis.SetLabelsUsingMaxMin = true;
		graph.autoAnimationsEnabled = false;
		graph.xAxis.hideLabels = true;
		graph.xAxis.hideTicks = true;
		graph.xAxis.hideGrid = true;
		graph.yAxis.AxisNumTicks = 5;
		graph.yAxis.hideTicks = true;
		graph.axisWidth = 1;
		graph.yAxis.MaxAutoGrow = true; // auto increase yAxis max if a point value exceeds max
		graph.yAxis.MinAutoGrow = true; // auto decrease yAxis min if a point value exceeds min
		series1 = graph.addSeries();
		series1.pointColor = Color.red;
		series1.lineColor = Color.green;
		series1.lineScale = 0.5f;
		series1.pointWidthHeight = 8;
		graph.changeSpriteColor(graph.graphBackground, Color.black);
		if (useAreaShading) {
			series1.areaShadingType = WMG_Series.areaShadingTypes.Gradient;
			series1.areaShadingAxisValue = graph.yAxis.AxisMinValue;
			series1.areaShadingColor = new Color(80f/255f, 100f/255f, 60f/255f, 1f);
		}
		graph.tooltipDisplaySeriesName = false;

		// define our own custom functions for labeling
		graph.theTooltip.tooltipLabeler = customTooltipLabeler; // override the default labeler for the tooltip
		graph.yAxis.axisLabelLabeler = customYAxisLabelLabeler; // override the default labeler for the yAxis
		series1.seriesDataLabeler = customSeriesDataLabeler; // override the default labeler for data labels (appear over points when data labels on the series are enabled)

		plottingDataC.OnChange += PlottingDataChanged;
		if (plotOnStart) {
			plottingData = true;
		}
	}

	void PlottingDataChanged() {
		//Debug.Log("plottingData: " + plottingData);
		if (plottingData) {
			StartCoroutine(plotData ());
		}
	}
	
	public IEnumerator plotData() {
		while(true) {
			yield return new WaitForSeconds(plotIntervalSeconds);
			if (!plottingData) break;
			animateAddPointFromEnd(new Vector2((series1.pointValues.Count == 0 ? 0 : (series1.pointValues[series1.pointValues.Count-1].x + xInterval)), Random.Range(graph.yAxis.AxisMinValue, graph.yAxis.AxisMaxValue*1.2f)), plotAnimationSeconds);
			if (blinkCurrentPoint) {
				blinkCurrentPointAnimation();
			}
		}
	}

	void animateAddPointFromEnd(Vector2 pointVec, float animDuration) {
		if (series1.pointValues.Count == 0) { // no end to animate from, just add the point
			series1.pointValues.Add(pointVec);
			indicatorGO.SetActive(true);
			graph.Refresh(); // Ensures gamobject list of series points is up to date based on pointValues
			updateIndicator();
		}
		else {
			series1.pointValues.Add (series1.pointValues[series1.pointValues.Count-1]);
			if (pointVec.x > graph.xAxis.AxisMaxValue) { // the new point will exceed the x-axis max
				addPointAnimTimeline = 0; // animates from 0 to 1
				Vector2 oldEnd = new Vector2(series1.pointValues[series1.pointValues.Count-1].x, series1.pointValues[series1.pointValues.Count-1].y);
				Vector2 newStart = new Vector2(series1.pointValues[1].x, series1.pointValues[1].y);
				Vector2 oldStart = new Vector2(series1.pointValues[0].x, series1.pointValues[0].y);
				WMG_Anim.animFloatCallbacks(() => addPointAnimTimeline, x=> addPointAnimTimeline = x, animDuration, 1, 
				                            () => onUpdateAnimateAddPoint(pointVec, oldEnd, newStart, oldStart), 
				                            () => onCompleteAnimateAddPoint(), plotEaseType);
			}
			else {
				WMG_Anim.animVec2CallbackU(()=> series1.pointValues[series1.pointValues.Count-1], x=> series1.pointValues[series1.pointValues.Count-1] = x, animDuration, pointVec,
				                           () => updateIndicator(), plotEaseType);
			}
		}
	}

	void blinkCurrentPointAnimation(bool fromOnCompleteAnimateAdd = false) {
		graph.Refresh(); // Ensures gamobject list of series points is up to date based on pointValues
		WMG_Node lastPoint = series1.getLastPoint().GetComponent<WMG_Node>();
		string blinkingPointAnimId = series1.GetHashCode() + "blinkingPointAnim";
		DOTween.Kill(blinkingPointAnimId);
		blinkingTween = lastPoint.objectToScale.transform.DOScale(new Vector3(blinkScale,blinkScale,blinkScale), blinkAnimDuration).SetEase(plotEaseType)
			.SetUpdate(false).SetId(blinkingPointAnimId).SetLoops(-1, LoopType.Yoyo);
		if (series1.pointValues.Count > 1) { // ensure previous point scale reset
			WMG_Node blinkingNode = series1.getPoints()[series1.getPoints().Count-2].GetComponent<WMG_Node>();
			if (fromOnCompleteAnimateAdd) { // removing a pointValues index deletes the gameobject at the end, so need to set the timeline from the previous tween
				blinkingTween.Goto(blinkAnimDuration * blinkingNode.objectToScale.transform.localScale.x/blinkScale, true);
			}
			blinkingNode.objectToScale.transform.localScale = Vector3.one;
		}
	}

	void updateIndicator() {
		if (series1.getPoints().Count == 0) return;
		WMG_Node lastPoint = series1.getLastPoint().GetComponent<WMG_Node>();
		graph.changeSpritePositionToY(indicatorGO, lastPoint.transform.localPosition.y);
		Vector2 nodeData = series1.getNodeValue(lastPoint);
		indicatorLabelNumberFormatInfo.CurrencyDecimalDigits = indicatorNumDecimals;
		string textToSet = nodeData.y.ToString("C", indicatorLabelNumberFormatInfo);
		graph.changeLabelText(indicatorGO.transform.GetChild(0).GetChild(0).gameObject, textToSet);
	}

	void onUpdateAnimateAddPoint(Vector2 newEnd, Vector2 oldEnd, Vector2 newStart, Vector2 oldStart) {
		series1.pointValues[series1.pointValues.Count-1] = WMG_Util.RemapVec2(addPointAnimTimeline, 0, 1, oldEnd, newEnd);
		graph.xAxis.AxisMaxValue = WMG_Util.RemapFloat(addPointAnimTimeline, 0, 1, oldEnd.x, newEnd.x);

		updateIndicator();

		if (moveXaxisMinimum) {
			series1.pointValues[0] = WMG_Util.RemapVec2(addPointAnimTimeline, 0, 1, oldStart, newStart);
			graph.xAxis.AxisMinValue = WMG_Util.RemapFloat(addPointAnimTimeline, 0, 1, oldStart.x, newStart.x);
		}
	}

	void onCompleteAnimateAddPoint() {
		if (moveXaxisMinimum) {
			series1.pointValues.RemoveAt(0);
			blinkCurrentPointAnimation(true);
		}
	}

	string customTooltipLabeler(WMG_Series aSeries, WMG_Node aNode) {
		Vector2 nodeData = aSeries.getNodeValue(aNode);
		tooltipNumberFormatInfo.CurrencyDecimalDigits = aSeries.theGraph.tooltipNumberDecimals;
		string textToSet = nodeData.y.ToString("C", tooltipNumberFormatInfo);
		if (aSeries.theGraph.tooltipDisplaySeriesName) {
			textToSet = aSeries.seriesName + ": " + textToSet;
		}
		return textToSet;
	}
	
	string customYAxisLabelLabeler(WMG_Axis axis, int labelIndex) {
		float num = axis.AxisMinValue + labelIndex * (axis.AxisMaxValue - axis.AxisMinValue) / (axis.axisLabels.Count-1);
		yAxisNumberFormatInfo.CurrencyDecimalDigits = axis.numDecimalsAxisLabels;
		return num.ToString("C", yAxisNumberFormatInfo);
	}
	
	string customSeriesDataLabeler(WMG_Series series, float val) {
		seriesDataLabelsNumberFormatInfo.CurrencyDecimalDigits = series.dataLabelsNumDecimals;
		return val.ToString("C", seriesDataLabelsNumberFormatInfo);
	}

	void UpdateIndicatorSize(WMG_Axis_Graph aGraph) {
		aGraph.changeSpritePositionTo(graphOverlay, aGraph.graphBackground.transform.parent.transform.localPosition);
		float indicatorWidth =  (aGraph.getSpriteWidth(aGraph.graphBackground) - aGraph.paddingLeftRight[0] - aGraph.paddingLeftRight[1]);
		aGraph.changeSpriteSize(indicatorGO, Mathf.RoundToInt(indicatorWidth), 2);
		aGraph.changeSpritePositionToX(indicatorGO, indicatorWidth/2f);
		//updateIndicator();
	}

}
