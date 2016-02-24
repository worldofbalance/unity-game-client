using UnityEngine;
using System;
using System.Collections;

namespace RR{
public class RequestRREndGame : NetworkRequest {
	
	// Use this for initialization
	public RequestRREndGame () {
		packet = new GamePacket(request_id = Constants.CMSG_RRENDGAME);
	}
	
	public void Send(bool gameCompleted, string finalTime) {
		packet.addBool(gameCompleted);
		packet.addString (finalTime);
	}
}
}