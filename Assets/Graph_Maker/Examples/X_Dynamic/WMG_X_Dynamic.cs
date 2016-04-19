using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.UI;

public class WMG_X_Dynamic : MonoBehaviour {
	public GameObject graphPrefab;
	public WMG_Axis_Graph graph;

	public bool performTests;
	public bool noTestDelay;
	
	public float testInterval;
	public float testGroupInterval = 2;

	public Ease easeType;
	public GameObject realTimePrefab;

	GameObject realTimeObj;
	float animDuration;
	WaitForSeconds waitTime;

	void Start() {
		GameObject graphGO = GameObject.Instantiate(graphPrefab) as GameObject;
		graph = graphGO.GetComponent<WMG_Axis_Graph>();

		graph.changeSpriteParent(graphGO, this.gameObject);
		graph.changeSpritePositionTo(graphGO, Vector3.zero);
		graph.graphTitleOffset = new Vector2(0, 60);
		graph.autoAnimationsDuration = testInterval - 0.1f;

		waitTime = new WaitForSeconds(testInterval);
		animDuration = testInterval - 0.1f; // have animations slightly faster than the test interval
		if (animDuration < 0) animDuration = 0;

		if (performTests) {
			StartCoroutine(startTests());
		}
	}

	void Update() {
//		if (Input.GetKeyDown(KeyCode.A)) {
//			WMG_Anim.animSize(graph.gameObject, 1, Ease.Linear, new Vector2(530, 420));
//		}
//		if (Input.GetKeyDown(KeyCode.B)) {
//			WMG_Anim.animSize(graph.gameObject, 1, Ease.Linear, new Vector2(300, 200));
//		}
	}

	IEnumerator startTests() {
		yield return new WaitForSeconds(testGroupInterval);

		// animation function tests
		graph.graphTitleString = "Animation Function Tests";
		StartCoroutine(animationFunctionTests());
		if (!noTestDelay) yield return new WaitForSeconds(testInterval * 12);
		yield return new WaitForSeconds(testGroupInterval);

		// auto animation tests
		graph.graphTitleString = "Auto Animation Tests";
		StartCoroutine(autoAnimationTests());
		if (!noTestDelay) yield return new WaitForSeconds(testInterval * 15);
		yield return new WaitForSeconds(testGroupInterval);

		// graph type and orientation tests
		graph.graphTitleString = "Graph Type and Orientation Tests";
		StartCoroutine(graphTypeAndOrientationTests());
		if (!noTestDelay) yield return new WaitForSeconds(testInterval * 13);
		yield return new WaitForSeconds(testGroupInterval);

		// data labels tests
		graph.graphTitleString = "Data Labels Tests";
		StartCoroutine(dataLabelsTests());
		if (!noTestDelay) yield return new WaitForSeconds(testInterval * 9);
		yield return new WaitForSeconds(testGroupInterval);

		// series tests
		graph.graphTitleString = "Series Tests";
		StartCoroutine(seriesTests());
		if (!noTestDelay) yield return new WaitForSeconds(testInterval * 24);
		yield return new WaitForSeconds(testGroupInterval);

		// grouping / null tests
		graph.graphTitleString = "Grouping / Null Tests";
		StartCoroutine(groupingTests());
		if (!noTestDelay) yield return new WaitForSeconds(testInterval * 6);
		yield return new WaitForSeconds(testGroupInterval);

//		// Autofit tests
//		graph.graphTitleString = "Autofit Tests";
//		StartCoroutine(autofitTests());
//		if (!noTestDelay) yield return new WaitForSeconds(testInterval * 7);
//		yield return new WaitForSeconds(testGroupInterval);

		// axes tests
		graph.graphTitleString = "Axes Tests";
		StartCoroutine(axesTests());
		if (!noTestDelay) yield return new WaitForSeconds(testInterval * 13);
		yield return new WaitForSeconds(testGroupInterval);

		// axes tests with bar chart
		graph.graphTitleString = "Axes Tests - Bar";
		graph.axisWidth = 2;
		graph.graphType = WMG_Axis_Graph.graphTypes.bar_side;
		if (!noTestDelay) yield return new WaitForSeconds(testInterval);
		StartCoroutine(axesTests());
		if (!noTestDelay) yield return new WaitForSeconds(testInterval * 13);
		yield return new WaitForSeconds(testGroupInterval);

		// axes tests with bar chart horizontal
		graph.graphTitleString = "Axes Tests - Bar - Horizontal";
		graph.orientationType = WMG_Axis_Graph.orientationTypes.horizontal;
		if (!noTestDelay) yield return new WaitForSeconds(testInterval);
		StartCoroutine(axesTests());
		if (!noTestDelay) yield return new WaitForSeconds(testInterval * 13);
		graph.graphType = WMG_Axis_Graph.graphTypes.line;
		graph.orientationType = WMG_Axis_Graph.orientationTypes.vertical;
		graph.axisWidth = 4;
		yield return new WaitForSeconds(testGroupInterval);

		// add delete tests
		graph.graphTitleString = "Add / Delete Series Tests";
		StartCoroutine(addDeleteTests());
		if (!noTestDelay) yield return new WaitForSeconds(testInterval * 11);
		yield return new WaitForSeconds(testGroupInterval);

		// add delete tests with bar chart
		graph.graphTitleString = "Add / Delete Series Tests - Bar";
		graph.graphType = WMG_Axis_Graph.graphTypes.bar_side;
		if (!noTestDelay) yield return new WaitForSeconds(testInterval);
		StartCoroutine(addDeleteTests());
		if (!noTestDelay) yield return new WaitForSeconds(testInterval * 11);
		graph.graphType = WMG_Axis_Graph.graphTypes.line;
		yield return new WaitForSeconds(testGroupInterval);

		// legend tests
		graph.graphTitleString = "Legend Tests";
		StartCoroutine(legendTests());
		if (!noTestDelay) yield return new WaitForSeconds(testInterval * 7);
		yield return new WaitForSeconds(testGroupInterval);

		// hide show tests
		graph.graphTitleString = "Hide / Show Tests";
		StartCoroutine(hideShowTests());
		if (!noTestDelay) yield return new WaitForSeconds(testInterval * 12);
		yield return new WaitForSeconds(testGroupInterval);

		// grids ticks tests
		graph.graphTitleString = "Grids / Ticks Tests";
		StartCoroutine(gridsTicksTests());
		if (!noTestDelay) yield return new WaitForSeconds(testInterval * 4);
		yield return new WaitForSeconds(testGroupInterval);

		// size tests
		graph.graphTitleString = "Resize Tests";
		StartCoroutine(sizeTests());
		if (!noTestDelay) yield return new WaitForSeconds(testInterval * 3);
		yield return new WaitForSeconds(testGroupInterval);

		// size tests
		graph.graphTitleString = "Resize Tests - Resize Content";
		graph.resizeEnabled = true;
		graph.resizeProperties = (WMG_Axis_Graph.ResizeProperties)~0;
		if (!noTestDelay) yield return new WaitForSeconds(testInterval);
		StartCoroutine(sizeTests());
		if (!noTestDelay) yield return new WaitForSeconds(testInterval * 3);
		graph.resizeEnabled = false;
		graph.resizeProperties = (WMG_Axis_Graph.ResizeProperties)0;
		yield return new WaitForSeconds(testGroupInterval);

		// Dynamic Data Population via Reflection tests
		graph.graphTitleString = "Dynamic Data Population via Reflection";
		StartCoroutine(dynamicDataPopulationViaReflectionTests());
		if (!noTestDelay) yield return new WaitForSeconds(testInterval * 8);
		yield return new WaitForSeconds(testGroupInterval);

		// real-time tests
		graph.graphTitleString = "Real-time Tests";
		StartCoroutine(realTimeTests());
		if (!noTestDelay) yield return new WaitForSeconds(10);
		yield return new WaitForSeconds(testGroupInterval);

		// axis auto grow / shrink Tests
		graph.graphTitleString = "Axis Auto Grow / Shrink Tests";
		StartCoroutine(axisAutoGrowShrinkTests());
		if (!noTestDelay) yield return new WaitForSeconds(23);
		yield return new WaitForSeconds(testGroupInterval);

		graph.graphTitleString = "Demo Tests Completed Successfully :)";
	}

