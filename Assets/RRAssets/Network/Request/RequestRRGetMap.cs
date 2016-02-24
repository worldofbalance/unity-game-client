using UnityEngine;
using System;
using System.Collections;

namespace RR{
	public class RequestRRGetMap : NetworkRequest {
		
		// Use this for initialization
		public RequestRRGetMap () {
			packet = new GamePacket(request_id = Constants.CMSG_RRGETMAP);
		}
		
		public void Send() {
			// no variables to send
		}
	}
}