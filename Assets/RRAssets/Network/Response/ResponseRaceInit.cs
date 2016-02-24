//author: Ye
using UnityEngine;

using System;
namespace RR{
public class ResponseRaceInitEventArgs : ExtendedEventArgs {
	public short status { get; set; }
	
	public ResponseRaceInitEventArgs() {
		event_id = Constants.SMSG_RACE_INIT;
	}
}
}

namespace RR {
public class ResponseRaceInit : NetworkResponse {
	
	private short status;//start a battle: 0; wait for a battle: 1
	
	public ResponseRaceInit() {
	}
	
	public override void parse() {
		status = DataReader.ReadShort(dataStream);
		//		Battle.stopSendRequest();
		if (status == 0) {
			//Debug.Log("Battle Preparation Started");
			//change to battle scene
			//when the battle is ended, change stopSendRequest to true;
		}
		else if (status ==1) {
			Debug.Log("request received by server, wait for a race");
		}
	}
	
	public override ExtendedEventArgs process() {
		ResponseRaceInitEventArgs args = null;
		
		args = new ResponseRaceInitEventArgs();
		args.status = status;
		
		if (status == 0) {
			//battle start, stop sending battle request
			Debug.Log("response received from server");
		} else if (status == 1) {
			//battle not start, continue sending battle request
			
		}
		return args;
	}
}
}