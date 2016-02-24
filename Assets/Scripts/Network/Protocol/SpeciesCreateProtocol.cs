using System;
using System.IO;

public class SpeciesCreateProtocol {
	
	public static NetworkRequest Prepare() {
		NetworkRequest request = new NetworkRequest(NetworkCode.SPECIES_CREATE);
		
		return request;
	}
	
	public static NetworkResponse Parse(MemoryStream dataStream) {
		ResponseSpeciesCreate response = new ResponseSpeciesCreate();
		response.status = DataReader.ReadShort(dataStream);
		
		if (response.status == 0) {
			response.eco_id = DataReader.ReadInt(dataStream);
			response.group_id = DataReader.ReadInt(dataStream);
			response.species_id = DataReader.ReadInt(dataStream);
			response.name = DataReader.ReadString(dataStream);
			response.model_id = DataReader.ReadInt(dataStream);
			response.biomass = DataReader.ReadInt(dataStream);
			response.x = DataReader.ReadFloat(dataStream);
			response.y = DataReader.ReadFloat(dataStream);
			response.z = DataReader.ReadFloat(dataStream);
			response.user_id = DataReader.ReadInt(dataStream);
		}

		return response;
	}
}

public class ResponseSpeciesCreate : NetworkResponse {

	public short status { get; set; }
	public int eco_id { get; set; }
	public int group_id { get; set; }
	public int species_id { get; set; }
	public string name { get; set; }
	public int model_id { get; set; }
	public int biomass { get; set; }
	public float x { get; set; }
	public float y { get; set; }
	public float z { get; set; }
	public int user_id { get; set; }
	
	public ResponseSpeciesCreate() {
		protocol_id = NetworkCode.SPECIES_CREATE;
	}
}
