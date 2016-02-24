using UnityEngine;
using System.Collections;
using System;
using System.IO;

namespace CW{
public class GetDeckProtocol {
	
	public static NetworkRequest Prepare(int playerID) {
		
		NetworkRequest request = new NetworkRequest(NetworkCode.GET_DECK);
		request.AddInt32(playerID);
		return request;
	}
	
	public static NetworkResponse Parse(MemoryStream dataStream) {
		ResponseGetDeck response = new ResponseGetDeck();
		response.numCards =  DataReader.ReadInt(dataStream);
		response.numFields = DataReader.ReadInt (dataStream);

		for (int i = 0; i < response.numCards; i++){

			int cardID = DataReader.ReadInt (dataStream);
			int health = DataReader.ReadInt (dataStream);
			int attack = DataReader.ReadInt (dataStream);
			int level = DataReader.ReadInt (dataStream);
			int dietType = DataReader.ReadInt(dataStream);
			string speciesName = DataReader.ReadString(dataStream);
			string description = DataReader.ReadString (dataStream);
			response.deck.pushCard(new CardData(cardID, health, attack, level, 
			                                    dietType, speciesName, description));

		}
		response.deck.setBuilt(true);
		return response;
	}
}


public class ResponseGetDeck: NetworkResponse {
	// update numFields if data type added to Cards
	public int numFields {get; set;}
	public int numCards {get ; set; }
	//public ArrayList deck {get; set;}
	public DeckData deck{get; set;}

	public ResponseGetDeck() {
		deck = new DeckData();
		protocol_id = NetworkCode.GET_DECK;
	}
}
}