using System;
using System.IO;
using System.Collections.Generic;

public class SpeciesInfoProtocol {
	
	public static NetworkRequest Prepare() {
		NetworkRequest request = new NetworkRequest(NetworkCode.SPECIES_INFO);
		
		return request;
	}
	
	public static NetworkResponse Parse(MemoryStream dataStream) {
		ResponseSpeciesInfo response = new ResponseSpeciesInfo();
		response.zoneX = DataReader.ReadInt(dataStream);
		response.zoneY = DataReader.ReadInt(dataStream);

		int count = DataReader.ReadInt(dataStream);
		response.speciesIds = new List<int> ();
		for (int idx = 0; idx < count; idx++) {
			response.speciesIds.Add (DataReader.ReadInt(dataStream));
		}

		return response;
	}
}

public class ResponseSpeciesInfo : NetworkResponse {

	public int zoneX { get; set; }
	public int zoneY { get; set; }
	public List<int> speciesIds { get; set; }
	
	public ResponseSpeciesInfo() {
		protocol_id = NetworkCode.SPECIES_INFO;
	}
}