	IEnumerator autofitTests() {

		string s1 = "Short";
		string s2 = "Medium length";
		string s3 = "This is a lonnnnnnnnnnnnng string";

//		graph.legend.hideLegend = true;
		graph.yAxis.SetLabelsUsingMaxMin = false;
		graph.paddingTopBottom = new Vector2 (40, 60);
		graph.paddingLeftRight = new Vector2 (60, 40);
//		graph.axesType = WMG_Axis_Graph.axesTypes.II;

		if (!noTestDelay) yield return waitTime;

		graph.yAxis.axisLabels.SetList(new List<string> () {s1, s1, s1});
		graph.xAxis.axisLabels.SetList(new List<string> () {s1, s1, s1, s1});

		graph.autoFitLabels = true;

		if (!noTestDelay) yield return waitTime;

		graph.yAxis.axisLabels.SetList(new List<string> () {s1, s1, s3});

		if (!noTestDelay) yield return waitTime;

		graph.yAxis.axisLabels.SetList(new List<string> () {s3, s1, s3});

		if (!noTestDelay) yield return waitTime;

		graph.yAxis.axisLabels.SetList(new List<string> () {s1, s1, s1});

		if (!noTestDelay) yield return waitTime;

		graph.xAxis.axisLabels.SetList(new List<string> () {s1, s2, s2, s1});

		if (!noTestDelay) yield return waitTime;
		
		graph.xAxis.axisLabels.SetList(new List<string> () {s1, s2, s2, s3});

		if (!noTestDelay) yield return waitTime;
		
		graph.xAxis.axisLabels.SetList(new List<string> () {s1, s1, s1, s1});

		if (!noTestDelay) yield return waitTime;

		graph.legend.hideLegend = false;
		graph.yAxis.SetLabelsUsingMaxMin = true;
		graph.paddingTopBottom = new Vector2 (40, 70);
		graph.paddingLeftRight = new Vector2 (45, 40);
		graph.xAxis.axisLabels.SetList(new List<string> () {"Q1 '15", "Q2 '15", "Q3 '15", "Q4 '15"});
		graph.autoFitLabels = false;

	}
	
	IEnumerator groupingTests() {
		List<string> xLabels = new List<string>(graph.xAxis.axisLabels);
		WMG_Series s1 = graph.lineSeries[0].GetComponent<WMG_Series>();
		Vector2 p1 = s1.pointValues [3];
		Vector2 p2 = s1.pointValues [6];
		Vector2 p3 = s1.pointValues [9];
		graph.useGroups = true;

		if (!noTestDelay) yield return waitTime;

		// null functionality by removing data
		// In this case when grouping is enabled graph maker will automatically add back these missing vector2's but with a negative x value.
		// Negative x-index (or group index) is how Graph Maker determines if it is null.
		s1.pointValues.RemoveAt (3);
		s1.pointValues.RemoveAt (5);

		if (!noTestDelay) yield return waitTime;

		// null functionality by setting negative x index
		s1.pointValues[9] = new Vector2(-s1.pointValues[9].x, s1.pointValues[9].y);

		if (!noTestDelay) yield return waitTime;

		// labels based on groups functionality
		graph.xAxis.LabelType = WMG_Axis.labelTypes.groups;
		graph.xAxis.AxisNumTicks = graph.groups.Count;

		if (!noTestDelay) yield return waitTime;

		// labels centered on each group of bars
		graph.graphType = WMG_Axis_Graph.graphTypes.bar_side;
		graph.xAxis.AxisNumTicks = 2; // can be whatever
		WMG_Anim.animFloat(()=> graph.xAxis.AxisLabelRotation, x=> graph.xAxis.AxisLabelRotation = x, animDuration, 60);

		if (!noTestDelay) yield return waitTime;

		// set graph to what it was originally
		s1.pointValues[3] = p1;
		s1.pointValues[6] = p2;
		s1.pointValues[9] = p3;

		if (!noTestDelay) yield return waitTime;

		graph.graphType = WMG_Axis_Graph.graphTypes.line;
		graph.xAxis.LabelType = WMG_Axis.labelTypes.ticks_center;
		graph.useGroups = false;
		graph.xAxis.AxisNumTicks = 5;
		graph.xAxis.AxisLabelRotation = 0;
		graph.xAxis.axisLabels.SetList(xLabels);
	}

