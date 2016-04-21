using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WMG_X_Ring_Graph : WMG_GUI_Functions {

	public Object ringGraphPrefab;

	public bool onlyRandomizeData;

	private List<WMG_Ring_Graph> ringGraphs = new List<WMG_Ring_Graph>();

	void Start() {

		GameObject ring1go = GameObject.Instantiate(ringGraphPrefab) as GameObject;
		changeSpriteParent(ring1go, this.gameObject);
		changeSpritePositionTo(ring1go, new Vector3(-230, 175));
		WMG_Ring_Graph ring1 = ring1go.GetComponent<WMG_Ring_Graph>();
		ring1.Init();
		ring1.values.Add(180);
		ring1.values.Add(335);
		ring1.leftRightPadding = new Vector2(60,60);
		ring1.topBotPadding = new Vector2(50,0);
		ring1.innerRadiusPercentage = 0.3f;
		ring1.ringPointWidthFactor = 20;
		changeSpriteSize(ring1go, 420, 350);
		ringGraphs.Add(ring1);

		GameObject ring2go = GameObject.Instantiate(ringGraphPrefab) as GameObject;
		changeSpriteParent(ring2go, this.gameObject);
		changeSpritePositionTo(ring2go, new Vector3(200, 115));
		WMG_Ring_Graph ring2 = ring2go.GetComponent<WMG_Ring_Graph>();
		ring2.Init();
		ring2.degrees = 180;
		ring2.leftRightPadding = new Vector2(60,60);
		ring2.topBotPadding = new Vector2(50,-120);
		ring2.innerRadiusPercentage = 0.3f;
		ring2.ringPointWidthFactor = 20;
		changeSpriteSize(ring2go, 420, 230);
		ringGraphs.Add(ring2);

		GameObject ring3go = GameObject.Instantiate(ringGraphPrefab) as GameObject;
		changeSpriteParent(ring3go, this.gameObject);
		changeSpritePositionTo(ring3go, new Vector3(-230, -180));
		WMG_Ring_Graph ring3 = ring3go.GetComponent<WMG_Ring_Graph>();
		ring3.Init();
		ring3.degrees = 0;
		ring3.leftRightPadding = new Vector2(60,60);
		ring3.topBotPadding = new Vector2(50,50);
		ring3.innerRadiusPercentage = 0.3f;
		ring3.ringPointWidthFactor = 20;
		changeSpriteSize(ring3go, 370, 350);
		ringGraphs.Add(ring3);

		GameObject ring4go = GameObject.Instantiate(ringGraphPrefab) as GameObject;
		changeSpriteParent(ring4go, this.gameObject);
		changeSpritePositionTo(ring4go, new Vector3(200, -180));
		WMG_Ring_Graph ring4 = ring4go.GetComponent<WMG_Ring_Graph>();
		ring4.degrees = 0;
		ring4.Init();
		ring4.leftRightPadding = new Vector2(60,60);
		ring4.topBotPadding = new Vector2(50,50);
		ring4.innerRadiusPercentage = 0.3f;
		ring4.ringPointWidthFactor = 20;
		ring4.bandMode = false;
		changeSpriteSize(ring4go, 370, 350);
		ringGraphs.Add(ring4);
	}

	public void randomize() {
		for (int i = 0; i < ringGraphs.Count; i++) {
			int numRings = ringGraphs[i].values.Count;
			if (!onlyRandomizeData) numRings = Random.Range(1,6);
			ringGraphs[i].values.SetList(ringGraphs[i].GenRandomList(numRings, ringGraphs[i].minValue, ringGraphs[i].maxValue));
			if (!onlyRandomizeData) {
				ringGraphs[i].bandMode = (1 == Random.Range(0,2));
				if (i == 3) {
					ringGraphs[i].degrees = Random.Range(0, 180);
					ringGraphs[i].topBotPadding = new Vector2(-ringGraphs[i].outerRadius * (1 - Mathf.Cos(ringGraphs[i].degrees/2 * Mathf.Deg2Rad)) + 50,
					                                          -ringGraphs[i].outerRadius * (1 - Mathf.Cos(ringGraphs[i].degrees/2 * Mathf.Deg2Rad)) + 50);
				}
			}
		}
	}

	public void dataOnlyChanged() {
		onlyRandomizeData = !onlyRandomizeData;
	}
	
}
