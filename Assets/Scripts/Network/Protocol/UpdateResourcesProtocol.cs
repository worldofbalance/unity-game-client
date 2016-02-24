using System;
using System.IO;

public class UpdateResourcesProtocol {
	
	public static NetworkRequest Prepare() {
		NetworkRequest request = new NetworkRequest(NetworkCode.UPDATE_RESOURCES);
		
		return request;
	}
	
	public static NetworkResponse Parse(MemoryStream dataStream) {
		ResponseUpdateResources response = new ResponseUpdateResources();
		response.type = DataReader.ReadShort(dataStream);
		response.amount = DataReader.ReadInt(dataStream);
		response.target = DataReader.ReadInt(dataStream);

		return response;
	}
}

public class ResponseUpdateResources : NetworkResponse {

	public short type { get; set; }
	public int amount { get; set; }
	public int target { get; set; }
	
	public ResponseUpdateResources() {
		protocol_id = NetworkCode.UPDATE_RESOURCES;
	}
}
