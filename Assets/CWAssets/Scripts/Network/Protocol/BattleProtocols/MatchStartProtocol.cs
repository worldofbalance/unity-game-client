using UnityEngine;
using System.Collections;
using System;
using System.IO;
namespace CW{
public class MatchStartProtocol {
	
	public static NetworkRequest Prepare(string sessionID) {
		NetworkRequest request = new NetworkRequest(NetworkCode.MATCH_START);
		request.AddString(sessionID);
		
		return request;
	}
	
	public static NetworkResponse Parse(MemoryStream dataStream) {
		ResponseMatchStart response = new ResponseMatchStart();
		response.status = DataReader.ReadShort(dataStream);
		
		if (response.status == 0) {
			response.matchID = DataReader.ReadInt(dataStream);
		}
		return response;
	}
}

public class ResponseMatchStart : NetworkResponse {
	
	public short status { get; set; }
	public int matchID { get; set;}
	
	public ResponseMatchStart() {
		protocol_id = NetworkCode.MATCH_START;
	}
}
}