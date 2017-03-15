using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using System.Globalization;

public class Graph : MonoBehaviour {

	private int window_id = Constants.GRAPH_WIN2;
	// MakeDayCounts parameters
	private const int NUM_YEARS = 50;
	private const int START_YEAR = 2015;
	private int[] dayCount = new int[NUM_YEARS * 12];
	private int[] months = new int[] {31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31};
	// Species Biomass parameters
	private Dictionary<int,int> biomassValues;
	private List<int> speciesIds;
	private List<Dictionary<int,int>> biomassHistory;
	int minDay, maxDay;
	int maxMonth, minMonth;
	int speciesCount;
	private Dictionary<int, SpeciesData> speciesTable = SpeciesTable.speciesList;

	// Window Properties
	private float left;
	private float top;
	private float width = 805;
	private float height = 430;   // 2017-3-15  was 420
	public bool isActive = false;
	private bool isReady = false;
	private bool isLegendActive = true;
	private bool buttonActive = true;
	// Other
	private string title;
	private Rect windowRect;
	private List<string> xAxisLabels = new List<string>();
	private int xRange;
	private int yRange;
	private List<string> seriesLabels = new List<string>();
	private int xMin;
	private int xMax;
	private List<string> excludeList = new List<string>();
	private Texture2D lineMarkerTex;
	private Rect graphRect;
	private Rect monthSliderRect;
	private Rect legendRect, buttonRect;
	private Rect legendScrollRect;
	private Vector2 legendScrollPos = Vector2.zero;
	private bool isLegendContentHidden = false;
	private float thickness = 2f;
	private Vector2 hStart;
	private Vector2 hEnd;
	private int xNumMarkers = 12;
	private int yNumMarkers = 5;
	public string xTitle { get; set; }
	public string yTitle { get; set; }
	private float xAxisLength;
	private float xUnitLength;
	private float yAxisLength;
	private float yUnitLength;
	private Vector2 vStart;
	private Vector2 vEnd;
	private string selected;
	private Vector3 lastMousePosition = Vector3.zero;
	private List<string> mouseOverLabels = new List<string>();
	private string lastSeriesToDraw;
	private float monthSliderValue;
	private float monthSliderStart;
	private float monthSliderDT;
	private float monthSliderMax;
	private int scrollToMonth = -1;
	private Dictionary<string, Series> seriesList = new Dictionary<string, Series>();
	private Texture2D bgTexture;
	private Font font;
	private int zoom = 1;
	private int lastGridX;
	
	void Awake() {
		title = "Graph";
		xTitle = "Day";
		yTitle = "Score";
		yTitle = "Biomass";

		left = (Screen.width - width) / 2;
		top = 100;

		windowRect = new Rect(left, top, width, height);
		graphRect = new Rect(20, 30, 550, 325);
		monthSliderRect = new Rect(graphRect.x, graphRect.x + graphRect.height + 20, graphRect.width, 30);
		legendRect = new Rect(width - 200 - 20, 30, 200, 325);
		buttonRect = new Rect(width - 200 - 20, 365, 200, 35);  
		legendScrollRect = new Rect(legendRect.width * 0.05f, 40, legendRect.width * 0.9f, legendRect.height * 0.75f);

		hStart = new Vector2(85, graphRect.height - 75);
		hEnd = new Vector2(graphRect.width - 50, hStart.y);
		
		xAxisLength = Vector2.Distance(hStart, hEnd) * 0.95f;
		xUnitLength = xAxisLength / xNumMarkers;
		
		vStart = new Vector2(hStart.x, hStart.y);
		vEnd = new Vector2(vStart.x, 30);

		yAxisLength = Vector2.Distance(vStart, vEnd) * 0.95f;
		yUnitLength = yAxisLength / yNumMarkers;

		lineMarkerTex = Resources.Load<Texture2D>("chart_dot");

		bgTexture = Resources.Load<Texture2D>(Constants.THEME_PATH + Constants.ACTIVE_THEME + "/gui_bg");
		font = Resources.Load<Font>("Fonts/" + "Chalkboard");
		MakeDayCounts ();
	}

