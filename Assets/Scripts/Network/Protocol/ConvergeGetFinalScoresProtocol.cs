using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

public class ConvergeGetFinalScoresProtocol
{
	public static NetworkRequest Prepare() {
		NetworkRequest request = new NetworkRequest(NetworkCode.MC_GET_FINAL_SCORES);
		
		return request;
	}

	// DH change

	public static NetworkResponse Parse(MemoryStream dataStream) {
		ResponseConvergeGetFinalScores response = new ResponseConvergeGetFinalScores();
		
		using (BinaryReader br = new BinaryReader(dataStream, Encoding.UTF8)) {
			response.status = br.ReadInt16 ();
			for (int i = 0; i < 5; i++) {
				response.playerId[i] = br.ReadInt32 ();
				response.playerWinnings[i] = br.ReadInt32 ();
				response.playerLastImprove[i] = br.ReadInt32 ();
			}
		}		
		return response;
	}
}

public class ResponseConvergeGetFinalScores : NetworkResponse {

	public short status { get; set; }
	public int[] playerId { get; set; }
	public int[] playerWinnings { get; set; }
	public int[] playerLastImprove { get; set; }

	public ResponseConvergeGetFinalScores() {
		protocol_id = NetworkCode.MC_GET_FINAL_SCORES;

		playerId = new int[5];
		playerWinnings = new int[5];
		playerLastImprove = new int[5];
	}

}

