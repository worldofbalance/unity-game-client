/*
 * WaitForGame protocol
 * Lets the server know that the client wants to play a mini game
 * response parameters:
 * status - 1 for succeed 0 for fail
 */

using UnityEngine;
using System;
using System.IO;

public class NoWaitForGameProtocol {
	
	public static NetworkRequest Prepare() {
		NetworkRequest request = new NetworkRequest(NetworkCode.NOWAITFORGAME);
		return request;
	}
	
	public static NetworkResponse Parse(MemoryStream dataStream) {
		ResponseNoWaitForGame response = new ResponseNoWaitForGame();
		response.status = DataReader.ReadInt(dataStream);
		
		return response;
	}
}

public class ResponseNoWaitForGame : NetworkResponse {
	
	public int status { get; set; }
	
	public ResponseNoWaitForGame() {
		protocol_id = NetworkCode.WAITFORGAME;
	}
}
