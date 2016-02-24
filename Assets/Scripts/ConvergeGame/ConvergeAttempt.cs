using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ConvergeAttempt {

	// Use this for initialization
	private int player_id;
	public int ecosystem_id;
	public int attempt_id { get; set; }
	public bool allow_hints { get; set; }
	public int hint_id { get; set; }
	//public string time { get; set; }
	public string config { get; set; }
	public string csv_string { get; private set; }
	public CSVObject csv_object { get; private set; }
	//public ConvergeManager manager { get; set; }
	public Dictionary<string, ConvergeParam> seriesParams { get; set; }

	public ConvergeAttempt(int player_id, 
	                       int ecosystem_id, 
	                       int attempt_id,
	                       bool allow_hints,
	                       int hint_id,
	                       string config,
	                       string csv_string
	                       //ConvergeManager manager
	                       ) {
		this.player_id = player_id;
		this.ecosystem_id = ecosystem_id;
		this.attempt_id = attempt_id;
		this.allow_hints = allow_hints;
		this.hint_id = hint_id;
		this.config = config;
		SetCSV (csv_string);
		this.seriesParams = null;
	}

	public void SetCSV (string str)
	{
		this.csv_string = str;
		this.csv_object = Functions.ParseCSV (str);
	}

	public bool ParamsUpdated () {
		foreach (ConvergeParam param in seriesParams.Values) {
			if (param.ValueUpdated ()) {
				return true;
			}
		}
		return false;
	}

	//separate out parameter value information from configuration string
	//note: link level parameters are not supported in this implementation
	public void ParseConfig (ConvergeManager manager)
	{
		//this.manager = manager;
		this.seriesParams = new Dictionary<string, ConvergeParam>();
		string remainder = config;
		int nodeCnt = 0;
		int nodeId;
		int start, end, nodeIdx;
		int paramCnt;
		float value, valNotNeeded;
		string name;
		string paramId;

		//sequence is nodeCnt,[node0],biomass0,perunitbiomass0,paramCnt0,(if any)paramID0,value0,paramID1,value1,...
		//[node1],biomass1,perunitbiomass1,paramCnt1,...,[nodeN],biomassN,...
		if (remainder != null) {
			//"nodeCnt,"
			nodeCnt = int.Parse(remainder.Substring(0, remainder.IndexOf(",")));
		}
		for (int i = 0; i < nodeCnt; i++) {
			//"[node_Id(i)],biomass(i),"
			start = remainder.IndexOf("[");
			end = remainder.IndexOf("],");
			nodeId = int.Parse(remainder.Substring(start + 1, end - start - 1));

			nodeIdx = manager.seriesNodes.IndexOf (nodeId);
			name = manager.seriesLabels[nodeIdx];

			remainder = remainder.Substring(end + 2);  //"],"
			end = remainder.IndexOf(",");
			valNotNeeded = float.Parse(remainder.Substring(0, end));  //biomass

			remainder = remainder.Substring(end + 1);
			end = remainder.IndexOf(",");
			valNotNeeded = float.Parse(remainder.Substring(0, end));  //per-unit-biomass

			remainder = remainder.Substring(end + 1);
			end = remainder.IndexOf(",");

			//get counts of node parameters for current node
			paramCnt = int.Parse(remainder.Substring(0, end));
			remainder = remainder.Substring(end + 1);

			//get node parameters
			for (int j = 0; j < paramCnt; j++) {
				//"paramID(j)=value(j),"
				start = remainder.IndexOf("=");
				paramId = remainder.Substring(0, start).ToUpper();
				end = remainder.IndexOf(",");
				value = float.Parse(remainder.Substring(start + 1, end - start - 1));
				remainder = remainder.Substring(end + 1);

				ConvergeParam param = new ConvergeParam (name, nodeId, paramId, value);
				seriesParams.Add (param.nodeIdParamId, param);
			}
			//get link parameters
			end = remainder.IndexOf(",");
			if (end == Constants.ID_NOT_SET) {
				end = remainder.Length;
			}
			paramCnt = int.Parse(remainder.Substring(0, end));
			if (paramCnt != 0) {
				Debug.LogError ("Error in ConvergeAttempt::ParseConfig() - link parameters found but not supported.");
			}
		}

	}

	public void UpdateConfig ()
	{
		string oldConfig = config;
		string newConfig = "";
		int nodeCnt, paramCnt;
		int offset, start, end, nextStart;
		int nodeId;
		string paramId;
		string nodeData;
		float newValue;

		//loop through nodes
		nodeCnt = int.Parse(oldConfig.Substring(0, oldConfig.IndexOf(",")));
		for (int i = 0; i < nodeCnt; i++) {
			start = oldConfig.IndexOf ("[");
			end = oldConfig.IndexOf ("],");
			if (end <= start) {
				Debug.LogError ("In ConvergeAttempt::UpdateConfig - invalid configuration string");
				break;
			}
			nodeId = int.Parse (oldConfig.Substring (start + 1, end - start - 1));
			newConfig = newConfig + oldConfig.Substring (0, end + 2);  //"],"

			nextStart = oldConfig.IndexOf ("[", end + 2);
			if (nextStart == Constants.ID_NOT_SET) {
				nextStart = oldConfig.Length;
			}
			nodeData = oldConfig.Substring (end + 2, nextStart - end - 2);
			if (nextStart < oldConfig.Length) {
				oldConfig = oldConfig.Substring (nextStart);
			} else {
				oldConfig = "";
			}

			//loop through node params
			int skip = nodeData.IndexOf (",");  //biomass
			skip = nodeData.IndexOf (",", skip + 1);  //per-unit-biomass
			paramCnt = int.Parse (nodeData.Substring (skip + 1, nodeData.IndexOf (",", skip + 1) - skip - 1));
			if (paramCnt == 0) {
				newConfig = newConfig + nodeData.Substring (0, nodeData.IndexOf (",", skip + 1));  //param-cnt->0
			}
			offset = 0;
			for (int j=0; j < paramCnt; j++) {
				start = nodeData.IndexOfAny (new char[]{'K', 'R', 'X'}, offset);
				end = nodeData.IndexOf ('=', start);
				paramId = nodeData.Substring (start, end - start);

				ConvergeParam param = seriesParams [ConvergeParam.NodeIdParamId (nodeId, paramId)];
				newValue = param.value;

				newConfig = newConfig + nodeData.Substring (offset, end + 1 - offset);  //i.e. through "="
				int thousands = Mathf.CeilToInt (newValue * 1000f);
				newConfig = newConfig + string.Format ("{0:0.000}", (float) thousands / 1000f);
				offset = nodeData.IndexOf (",", end + 1);
			}

			//holding place for (unsupported) link params
			newConfig = newConfig + ",0" + (i == nodeCnt - 1 ? "" : ",");
		}

		this.config = newConfig;
	}
}
