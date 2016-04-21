//author: Ye
using UnityEngine;

using System;
namespace RR{

public class ResponseRRStartGameEventArgs : ExtendedEventArgs {
	public short status { get; set; }
	
	public ResponseRRStartGameEventArgs() {
		event_id = Constants.SMSG_RRSTARTGAME;
	}
}
}

namespace RR {
public class ResponseRRStartGame : NetworkResponse {
	
	private short status;//start a battle: 0; wait for a battle: 1
	
	public ResponseRRStartGame() {
	}
	
	public override void parse() {
		status = DataReader.ReadShort(dataStream);
		Debug.Log("ResponseRRStartGame has been parsed.");
		//		Battle.stopSendRequest();
		if (status == 0) {
			Debug.Log("Battle Preparation Started");
			//change to battle scene
			//when the battle is ended, change stopSendRequest to true;
			Application.LoadLevel("RRCountdownScene");
		}
		else if (status ==1) {
			Debug.Log("request received by server, wait for a race");
		}
	}
	
	public override ExtendedEventArgs process() {
		ResponseRRStartGameEventArgs args = null;
		
		args = new ResponseRRStartGameEventArgs();
		args.status = status;
		
		if (status == 0) {
			//battle start, stop sending battle request
//			Debug.Log("response received from server");
		} else if (status == 1) {
			//battle not start, continue sending battle request
			
		}
		return args;
	}
}
}
