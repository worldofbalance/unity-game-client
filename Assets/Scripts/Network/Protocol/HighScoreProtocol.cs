using System;
using System.IO;

public class HighScoreProtocol {
	
	public static NetworkRequest Prepare(short type) {
		NetworkRequest request = new NetworkRequest(NetworkCode.HIGH_SCORE);
		request.AddShort16(type);
		
		return request;
	}
	
	public static NetworkResponse Parse(MemoryStream dataStream) {
		ResponseHighScore response = new ResponseHighScore();
		response.type = DataReader.ReadShort(dataStream);

		return response;
	}
}

public class ResponseHighScore : NetworkResponse {

	public short type { get; set; }
	
	public ResponseHighScore() {
		protocol_id = NetworkCode.HIGH_SCORE;
	}
}
