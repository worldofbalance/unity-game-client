/*
 * StartGame protocol
 * starts a game with another player
 * request parameters:
 * pid - id of player to start game with
 * response parameters:
 * status - status of request -- 0 if invite failed, 1 if game started successfully
 * id - game id -- so minigame server can place you with the right partner
 */

using UnityEngine;
using System;
using System.IO;

public class StartGameProtocol {
	
	public static NetworkRequest Prepare(int pid) {
		NetworkRequest request = new NetworkRequest(NetworkCode.STARTGAME);
		request.AddInt32(pid);
		return request;
	}
	
	public static NetworkResponse Parse(MemoryStream dataStream) {
		ResponseStartGame response = new ResponseStartGame();
		response.status = DataReader.ReadInt(dataStream);
		response.id = DataReader.ReadInt(dataStream);
		return response;
	}
}

public class ResponseStartGame : NetworkResponse {
	
	public int status { get; set; }
	public int id { get; set; }
	
	public ResponseStartGame() {
		protocol_id = NetworkCode.WAITFORGAME;
	}
}
