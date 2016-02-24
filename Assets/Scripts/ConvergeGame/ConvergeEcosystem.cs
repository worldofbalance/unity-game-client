using UnityEngine;
using System.Collections.Generic;

public class ConvergeEcosystem {

	// Use this for initialization
	public int ecosystem_id { get; private set; }
	public string description { get; set; }
	public int timesteps { get; set; }
	public string config_default { get; set; }
	public string config_target { get; set; }
	private string _csv_default_string;
	public string csv_default_string
	{
		get
		{
			return this._csv_default_string;
		}
		set
		{
			this._csv_default_string = value;
			this.csv_default_object = Functions.ParseCSV(this._csv_default_string);
		}
	}	
	public CSVObject csv_default_object { get; private set; }
	private string _csv_target_string;
	public string csv_target_string
	{
		get
		{
			return this._csv_target_string;
		}
		set
		{
			this._csv_target_string = value;
			this.csv_target_object = Functions.ParseCSV(this._csv_target_string);
		}
	}	
	public CSVObject csv_target_object { get; private set; }

	public ConvergeEcosystem(int ecosystem_id) {
		this.ecosystem_id = ecosystem_id;
	}
	
	public static string[] GetDescriptions (List<ConvergeEcosystem> ecosystemList) {
		string[] descriptions = new string[ecosystemList.Count];
		int i = 0;
		foreach (ConvergeEcosystem ecosystem in ecosystemList)
		{
			descriptions[i++] = ecosystem.description;
		}
		return descriptions;
	}

}