	IEnumerator seriesTests() {
		WMG_Series s1 = graph.lineSeries[0].GetComponent<WMG_Series>();
		WMG_Series s2 = graph.lineSeries[1].GetComponent<WMG_Series>();
		List<Vector2> s1Data = s1.pointValues.list;
		List<Vector2> s2Data = s2.pointValues.list;
		Color s1PointColor = s1.pointColor;
		Color s2PointColor = s2.pointColor;
		Vector2 origSize = graph.getSpriteSize(graph.gameObject);

		WMG_Anim.animSize(graph.gameObject, animDuration, Ease.Linear, new Vector2(origSize.x * 2, origSize.y * 2));

		if (!noTestDelay) yield return waitTime;
		graph.deleteSeries();

		if (!noTestDelay) yield return waitTime;
		s1.pointWidthHeight = 15;

		if (!noTestDelay) yield return waitTime;
		s1.pointPrefab = 1;

		if (!noTestDelay) yield return waitTime;
		s1.pointPrefab = 0;

		if (!noTestDelay) yield return waitTime;
		s1.linkPrefab = 1;
		s1.lineScale = 1;

		if (!noTestDelay) yield return waitTime;
		WMG_Anim.animFloat(()=> s1.linePadding, x=> s1.linePadding = x, animDuration, -15);

		if (!noTestDelay) yield return waitTime;
		s1.linkPrefab = 0;
		s1.lineScale = 0.5f;

		if (!noTestDelay) yield return waitTime;
		List<Color> pointColors = new List<Color>();
		for (int i = 0; i < s1.pointValues.Count; i++) {
			pointColors.Add(new Color(Random.Range(0, 1f),Random.Range(0, 1f),Random.Range(0, 1f),1));
		}
		s1.usePointColors = true;
		s1.pointColors.SetList(pointColors);

		if (!noTestDelay) yield return waitTime;
		graph.graphType = WMG_Axis_Graph.graphTypes.bar_side;

		if (!noTestDelay) yield return waitTime;
		graph.graphType = WMG_Axis_Graph.graphTypes.line;

		if (!noTestDelay) yield return waitTime;
		s1.usePointColors = false;

		// Create a circle
		if (!noTestDelay) yield return waitTime;
		s1.UseXDistBetweenToSpace = false;
		graph.xAxis.AxisMaxValue = graph.yAxis.AxisMaxValue * (graph.xAxisLength / graph.yAxisLength);
		graph.xAxis.SetLabelsUsingMaxMin = true;
		graph.xAxis.LabelType = WMG_Axis.labelTypes.ticks;
		graph.xAxis.numDecimalsAxisLabels = 1;
		s1.pointValues.SetList(graph.GenCircular(s1.pointValues.Count, graph.xAxis.AxisMaxValue/2f, graph.yAxis.AxisMaxValue/2f, graph.yAxis.AxisMaxValue/2f-2));
		s1.connectFirstToLast = true;

		// Triangle
		if (!noTestDelay) yield return waitTime;
		s1.pointValues.SetList(graph.GenCircular(3, graph.xAxis.AxisMaxValue/2f, graph.yAxis.AxisMaxValue/2f, graph.yAxis.AxisMaxValue/2f-2));

		// Triangle rotated 90 degrees, auto animations enabled for awesomeness
		graph.autoAnimationsEnabled = true;
		if (!noTestDelay) yield return waitTime;
		s1.pointValues.SetList(graph.GenCircular2(3, graph.xAxis.AxisMaxValue/2f, graph.yAxis.AxisMaxValue/2f, graph.yAxis.AxisMaxValue/2f-2, 90));

		if (!noTestDelay) yield return waitTime;
		graph.autoAnimationsEnabled = false;
		s1.pointValues.SetList(graph.GenCircular(50, graph.xAxis.AxisMaxValue/2f, graph.yAxis.AxisMaxValue/2f, graph.yAxis.AxisMaxValue/2f-2));
		s1.linePadding = 0;

		if (!noTestDelay) yield return waitTime;
		s1.hidePoints = true;

		if (!noTestDelay) yield return waitTime;
		s1.lineColor = Color.green;
		WMG_Anim.animFloat(()=> s1.lineScale, x=> s1.lineScale = x, animDuration, 2);

		if (!noTestDelay) yield return waitTime;
		WMG_Anim.animFloat(()=> s1.lineScale, x=> s1.lineScale = x, animDuration, 0.5f);
		 
		if (!noTestDelay) yield return waitTime;
		s1.hideLines = true;
		s1.hidePoints = false;

		if (!noTestDelay) yield return waitTime;
		s1.pointValues.SetList(graph.GenRandomXY(50, graph.xAxis.AxisMinValue, graph.xAxis.AxisMaxValue, graph.yAxis.AxisMinValue, graph.yAxis.AxisMaxValue));

		if (!noTestDelay) yield return waitTime;
		graph.autoAnimationsEnabled = true;
		s1.pointColor = Color.green;
		s1.pointValues.SetList(graph.GenRandomXY(50, graph.xAxis.AxisMinValue, graph.xAxis.AxisMaxValue, graph.yAxis.AxisMinValue, graph.yAxis.AxisMaxValue));

		if (!noTestDelay) yield return waitTime;
		graph.autoAnimationsEnabled = false;

		// restore
		if (!noTestDelay) yield return waitTime;
		s1.lineColor = Color.white;
		s1.pointColor = s1PointColor;
		s1.hideLines = false;
		s1.pointValues.SetList(s1Data);
		s1.connectFirstToLast = false;
		s1.UseXDistBetweenToSpace = true;
		s1.pointWidthHeight = 10;
		addSeriesWithRandomData();
		graph.lineSeries[1].GetComponent<WMG_Series>().pointValues.SetList(s2Data);
		graph.lineSeries[1].GetComponent<WMG_Series>().pointColor = s2PointColor;
		graph.lineSeries[1].GetComponent<WMG_Series>().pointPrefab = 1;
		graph.xAxis.SetLabelsUsingMaxMin = false;
		graph.xAxis.axisLabels.SetList(new List<string> () {"Q1 '15", "Q2 '15", "Q3 '15", "Q4 '15"});
		graph.xAxis.LabelType = WMG_Axis.labelTypes.ticks_center;
		graph.xAxis.numDecimalsAxisLabels = 0;
		graph.xAxis.AxisMaxValue = 100;
		WMG_Anim.animSize(graph.gameObject, animDuration, Ease.Linear, new Vector2(origSize.x, origSize.y));

		if (!noTestDelay) yield return waitTime;
	}

