using UnityEngine;

using System;
using System.Collections.Generic;
using System.IO;

public class ShopActionProtocol {
	
	public static NetworkRequest Prepare(short type, Dictionary<int, int> cartList, int totalCost) {
		NetworkRequest request = new NetworkRequest(NetworkCode.SHOP_ACTION);
		Debug.Log("ShopActionProtocol, type/count = " + type + " " + cartList.Count);
		request.AddShort16(type);
		request.AddShort16((short) cartList.Count);
		request.AddInt32(totalCost);
		
		foreach (KeyValuePair<int, int> entry in cartList) {
			request.AddInt32(entry.Key);
			request.AddInt32(entry.Value);
			Debug.Log("ShopActionProtocol, id/biomass:" + entry.Key + " " + entry.Value);
		}

		return request;
	}

	public static NetworkResponse Parse(MemoryStream dataStream) {
		ResponseShopAction response = new ResponseShopAction();

		response.action = DataReader.ReadShort (dataStream);
		response.status = DataReader.ReadShort (dataStream);
		response.credits = DataReader.ReadInt (dataStream);

		return response;
	}
}

public class ResponseShopAction : NetworkResponse {

	public short action { get; set; }
	public short status { get; set; }
	public int credits { get; set; }
	
	public ResponseShopAction() {
		protocol_id = NetworkCode.SHOP_ACTION;
	}
}
