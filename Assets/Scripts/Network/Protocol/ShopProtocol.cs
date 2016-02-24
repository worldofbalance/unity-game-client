using System;
using System.IO;

public class ShopProtocol {
	
	public static NetworkRequest Prepare(short type) {
		NetworkRequest request = new NetworkRequest(NetworkCode.SHOP);
//		request.addShort16(0);
		request.AddShort16(type);
		
		return request;
	}
	
	public static NetworkResponse Parse(MemoryStream dataStream) {
		ResponseShop response = new ResponseShop();
//		short action = DataReader.ReadShort(dataStream);
//		short status = DataReader.ReadShort(dataStream);
//		short type = DataReader.ReadShort(dataStream);
		
		short size = DataReader.ReadShort(dataStream);
		response.config = new string[size];
		
		/*for (int i = 0; i < size; i++) {
			config[i] = DataReader.ReadString(dataStream);
		}
		
		string species = DataReader.ReadString(dataStream);
		speciesList = Array.ConvertAll(species.Split(','), new Converter<string, int>(int.Parse));*/
		
		for (int i = 0; i < size; i++) {
			response.speciesList[i] = DataReader.ReadInt(dataStream);
		}

		return response;
	}
}

public class ResponseShop : NetworkResponse {

//	public short action { get; set; }
//	public short status { get; set; }
//	public short type { get; set; }
	public string[] config { get; set; }
	public int[] speciesList { get; set; }
	
	public ResponseShop() {
		protocol_id = NetworkCode.SHOP;
	}
}