	IEnumerator autoAnimationTests() {
		WMG_Series s1 = graph.lineSeries[0].GetComponent<WMG_Series>();
		WMG_Series s2 = graph.lineSeries[1].GetComponent<WMG_Series>();
		List<Vector2> s1Data = s1.pointValues.list;
		List<Vector2> s2Data = s2.pointValues.list;
		graph.autoAnimationsEnabled = true;
		graph.graphType = WMG_Axis_Graph.graphTypes.line;
		
		if (!noTestDelay) yield return waitTime;
		graph.orientationType = WMG_Axis_Graph.orientationTypes.horizontal;
		
		if (!noTestDelay) yield return waitTime;
		graph.orientationType = WMG_Axis_Graph.orientationTypes.vertical;
		
		if (!noTestDelay) yield return waitTime;
		graph.graphType = WMG_Axis_Graph.graphTypes.bar_side;
		
		if (!noTestDelay) yield return waitTime;
		graph.orientationType = WMG_Axis_Graph.orientationTypes.horizontal;
		
		if (!noTestDelay) yield return waitTime;
		graph.orientationType = WMG_Axis_Graph.orientationTypes.vertical;
		
		if (!noTestDelay) yield return waitTime;
		graph.graphType = WMG_Axis_Graph.graphTypes.line;

		// change 1 value
		if (!noTestDelay) yield return waitTime;
		List<Vector2> s1Data2 = new List<Vector2>(s1Data);
		s1Data2[6] = new Vector2(s1Data2[6].x, s1Data2[6].y + 5);
		s1.pointValues.SetList(s1Data2);

		if (!noTestDelay) yield return waitTime;
		s1.pointValues.SetList(s1Data);

		// change multiple values
		if (!noTestDelay) yield return waitTime;
		s1.pointValues.SetList(graph.GenRandomY(s1Data.Count, 0, s1Data.Count-1, graph.yAxis.AxisMinValue, graph.yAxis.AxisMaxValue));

		if (!noTestDelay) yield return waitTime;
		s1.pointValues.SetList(s1Data);

		// change multiple series multiple values multiple times before animation can finish
		if (!noTestDelay) yield return waitTime;
		graph.autoAnimationsDuration = 2*testInterval - 0.1f;
		s1.pointValues.SetList(graph.GenRandomY(s1Data.Count, 0, s1Data.Count-1, graph.yAxis.AxisMinValue, graph.yAxis.AxisMaxValue));
		s2.pointValues.SetList(graph.GenRandomY(s2Data.Count, 0, s2Data.Count-1, graph.yAxis.AxisMinValue, graph.yAxis.AxisMaxValue));

		if (!noTestDelay) yield return waitTime;
		s1.pointValues.SetList(graph.GenRandomY(s1Data.Count, 0, s1Data.Count-1, graph.yAxis.AxisMinValue, graph.yAxis.AxisMaxValue));

		if (!noTestDelay) yield return waitTime;
		if (!noTestDelay) yield return waitTime;
		graph.autoAnimationsDuration = testInterval - 0.1f;
		s1.pointValues.SetList(s1Data);
		s2.pointValues.SetList(s2Data);

		if (!noTestDelay) yield return waitTime;

		graph.autoAnimationsEnabled = false;
	}

	IEnumerator animationFunctionTests() {
		// Get before and after scale vectors for each series. Sometimes we need to use series data (line widths).
		List<Vector3> beforeScaleLine = graph.getSeriesScaleVectors(true, -1, 0);
		List<Vector3> afterScaleLine = graph.getSeriesScaleVectors(true, -1, 1);
		List<Vector3> beforeScalePoint = graph.getSeriesScaleVectors(false, 0, 0);
		List<Vector3> afterScalePoint = graph.getSeriesScaleVectors(false, 1, 1);
		List<Vector3> beforeScaleBar;
		if (graph.orientationType == WMG_Axis_Graph.orientationTypes.vertical) beforeScaleBar = graph.getSeriesScaleVectors(false, 1, 0);
		else beforeScaleBar = graph.getSeriesScaleVectors(false, 0, 1);

		// "Line: All - Center"
		graph.changeAllLinePivots(WMG_Graph_Manager.WMGpivotTypes.Center);
		graph.animScaleAllAtOnce(false, animDuration, 0, easeType, beforeScaleLine, afterScaleLine);
		graph.animScaleAllAtOnce(true, animDuration, 0, easeType, beforeScalePoint, afterScalePoint);

		if (!noTestDelay) yield return waitTime;

		// "Line: All - Left"
		graph.changeAllLinePivots(WMG_Graph_Manager.WMGpivotTypes.Top);
		graph.animScaleAllAtOnce(false, animDuration, 0, easeType, beforeScaleLine, afterScaleLine);
		graph.animScaleAllAtOnce(true, animDuration, 0, easeType, beforeScalePoint, afterScalePoint);

		if (!noTestDelay) yield return waitTime;

		// "Line: All - Right"
		graph.changeAllLinePivots(WMG_Graph_Manager.WMGpivotTypes.Bottom);
		graph.animScaleAllAtOnce(false, animDuration, 0, easeType, beforeScaleLine, afterScaleLine);
		graph.animScaleAllAtOnce(true, animDuration, 0, easeType, beforeScalePoint, afterScalePoint);

		if (!noTestDelay) yield return waitTime;

		// "Line: Series - Center"
		graph.changeAllLinePivots(WMG_Graph_Manager.WMGpivotTypes.Center);
		graph.animScaleBySeries(false, animDuration, 0, easeType, beforeScaleLine, afterScaleLine);
		graph.animScaleBySeries(true, animDuration, 0, easeType, beforeScalePoint, afterScalePoint);

		if (!noTestDelay) yield return waitTime;

		// "Line: Series - Left"
		graph.changeAllLinePivots(WMG_Graph_Manager.WMGpivotTypes.Top);
		graph.animScaleBySeries(false, animDuration, 0, easeType, beforeScaleLine, afterScaleLine);
		graph.animScaleBySeries(true, animDuration, 0, easeType, beforeScalePoint, afterScalePoint);

		if (!noTestDelay) yield return waitTime;

		// "Line: Series - Right"
		graph.changeAllLinePivots(WMG_Graph_Manager.WMGpivotTypes.Bottom);
		graph.animScaleBySeries(false, animDuration, 0, easeType, beforeScaleLine, afterScaleLine);
		graph.animScaleBySeries(true, animDuration, 0, easeType, beforeScalePoint, afterScalePoint);

		if (!noTestDelay) yield return waitTime;

		// "Line: Point - Center"
		graph.changeAllLinePivots(WMG_Graph_Manager.WMGpivotTypes.Center);
		graph.animScaleOneByOne(false, animDuration, 0, easeType, beforeScaleLine, afterScaleLine, 2);
		graph.animScaleOneByOne(true, animDuration/2f, animDuration/2f, easeType, beforeScalePoint, afterScalePoint, 2);

		if (!noTestDelay) yield return waitTime;

		// "Line: Point - Left"
		graph.changeAllLinePivots(WMG_Graph_Manager.WMGpivotTypes.Top);
		graph.animScaleOneByOne(false, animDuration, 0, easeType, beforeScaleLine, afterScaleLine, 0);
		graph.animScaleOneByOne(true, animDuration/2f, animDuration/2f, easeType, beforeScalePoint, afterScalePoint, 0);

		if (!noTestDelay) yield return waitTime;

		// "Line: Point - Right"
		graph.changeAllLinePivots(WMG_Graph_Manager.WMGpivotTypes.Bottom);
		graph.animScaleOneByOne(false, animDuration, 0, easeType, beforeScaleLine, afterScaleLine, 1);
		graph.animScaleOneByOne(true, animDuration/2f, animDuration/2f, easeType, beforeScalePoint, afterScalePoint, 1);

		if (!noTestDelay) yield return waitTime;

		graph.changeAllLinePivots(WMG_Graph_Manager.WMGpivotTypes.Center);
		graph.graphType = WMG_Axis_Graph.graphTypes.bar_side;

		if (!noTestDelay) yield return waitTime;

		// "Bar: All"
		graph.animScaleAllAtOnce(true, animDuration, 0, easeType, beforeScaleBar, afterScalePoint);

		if (!noTestDelay) yield return waitTime;

		// "Bar: Series"
		graph.animScaleBySeries(true, animDuration, 0, easeType, beforeScaleBar, afterScalePoint);

		if (!noTestDelay) yield return waitTime;

		// "Bar: Point"
		graph.animScaleOneByOne(true, animDuration, 0, easeType, beforeScaleBar, afterScalePoint, 0);

	}

