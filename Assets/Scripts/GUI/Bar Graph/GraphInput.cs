using UnityEngine;
using System.Collections;

public class GraphInput : MonoBehaviour {

	private int window_id = Random.Range(4000, 5000);
	// Window Properties
	private float left;
	private float top;
	private float width = 805;
	private float height = 400;
	private bool isActive = true;
	// Other
	private Rect inputWinRect;
	private string inputText = null;
	public BarGraph graph { get; set; }

	void Awake() {
		inputWinRect = new Rect(0, Screen.height - 300, 400, 300);
	}

	// Use this for initialization
	void Start() {
//		graph.colorList["Species A"] = Color.red;
//		graph.colorList["Species B"] = Color.green;
//		graph.colorList["Species C"] = Color.blue;
		graph.colorList["Cape teal [33]"] = new Color32(0, 255, 0, 255);
		graph.colorList["Marabou stork [56]"] = new Color32(255, 0, 0, 255);
		graph.colorList["Grass and herbs [5]"] = new Color32(255, 153, 0, 255);
		graph.colorList["Fat or tree mouse [31]"] = new Color32(255, 255, 0, 255);
		graph.colorList["Bat-eared fox [59]"] = new Color32(74, 134, 232, 255);

		string csv_target = ",1,2,3,4,5\nSpecies A,4700,4000,3200,2300,1400\nSpecies B,2800,4300,4400,5300,1300\nSpecies C,1100,1000,950,700,200";
//		InputToCSVObject(csv_target);
		
		string csv_current = ",1,2,3,4,5\nSpecies A,5000,4300,3500,2600,1700\nSpecies B,2500,4000,4100,5000,1000\nSpecies C,1000,900,850,600,100";
//		InputToCSVObject(csv_current);
	}
	
	// Update is called once per frame
	void Update() {
	
	}

	void OnGUI() {
		if (!isActive) {
			return;
		}

		inputWinRect = GUI.Window(window_id, inputWinRect, MakeWindow, "Input", GUIStyle.none);
	}

	public void MakeWindow(int id) {
		Functions.DrawBackground(new Rect(0, 0, inputWinRect.width, inputWinRect.height), Constants.BG_TEXTURE_01);
		// Window Title Styling
		GUIStyle style = new GUIStyle(GUI.skin.label);
		style.alignment = TextAnchor.UpperCenter;
		style.font = Constants.FONT_01;
		style.fontSize = 16;
		// Window Title
		GUI.Label(new Rect((inputWinRect.width - 100) / 2, 0, 100, 30), Strings.INPUT, style);
		
		if (inputText == null) {
			inputText = "";
		}
		
		Rect inputRect = new Rect(0, 0, inputWinRect.width, inputWinRect.height);
		GUI.BeginGroup(inputRect);
			inputText = GUI.TextArea(new Rect((inputRect.width - inputRect.width * 0.9f) / 2, 30, inputRect.width * 0.9f, inputRect.height * 0.7f), inputText);
		
			GUI.BeginGroup(new Rect((inputRect.width - 210) / 2, inputRect.height - 50, 210, 30));
				if (GUI.Button(new Rect(0, 0, 100, 30), Strings.ADD)) {
					if (inputText.Trim() != "") {
						InputToCSVObject(inputText);
					}
					inputText = "";
				}
				
				if (GUI.Button(new Rect(110, 0, 100, 30), Strings.CLEAR)) {
					graph.Clear();
				}
			GUI.EndGroup();
		GUI.EndGroup ();
		
		GUI.DragWindow();
	}
	
	public void SetActive(bool active) {
		isActive = active;
	}
	
	public void InputToCSVObject(string text) {
		CSVObject csvObject = Functions.ParseCSV(text);
		graph.CreateSeriesSet(csvObject);
	}
}
