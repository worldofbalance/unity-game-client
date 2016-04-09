using UnityEngine;
using System.Collections;
using System;
using System.IO;
namespace CW{
public class SummonCardProtocol {
	
	// Card Constructor
	//public void init(BattlePlayer player, int cardID, int diet, 
	// int level, int attack, int health,string species_name, string type, string description
	
	public static NetworkRequest Prepare(int playerID, int cardID, string diet, 
	                                     int level, int attack, int health, 
	                                     string species_name, string type, 
	                                     string description) {
		NetworkRequest request = new NetworkRequest(NetworkCode.SUMMON_CARD);
		request.AddInt32(playerID);
		request.AddInt32(cardID);
		request.AddString (diet);
		request.AddInt32(level);
		request.AddInt32(attack);
		request.AddInt32 (health);
		request.AddString (species_name);

		// These two not used
		request.AddString (type);
		request.AddString (description);

		return request;
	}
	
	public static NetworkResponse Parse(MemoryStream dataStream) {
		ResponseSummonCard response = new ResponseSummonCard();
		
		return response;
	}
}

public class ResponseSummonCard : NetworkResponse {
	
	public short status {get ; set; }
	
	
	public ResponseSummonCard() {
		protocol_id = NetworkCode.SUMMON_CARD;
	}
}
}