	// Use this for initialization
	void Start() {
		// UpdateData();
//		StartCoroutine(UpdateDataRoutine(10f));
	}

	// Update is called once per frame
	void Update() {
		if (isActive && Input.GetKeyDown("1")) {
			GetData ();
		}
		/* These have a bug with the new zoom feature. They need to be fixed.  
		if (isActive && Input.GetKeyDown("2")) {
			ShowMonth(1);
		}
		if (isActive && Input.GetKeyDown("3")) {
			ShowLastMonth();
		}
		*/
		if (Input.GetKeyDown(KeyCode.UpArrow) && isActive) {
			ZoomIn();
		}
		if (Input.GetKeyDown (KeyCode.DownArrow) && isActive) {
			ZoomOut();
		}
	}


	void ZoomIn() {
		if (zoom > 1) {
			zoom /= 2;
			monthSliderValue = 0;
			xMin = 0;
		}
	}


	void ZoomOut() {
		if ((xAxisLabels != null) && (xAxisLabels.Count / zoom > xNumMarkers)) {
			zoom *= 2;
			monthSliderValue = 0;
			xMin = 0;
		}
	}
		
	
	void OnGUI() {
		if (buttonActive) {
			if (GUI.Button(new Rect(200, Screen.height - 100f, 80, 30), "Graph")) {
				ToggleGraph();
			}
		}

		if (isActive) {
			windowRect = GUI.Window(window_id, windowRect, MakeWindow, title);
		}
	}

	void ToggleGraph() {
		isActive = !isActive;
		if (isActive) {
			GameObject.Find ("MenuScript").GetComponent<MenuScript> ().menuOpen = true;
			GameObject.Find ("MenuScript").GetComponent<MenuScript> ().disableDropDown ();
			GameObject.Find ("Local Object").GetComponent<WorldMouse> ().popOversEnabled = false;
			GetData ();
		} else {
			GameObject.Find ("MenuScript").GetComponent<MenuScript> ().menuOpen = false;
			GameObject.Find ("MenuScript").GetComponent<MenuScript> ().enableDropdown ();
			GameObject.Find ("Local Object").GetComponent<WorldMouse> ().popOversEnabled = true;
		}
	}

	
	void MakeWindow(int id) {
		Functions.DrawBackground(new Rect(0, 0, width, height), bgTexture);
		
		GUIStyle style = new GUIStyle(GUI.skin.label);
		style.alignment = TextAnchor.UpperCenter;
		style.font = font;
		style.fontSize = 16;

		if (!isReady) {
			style.fontSize = 20;
			GUI.Label(new Rect((windowRect.width - 500) / 2, windowRect.height/2-25, 500, 40), "Please wait while graph prepares", style);
			return;
		}

		GUI.Label(new Rect((windowRect.width - 100) / 2, 0, 100, 30), "Graph", style);

		DrawGrid();

		// "Collision" Detection for Lines/Markers
		if (mouseOverLabels.Count > 0) {
			// Solve Stacking Issue by Selecting Topmost
			if (mouseOverLabels.Contains(lastSeriesToDraw)) {
				selected = lastSeriesToDraw;
			} else {
				selected = mouseOverLabels[0]; // Select 1st Collision
			}

			if (selected != lastSeriesToDraw) {
				lastSeriesToDraw = selected;
			}
			// Clear "Collisions"
			mouseOverLabels.Clear();
		} else {
			selected = null;
		}

		DrawMonthSlider();

		if (isLegendActive) {
			DrawLegend();
		}

		GUI.DragWindow();
	}
	
	public void Show() {
		isActive = true;
		GameObject.Find("MenuScript").GetComponent<MenuScript>().menuOpen = true;
		GameObject.Find("MenuScript").GetComponent<MenuScript>().disableDropDown();
		GameObject.Find("Local Object").GetComponent<WorldMouse>().popOversEnabled = false;
		GetData ();
	}
	
