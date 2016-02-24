using UnityEngine;
using System.Collections;
using System;
using System.IO;

// Run on EndTurn clicked

namespace CW{
public class EndTurnProtocol {
	
	public static NetworkRequest Prepare(int playerID) {
		NetworkRequest request = new NetworkRequest(NetworkCode.END_TURN);
		request.AddInt32(playerID);
		return request;
	}
	
	public static NetworkResponse Parse(MemoryStream dataStream) {
		ResponseEndTurn response = new ResponseEndTurn();
		
		response.status = DataReader.ReadShort(dataStream);
		return response;
	}
}

public class ResponseEndTurn : NetworkResponse {
	public short status {get; set;}
	
	public ResponseEndTurn() {
		protocol_id = NetworkCode.END_TURN;
	}
	}}
