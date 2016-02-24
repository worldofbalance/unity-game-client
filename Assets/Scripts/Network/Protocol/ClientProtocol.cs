using System;
using System.IO;

public class ClientProtocol {

	public static NetworkRequest Prepare(string version, string session_id) {
		NetworkRequest request = new NetworkRequest(NetworkCode.CLIENT);
		request.AddString(version);
		request.AddString(session_id);
		
		return request;
	}
	
	public static NetworkResponse Parse(MemoryStream dataStream) {
		ResponseClient response = new ResponseClient();
		response.status = DataReader.ReadShort(dataStream);
		
		if (response.status == 0) {
			response.session_id = DataReader.ReadString(dataStream);
		}

		return response;
	}
}

public class ResponseClient : NetworkResponse {

	public short status { get; set; }
	public string session_id { get; set; }
	
	public ResponseClient() {
		protocol_id = NetworkCode.CLIENT;
	}
}
