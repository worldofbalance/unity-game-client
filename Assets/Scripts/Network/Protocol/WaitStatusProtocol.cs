/*
 * WaitStatus protocol
 * Asks server if anyone has invited us to a game yet
 * if invited, client should then connect to the appropriate server
 * response parameters:
 * status - 1 if invited, 0 if not invited, -1 if you're not currently on a wait list
 * id - game id -- so minigame server can place you with the right partner
 */

using UnityEngine;
using System;
using System.IO;

public class WaitStatusProtocol {
	
	public static NetworkRequest Prepare() {
		NetworkRequest request = new NetworkRequest(NetworkCode.WAITSTATUS);

		return request;
	}
	
	public static NetworkResponse Parse(MemoryStream dataStream) {
		ResponseWaitStatus response = new ResponseWaitStatus();
		response.status = DataReader.ReadInt(dataStream);
		response.id = DataReader.ReadInt(dataStream);
		return response;
	}
}

public class ResponseWaitStatus : NetworkResponse {
	
	public int status { get; set; }
	public int id { get; set; }
	
	public ResponseWaitStatus() {
		protocol_id = NetworkCode.WAITFORGAME;
	}
}
