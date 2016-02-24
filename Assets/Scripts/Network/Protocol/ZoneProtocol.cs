using System;
using System.IO;

public class ZoneProtocol {
	
	public static NetworkRequest Prepare(int zone_id, int user_id) {
		NetworkRequest request = new NetworkRequest(NetworkCode.ZONE);
		request.AddInt32(zone_id);
		request.AddInt32(user_id);
		
		return request;
	}
	
	public static NetworkResponse Parse(MemoryStream dataStream) {
		ResponseZone response = new ResponseZone();
		response.status = DataReader.ReadShort(dataStream);
		response.status_msg = DataReader.ReadString (dataStream);

		return response;
	}
}

public class ResponseZone : NetworkResponse {

	public short status { get; set; }
	public string status_msg { get; set; }
	
	public ResponseZone() {
		protocol_id = NetworkCode.ZONE;
	}
}
