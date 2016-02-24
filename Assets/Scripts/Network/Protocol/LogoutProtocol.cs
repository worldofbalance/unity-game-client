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
		response.status = DataReader.ReadShort(dataStream);

		return response;
	}
}

public class ResponseLogout : NetworkResponse {

	public short status { get; set; }
	
	public ResponseLogout() {
		protocol_id = NetworkCode.LOGOUT;
	}
}
