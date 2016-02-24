using UnityEngine;
using System.Collections;
namespace RR{
public class RequestKeyboard : NetworkRequest {

	public RequestKeyboard() {
		request_id = Constants.CMSG_KEYBOARD;
	}

	public void send(int keytype, int key) {
		packet = new GamePacket(request_id);
		packet.addInt32 (keytype);
		packet.addInt32 (key);
	}

}
}
