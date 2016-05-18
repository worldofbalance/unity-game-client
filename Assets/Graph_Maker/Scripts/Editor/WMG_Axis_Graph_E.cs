using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using DG.Tweening;

[CustomEditor(typeof(WMG_Axis_Graph))]
public class WMG_Axis_Graph_E : WMG_E_Util
{
	WMG_Axis_Graph graph;
	Dictionary<string, WMG_PropertyField> fields;
	SerializedObject yAxisSO;
	SerializedObject xAxisSO;
	Dictionary<string, WMG_PropertyField> yAxisfields;
	Dictionary<string, WMG_PropertyField> xAxisfields;

	enum eTabType
	{
		Core,
		YAxis,
		XAxis,
		Tooltip,
		Anim,
		Misc
	}

	private eTabType m_tabType = eTabType.Core;

	public void Init() {
		graph = (WMG_Axis_Graph)target;
		fields = GetProperties(graph);
		yAxisSO = new SerializedObject(serializedObject.FindProperty("yAxis").objectReferenceValue);
		xAxisSO = new SerializedObject(serializedObject.FindProperty("xAxis").objectReferenceValue);
		yAxisfields = GetProperties(graph.yAxis);
		xAxisfields = GetProperties(graph.xAxis);
	}

	void OnEnable()
	{
		Init();
	}

	public override void OnInspectorGUI()
	{
		// Update the serializedProperty - always do this in the beginning of OnInspectorGUI.
		serializedObject.Update();
		yAxisSO.Update();
		xAxisSO.Update();

		string[] toolBarButtonNames = System.Enum.GetNames(typeof(eTabType));
		
		m_tabType = (eTabType)GUILayout.Toolbar((int)m_tabType, toolBarButtonNames);

		switch (m_tabType)
		{
		case eTabType.Core: DrawCore(); break;
		case eTabType.YAxis: DrawYAxis(); break;
		case eTabType.XAxis: DrawXAxis(); break;
		case eTabType.Tooltip: DrawTooltip(); break;
		case eTabType.Anim: DrawAnim(); break;
		case eTabType.Misc: DrawMisc(); break;
		}								

		// In editor mode, update graphics based on graph width and height
		if (!Application.isPlaying) {
			UpdateSceneView();
		}

		if( GUI.changed ) {
			EditorUtility.SetDirty( graph );
			EditorUtility.SetDirty( yAxisSO.targetObject);
			EditorUtility.SetDirty( xAxisSO.targetObject);
		}
		
		// Apply changes to the serializedProperty - always do this in the end of OnInspectorGUI.
		serializedObject.ApplyModifiedProperties();
		yAxisSO.ApplyModifiedProperties();
		xAxisSO.ApplyModifiedProperties();
	}

	void UpdateSceneView() {
		Vector2 newSize = graph.getSpriteSize(graph.gameObject);
		graph.changeSpriteSize(graph.graphBackground, Mathf.RoundToInt(newSize.x), Mathf.RoundToInt(newSize.y));
		graph.changeSpritePositionToX(graph.graphBackground, -graph.paddingLeftRight.x);
		graph.changeSpritePositionToY(graph.graphBackground, -graph.paddingTopBottom.y);
		graph.UpdateBGandSeriesParentPositions(newSize.x, newSize.y);
		// Update axes lines
		int newX = Mathf.RoundToInt(newSize.x - graph.paddingLeftRight.x - graph.paddingLeftRight.y + graph.xAxis.AxisLinePadding);
		if (newX < 0) newX = 0;
		graph.changeSpriteSize(graph.xAxis.AxisLine, newX, graph.axisWidth);
		graph.changeSpritePositionToX(graph.xAxis.AxisLine, newX / 2f);
		int newY = Mathf.RoundToInt(newSize.y - graph.paddingTopBottom.x - graph.paddingTopBottom.y + graph.yAxis.AxisLinePadding);
		if (newY < 0) newY = 0;
		graph.changeSpriteSize(graph.yAxis.AxisLine, graph.axisWidth, newY);
		graph.changeSpritePositionToY(graph.yAxis.AxisLine, newY / 2f);
	}
	
	void DrawCore() {
		ExposeProperty(fields["graphType"]);
		ExposeProperty(fields["orientationType"]);
		ExposeProperty(fields["axesType"]);
		ExposeProperty(fields["resizeEnabled"]);
		ExposeEnumMaskProperty(fields["resizeProperties"]);
		ExposeProperty(fields["useGroups"]);
		ArrayGUIoc<string> (graph.groups, "Groups", "_groups");
		ArrayGUI("Series", "lineSeries");
		ExposeProperty(fields["paddingLeftRight"]);
		ExposeProperty(fields["paddingTopBottom"]);
		ExposeProperty(fields["theOrigin"]);
		ExposeProperty(fields["barWidth"]);
		ExposeProperty(fields["barAxisValue"]);
		ExposeProperty(fields["autoUpdateOrigin"]);
		ExposeProperty(fields["autoUpdateBarWidth"]);
		ExposeProperty(fields["autoUpdateBarWidthSpacing"]);
		ExposeProperty(fields["autoUpdateSeriesAxisSpacing"]);
		ExposeProperty(fields["autoUpdateBarAxisValue"]);
		ExposeProperty(fields["autoFitLabels"]);
		ExposeProperty(fields["autoFitPadding"]);
	}

