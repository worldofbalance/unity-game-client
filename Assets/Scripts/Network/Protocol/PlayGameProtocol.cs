// sends a request to play a game
// server checks if the player has enough currency,
// then either deducts that currency and sends a success message
// or if they don't have enough, sends a fail message

// request parameters:
// game_id - the game to be played
//		0 - converge
//		1 - don't eat me
//		2 - clash of species
//		3 - running rhino
//		4 - cards of wild
//
// response parameters:
// status - the status of your request
//		0 - failed - not enough currency
//		1 - success
// creditDiff - if status==1, number of credits that were subtracted (always positive)

using UnityEngine;
using System;
using System.IO;

public class PlayGameProtocol {
	
	public static NetworkRequest Prepare(short game_id) {
		NetworkRequest request = new NetworkRequest(NetworkCode.PLAY_GAME);
		request.AddShort16(game_id);
		
		return request;
	}
	
	public static NetworkResponse Parse(MemoryStream dataStream) {
		ResponsePlayGame response = new ResponsePlayGame();
		response.status = DataReader.ReadShort(dataStream);

		if (response.status == 1) {
			response.creditDiff = DataReader.ReadInt(dataStream);
		}

		return response;
	}
}

public class ResponsePlayGame : NetworkResponse {
	
	public short status { get; set; }
	public int creditDiff { get; set; }
	
	public ResponsePlayGame() {
		protocol_id = NetworkCode.PLAY_GAME;
	}
}