	IEnumerator dynamicDataPopulationViaReflectionTests() {
		// Create a WMG_Data_Source which acts as the connection between arbitrary data and a Graph Maker graph.
		WMG_Data_Source ds = this.gameObject.AddComponent<WMG_Data_Source>();
		ds.dataSourceType = WMG_Data_Source.WMG_DataSourceTypes.Multiple_Objects_Single_Variable; // in this example we'll use multiple objects

		// Note that WMG_Data_Source does not do data conversion or data transformations (e.g. string / float to vector2)
		// The series pointValues expects a List<Vector2>, so the input for multiple objects needs to be a single Vector2 variable on each of the objects
		// For single object single variable, it would need to be a List<Vector2>, and for single object multiple variable, it would need one or more Vector2 variables

		// create the data providers (can be any objects or scripts, this is just an example)
		List<Vector2> randomData = graph.GenRandomY(graph.groups.Count, 1, graph.groups.Count, graph.yAxis.AxisMinValue, graph.yAxis.AxisMaxValue);
		List<GameObject> dataProviders = new List<GameObject>();
		for (int i = 0; i < graph.groups.Count; i++) {
			GameObject emptyObj = new GameObject();
			dataProviders.Add(emptyObj);
			WMG_X_Data_Provider dp = emptyObj.AddComponent<WMG_X_Data_Provider>();
			dp.vec1 = randomData[i]; // our random data, in real applications this would come from other sources
			ds.addDataProviderToList<WMG_X_Data_Provider>(dp); // Add our data provider to the data source
		}
		ds.setVariableName("vec1"); // set the variable name on the data source which matches the Vector2 on the data provider which we are interested in graphing
		ds.variableType = WMG_Data_Source.WMG_VariableTypes.Field; // optional - set the variable type to slightly improve performance.

		if (!noTestDelay) yield return waitTime;
		if (!noTestDelay) yield return waitTime;
		// assign the data source to the series, once this happens, the series will detect that the dataSource is not null 
		// and attempt to pull data from data providers on the referenced data source.
		WMG_Series s1 = graph.lineSeries[0].GetComponent<WMG_Series>();
		List<Vector2> s1Data = s1.pointValues.list; // just grab current data so we can reset the graph to its original state later
		s1.pointValuesDataSource = ds; // assign the data source to a series.

		if (!noTestDelay) yield return waitTime;
		if (!noTestDelay) yield return waitTime;

		// now if data changes occur on the data provider(s), the changes will be automatically applied to the graph
		randomData = graph.GenRandomY(graph.groups.Count, 1, graph.groups.Count, graph.yAxis.AxisMinValue, graph.yAxis.AxisMaxValue);
		for (int i = 0; i < graph.groups.Count; i++) {
			dataProviders[i].GetComponent<WMG_X_Data_Provider>().vec1 = randomData[i];
		}

		if (!noTestDelay) yield return waitTime;
		if (!noTestDelay) yield return waitTime;

		graph.autoAnimationsEnabled = true;
		// also auto animations should work as well :)
		randomData = graph.GenRandomY(graph.groups.Count, 1, graph.groups.Count, graph.yAxis.AxisMinValue, graph.yAxis.AxisMaxValue);
		for (int i = 0; i < graph.groups.Count; i++) {
			dataProviders[i].GetComponent<WMG_X_Data_Provider>().vec1 = randomData[i];
		}

		if (!noTestDelay) yield return waitTime;
		if (!noTestDelay) yield return waitTime;

		// set graph to original state
		graph.autoAnimationsEnabled = false;
		s1.pointValuesDataSource = null;
		s1.pointValues.SetList(s1Data);
	}