	public void Hide() {
		isActive = false;
		GameObject.Find ("MenuScript").GetComponent<MenuScript> ().menuOpen = false;
		GameObject.Find ("MenuScript").GetComponent<MenuScript> ().enableDropdown ();
		GameObject.Find ("Local Object").GetComponent<WorldMouse> ().popOversEnabled = true;
	}

	public void ShowButton() {
		buttonActive = true;
	}

	public void HideButton() {
		buttonActive = false;
	}

	private void DrawGrid() {
		Color color = Color.white;

		GUI.BeginGroup(graphRect, GUI.skin.box);
			GUIStyle style = new GUIStyle(GUI.skin.label);
			style.normal.textColor = Color.white;
			style.richText = true;
			
			// X-Axis
			Drawing.DrawLine(hStart, hEnd, color, thickness, false);
			// X-Axis Markers
			string tStr;
			for (int i = 0; i < xNumMarkers; i++) {
				float xPos = hStart.x + xUnitLength * (i + 1), yPos = hStart.y - 5;
				// Unit Line
				Drawing.DrawLine(new Vector2(xPos, yPos), new Vector2(xPos, yPos + 10), color, thickness, false);
				// Unit Label
				style.alignment = TextAnchor.UpperCenter;
				if ((xMin + i * zoom) < xAxisLabels.Count) {
					lastGridX = i;
					tStr = xAxisLabels [xMin + i * zoom];
					/*
					if ((i != 0) && (!tStr.Contains("Jan"))) {
						tStr = tStr.Substring (0, 3);
					}
					*/
					GUI.Label(new Rect(xPos - 42, yPos + 10, 80, 70), tStr, style);
				}

			}
			// X-Axis Label
			style.alignment = TextAnchor.UpperCenter;
			GUI.Label(new Rect(hStart.x + (xAxisLength - 80) / 2, hStart.y + 35, 80, 30), "<color=yellow>" + xTitle + "</color>", style);
			
			// Y-Axis
			Drawing.DrawLine(vStart, vEnd, color, thickness, false);
			// Y-Axis Markers
			for (int i = 0; i <= yNumMarkers; i++) {
				float xPos = vStart.x - 5, yPos = vStart.y - yUnitLength * i;
				// Unit Line
				Drawing.DrawLine(new Vector2(xPos, yPos), new Vector2(xPos + 10, yPos), color, thickness, false);
				// Unit Label
				style.alignment = TextAnchor.UpperRight;
				GUI.Label(new Rect(xPos - 85, yPos - 11, 80, 30), (i * yRange / 5).ToString(), style);
			}
			// Y-Axis Label
			style.alignment = TextAnchor.UpperCenter;
			
			Matrix4x4 matrix = GUI.matrix;
			GUIUtility.RotateAroundPivot(-90, new Vector2(hStart.x - 35, graphRect.height / 2));
			GUI.Label(new Rect(hStart.x - 50, graphRect.height / 2 - 35, 80, 30), "<color=orange>" + yTitle + "</color>", style);
			GUI.matrix = matrix;
			
			// Draw Behind Series Only
			foreach (string label in seriesLabels) {
				if (label == lastSeriesToDraw || excludeList.Contains(label)) {
					continue;
				}
				
				DrawSeries(seriesList[label]);
			}
			// Front Series Drawn Last
			if (!excludeList.Contains(lastSeriesToDraw) && (seriesList.Count > 0)) {
				DrawSeries(seriesList[lastSeriesToDraw]);
			}
		GUI.EndGroup();
	}

