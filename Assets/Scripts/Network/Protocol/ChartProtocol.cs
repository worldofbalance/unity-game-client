using System;
using System.IO;

public class ChartProtocol {
	
	public static NetworkRequest Prepare(short type) {
		NetworkRequest request = new NetworkRequest(NetworkCode.CHART);
		request.AddShort16(type);
		
		return request;
	}
	
	public static NetworkResponse Parse(MemoryStream dataStream) {
		ResponseChart response = new ResponseChart();
		response.type = DataReader.ReadShort(dataStream);

		return response;
	}
}

public class ResponseChart : NetworkResponse {

	public short type { get; set; }
	
	public ResponseChart() {
		protocol_id = NetworkCode.CHART;
	}
}
