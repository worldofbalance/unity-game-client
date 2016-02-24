using UnityEngine;

using System;
using System.Collections.Generic;
using System.Reflection;

namespace RR {
public class NetworkResponseTable {

	public static Dictionary<short, Type> responseTable { get; set; }
	
	public static void init() {
		responseTable = new Dictionary<short, Type>();
		
		add(Constants.SMSG_AUTH, "ResponseLogin");
		add(Constants.SMSG_HEARTBEAT, "ResponseHeartbeat");
		add(Constants.SMSG_GAME_STATE, "ResponseGameState");
		add(Constants.SMSG_RACE_INIT, "ResponseRaceInit");
		add(Constants.SMSG_KEYBOARD, "ResponseKeyboard");
//<<<<<<< HEAD
//		add(Constants.SMSG_RRPOSITION, "RRResponsePostion");
//<<<<<<< HEAD
//=======
//		add(Constants.SMSG_RRSPECIES, "RRResponseSpecies");

//>>>>>>> Dong
//=======
		add(Constants.SMSG_RRPOSITION, "ResponseRRPostion");
		add(Constants.SMSG_RRSPECIES, "ResponseRRSpecies");
		add(Constants.SMSG_RRSTARTGAME, "ResponseRRStartGame");

		add (Constants.SMSG_RRENDGAME, "ResponseRREndGame");
		add (Constants.SMSG_RRGETMAP, "ResponseRRGetMap");
//>>>>>>> start
		
//		Debug.Log("dictionary check: " + responseTable.TryGetValue);
//		Debug.Log("Response table: " + (responseTable[Constants.SMSG_AUTH]).ToString());
	}
	
	public static void add(short response_id, string name) {
		responseTable.Add(response_id, Type.GetType("RR."+name));
	}
	
	public static NetworkResponse get(short response_id) {
		NetworkResponse response = null;

//		Debug.Log("Response table: " + responseTable[response_id].ToString());
		
		if (responseTable.ContainsKey(response_id)) {
			response = (NetworkResponse) Activator.CreateInstance(responseTable[response_id]);
			response.response_id = response_id;
		} else {
			Debug.Log("Response [" + response_id + "] Not Found");
			Debug.Log (response.ToString());
		}
		
		return response;
	}
}
}