using System;
using System.IO;

public class UpdateTimeProtocol {
	
	public static NetworkRequest Prepare() {
		NetworkRequest request = new NetworkRequest(NetworkCode.UPDATE_TIME);
		
		return request;
	}
	
	public static NetworkResponse Parse(MemoryStream dataStream) {
		ResponseUpdateTime response = new ResponseUpdateTime();
		response.day = DataReader.ReadInt(dataStream);
		response.time = DataReader.ReadInt(dataStream);
		response.rate = DataReader.ReadFloat(dataStream);

		return response;
	}
}

public class ResponseUpdateTime : NetworkResponse {

	public int day { get; set; }
	public int time { get; set; }
	public float rate { get; set; }
	
	public ResponseUpdateTime() {
		protocol_id = NetworkCode.UPDATE_TIME;
	}
}
