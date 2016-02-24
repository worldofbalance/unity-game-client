using UnityEngine;

using System;
namespace RR{
public class RequestHeartbeat : NetworkRequest {

	public RequestHeartbeat() {
		request_id = Constants.CMSG_HEARTBEAT;
	}
	
	public void Send() {
		packet = new GamePacket(request_id);
	}
}
}