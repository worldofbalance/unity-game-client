using UnityEngine;
using System.Collections;

namespace RR{
public class ResponseRREndGameEventArgs : ExtendedEventArgs {
	public bool win { get; set; }
	public string winningTime { get; set; }
	
	public ResponseRREndGameEventArgs() {
		event_id = Constants.SMSG_RRENDGAME;
	}
}

public class ResponseRREndGame : NetworkResponse {
	
	private bool win;
	private string winningTime;
	
	public ResponseRREndGame() {
	}
	
	public override void parse() {
		win = DataReader.ReadBool(dataStream);
		winningTime = DataReader.ReadString(dataStream);
	}
	
	public override ExtendedEventArgs process() {
		//Debug.Log ("ResponseRREndGame ExtendedEventArgs");
		ResponseRREndGameEventArgs args = new ResponseRREndGameEventArgs();
		
		args.win = this.win;
		args.winningTime = this.winningTime;
		
		return args;
	}
}
}