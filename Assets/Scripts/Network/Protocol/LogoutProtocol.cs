using System;
using System.IO;

public class LogoutProtocol {
	
	public static NetworkRequest Prepare(short type) {
		NetworkRequest request = new NetworkRequest(NetworkCode.LOGOUT);
		request.AddShort16(type);
		
		return request;
	}
	
	public static NetworkResponse Parse(MemoryStream dataStream) {
		ResponseLogout response = new ResponseLogout();
		response.type = DataReader.ReadShort(dataStream);
		response.status = DataReader.ReadShort(dataStream);
		response.playerId = DataReader.ReadInt(dataStream);

		return response;
	}
}

public class ResponseLogout : NetworkResponse {

	public short type { get; set; }
	public short status { get; set; }
	public int playerId { get; set; }
	
	public ResponseLogout() {
		protocol_id = NetworkCode.LOGOUT;
	}
}
