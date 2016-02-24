using UnityEngine;
using System.Collections;
using System;
using System.IO;
namespace CW{
public class MatchStatusProtocol {
	
public static NetworkRequest Prepare(int playerID, string playerName) {
	NetworkRequest request = new NetworkRequest(NetworkCode.MATCH_STATUS);
	request.AddInt32(playerID);
	request.AddString (playerName);
	return request;
}

public static NetworkResponse Parse(MemoryStream dataStream) {
	ResponseMatchStatus response = new ResponseMatchStatus();
	response.status = DataReader.ReadShort(dataStream);
	response.matchID = DataReader.ReadInt(dataStream);
	response.isActive = DataReader.ReadBool(dataStream);
	response.opponentIsReady = DataReader.ReadBool (dataStream);
		Debug.Log("matchstatus status " + response.status);
	return response;
	}
}

public class ResponseMatchStatus : NetworkResponse {

	public short status{get; set;}
	public int matchID{get; set;}
	public bool isActive{ get; set; }
	public bool opponentIsReady { get; set;}
	
	public ResponseMatchStatus() {
		protocol_id = NetworkCode.MATCH_STATUS;
	}
}
}