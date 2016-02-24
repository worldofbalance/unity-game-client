using UnityEngine;
using System.Collections;

namespace RR{
public class ResponseGameStateEventArgs : ExtendedEventArgs {
	public short gameState { get; set; }


	public ResponseGameStateEventArgs() {
		event_id = Constants.SMSG_GAME_STATE;
	}
}
}

/*
 * The class ResponseGameState is responsible for checking whether or not it is
 * okay for the client to make request involing colleting data from an opponent
 * client.  This can be class This request should be made in a heartbeat like function asking at a
 * rate that would mini
 */

namespace RR {
public class ResponseGameState : NetworkResponse {
	private GameObject gameObject;
	private Running running;
	private short gameState;

	public ResponseGameState() {
		gameObject = GameObject.Find("GameLogic");
		//Debug.Log(gameObject.name);
		running = gameObject.GetComponent<Running>();
	}

	public override void parse() {
		gameState = DataReader.ReadShort (dataStream);

		if (gameState > 0) {
			// Change something to cause RequestHeartbeat to be sent
		}

	}

	public override ExtendedEventArgs process() {

		// Change the game state
		if (gameState == 1) {
			running.SetGameStateOn();
		}

		ResponseGameStateEventArgs args = new ResponseGameStateEventArgs ();

		args.gameState = gameState;
		return args;



	}



}
}
