using System;
using System.IO;

public class UpdateEnvScoreProtocol {
	
	public static NetworkRequest Prepare() {
		NetworkRequest request = new NetworkRequest(NetworkCode.UPDATE_ENV_SCORE);
		
		return request;
	}
	
	public static NetworkResponse Parse(MemoryStream dataStream) {
		ResponseUpdateEnvScore response = new ResponseUpdateEnvScore();
		response.env_id = DataReader.ReadInt(dataStream);
		response.score = DataReader.ReadInt(dataStream);

		return response;
	}
}

public class ResponseUpdateEnvScore : NetworkResponse {

	public int env_id { get; set; }
	public int score { get; set; }
	
	public ResponseUpdateEnvScore() {
		protocol_id = NetworkCode.UPDATE_ENV_SCORE;
	}
}
