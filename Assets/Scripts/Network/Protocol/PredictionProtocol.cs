using System;
using System.Collections.Generic;
using System.IO;

public class PredictionProtocol : NetworkResponse {

	public static NetworkRequest Prepare() {
		NetworkRequest request = new NetworkRequest(NetworkCode.PREDICTION);
		
		return request;
	}
	
	public static NetworkResponse Parse(MemoryStream dataStream) {
		ResponsePrediction response = new ResponsePrediction();
		response.status = DataReader.ReadShort(dataStream);
		
		if (response.status == 0) {
			Dictionary<int, int> results = new Dictionary<int, int>();

			short size = DataReader.ReadShort(dataStream);
			
			for (int i = 0; i < size; i++) {
				int species_id = DataReader.ReadInt(dataStream);
				int change = DataReader.ReadInt(dataStream);
				
				results.Add(species_id, change);
			}

			response.results = results;
		}

		return response;
	}
}

public class ResponsePrediction : NetworkResponse {
	public short status { get; set; }
	public Dictionary<int, int> results { get; set; }
	
	public ResponsePrediction() {
		protocol_id = NetworkCode.PREDICTION;
	}
}