	private void DrawSeries(Series series) {
		if (Mathf.Abs(series.width - graphRect.width) > 0.1f) {
			series.width = Mathf.Lerp(0, graphRect.width, series.deltaTime += Time.deltaTime * 0.5f);
		}
		
		GUI.BeginGroup(series.GetRect());
			List<float> values = series.values;
			Color color = (series.label == selected) ? Color.white : series.color;
			
			if (values [xMin] >= 0) {
				// Draw First Point
				string text = ("<color=#" + series.colorHex + ">" + series.label + "</color>" + '\n' + values[xMin].ToString("F2"));
				DrawMarker(series.label, new Rect(hStart.x + xUnitLength - 7, hStart.y - (values[xMin] / yRange * yAxisLength) - 7, 14, 14), color, text);
			}

			int lastIdx = 0;
			for (int i = 1; i <= Mathf.Min(series.values.Count/zoom + 3, xNumMarkers); i++) {
				if (((xMin + (i - 1) * zoom) >= values.Count) || (values [xMin + (i - 1) * zoom] < 0)) {
					continue;
				}
				// Previous Point
				float xPos = hStart.x + xUnitLength * i, yPos = hStart.y - (values[xMin + (i - 1) * zoom] / yRange * yAxisLength);
				string text = ("<color=#" + series.colorHex + ">" + series.label + "</color>" + '\n' + values[xMin + (i - 1) * zoom].ToString("F2"));
				lastIdx = i;
				DrawMarker(series.label, new Rect(xPos - 7, yPos - 7, 14, 14), color, text);
				int j = i;
				while (((xMin + j * zoom) < series.values.Count) && (j < xNumMarkers) && (values[xMin + j * zoom] < 0)) {
					j++;
				}
				if (i == Mathf.Min(series.values.Count/zoom + 3, xNumMarkers)) {
					continue;
				}
				if (((xMin + j * zoom) < series.values.Count) && (j < xNumMarkers)) {
					// Current Point
					float xPosNext = hStart.x + xUnitLength * (j + 1), yPosNext = hStart.y - (values[xMin + j * zoom] / yRange * yAxisLength);
					// Connect the Points by Drawing Line
					Drawing.DrawLine(new Vector2(xPos, yPos), new Vector2(xPosNext, yPosNext), color, 1.5f, true);
					// Draw End Point
					text = ("<color=#" + series.colorHex + ">" + series.label + "</color>" + '\n' + values[xMin + j].ToString("F2"));
					lastIdx = j + 1;
					DrawMarker(series.label, new Rect(xPosNext - 7, yPosNext - 7, 14, 14), color, text);
				}
			}

			if (((monthSliderMax - monthSliderValue) < 0.5) && ((xMin + (lastIdx - 1) * zoom) < (values.Count - 1))) {
				int lastVIdx = values.Count - 1;
				bool found = false;
				while (!found && ((xMin + (lastIdx - 1) * zoom) < lastVIdx)) {
					if (values [lastVIdx] >= 0) {
						found = true;
					} else {
						lastVIdx--;
					}
				}
				if (found) {
					float xPosNext = hStart.x + xUnitLength * (lastGridX + 2), 
							yPosNext = hStart.y - (values[lastVIdx] / yRange * yAxisLength);
					if (lastIdx > 0) {
						float xPos = hStart.x + xUnitLength * lastIdx, 
						yPos = hStart.y - (values[xMin + (lastIdx - 1) * zoom] / yRange * yAxisLength);
						Drawing.DrawLine(new Vector2(xPos, yPos), new Vector2(xPosNext, yPosNext), color, 1.5f, true);
					}
					string text = ("<color=#" + series.colorHex + ">" + series.label + "</color>" + '\n' + values[lastVIdx].ToString("F2"));
					DrawMarker(series.label, new Rect(xPosNext - 7, yPosNext - 7, 14, 14), color, text);
					yPosNext = hStart.y - 5;
					GUIStyle style = new GUIStyle(GUI.skin.label);
					style.normal.textColor = Color.white;
					// style.richText = true;
					GUI.Label(new Rect(xPosNext + xUnitLength - 42, yPosNext + 10, 80, 70), ("last"), style);
					// yPosNext = hStart.y - 5;
					// Drawing.DrawLine(new Vector2(xPosNext, yPosNext), new Vector2(xPosNext, yPosNext + 10), color, thickness, false);
				}
			}

			/*

			if (values [xMin + Mathf.Min(series.values.Count, xNumMarkers) - 1] >= 0) {
				// Draw Last Point
				float xPos = hStart.x + xUnitLength * (Mathf.Min(series.values.Count, xNumMarkers));
				float yPos = hStart.y - (values[xMin + Mathf.Min(series.values.Count, xNumMarkers) - 1 ] / yRange * yAxisLength);
				string text = ("<color=#" + series.colorHex + ">" + series.label + "</color>" + '\n' + values[xMin].ToString("F2"));
				DrawMarker(series.label, new Rect(xPos - 7, yPos - 7, 14, 14), color, text);
			}





			if (values [xMin + Mathf.Min(series.values.Count, (xNumMarkers-1) *zoom)] >= 0) {
				// Draw Last Point
				float xPos = hStart.x + xUnitLength * (Mathf.Min(series.values.Count, xNumMarkers));
				float yPos = hStart.y - (values[xMin + Mathf.Min(series.values.Count, xNumMarkers) - 1 ] / yRange * yAxisLength);
				string text = ("<color=#" + series.colorHex + ">" + series.label + "</color>" + '\n' + values[xMin].ToString("F2"));
				DrawMarker(series.label, new Rect(xPos - 7, yPos - 7, 14, 14), color, text);
			}
			*/
		GUI.EndGroup();
	}

