using System;
using System.Collections.Generic;
using System.IO;

/// <summary>
/// Sent when battle begins
/// </summary>
public class ClashInitiateBattleProtocol {

	/// <summary>
	/// Generates a request with the following format:
	/// 	id of this protocol (short)
	/// 	id of player to be attacked (int)
	/// 	# of species in attack config (int)
	/// 	for each species in attack config:
	/// 		species id (int)
	/// </summary>
	/// <param name="otherPlayerID">the player to attack</param>
	/// <param name="species">list of species ids to use in attack</param>
	public static NetworkRequest Prepare(int otherPlayerID, List<int> species) {
		NetworkRequest request = new NetworkRequest(NetworkCode.CLASH_INITIATE_BATTLE);
		request.AddInt32(otherPlayerID);
		request.AddInt32(species.Count);
		foreach(int s in species){
			request.AddInt32(s);
		}
		
		return request;
	}

	/// <summary>
	/// Reads in validity of the submitted attack config from server
	/// </summary>
	/// <param name="dataStream">the input stream</param>
	public static NetworkResponse Parse(MemoryStream dataStream) {
		ResponseClashInitiateBattle response = new ResponseClashInitiateBattle();

		response.valid = DataReader.ReadBool(dataStream);

		return response;
	}
}

/// <summary>
/// Stores the validity of attack config received from server
/// </summary>
public class ResponseClashInitiateBattle : NetworkResponse {

	/// <summary>
	/// Gets or sets whether the submitted attack config is valid.
	/// </summary>
	/// <value><c>true</c> if valid; otherwise, <c>false</c>.</value>
	public bool valid {get; set;}

	public ResponseClashInitiateBattle() {
		protocol_id = NetworkCode.CLASH_INITIATE_BATTLE;
	}
}
