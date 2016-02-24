/*
 * WaitingList protocol
 * Fetch the waiting list for a specific game
 * request parameters:
 * gameType - which game
 * response parameters:
 * series of strings (player name) and ints (player id) terminated by an empty string and id of -1
 */

using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;

public class WaitingListProtocol {

	/* which game the client wants list for
     * 0 - Don't eat me! (no server required)
     * 1 - Cards of the Wild
     * 2 - Running Rhino
     * 3 - Clash of Species
     */
	
	public static NetworkRequest Prepare(int gameType) {
		NetworkRequest request = new NetworkRequest(NetworkCode.WAITLIST);
		request.AddInt32(gameType);
		return request;
	}
	
	public static NetworkResponse Parse(MemoryStream dataStream) {
		ResponseWaitingList response = new ResponseWaitingList();
		String tempName;
		int tempId;

		//response contains series of strings and ints (player name and id) terminated by an empty string and id of -1

		tempName = DataReader.ReadString(dataStream);
		tempId = DataReader.ReadInt(dataStream);

		while (tempName != "" || tempId != -1) {
			response.nameQueue.Enqueue(tempName);
			response.idQueue.Enqueue(tempId);

			tempName = DataReader.ReadString(dataStream);
			tempId = DataReader.ReadInt(dataStream);
		}
		
		return response;
	}
}

public class ResponseWaitingList : NetworkResponse {

	public Queue<String> nameQueue = new Queue<String>();
	public Queue<int> idQueue = new Queue<int>();
	
	public ResponseWaitingList() {
		protocol_id = NetworkCode.WAITLIST;
	}

}