	private void DrawMarker(string label, Rect rect, Color color, string text) {
		Color temp = GUI.color;

		if (rect.Contains(Event.current.mousePosition)) {
			mouseOverLabels.Add(label);

			if (label == selected) {
				GUIStyle style = new GUIStyle(GUI.skin.label);
				style.alignment = TextAnchor.UpperCenter;
				style.richText = true;

				GUI.Label(new Rect(Event.current.mousePosition.x - 100, Event.current.mousePosition.y - 50, 200, 60), text, style);
			}
		}

		GUI.color = color;
		GUI.DrawTexture(rect, lineMarkerTex);

		GUI.color = temp;
	}

	private void DrawMonthSlider() {
		int offset = (zoom > 1) ? 1 : 0;
		monthSliderMax = Mathf.Max (0, 1 + offset + xAxisLabels.Count / zoom - xNumMarkers);
		monthSliderValue = Mathf.RoundToInt(GUI.HorizontalSlider(monthSliderRect, monthSliderValue, 0, monthSliderMax));
		xMin = (int) monthSliderValue * zoom;
		
		if (scrollToMonth != -1) {
			if (Mathf.Abs(monthSliderValue - scrollToMonth) > 0.1f) {
				monthSliderValue = Mathf.Lerp(monthSliderStart, scrollToMonth, monthSliderDT += Time.deltaTime);
			} else {
				scrollToMonth = -1;
			}
		}
	}
	
	public void ShowMonth(int month) {
		if (xAxisLabels.Count > xNumMarkers) {
			monthSliderStart = monthSliderValue;
			monthSliderDT = 0;
			scrollToMonth = Mathf.Clamp(month, 1, xAxisLabels.Count - xNumMarkers + 1) - 1;
		}
	}
	
	public void ShowLastMonth() {
		if (xAxisLabels.Count > xNumMarkers) {
			ShowMonth(xAxisLabels.Count - xNumMarkers + 1);
		}
	}