	void DrawYAxis() {
		ExposeProperty(yAxisfields["AxisMinValue"]);
		ExposeProperty(yAxisfields["AxisMaxValue"]);
		ExposeProperty(yAxisfields["AxisNumTicks"]);
		ExposeProperty(yAxisfields["MinAutoGrow"]);
		ExposeProperty(yAxisfields["MaxAutoGrow"]);
		ExposeProperty(yAxisfields["MinAutoShrink"]);
		ExposeProperty(yAxisfields["MaxAutoShrink"]);
		ExposeProperty(yAxisfields["AxisLinePadding"]);
		ExposeProperty(yAxisfields["hideGrid"]);
		ExposeProperty(yAxisfields["hideTicks"]);
		ExposeProperty(yAxisfields["AxisTitleString"]);
		ExposeProperty(yAxisfields["AxisTitleOffset"]);
		ExposeProperty(yAxisfields["AxisTitleFontSize"]);
		ExposeProperty(yAxisfields["AxisUseNonTickPercent"]);

		EditorGUILayout.Separator();
		EditorGUILayout.LabelField("Label Parameters", EditorStyles.boldLabel);

		ExposeProperty(yAxisfields["LabelType"]);
		ArrayGUIoc<string> (((WMG_Axis)yAxisSO.targetObject).axisLabels, "Labels", "_axisLabels", yAxisSO);
		ExposeProperty(yAxisfields["AxisLabelSkipInterval"]);
		ExposeProperty(yAxisfields["AxisLabelSkipStart"]);
		ExposeProperty(yAxisfields["AxisLabelRotation"]);
		ExposeProperty(yAxisfields["SetLabelsUsingMaxMin"]);
		ExposeProperty(yAxisfields["AxisLabelSize"]);
		ExposeProperty(yAxisfields["numDecimalsAxisLabels"]);
		ExposeProperty(yAxisfields["hideLabels"]);
		ExposeProperty(yAxisfields["AxisLabelSpaceOffset"]);
		ExposeProperty(yAxisfields["autoFitRotation"]);
		ExposeProperty(yAxisfields["autoFitMaxBorder"]);

		EditorGUILayout.Separator();
		EditorGUILayout.LabelField("Manual Axes Type Parameters", EditorStyles.boldLabel);
		ExposeProperty(yAxisfields["AxisNonTickPercent"]);
		ExposeProperty(yAxisfields["AxisArrowTopRight"]);
		ExposeProperty(yAxisfields["AxisArrowBotLeft"]);
		ExposeProperty(yAxisfields["AxisTicksRightAbove"]);
		ExposeProperty(yAxisfields["AxisTick"]);
		ExposeProperty(yAxisfields["hideTick"]);

		EditorGUILayout.Separator();
		EditorGUILayout.LabelField("Manual Label Type Parameters", EditorStyles.boldLabel);
		ExposeProperty(yAxisfields["AxisLabelSpacing"]);
		ExposeProperty(yAxisfields["AxisLabelDistBetween"]);
	}

