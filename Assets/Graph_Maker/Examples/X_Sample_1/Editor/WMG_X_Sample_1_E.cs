using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(WMG_X_Sample_1))]
public class WMG_X_Sample_1_E : WMG_E_Util
{
	WMG_X_Sample_1 script;
	Dictionary<string, WMG_PropertyField> fields;
	
	void OnEnable()
	{
		script = (WMG_X_Sample_1)target;
		fields = GetProperties(script);
	}
	
	public override void OnInspectorGUI()
	{
		// Update the serializedProperty - always do this in the beginning of OnInspectorGUI.
		serializedObject.Update();
		
		DrawCore();							
		
		if( GUI.changed ) {
			EditorUtility.SetDirty( script );
		}
		
		// Apply changes to the serializedProperty - always do this in the end of OnInspectorGUI.
		serializedObject.ApplyModifiedProperties();
	}
	
	void DrawCore() {
		script.emptyGraphPrefab = EditorGUILayout.ObjectField("Empty Graph Prefab", script.emptyGraphPrefab, typeof(Object), false);
		script.plotOnStart = EditorGUILayout.Toggle("Plot On Start", script.plotOnStart);
		ExposeProperty(fields["plottingData"]);
		script.plotIntervalSeconds = EditorGUILayout.FloatField("Plot Interval Seconds", script.plotIntervalSeconds);
		script.plotAnimationSeconds = EditorGUILayout.FloatField("Plot Animation Seconds", script.plotAnimationSeconds);
		script.xInterval = EditorGUILayout.FloatField("X Interval", script.xInterval);
		script.useAreaShading = EditorGUILayout.Toggle("Use Area Shading", script.useAreaShading);
		script.blinkCurrentPoint = EditorGUILayout.Toggle("Blink Current Point", script.blinkCurrentPoint);
		script.blinkAnimDuration = EditorGUILayout.FloatField("Blink Anim Duration", script.blinkAnimDuration);
		script.moveXaxisMinimum = EditorGUILayout.Toggle("Move xAxis Minimum", script.moveXaxisMinimum);
		script.indicatorPrefab = EditorGUILayout.ObjectField("Indicator Prefab", script.indicatorPrefab, typeof(Object), false);
		script.indicatorNumDecimals = EditorGUILayout.IntField("Indicator Num Decimals", script.indicatorNumDecimals);
	}
	

}
