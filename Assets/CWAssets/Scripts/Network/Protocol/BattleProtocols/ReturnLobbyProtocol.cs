using System;
using System.IO;
namespace CW{
public class ReturnLobbyProtocol {
	
	public static NetworkRequest Prepare() {
		NetworkRequest request = new NetworkRequest(NetworkCode.RETURN_LOBBY);
		
		return request;
	}
	
	public static NetworkResponse Parse(MemoryStream dataStream) {
		ResponseReturnLobby response = new ResponseReturnLobby();
		response.status = DataReader.ReadShort(dataStream);
		
		return response;
	}
}

public class ResponseReturnLobby : NetworkResponse {
	
	public short status { get; set; }
	
	public ResponseReturnLobby() {
		protocol_id = NetworkCode.RETURN_LOBBY;
	}
}
}