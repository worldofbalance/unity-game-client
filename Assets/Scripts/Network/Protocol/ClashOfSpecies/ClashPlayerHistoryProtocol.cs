using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
public class ClashPlayerHistoryProtocol
{
	public static NetworkRequest Prepare() {
		NetworkRequest request = new NetworkRequest (NetworkCode.CLASH_PLAYER_HISTORY);
		return request;
	}

	public static NetworkResponse Parse(MemoryStream dataStream) {

		//same deal as ClashSpeciesListProtocol
		ResponseClashPlayerHistory response = new ResponseClashPlayerHistory();
		int count = DataReader.ReadInt(dataStream);
		for(int i = 0; i < count; i++){
			int gameId = DataReader.ReadInt(dataStream);
			string playerName = DataReader.ReadString(dataStream);
			string opponentName = DataReader.ReadString(dataStream);
			string matchResult = DataReader.ReadString(dataStream);
			string playDate = DataReader.ReadString(dataStream);
			Debug.Log ("Game ID: " + gameId);
			response.addMatch (gameId, playerName, opponentName, matchResult, playDate);
		}

		return response;
	}
}

public class ResponseClashPlayerHistory : NetworkResponse {
	/// <summary>
	/// Gets or sets the players.
	/// </summary>
	/// <value>The players dictionary with player ids as keys and names as values.</value>
	public Dictionary<int, string> playerNames {get; set;}
	public Dictionary<int, string> opponentNames {get; set;}
	public Dictionary<int, string> matchResults {get; set;}
	public Dictionary<int, string> playDates {get; set;}


	/// <summary>
	/// Adds a player to the dictioanary
	/// </summary>
	/// <param name="player_id">Player id.</param>
	/// <param name="player_name">Player name.</param>
	public void addMatch(int gameId, string playerName, string opponentName, string matchResult, string playDate){
		playerNames.Add(gameId, playerName);
		opponentNames.Add(gameId, opponentName);
		matchResults.Add(gameId, matchResult);
		playDates.Add(gameId, playDate);	
	}

	public ResponseClashPlayerHistory() {
		protocol_id = NetworkCode.CLASH_PLAYER_HISTORY;
		playerNames = new Dictionary<int, string>();
		opponentNames = new Dictionary<int, string>();
		matchResults = new Dictionary<int, string>();
		playDates = new Dictionary<int, string>();

	}
}