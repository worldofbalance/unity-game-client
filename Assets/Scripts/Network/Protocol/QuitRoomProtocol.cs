using System;
using System.IO;

public class QuitRoomProtocol {
	public static NetworkRequest Prepare() {
		NetworkRequest request = new NetworkRequest(NetworkCode.QUIT_ROOM);
		return request;
	}
	
	public static NetworkResponse Parse(MemoryStream dataStream) {
		var response = new ResponseQuitRoom();
		response.status = DataReader.ReadShort(dataStream);
		return response;
	}
}

public class ResponseQuitRoom : NetworkResponse {
	public short status { get; set; }

	public ResponseQuitRoom() {
		protocol_id = NetworkCode.QUIT_ROOM;
	}
}