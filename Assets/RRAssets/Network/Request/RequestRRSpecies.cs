using UnityEngine;
using System.Collections;
namespace RR{
public class RequestRRSpecies : NetworkRequest {
	
	// Use this for initialization
	public RequestRRSpecies() {
		request_id = Constants.CMSG_RRSPECIES;
	}
	// Update is called once per frame
	public void send(int id) {
		
		packet = new GamePacket(request_id);
		packet.addInt32 (id);
		
	}
}
}
