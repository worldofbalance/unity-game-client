using System;
using System.IO;

public class MCSetupProtocol {
	
	public static NetworkRequest Prepare(int gameid, short totalPlayers, short numRounds, short secPerRound, short betAmt, 
			short ecoNum, short helps) {
		NetworkRequest request = new NetworkRequest(NetworkCode.MC_SETUP);
		request.AddInt32 (gameid);
		request.AddShort16 (totalPlayers);
		request.AddShort16 (numRounds);
		request.AddShort16 (secPerRound);
		request.AddShort16 (betAmt);
		request.AddShort16 (ecoNum);
		request.AddShort16 (helps);
		return request;
	}
	
	public static NetworkResponse Parse(MemoryStream dataStream) {
		var response = new ResponsePair();
		response.status = DataReader.ReadShort(dataStream);
		response.id = DataReader.ReadInt(dataStream);
		response.gameID = DataReader.ReadInt(dataStream);
		return response;
	}
}

public class ResponseMCSetup : NetworkResponse {
	
	public short status { get; set; }
	public int id { get; set; }
	public int gameID { get; set; }
	
	public ResponseMCSetup() {
		protocol_id = NetworkCode.PAIR;   // MultiplayerGames watches for PAIR response 
	}
}
