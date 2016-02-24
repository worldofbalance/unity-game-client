using UnityEngine;
using System.Collections;

namespace RR{
public class ResponseHeartbeatEventArgs : ExtendedEventArgs {
	public int opponentX { get; set; }
	public int opponenty { get; set; }
	public int opponentDistanceTraveled  { get; set; }
	public short opponentGameover { get; set; }


	public ResponseHeartbeatEventArgs() {
		event_id = Constants.SMSG_HEARTBEAT;
	}
}
}

namespace RR {
public class ResponseHeartbeat : NetworkResponse {
	private GameObject gameObject;
	private Running running;
	private int opponentX,opponentY,opponentDistanceTraveled;
	private short opponentGameover;

	public ResponseHeartbeat() {
		gameObject = GameObject.Find("Running");
		running = gameObject.GetComponent<Running>();
	}

	public override void parse() {

		opponentX = DataReader.ReadInt (dataStream);
		opponentY = DataReader.ReadInt (dataStream);
		opponentDistanceTraveled = DataReader.ReadInt (dataStream);
		opponentGameover = DataReader.ReadShort (dataStream);


	}
	public override ExtendedEventArgs process() {

		running.Player2Move(new Vector2(opponentX, opponentY));

		ResponseHeartbeatEventArgs args = new ResponseHeartbeatEventArgs ();
		args.opponentX = opponentX;
		args.opponenty = opponentY;
		args.opponentDistanceTraveled = opponentDistanceTraveled;
		args.opponentGameover = opponentGameover;
		return args;



	}



}
}