	private void DrawLegend() {
		GUIStyle style = new GUIStyle(GUI.skin.label);
		style.richText = true;

		Texture2D markerTex = Functions.CreateTexture2D(new Color(0.85f, 0.85f, 0.85f));
		Texture2D markerHoverTex = Functions.CreateTexture2D(Color.white);

		Color temp = GUI.color;

		GUI.BeginGroup(legendRect, GUI.skin.box);
			style.alignment = TextAnchor.UpperCenter;
			GUI.color = Color.yellow;
			GUI.Label(new Rect((legendRect.width - 200) / 2, 10, 200, 30), "Legend", style);
			GUI.color = temp;

			legendScrollPos = GUI.BeginScrollView(legendScrollRect, legendScrollPos, new Rect(0, 0, legendScrollRect.width - 20, seriesLabels.Count * 30));
				style.alignment = TextAnchor.UpperLeft;
				for (int i = 0; i < seriesLabels.Count; i++) {
					Series series = seriesList[seriesLabels[i]];
					string label = series.label;

					Rect itemRect = new Rect(10, i * 30, legendScrollRect.width * 0.8f, 30);

					if (itemRect.Contains(Event.current.mousePosition)) {
						selected = label;
						lastSeriesToDraw = label;
					}
					// Marquee Effect
					if (label == selected) {
						float textWidth = GUI.skin.label.CalcSize(new GUIContent(label)).x;
						float maxViewWidth = itemRect.width - 50;

						if (textWidth > maxViewWidth) {
							series.labelX = Mathf.Lerp(0, maxViewWidth - textWidth, series.labelDT += Time.deltaTime * 0.75f);
						}
					} else {
						series.labelX = 0;
						series.labelDT = 0;
					}

					GUI.BeginGroup(itemRect);
						GUI.color = excludeList.Contains(label) ? Color.gray : series.color;
						GUI.DrawTexture(new Rect(5, 10, 30, 15), (label == selected) ? markerHoverTex : markerTex);
						GUI.color = temp;
			
						GUI.color = excludeList.Contains(label) ? Color.gray : temp;
						style.normal.textColor = (label == selected) ? series.color : Color.white;

						GUI.BeginGroup(new Rect(50, 7, 200, 30));
							GUI.Label(new Rect(series.labelX, 0, 200, 30), label, style);
						GUI.EndGroup();

						GUI.color = temp;

						if (GUI.Button(new Rect(0, 0, itemRect.width, itemRect.height), "", GUIStyle.none)) {
							SetSeriesActive(label, excludeList.Contains(label));
						}
					GUI.EndGroup();
				}
			GUI.EndScrollView();

			if (GUI.Button(new Rect((legendRect.width - 85) / 2, legendRect.height - 33, 85, 25), isLegendContentHidden ? "Show Items" : "Hide Items")) {
				isLegendContentHidden = !isLegendContentHidden;

				if (isLegendContentHidden) {
					foreach (string label in seriesLabels) {
						if (!excludeList.Contains(label)) {
							excludeList.Add(label);
						}
					}
				} else {
					foreach (string label in new List<string>(excludeList)) {
						SetSeriesActive(label, true);
					}
				}
			}

		GUI.EndGroup();

		// GUI.BeginGroup(buttonRect, GUI.skin.box);

		if (GUI.Button (new Rect (width - 300, 385, 70, 25), "Zoom In") && isActive) {
			ZoomIn();
		}

		if (GUI.Button (new Rect (width - 200, 385, 80, 25), "Zoom Out") && isActive) {
			ZoomOut();
		}

		if (GUI.Button (new Rect (width - 90, 385, 65, 25), "Close") && isActive) {
			ToggleGraph();
		}



		// GUI.EndGroup();
	}

	public void SetSeriesActive(string label, bool active) {
		Series series = seriesList[label];

		if (active) {
			excludeList.Remove(label);

			series.width = 0;
			series.deltaTime = 0;
		} else {
			excludeList.Add(label);
		}
	}

