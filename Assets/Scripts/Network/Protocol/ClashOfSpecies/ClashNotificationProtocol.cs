using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
public class ClashNotificationProtocol
{
	public static NetworkRequest Prepare() {
		NetworkRequest request = new NetworkRequest (NetworkCode.CLASH_NOTIFICATION);
		return request;
	}

	public static NetworkResponse Parse(MemoryStream dataStream) {

		//same deal as ClashSpeciesListProtocol
		ResponseClashNotification response = new ResponseClashNotification();
		int count = DataReader.ReadInt(dataStream);
		for(int i = 0; i < count; i++){
			string playerName = DataReader.ReadString(dataStream);
			string matchResult = DataReader.ReadString(dataStream);
			string playDate = DataReader.ReadString(dataStream);
			response.addEntry (i, playerName, matchResult, playDate);
		}

		return response;
	}
}

public class ResponseClashNotification : NetworkResponse {
	/// <summary>
	/// Gets or sets the players.
	/// </summary>
	/// <value>The players dictionary with player ids as keys and names as values.</value>
	public Dictionary<int, string> playerNames {get; set;}
	public Dictionary<int, string> matchResults {get; set;}
	public Dictionary<int, string> playDates {get; set;}


	/// <summary>
	/// Adds a player to the dictioanary
	/// </summary>
	/// <param name="player_id">Player id.</param>
	/// <param name="player_name">Player name.</param>
	public void addEntry(int gameId, string playerName, string matchResult, string playDate){
		playerNames.Add(gameId, playerName);
		matchResults.Add(gameId, matchResult);
		playDates.Add(gameId, playDate);	
	}

	public ResponseClashNotification() {
		protocol_id = NetworkCode.CLASH_NOTIFICATION;
		playerNames = new Dictionary<int, string>();
		matchResults = new Dictionary<int, string>();
		playDates = new Dictionary<int, string>();
	}
}