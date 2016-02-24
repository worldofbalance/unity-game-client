using System;
using System.IO;

public class WorldProtocol {
	
	public static NetworkRequest Prepare() {
		NetworkRequest request = new NetworkRequest(NetworkCode.WORLD);
		
		return request;
	}
	
	public static NetworkResponse Parse(MemoryStream dataStream) {
		ResponseWorld response = new ResponseWorld();
		response.status = DataReader.ReadShort(dataStream);
		
		if (response.status == 0) {
			response.world_id = DataReader.ReadInt(dataStream);
			response.name = DataReader.ReadString(dataStream);
			response.type = DataReader.ReadShort(dataStream);
			response.time_rate = DataReader.ReadFloat(dataStream);
			response.day = DataReader.ReadShort(dataStream);

			World world = new World(response.world_id);
			world.name = response.name;
			world.day = response.day;
			
			response.world = world;
		}

		return response;
	}
}

public class ResponseWorld : NetworkResponse {

	public short status { get; set; }
	public int world_id { get; set; }
	public string name { get; set; }
	public short type { get; set; }
	public float time_rate { get; set; }
	public short day { get; set; }
	public World world { get; set; }
	
	public ResponseWorld() {
		protocol_id = NetworkCode.WORLD;
	}
}
