using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

public class ConvergeBetUpdateProtocol
{

	public static NetworkRequest Prepare(short betEntered, short round, int improveValue, int s0, int s1, int s2, int s3, int s4) {
		NetworkRequest request = new NetworkRequest(NetworkCode.MC_BET_UPDATE);

		request.AddShort16(betEntered);
		request.AddShort16(round);
		request.AddInt32(improveValue);
		request.AddInt32(s0);
		request.AddInt32(s1);
		request.AddInt32(s2);
		request.AddInt32(s3);
		request.AddInt32(s4);

		return request;
	}
	
	public static NetworkResponse Parse(MemoryStream dataStream) {
		ResponseConvergeBetUpdate response = new ResponseConvergeBetUpdate();
		
		using (BinaryReader br = new BinaryReader(dataStream, Encoding.UTF8)) {
			short roundComplete = br.ReadInt16 ();
			short winStatus = br.ReadInt16 ();
			short round = br.ReadInt16 ();
			int wonAmount = br.ReadInt32 ();
			int playerWinner = br.ReadInt32 ();

			// roundComplete = 1 -> Everyone bet or was dropped, 0 -> Not complete yet
			response.roundComplete = roundComplete;
			// winStatus = 1 -> won round, 0 -> lost round, or didn't play
			response.winStatus = winStatus;
			response.round = round;
			response.wonAmount = wonAmount;
			response.playerWinner = playerWinner;    // winner of the round
		}
		
		return response;
	}
}

public class ResponseConvergeBetUpdate : NetworkResponse {

	public short roundComplete { get; set; }
	public short winStatus { get; set; }
	public short round { get; set; }
	public int wonAmount { get; set; }
	public int playerWinner { get; set; }
	
	public ResponseConvergeBetUpdate() {
		protocol_id = NetworkCode.MC_BET_UPDATE;
	}

}

