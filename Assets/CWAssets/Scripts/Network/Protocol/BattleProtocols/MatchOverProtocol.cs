using UnityEngine;
using System.Collections;
using System;
using System.IO;
namespace CW{
public class MatchOverProtocol {
	
	public static NetworkRequest Prepare(int playerID, int wonGame) {
		NetworkRequest request = new NetworkRequest(NetworkCode.MATCH_OVER);
		request.AddInt32(playerID);
		request.AddInt32(wonGame);
		return request;
	}
	
	public static NetworkResponse Parse(MemoryStream dataStream) {
		ResponseMatchOver response = new ResponseMatchOver();
		// TODO: update Constants to set server status messages
		response.status = DataReader.ReadShort(dataStream);
		response.creditsWon = DataReader.ReadInt (dataStream);
		return response;
	}
}

public class ResponseMatchOver : NetworkResponse {
	public short status {get ; set; }
	public int creditsWon{get; set;}
	public ResponseMatchOver() {
		protocol_id = NetworkCode.MATCH_OVER;
	}
}
}