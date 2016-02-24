// tells the server that we've finished playing a game (and won),
// so server can update credits

// request parameters:
// game_id - the game to be played
//		0 - converge
//		1 - don't eat me
//		2 - clash of species
//		3 - running rhino
//		4 - cards of wild
// credits - if don't eat me, # of credits won
//
// response parameters:
// creditDiff - number of credits that were added

using UnityEngine;
using System;
using System.IO;

public class EndGameProtocol {
	
	public static NetworkRequest Prepare(short game_id, int credits) {
		NetworkRequest request = new NetworkRequest(NetworkCode.END_GAME);
		request.AddShort16(game_id);

		if (game_id == 1) {
			request.AddInt32(credits);
		}
		
		return request;
	}
	
	public static NetworkResponse Parse(MemoryStream dataStream) {
		ResponseEndGame response = new ResponseEndGame();
		response.creditDiff = DataReader.ReadInt(dataStream);
		
		return response;
	}
}

public class ResponseEndGame : NetworkResponse {
	public int creditDiff { get; set; }
	
	public ResponseEndGame() {
		protocol_id = NetworkCode.END_GAME;
	}
}