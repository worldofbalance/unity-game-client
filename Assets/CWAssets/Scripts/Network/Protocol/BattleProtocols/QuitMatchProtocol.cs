using UnityEngine;
using System.Collections;
using System;
using System.IO;
namespace CW{
public class QuitMatchProtocol {
	
	public static NetworkRequest Prepare(int playerID) {
		NetworkRequest request = new NetworkRequest(NetworkCode.QUIT_MATCH);
		request.AddInt32(playerID);
		return request;
	}
	
	public static NetworkResponse Parse(MemoryStream dataStream) {
		ResponseQuitMatch response = new ResponseQuitMatch();
		
		response.opponentIsReady = DataReader.ReadBool(dataStream);
		return response;
	}
}

public class ResponseQuitMatch : NetworkResponse {
	public Boolean opponentIsReady {get ; set; }
	
	public ResponseQuitMatch() {
		protocol_id = NetworkCode.QUIT_MATCH;
	}
}
}
//