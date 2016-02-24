using UnityEngine;
using System.Collections;
using System;
using System.IO;

namespace CW{
public class DealCardProtocol {
	
	public static NetworkRequest Prepare(int playerID, int handPosition) {
		NetworkRequest request = new NetworkRequest(NetworkCode.DEAL_CARD);
		request.AddInt32(playerID);
		request.AddInt32(handPosition);
		Debug.Log("Deal Card , playerID: " + playerID);
		return request;
	}
	
	public static NetworkResponse Parse(MemoryStream dataStream) {
		ResponseDealCard response = new ResponseDealCard();
		response.status = DataReader.ReadShort(dataStream);

		return response;
	}
}


public class ResponseDealCard : NetworkResponse {
	
	public short status {get ; set; }
	
	public ResponseDealCard() {
		protocol_id = NetworkCode.DEAL_CARD;
	}
}
}