	IEnumerator realTimeTests() {
		// This is very similar to the dynamic data population via reflection.
		// Main difference being the use of some WMG_Axis_Graph functions to start and stop real-time updating / and the x-axis updates based on time.
		WMG_Data_Source ds1 = graph.lineSeries[0].AddComponent<WMG_Data_Source>();
		WMG_Data_Source ds2 = graph.lineSeries[1].AddComponent<WMG_Data_Source>();
		ds1.dataSourceType = WMG_Data_Source.WMG_DataSourceTypes.Single_Object_Single_Variable;
		ds2.dataSourceType = WMG_Data_Source.WMG_DataSourceTypes.Single_Object_Single_Variable;
		WMG_Series s1 = graph.lineSeries[0].GetComponent<WMG_Series>();
		WMG_Series s2 = graph.lineSeries[1].GetComponent<WMG_Series>();
		realTimeObj = GameObject.Instantiate(realTimePrefab) as GameObject;
		graph.changeSpriteParent(realTimeObj, this.gameObject);
		ds1.setDataProvider<Transform>(realTimeObj.transform);
		ds2.setDataProvider<Transform>(realTimeObj.transform);
		ds1.setVariableName("localPosition.x");
		ds2.setVariableName("localPosition.y");
		s1.realTimeDataSource = ds1;
		s2.realTimeDataSource = ds2;
		graph.xAxis.AxisMaxValue = 0;
		graph.xAxis.AxisMaxValue = 5;
		graph.yAxis.AxisMinValue = -200;
		graph.yAxis.AxisMaxValue = 200;
		s1.seriesName = "Hex X";
		s2.seriesName = "Hex Y";
		s1.UseXDistBetweenToSpace = false;
		s2.UseXDistBetweenToSpace = false;
		graph.xAxis.SetLabelsUsingMaxMin = true;
		graph.xAxis.LabelType = WMG_Axis.labelTypes.ticks;
		graph.xAxis.numDecimalsAxisLabels = 1;
		s1.StartRealTimeUpdate();
		s2.StartRealTimeUpdate();
		WMG_Anim.animPosition(realTimeObj, 3, Ease.Linear, new Vector3(200,-150,0));
		yield return new WaitForSeconds (4);

		WMG_Anim.animPosition(realTimeObj, 1, Ease.Linear, new Vector3(-150,100,0));
		yield return new WaitForSeconds (3);

		WMG_Anim.animPosition(realTimeObj, 1, Ease.Linear, new Vector3(-125,75,0));
		yield return new WaitForSeconds (3);

		s1.StopRealTimeUpdate();
		s2.StopRealTimeUpdate();
	}

	IEnumerator axisAutoGrowShrinkTests() {
		// assumes realTimeTests occurred before this
		WMG_Series s1 = graph.lineSeries[0].GetComponent<WMG_Series>();
		WMG_Series s2 = graph.lineSeries[1].GetComponent<WMG_Series>();

		s1.ResumeRealTimeUpdate();
		s2.ResumeRealTimeUpdate();
		yield return new WaitForSeconds (1);

		graph.graphTitleString = "Axis Auto Grow / Shrink - Disabled";

		WMG_Anim.animPosition(realTimeObj, 1, Ease.Linear, new Vector3(-125,300,0));
		yield return new WaitForSeconds (2);

		WMG_Anim.animPosition(realTimeObj, 1, Ease.Linear, new Vector3(-125,75,0));
		yield return new WaitForSeconds (6);

		graph.graphTitleString = "Axis Auto Grow / Shrink - Enabled";
		graph.yAxis.MaxAutoGrow = true; // increases absolute value of the axis maximum
		graph.yAxis.MinAutoGrow = true; // increases absolute value of the axis minimum
		graph.yAxis.MaxAutoShrink = true; // decreases absolute value of the axis maximum if series' data is below a % threshold
		graph.yAxis.MinAutoShrink = true; // decreases absolute value of the axis minimum if series' data is above a % threshold
		graph.autoShrinkAtPercent = 0.6f; // the % threshold for auto strinking
		graph.autoGrowAndShrinkByPercent = 0.2f; // the % relative to axis total value that is added / subtracted for grow and shrink occurrences.

		WMG_Anim.animPosition(realTimeObj, 2, Ease.Linear, new Vector3(-125,350,0));
		yield return new WaitForSeconds (3);

		WMG_Anim.animPosition(realTimeObj, 2, Ease.Linear, new Vector3(-125,75,0));
		yield return new WaitForSeconds (3);

		WMG_Anim.animPosition(realTimeObj, 2, Ease.Linear, new Vector3(-5,5,0));
		yield return new WaitForSeconds (8);

		s1.StopRealTimeUpdate();
		s2.StopRealTimeUpdate();
	}

	IEnumerator hideShowTests() {
		WMG_Series s1 = graph.lineSeries[0].GetComponent<WMG_Series>();
		WMG_Series s2 = graph.lineSeries[1].GetComponent<WMG_Series>();

		graph.legend.hideLegend = true;

		if (!noTestDelay) yield return waitTime;
		graph.xAxis.hideLabels = true;

		if (!noTestDelay) yield return waitTime;
		graph.yAxis.hideLabels = true;

		if (!noTestDelay) yield return waitTime;
		graph.xAxis.hideTicks = true;

		if (!noTestDelay) yield return waitTime;
		graph.yAxis.hideTicks = true;

		if (!noTestDelay) yield return waitTime;
		graph.xAxis.hideGrid = true;

		if (!noTestDelay) yield return waitTime;
		graph.yAxis.hideGrid = true;

		if (!noTestDelay) yield return waitTime;
		graph.SetActive(graph.xAxis.AxisObj, false);

		if (!noTestDelay) yield return waitTime;
		graph.SetActive(graph.yAxis.AxisObj, false);

		if (!noTestDelay) yield return waitTime;
		s1.hidePoints = true;

		if (!noTestDelay) yield return waitTime;
		s2.hideLines = true;

		if (!noTestDelay) yield return waitTime;
		s1.hideLines = true;
		s2.hidePoints = true;

		if (!noTestDelay) yield return waitTime;
		graph.legend.hideLegend = false;
		graph.xAxis.hideLabels = false;
		graph.yAxis.hideLabels = false;
		graph.xAxis.hideTicks = false;
		graph.yAxis.hideTicks = false;
		graph.xAxis.hideGrid = false;
		graph.yAxis.hideGrid = false;
		graph.SetActive(graph.xAxis.AxisObj, true);
		graph.SetActive(graph.yAxis.AxisObj, true);
		s1.hideLines = false;
		s2.hideLines = false;
		s1.hidePoints = false;
		s2.hidePoints = false;
	}

