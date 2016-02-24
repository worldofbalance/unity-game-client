using System;
using System.IO;

public class ZoneUpdateProtocol {
	
	public static NetworkRequest Prepare(int zone_id, int player_id, int vegetation_capacity, int natural_event) {
		NetworkRequest request = new NetworkRequest(NetworkCode.ZONE_UPDATE);
		request.AddInt32(zone_id);
//		request.addInt32(Constants.USER_ID);
		request.AddInt32(player_id);
		request.AddInt32(vegetation_capacity);
		request.AddInt32(natural_event);
		
		return request;
	}
	
	public static NetworkResponse Parse(MemoryStream dataStream) {
		ResponseZoneUpdate response = new ResponseZoneUpdate();
		response.status = DataReader.ReadShort(dataStream);

		if (response.status == 0) {
			response.tile_id = DataReader.ReadInt(dataStream);
			response.owner_id = DataReader.ReadInt(dataStream);
			response.status_msg = DataReader.ReadString(dataStream);
		} else {
			response.status_msg = DataReader.ReadString(dataStream);
		}

		return response;
	}
}

public class ResponseZoneUpdate : NetworkResponse {

	public short status { get; set; }
	public string status_msg { get; set; }
	public int tile_id { get; set; }
	public int owner_id { get; set; }
	
	public ResponseZoneUpdate() {
		protocol_id = NetworkCode.ZONE_UPDATE;
	}
}
