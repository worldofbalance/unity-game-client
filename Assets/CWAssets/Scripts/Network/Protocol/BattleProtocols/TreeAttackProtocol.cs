using UnityEngine;
using System.Collections;
using System;
using System.IO;
namespace CW{
public class TreeAttackProtocol {
	
	public static NetworkRequest Prepare(int playerID, int attackersPosition) {
		
		NetworkRequest request = new NetworkRequest(NetworkCode.TREE_ATTACK);
		request.AddInt32(playerID);
		request.AddInt32(attackersPosition);
		return request;
	}
	
	public static NetworkResponse Parse(MemoryStream dataStream) {
		ResponseTreeAttack response = new ResponseTreeAttack();
		response.status = DataReader.ReadShort(dataStream);
	
		return response;
	}
}

public class ResponseTreeAttack : NetworkResponse {
	public short status {get ; set; }
	
	public ResponseTreeAttack() {
		protocol_id = NetworkCode.TREE_ATTACK;
	}
}
}