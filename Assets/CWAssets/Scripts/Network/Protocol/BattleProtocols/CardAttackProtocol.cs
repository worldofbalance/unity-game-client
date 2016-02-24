using UnityEngine;
using System.Collections;
using System;
using System.IO;
namespace CW{
public class CardAttackProtocol {
	
	public static NetworkRequest Prepare(int playerID, int attackersPosition, int attackedPosition) {

		NetworkRequest request = new NetworkRequest(NetworkCode.CARD_ATTACK);
		request.AddInt32(playerID);
		request.AddInt32(attackersPosition);
		request.AddInt32(attackedPosition);
		return request;
	}
	
	public static NetworkResponse Parse(MemoryStream dataStream) {
		ResponseCardAttack response = new ResponseCardAttack();
		
		response.status = DataReader.ReadShort(dataStream);
		
		return response;
	}
}


public class ResponseCardAttack : NetworkResponse {
	
	public short status {get ; set; }

	public ResponseCardAttack() {
		protocol_id = NetworkCode.CARD_ATTACK;
	}
}
}