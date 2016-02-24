using System;
using System.IO;
namespace CW{
public class MessageProtocol {

	public static NetworkRequest Prepare(short type, string message) {
		NetworkRequest request = new NetworkRequest(NetworkCode.MESSAGE);
		request.AddShort16(type);
		request.AddString(message);
		
		return request;
	}
	
	public static NetworkResponse Parse(MemoryStream dataStream) {
		ResponseMessage response = new ResponseMessage();
		response.status = DataReader.ReadShort(dataStream);
		
		if (response.status == 0) {
			response.type = DataReader.ReadShort(dataStream);
			
			if (response.type == 0) {
				response.username = DataReader.ReadString(dataStream);
			}
			
			response.message = DataReader.ReadString(dataStream);
		}

		return response;
	}
}

public class ResponseMessage : NetworkResponse {

	public short status { get; set; }
	public short type { get; set; }
	public string username { get; set; }
	public string message { get; set; }
	
	public ResponseMessage() {
		protocol_id = NetworkCode.MESSAGE;
	}
}
}