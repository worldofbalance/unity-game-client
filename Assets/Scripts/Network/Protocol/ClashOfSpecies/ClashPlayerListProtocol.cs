using System;
using System.Collections.Generic;
using System.IO;

/// <summary>
/// Gets a list of players (other than oneself) that have a defense setup
/// for Clash of Species
/// </summary>
public class ClashPlayerListProtocol{
	
	public static NetworkRequest Prepare() {
		NetworkRequest request = new NetworkRequest(NetworkCode.CLASH_PLAYER_LIST);
		return request;
	}

	/// <summary>
	/// Reads the player list into the response object
	/// </summary>
	/// <param name="dataStream">The input stream.</param>
	public static NetworkResponse Parse(MemoryStream dataStream) {

		//same deal as ClashSpeciesListProtocol
		ResponseClashPlayerList response = new ResponseClashPlayerList();
		int count = DataReader.ReadInt(dataStream);
		for(int i = 0; i < count; i++){
			int pid = DataReader.ReadInt(dataStream);
			string pname = DataReader.ReadString(dataStream);
			response.players.Add(pid, pname);
		}

		return response;
	}
}

/// <summary>
/// Stores a list of players received from server
/// </summary>
public class ResponseClashPlayerList : NetworkResponse {
	/// <summary>
	/// Gets or sets the players.
	/// </summary>
	/// <value>The players dictionary with player ids as keys and names as values.</value>
	public Dictionary<int, string> players {get; set;}

	/// <summary>
	/// Adds a player to the dictioanary
	/// </summary>
	/// <param name="player_id">Player id.</param>
	/// <param name="player_name">Player name.</param>
	public void addPlayer(int player_id, string player_name){
		players.Add(player_id, player_name);
	}

	public ResponseClashPlayerList() {
		protocol_id = NetworkCode.CLASH_PLAYER_LIST;
		players = new Dictionary<int, string >();
	}
}
