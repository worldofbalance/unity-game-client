using System;
using System.IO;

public class PairProtocol {
	
	public static NetworkRequest Prepare(int gameid, int pairParam) {
		NetworkRequest request = new NetworkRequest(NetworkCode.PAIR);
		request.AddInt32 (gameid);
		request.AddInt32 (pairParam);
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

public class ResponsePair : NetworkResponse {
	
	public short status { get; set; }
	public int id { get; set; }
	public int gameID { get; set; }
	
	public ResponsePair() {
		protocol_id = NetworkCode.PAIR;
	}
}
