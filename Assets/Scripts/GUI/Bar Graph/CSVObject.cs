using UnityEngine;

using System.Collections.Generic;

public class CSVObject {
	
	public Dictionary<string, List<string>> csvList { get; private set; }
	public List<string> xLabels { get; set; }
	public float score { get; set; }

	public CSVObject() {
		csvList = new Dictionary<string, List<string>>();
	}

	//get average difference between current CSV object values and object a 
	//(using root mean square distrib)
	public Dictionary<string, int> AverageDifferenceCSVs (CSVObject a)
	{
		Dictionary<string, int> diffList = new Dictionary<string, int> ();

		foreach (string name in a.csvList.Keys) {
			float sum = 0.0f;
			for (int i = 0; i < a.csvList[name].Count; i++) {
				if (!this.csvList.ContainsKey(name)) {
					Debug.LogError ("In AverageDifferenceCSVs, name not found: " + name);
					continue;
				}
				string aVal = a.csvList [name] [i], thisVal = this.csvList [name] [i];
				
				if (aVal == "" || thisVal == "") {
					continue;
				}
				
				float diff = (float.Parse (aVal) - float.Parse (thisVal));
				sum += diff * diff;
			}
			diffList.Add (name, (int) Mathf.Sqrt (sum / (float) a.csvList[name].Count));
		}
		
		return diffList;
	}
	
	public int CalculateScore (CSVObject a)
	{
		int score = 0;
		Dictionary<string, int> diffList = AverageDifferenceCSVs (a);
		foreach (KeyValuePair<string, int> entry in diffList) {
			score += entry.Value;
		}
		return score;

	}
}
