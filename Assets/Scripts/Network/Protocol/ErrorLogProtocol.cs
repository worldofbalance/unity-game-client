using System;
using System.IO;

public class ErrorLogProtocol {
	
	public static NetworkRequest Prepare(string message) {
		NetworkRequest request = new NetworkRequest(NetworkCode.ERROR_LOG);
		request.AddString(message);
		
		return request;
	}
	
	public static NetworkResponse Parse(MemoryStream dataStream) {
		ResponseErrorLog response = new ResponseErrorLog();
		response.status = DataReader.ReadShort(dataStream);

		return response;
	}
}

public class ResponseErrorLog : NetworkResponse {

	public short status { get; set; }
	
	public ResponseErrorLog() {
		protocol_id = NetworkCode.ERROR_LOG;
	}
}