	public void UpdateData() {
		// Update X-Axis Labels
		// List<string> labels = GameState.csvList.xLabels;
		xAxisLabels.Clear();
		for (int i = minDay; i <= maxDay; i++) {
			// Debug.Log("Graph: i, labels[i] = " + i + " " + labels[i]);
			// int month = i + 1 + 14*12;   // int.Parse(labels[i]);
			// string name = DateTimeFormatInfo.CurrentInfo.GetMonthName((month - 1) % 12 + 1).Substring(0, 3);
			xAxisLabels.Add("" + i);      // .ToString("00"));
			/*
			if ((i == minMonth) || (((month - 1) % 12) == 0) ) {
				xAxisLabels.Add(name + "\n'" + (month / 12 + 1).ToString("00"));
			} else {
				xAxisLabels.Add(name);
			}
			*/
			Debug.Log("Graph: i, xAxisLabels[i] = " + (i-minDay) + " " + xAxisLabels[i-minDay]);
		}
		
		// Update Values
		float yMaxValue = 1;

		CSVObject csv = GameState.csvList;

		for (int idx2 = 0; idx2 < speciesIds.Count; idx2++) {
		// foreach (KeyValuePair<string, List<string>> entry in csv.csvList) {
			string name = speciesTable[speciesIds[idx2]].name;            // entry.Key;
			List<string> values = new List<string>();    // entry.Value;

			for (int idx = minDay; idx <= maxDay; idx++) {
				if (biomassHistory[idx2].ContainsKey(idx)) {
					values.Add ((biomassHistory [idx2]) [idx].ToString());
				} else {
					values.Add ("");
				}
			}
				
			Debug.Log("Graph: KeyValuePair, key = " + name);
			for (int i2 = 0; i2 < values.Count; i2++) {
				Debug.Log("Graph: KeyValuePair, i, value[i] = " + i2 + " :" + values[i2] + ":");
			}
				
			if (name == ".xLabels") {
				continue;
			}

			if (!seriesList.ContainsKey(name)) {
				Rect seriesRect = new Rect(0, 0, 0, graphRect.height);
				Color color = new Color(Random.Range(0.5f, 0.9f), Random.Range(0.5f, 0.9f), Random.Range(0.5f, 0.9f));

				seriesList[name] = new Series(name, seriesRect, color);
				seriesLabels.Add(name);
				
				if (lastSeriesToDraw == null) {
					lastSeriesToDraw = name;
				}
			}

			List<float> temp = new List<float>();
			for (int i = 0; i < values.Count; i++) {
				float value = (values[i] == "" ? -1f : float.Parse(values[i]));
				temp.Add(value);

				/*
				if (temp.Count != values.Count) {
					temp.Add(value);
				} else if (!Mathf.Approximately(temp[i], value)) {
					temp[i] = value;
				}
				*/

				yMaxValue = Mathf.Max(value, yMaxValue);
			}
			seriesList [name].values = temp;
			Debug.Log ("Graph, UpdateData: seriesList[name].values.Count = " + seriesList [name].values.Count);
		}

		seriesLabels.Sort();

		int roundTo = int.Parse("5".PadRight(((int) yMaxValue).ToString().Length - 1, '0'));
		yRange = Mathf.CeilToInt(yMaxValue / roundTo) * roundTo;
		isReady = true;
	}
	
	public IEnumerator UpdateDataRoutine(float time) {
		while (true) {
			// UpdateData();
			yield return new WaitForSeconds(time);
		}
	}
		
	void MakeDayCounts() {
		int month;
		int year = START_YEAR;
		int value;
		dayCount [0] = 0;
		for (int idx = 1; idx < NUM_YEARS * 12; idx++) {
			month = (idx - 1) % 12;
			value = months [month];
			if ((month == 1) && ((year % 100) != 0) && ((year % 4) == 0)) {
				value++;
			}
			dayCount [idx] = dayCount [idx - 1] + value;
			if (month == 10) {
				year++;
			}
		}
	}

	int GetMonth(int day) {
		int result = -1;
		int idx = 1;
		while (result == -1) {
			if (dayCount [idx] > day) {
				result = idx - 1;
			}
			idx++;
		}
		return result;
	}
		
	void GetData() {
		isReady = false;
		biomassValues = new Dictionary<int,int> ();
		speciesIds = new List<int> ();
		biomassHistory = new List<Dictionary<int,int>> ();
		minDay = 1000000;
		maxDay = 0;
		minMonth = NUM_YEARS * 12;
		maxMonth = 0;
		Debug.Log("Graph: Send SpeciesActionProtocol, action = 2");
		int action = 2;
		Game.networkManager.Send (SpeciesActionProtocol.Prepare ((short) action), ProcessSpeciesAction);
	}
		
