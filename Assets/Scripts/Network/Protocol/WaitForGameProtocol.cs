/*
 * WaitForGame protocol
 * Lets the server know that the client wants to play a mini game
 * request parameters:
 * gameType - which game to wait for
 * response parameters:
 * status - 1 for succeed 0 for fail
 */

using UnityEngine;
using System;
using System.IO;

public class WaitForGameProtocol {

	/* which game the client wants to play
     * 0 - Don't eat me! (no server required)
     * 1 - Cards of the Wild
     * 2 - Running Rhino
     * 3 - Clash of Species
     */
	
	public static NetworkRequest Prepare(int gameType) {
		NetworkRequest request = new NetworkRequest(NetworkCode.WAITFORGAME);
		request.AddInt32(gameType);
		return request;
	}
	
	public static NetworkResponse Parse(MemoryStream dataStream) {
		ResponseWaitForGame response = new ResponseWaitForGame();
		response.status = DataReader.ReadInt(dataStream);
		
		return response;
	}
}

public class ResponseWaitForGame : NetworkResponse {
	
	public int status { get; set; }
	
	public ResponseWaitForGame() {
		protocol_id = NetworkCode.WAITFORGAME;
	}
}
