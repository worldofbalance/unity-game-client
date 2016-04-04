using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// Contains GUI system dependent functions

public class WMG_Graph_Tooltip : WMG_GUI_Functions {

	public delegate string TooltipLabeler(WMG_Series series, WMG_Node node);
	public TooltipLabeler tooltipLabeler;
	
	public WMG_Axis_Graph theGraph;

	Canvas _canvas;
	
	void Start() {
		_canvas = theGraph.toolTipPanel.GetComponent<Graphic>().canvas;
	}

	void Update () {
		if (theGraph.tooltipEnabled) {
			if (isTooltipObjectNull()) return;
			if(getControlVisibility(theGraph.toolTipPanel)) {
				repositionTooltip();
			}
		}
	}

	public void subscribeToEvents(bool val) {
		if (val) {
			theGraph.WMG_MouseEnter += TooltipNodeMouseEnter;
			theGraph.WMG_MouseEnter_Leg += TooltipLegendNodeMouseEnter;
			theGraph.WMG_Link_MouseEnter_Leg += TooltipLegendLinkMouseEnter;
			tooltipLabeler = defaultTooltipLabeler;
		}
		else {
			theGraph.WMG_MouseEnter -= TooltipNodeMouseEnter;
			theGraph.WMG_MouseEnter_Leg -= TooltipLegendNodeMouseEnter;
			theGraph.WMG_Link_MouseEnter_Leg -= TooltipLegendLinkMouseEnter;
		}
	}
	
	private bool isTooltipObjectNull() {
		if (theGraph.toolTipPanel == null) return true;
		if (theGraph.toolTipLabel == null) return true;
		return false;
	}
	
	private void repositionTooltip() {
		// This is called continuously during update if control is visible, and also once before shown visible so tooltip doesn't appear to jump positions
		// Convert position from "screen coordinates" to "gui coordinates"

		Vector3 position;
		RectTransformUtility.ScreenPointToWorldPointInRectangle(theGraph.toolTipPanel.GetComponent<RectTransform>(), 
		                                                        new Vector2(Input.mousePosition.x, Input.mousePosition.y),
		                                                        (_canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : _canvas.worldCamera),
		                                                        out position);
		// Without offset, the tooltip's bottom left corner will be at the cursor position
		float offsetX = theGraph.tooltipOffset.x;
		float offsetY = theGraph.tooltipOffset.y;
		// Center the control on the mouse/touch
		theGraph.toolTipPanel.transform.localPosition = theGraph.toolTipPanel.transform.parent.InverseTransformPoint(position) + new Vector3( offsetX, offsetY + 13, 0);

		EnsureTooltipStaysOnScreen(position, offsetX, offsetY);
	}

	void EnsureTooltipStaysOnScreen(Vector3 position, float offsetX, float offsetY) {
		Vector3 newPos = theGraph.toolTipPanel.transform.position;

		Vector3[] corners = new Vector3[4];
		((RectTransform) theGraph.toolTipPanel.transform).GetWorldCorners(corners);
		var width = corners[2].x - corners[0].x;
		var height = corners[1].y - corners[0].y;
		
		float distPastX = position.x + offsetX + width - Screen.width;
		if (distPastX > 0) { // passed right edge
			newPos = new Vector3(position.x - distPastX + offsetX, newPos.y, newPos.z);
		}
		else {
			distPastX = position.x + offsetX;
			if (distPastX < 0) { // passed left edge
				newPos = new Vector3(position.x - distPastX + offsetX, newPos.y, newPos.z);
			}
		}
		float distPastY = position.y + offsetY + height - Screen.height;
		if (distPastY > 0) { // passed top edge
			newPos = new Vector3(newPos.x, position.y - distPastY + offsetY + height/2f, newPos.z);
		}
		else {
			distPastY = position.y + offsetY;
			if (distPastY < 0) { // passed bottom edge
				newPos = new Vector3(newPos.x, position.y - distPastY + offsetY + height/2f, newPos.z);
			}
		}
		
		theGraph.toolTipPanel.transform.position = newPos;
	}
	