	public void ProcessSpeciesAction (NetworkResponse response)
	{
		ResponseSpeciesAction args = response as ResponseSpeciesAction;
		int action = args.action;
		int status = args.status;
		if ((action != 2) || (status != 0)) {
			Debug.Log ("Graph: ResponseSpeciesAction unexpected result2");
			Debug.Log ("action, status = " + action + " " + status);
		}
		Dictionary<int, int> speciesList = args.speciesList;
		speciesCount = speciesList.Count;
		Debug.Log ("Graph, ProcessSpeciesAction, size = " + speciesCount);
		foreach (KeyValuePair<int, int> entry in speciesList) {
			Debug.Log ("Graph: species, biomass = " + entry.Key + " " + entry.Value);
			int action2 = 4;
			biomassValues.Add (entry.Key, entry.Value);
			Game.networkManager.Send (SpeciesActionProtocol.Prepare ((short) action2, entry.Key), ProcessSpeciesHistory);
		}
		if (speciesCount == 0) {
			UpdateData ();
		}
	}
		
	public void ProcessSpeciesHistory (NetworkResponse response)
	{
		List<int> keys = new List<int> ();
		int idx;
		Dictionary<int, int> values = new Dictionary<int, int> ();
		ResponseSpeciesAction args = response as ResponseSpeciesAction;
		int action = args.action;
		int status = args.status;
		int species_id = args.species_id;
		if ((action != 4) || (status != 0)) {
			Debug.Log ("Graph: ResponseSpeciesAction unexpected result4");
			Debug.Log ("action, status = " + action + " " + status);
		}
		Dictionary<int, int> speciesList = args.speciesHistoryList;
		Debug.Log ("Graph: ProcessSpeciesHistory, species_id = " + species_id);
		// Debug.Log ("Graph: ProcessSpeciesHistory, size = " + speciesList.Count);  // null pointer in some cases 
		if (speciesList.Count > 0) {
			foreach (KeyValuePair<int, int> entry in speciesList) {
				Debug.Log ("day, biomass change = " + entry.Key + " " + entry.Value);
				keys.Add (entry.Key);
				values.Add (entry.Key, entry.Value);
			}
			keys.Sort ();
			for (int i = 0; i < keys.Count; i++) {
				Debug.Log ("species_id, i, keys[i], biomass change: " + species_id + " " + i + " " + keys [i] + " " + values[keys[i]]);
			}
			int size = keys.Count;
			int[] day = new int[size];
			int[] dayValue = new int[size];
			day [0] = keys [size - 1];
			dayValue [0] = biomassValues [species_id];
			for (idx = 1; idx < size; idx++) {
				day [idx] = keys [size - idx - 1];
				dayValue [idx] = dayValue [idx - 1] - values [keys[size - idx]];
			}
			Debug.Log ("Sorted values, species_id = " + species_id);
			for (idx = 0; idx < size; idx++) {
				Debug.Log ("idx, day[idx], dayValue[idx]: " + idx + " " + day [idx] + " " + dayValue [idx]);
			}
			if (day [0] > maxDay) {
				maxDay = day [0];
			}
			if (day [size - 1] < minDay) {
				minDay = day [size - 1];
			}

			/*
			List<int> mKeys = new List<int> ();
			List<int> mValues = new List<int> ();
			int month = GetMonth(day [0]);
			mKeys.Add (month);
			mValues.Add (dayValue [0]);
			for (idx = 1; idx < size; idx++) {
				if (GetMonth (day [idx]) < month) {
					month = GetMonth (day [idx]);
					mKeys.Add (month);
					mValues.Add (dayValue [idx]);
				}
			}
			if (mKeys [0] > maxMonth) {
				maxMonth = mKeys [0];
			}
			if (mKeys [mKeys.Count - 1] < minMonth) {
				minMonth = mKeys [mKeys.Count - 1];
			}
			*/
			values = new Dictionary<int, int> ();				
			Debug.Log ("Sorted month values, species_id = " + species_id);
			for (idx = 0; idx < size; idx++) {
				values.Add (day[idx], dayValue [idx]);
				Debug.Log ("idx, day[idx], dayValue[idx]: " + idx + " " + day [idx] + " " + dayValue [idx]);
			}			
			speciesIds.Add(species_id);
			biomassHistory.Add(values);
		}
		speciesCount--;
		if (speciesCount == 0) {
			Debug.Log ("Graph: Processed last species history");
			UpdateData ();
		}
	}
		
}