	IEnumerator gridsTicksTests() {
		List<string> xLabels = new List<string>(graph.xAxis.axisLabels);

		WMG_Anim.animInt(()=> graph.yAxis.AxisNumTicks, x=> graph.yAxis.AxisNumTicks = x, animDuration, 11);

		if (!noTestDelay) yield return waitTime;
		graph.xAxis.LabelType = WMG_Axis.labelTypes.ticks;
		graph.xAxis.SetLabelsUsingMaxMin = true;
		WMG_Anim.animInt(()=> graph.xAxis.AxisNumTicks, x=> graph.xAxis.AxisNumTicks = x, animDuration, 11);

		if (!noTestDelay) yield return waitTime;
		WMG_Anim.animInt(()=> graph.yAxis.AxisNumTicks, x=> graph.yAxis.AxisNumTicks = x, animDuration, 3);

		if (!noTestDelay) yield return waitTime;
		WMG_Anim.animInt(()=> graph.xAxis.AxisNumTicks, x=> graph.xAxis.AxisNumTicks = x, animDuration, 5);

		if (!noTestDelay) yield return waitTime;
		graph.xAxis.LabelType = WMG_Axis.labelTypes.ticks_center;
		graph.xAxis.SetLabelsUsingMaxMin = false;
		graph.xAxis.axisLabels.SetList(xLabels);
	}

	IEnumerator sizeTests() {
		Vector2 origSize = graph.getSpriteSize(graph.gameObject);

		WMG_Anim.animSize(graph.gameObject, animDuration, Ease.Linear, new Vector2(origSize.x * 2, origSize.y * 2));

		if (!noTestDelay) yield return waitTime;
		WMG_Anim.animSize(graph.gameObject, animDuration, Ease.Linear, new Vector2(origSize.x * 2, origSize.y));

		if (!noTestDelay) yield return waitTime;
		WMG_Anim.animSize(graph.gameObject, animDuration, Ease.Linear, new Vector2(origSize.x, origSize.y * 2));

		if (!noTestDelay) yield return waitTime;
		WMG_Anim.animSize(graph.gameObject, animDuration, Ease.Linear, new Vector2(origSize.x, origSize.y));
	}

	IEnumerator legendTests() {

		graph.legend.legendType = WMG_Legend.legendTypes.Right;

		if (!noTestDelay) yield return waitTime;
		graph.legend.legendType = WMG_Legend.legendTypes.Bottom;
		graph.legend.oppositeSideLegend = true;

		if (!noTestDelay) yield return waitTime;
		graph.legend.legendType = WMG_Legend.legendTypes.Right;

		if (!noTestDelay) yield return waitTime;
		graph.legend.legendType = WMG_Legend.legendTypes.Bottom;
		graph.legend.oppositeSideLegend = false;

		if (!noTestDelay) yield return waitTime;
		addSeriesWithRandomData();
		addSeriesWithRandomData();
		addSeriesWithRandomData();

		if (!noTestDelay) yield return waitTime;
		graph.legend.legendType = WMG_Legend.legendTypes.Right;

		if (!noTestDelay) yield return waitTime;
		graph.deleteSeries();
		graph.deleteSeries();
		graph.deleteSeries();

		if (!noTestDelay) yield return waitTime;
		graph.legend.legendType = WMG_Legend.legendTypes.Bottom;

	}

	IEnumerator dataLabelsTests() {
		WMG_Series s1 = graph.lineSeries[0].GetComponent<WMG_Series>();
		WMG_Series s2 = graph.lineSeries[1].GetComponent<WMG_Series>();

		s1.dataLabelsEnabled = true;
		s2.dataLabelsEnabled = true;
		graph.graphType = WMG_Axis_Graph.graphTypes.line;

		if (!noTestDelay) yield return waitTime;
		graph.graphType = WMG_Axis_Graph.graphTypes.bar_side;

		if (!noTestDelay) yield return waitTime;
		graph.graphType = WMG_Axis_Graph.graphTypes.combo;

		if (!noTestDelay) yield return waitTime;
		graph.graphType = WMG_Axis_Graph.graphTypes.line_stacked;

		if (!noTestDelay) yield return waitTime;
		graph.orientationType = WMG_Axis_Graph.orientationTypes.horizontal;

		if (!noTestDelay) yield return waitTime;
		graph.graphType = WMG_Axis_Graph.graphTypes.combo;

		if (!noTestDelay) yield return waitTime;
		graph.graphType = WMG_Axis_Graph.graphTypes.bar_side;

		if (!noTestDelay) yield return waitTime;
		graph.graphType = WMG_Axis_Graph.graphTypes.line;

		if (!noTestDelay) yield return waitTime;
		graph.orientationType = WMG_Axis_Graph.orientationTypes.vertical;

		if (!noTestDelay) yield return waitTime;
		s1.dataLabelsEnabled = false;
		s2.dataLabelsEnabled = false;
	}

	IEnumerator graphTypeAndOrientationTests() {
		graph.graphType = WMG_Axis_Graph.graphTypes.bar_side;

		if (!noTestDelay) yield return waitTime;
		graph.graphType = WMG_Axis_Graph.graphTypes.combo;

		if (!noTestDelay) yield return waitTime;
		graph.graphType = WMG_Axis_Graph.graphTypes.bar_stacked;

		if (!noTestDelay) yield return waitTime;
		graph.graphType = WMG_Axis_Graph.graphTypes.line_stacked;

		if (!noTestDelay) yield return waitTime;
		graph.graphType = WMG_Axis_Graph.graphTypes.bar_stacked_percent;

		if (!noTestDelay) yield return waitTime;
		graph.graphType = WMG_Axis_Graph.graphTypes.line;

		if (!noTestDelay) yield return waitTime;
		graph.orientationType = WMG_Axis_Graph.orientationTypes.horizontal;

		if (!noTestDelay) yield return waitTime;
		graph.graphType = WMG_Axis_Graph.graphTypes.bar_side;

		if (!noTestDelay) yield return waitTime;
		graph.graphType = WMG_Axis_Graph.graphTypes.combo;
		
		if (!noTestDelay) yield return waitTime;
		graph.graphType = WMG_Axis_Graph.graphTypes.bar_stacked;

		if (!noTestDelay) yield return waitTime;
		graph.graphType = WMG_Axis_Graph.graphTypes.line_stacked;
		
		if (!noTestDelay) yield return waitTime;
		graph.graphType = WMG_Axis_Graph.graphTypes.bar_stacked_percent;

		if (!noTestDelay) yield return waitTime;
		graph.graphType = WMG_Axis_Graph.graphTypes.line;

		if (!noTestDelay) yield return waitTime;
		graph.orientationType = WMG_Axis_Graph.orientationTypes.vertical;
	}