	void DrawXAxis() {
		ExposeProperty(xAxisfields["AxisMinValue"]);
		ExposeProperty(xAxisfields["AxisMaxValue"]);
		ExposeProperty(xAxisfields["AxisNumTicks"]);
		ExposeProperty(xAxisfields["MinAutoGrow"]);
		ExposeProperty(xAxisfields["MaxAutoGrow"]);
		ExposeProperty(xAxisfields["MinAutoShrink"]);
		ExposeProperty(xAxisfields["MaxAutoShrink"]);
		ExposeProperty(xAxisfields["AxisLinePadding"]);
		ExposeProperty(xAxisfields["hideGrid"]);
		ExposeProperty(xAxisfields["hideTicks"]);
		ExposeProperty(xAxisfields["AxisTitleString"]);
		ExposeProperty(xAxisfields["AxisTitleOffset"]);
		ExposeProperty(xAxisfields["AxisTitleFontSize"]);
		ExposeProperty(xAxisfields["AxisUseNonTickPercent"]);
		
		EditorGUILayout.Separator();
		EditorGUILayout.LabelField("Label Parameters", EditorStyles.boldLabel);
		
		ExposeProperty(xAxisfields["LabelType"]);
		ArrayGUIoc<string> (((WMG_Axis)xAxisSO.targetObject).axisLabels, "Labels", "_axisLabels", xAxisSO);
		ExposeProperty(xAxisfields["AxisLabelSkipInterval"]);
		ExposeProperty(xAxisfields["AxisLabelSkipStart"]);
		ExposeProperty(xAxisfields["AxisLabelRotation"]);
		ExposeProperty(xAxisfields["SetLabelsUsingMaxMin"]);
		ExposeProperty(xAxisfields["AxisLabelSize"]);
		ExposeProperty(xAxisfields["numDecimalsAxisLabels"]);
		ExposeProperty(xAxisfields["hideLabels"]);
		ExposeProperty(xAxisfields["AxisLabelSpaceOffset"]);
		ExposeProperty(xAxisfields["autoFitRotation"]);
		ExposeProperty(xAxisfields["autoFitMaxBorder"]);
		
		EditorGUILayout.Separator();
		EditorGUILayout.LabelField("Manual Axes Type Parameters", EditorStyles.boldLabel);
		ExposeProperty(xAxisfields["AxisNonTickPercent"]);
		ExposeProperty(xAxisfields["AxisArrowTopRight"]);
		ExposeProperty(xAxisfields["AxisArrowBotLeft"]);
		ExposeProperty(xAxisfields["AxisTicksRightAbove"]);
		ExposeProperty(xAxisfields["AxisTick"]);
		ExposeProperty(xAxisfields["hideTick"]);
		
		EditorGUILayout.Separator();
		EditorGUILayout.LabelField("Manual Label Type Parameters", EditorStyles.boldLabel);
		ExposeProperty(xAxisfields["AxisLabelSpacing"]);
		ExposeProperty(xAxisfields["AxisLabelDistBetween"]);
	}

	void DrawTooltip() {
		ExposeProperty(fields["tooltipEnabled"]);
		graph.tooltipOffset = EditorGUILayout.Vector2Field("Offset", graph.tooltipOffset);
		graph.tooltipNumberDecimals = EditorGUILayout.IntField("Number Decimals", graph.tooltipNumberDecimals);
		graph.tooltipDisplaySeriesName = EditorGUILayout.Toggle ("Display Series Name", graph.tooltipDisplaySeriesName);
	}

	void DrawAnim() {
		graph.tooltipAnimationsEnabled = EditorGUILayout.Toggle ("Tooltip Animations Enabled", graph.tooltipAnimationsEnabled);
		graph.tooltipAnimationsEasetype = (Ease)EditorGUILayout.EnumPopup("Tooltip Animations Easetype", graph.tooltipAnimationsEasetype);
		graph.tooltipAnimationsDuration = EditorGUILayout.FloatField("Tooltip Animations Duration", graph.tooltipAnimationsDuration);
		ExposeProperty(fields["autoAnimationsEnabled"]);
		graph.autoAnimationsEasetype = (Ease)EditorGUILayout.EnumPopup("Auto Animations Easetype", graph.autoAnimationsEasetype);
		graph.autoAnimationsDuration = EditorGUILayout.FloatField("Auto Animations Duration", graph.autoAnimationsDuration);
	}

	void DrawMisc() {
		ExposeProperty(fields["axisWidth"]);
		ExposeProperty(fields["autoShrinkAtPercent"]);
		ExposeProperty(fields["autoGrowAndShrinkByPercent"]);
		ArrayGUI("Point Prefabs", "pointPrefabs");
		ArrayGUI("Link Prefabs", "linkPrefabs");
		graph.barPrefab = EditorGUILayout.ObjectField("Bar Prefab", graph.barPrefab, typeof(Object), false);
		graph.seriesPrefab = EditorGUILayout.ObjectField("Series Prefab", graph.seriesPrefab, typeof(Object), false);
		ExposeProperty(fields["tickSize"]);
		ExposeProperty(fields["graphTitleString"]);
		ExposeProperty(fields["graphTitleOffset"]);
		graph.yAxis = (WMG_Axis)EditorGUILayout.ObjectField("Y Axis", graph.yAxis, typeof(WMG_Axis), true);
		graph.xAxis = (WMG_Axis)EditorGUILayout.ObjectField("X Axis", graph.xAxis, typeof(WMG_Axis), true);
		graph.legend = (WMG_Legend)EditorGUILayout.ObjectField("Legend", graph.legend, typeof(WMG_Legend), true);
		graph.anchoredParent = (GameObject)EditorGUILayout.ObjectField("Anchored Parent", graph.anchoredParent, typeof(GameObject), true);
		graph.graphBackground = (GameObject)EditorGUILayout.ObjectField("Graph Background", graph.graphBackground, typeof(GameObject), true);
		graph.seriesParent = (GameObject)EditorGUILayout.ObjectField("Series Parent", graph.seriesParent, typeof(GameObject), true);
	}
	

}