	private void TooltipNodeMouseEnter(WMG_Series aSeries, WMG_Node aNode, bool state) {
		if (isTooltipObjectNull()) return;
		if (state) {
			// Set tooltip text
			changeLabelText(theGraph.toolTipLabel, tooltipLabeler(aSeries, aNode));
			
			// Resize this control to match the size of the contents
			changeSpriteWidth(theGraph.toolTipPanel, Mathf.RoundToInt(getSpriteWidth(theGraph.toolTipLabel)) + 24);
			
			// Ensure tooltip is in position before showing it so it doesn't appear to jump
			repositionTooltip();
			
			// Display the base panel
			showControl(theGraph.toolTipPanel);
			bringSpriteToFront(theGraph.toolTipPanel);
			
			Vector3 newVec = new Vector3(2,2,1);
			if (!aSeries.seriesIsLine) {
				if (theGraph.orientationType == WMG_Axis_Graph.orientationTypes.vertical) {
					newVec = new Vector3(1,1.1f,1);
				}
				else {
					newVec = new Vector3(1.1f,1,1);
				}
			}
			
			performTooltipAnimation(aNode.transform, newVec);
		}
		else {
			hideControl(theGraph.toolTipPanel);
			sendSpriteToBack(theGraph.toolTipPanel);
			
			performTooltipAnimation(aNode.transform, new Vector3(1,1,1));
		}
	}

	private string defaultTooltipLabeler(WMG_Series aSeries, WMG_Node aNode) {
		// Find out the point value data for this node
		Vector2 nodeData = aSeries.getNodeValue(aNode);
		float numberToMult = Mathf.Pow(10f, aSeries.theGraph.tooltipNumberDecimals);
		string nodeX = (Mathf.Round(nodeData.x*numberToMult)/numberToMult).ToString();
		string nodeY = (Mathf.Round(nodeData.y*numberToMult)/numberToMult).ToString();
		
		// Determine the tooltip text to display
		string textToSet;
		if (aSeries.seriesIsLine) {
			textToSet = "(" + nodeX + ", " + nodeY + ")";
		}
		else {
			textToSet = nodeY;
		}
		if (aSeries.theGraph.tooltipDisplaySeriesName) {
			textToSet = aSeries.seriesName + ": " + textToSet;
		}
		return textToSet;
	}
	
	private void TooltipLegendNodeMouseEnter(WMG_Series aSeries, WMG_Node aNode, bool state) {
		if (isTooltipObjectNull()) return;
		if (state) {
			// Set the text
			changeLabelText(theGraph.toolTipLabel, aSeries.seriesName);
			
			// Resize this control to match the size of the contents
			changeSpriteWidth(theGraph.toolTipPanel, Mathf.RoundToInt(getSpriteWidth(theGraph.toolTipLabel)) + 24);
			
			// Ensure tooltip is in position before showing it so it doesn't appear to jump
			repositionTooltip();
			
			// Display the base panel
			showControl(theGraph.toolTipPanel);
			bringSpriteToFront(theGraph.toolTipPanel);
			
			performTooltipAnimation(aNode.transform, new Vector3(2,2,1));
		}
		else {
			hideControl(theGraph.toolTipPanel);
			sendSpriteToBack(theGraph.toolTipPanel);
			
			performTooltipAnimation(aNode.transform, new Vector3(1,1,1));
		}
	}
	
	private void TooltipLegendLinkMouseEnter(WMG_Series aSeries, WMG_Link aLink, bool state) {
		if (isTooltipObjectNull()) return;
		if (!aSeries.hidePoints) return;
		if (state) {
			// Set the text
			changeLabelText(theGraph.toolTipLabel, aSeries.seriesName);
			
			// Resize this control to match the size of the contents
			changeSpriteWidth(theGraph.toolTipPanel, Mathf.RoundToInt(getSpriteWidth(theGraph.toolTipLabel)) + 24);
			
			// Ensure tooltip is in position before showing it so it doesn't appear to jump
			repositionTooltip();
			
			// Display the base panel
			showControl(theGraph.toolTipPanel);
			bringSpriteToFront(theGraph.toolTipPanel);
			
			performTooltipAnimation(aLink.transform, new Vector3(2,1.05f,1));
		}
		else {
			hideControl(theGraph.toolTipPanel);
			sendSpriteToBack(theGraph.toolTipPanel);
			
			performTooltipAnimation(aLink.transform, new Vector3(1,1,1));
		}
	}
	
	private void performTooltipAnimation (Transform trans, Vector3 newScale) {
		if (theGraph.tooltipAnimationsEnabled) {
			WMG_Anim.animScale(trans.gameObject, theGraph.tooltipAnimationsDuration, theGraph.tooltipAnimationsEasetype, newScale, 0);
		}
	}
}