	IEnumerator axesTests() {
		graph.axesType = WMG_Axis_Graph.axesTypes.I_II;

		if (!noTestDelay) yield return waitTime;
		graph.axesType = WMG_Axis_Graph.axesTypes.II;

		if (!noTestDelay) yield return waitTime;
		graph.axesType = WMG_Axis_Graph.axesTypes.II_III;

		if (!noTestDelay) yield return waitTime;
		graph.axesType = WMG_Axis_Graph.axesTypes.III;

		if (!noTestDelay) yield return waitTime;
		graph.axesType = WMG_Axis_Graph.axesTypes.III_IV;

		if (!noTestDelay) yield return waitTime;
		graph.axesType = WMG_Axis_Graph.axesTypes.IV;

		if (!noTestDelay) yield return waitTime;
		graph.axesType = WMG_Axis_Graph.axesTypes.I_IV;

		if (!noTestDelay) yield return waitTime;
		graph.axesType = WMG_Axis_Graph.axesTypes.CENTER;

		if (!noTestDelay) yield return waitTime;
		graph.axesType = WMG_Axis_Graph.axesTypes.I;

		if (!noTestDelay) yield return waitTime;
		graph.axesType = WMG_Axis_Graph.axesTypes.AUTO_ORIGIN_X;
		graph.xAxis.AxisUseNonTickPercent = true;
		WMG_Anim.animVec2(()=> graph.theOrigin, x=> graph.theOrigin = x, animDuration, new Vector2(graph.theOrigin.x, graph.yAxis.AxisMaxValue), Ease.Linear);

		if (!noTestDelay) yield return waitTime;
		graph.axesType = WMG_Axis_Graph.axesTypes.AUTO_ORIGIN_Y;
		graph.yAxis.AxisUseNonTickPercent = true;
		WMG_Anim.animVec2(()=> graph.theOrigin, x=> graph.theOrigin = x, animDuration, new Vector2(graph.xAxis.AxisMaxValue, graph.theOrigin.y), Ease.Linear);

		if (!noTestDelay) yield return waitTime;
		graph.axesType = WMG_Axis_Graph.axesTypes.AUTO_ORIGIN;
		WMG_Anim.animVec2(()=> graph.theOrigin, x=> graph.theOrigin = x, animDuration, new Vector2(graph.xAxis.AxisMaxValue / 4, graph.yAxis.AxisMaxValue / 2), Ease.Linear);

		if (!noTestDelay) yield return waitTime;
		graph.axesType = WMG_Axis_Graph.axesTypes.I;

		if (!noTestDelay) yield return waitTime;
	}
	
	IEnumerator addDeleteTests() {
		WMG_Series s1 = graph.lineSeries[0].GetComponent<WMG_Series>();
		WMG_Series s2 = graph.lineSeries[1].GetComponent<WMG_Series>();
		List<Vector2> s1Data = s1.pointValues.list;
		List<Vector2> s2Data = s2.pointValues.list;
		Color s1PointColor = s1.pointColor;
		Color s2PointColor = s2.pointColor;
		float barWidth = graph.barWidth;

		addSeriesWithRandomData();

		if (!noTestDelay) yield return waitTime;
		graph.deleteSeries();

		if (!noTestDelay) yield return waitTime;
		addSeriesWithRandomData();

		if (!noTestDelay) yield return waitTime;
		addSeriesWithRandomData();

		if (!noTestDelay) yield return waitTime;
		graph.deleteSeries();
		
		if (!noTestDelay) yield return waitTime;
		graph.deleteSeries();

		if (!noTestDelay) yield return waitTime;
		graph.deleteSeries();

		if (!noTestDelay) yield return waitTime;
		graph.deleteSeries();

		if (!noTestDelay) yield return waitTime;
		addSeriesWithRandomData();

		if (!noTestDelay) yield return waitTime;
		graph.deleteSeries();

		if (!noTestDelay) yield return waitTime;
		addSeriesWithRandomData();
		graph.lineSeries[0].GetComponent<WMG_Series>().pointValues.SetList(s1Data);
		graph.lineSeries[0].GetComponent<WMG_Series>().pointColor = s1PointColor;

		if (!noTestDelay) yield return waitTime;
		addSeriesWithRandomData();
		graph.lineSeries[1].GetComponent<WMG_Series>().pointValues.SetList(s2Data);
		graph.lineSeries[1].GetComponent<WMG_Series>().pointColor = s2PointColor;
		graph.lineSeries[1].GetComponent<WMG_Series>().pointPrefab = 1;
		graph.barWidth = barWidth;
	}

	void addSeriesWithRandomData() {

		WMG_Series series = graph.addSeries();
		series.UseXDistBetweenToSpace = true;
		series.lineScale = 0.5f;
		series.pointColor = new Color(Random.Range(0, 1f),Random.Range(0, 1f),Random.Range(0, 1f),1);
		series.seriesName = "Series " + graph.lineSeries.Count;
		series.pointValues.SetList(graph.GenRandomY(graph.groups.Count, 1, graph.groups.Count, graph.yAxis.AxisMinValue, graph.yAxis.AxisMaxValue));
		// only need to do this for the Resize Tests - Resize Content, since addSeries calls Init() which does setOriginalPropertyValues
		// since we set line scale to 0.5f which is different from the series prefab of 1, the original value for linescale would be 1 without calling setOriginalPropertyValues
		series.setOriginalPropertyValues(); 
	}

}
