using UnityEngine;

using System;
/*
 * The class RequestGameState is responsible for checking whether or not it is
 * okay for the client to make request involing colleting data from an opponent
 * client.  This can be class This request should be made in a heartbeat like function asking at a
 * rate that would mini
 */
namespace RR{
public class RequestGameState : NetworkRequest {
	
	public RequestGameState() {
		request_id = Constants.CMSG_GAME_STATE;
	}
	
	public void send() {	
		packet = new GamePacket(request_id);		
	}
}
}
