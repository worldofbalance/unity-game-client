using System;
using System.IO;

public class BackToLobbyProtocol {
	
	public static NetworkRequest Prepare() {
		NetworkRequest request = new NetworkRequest(NetworkCode.BACK_TO_LOBBY);
		return request;
	}
	
	public static NetworkResponse Parse(MemoryStream dataStream) {
		return null;
	}
}