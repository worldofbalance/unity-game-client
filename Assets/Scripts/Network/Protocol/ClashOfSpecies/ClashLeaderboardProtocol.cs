using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
public class ClashLeaderboardProtocol
{
	public static NetworkRequest Prepare() {
		NetworkRequest request = new NetworkRequest (NetworkCode.CLASH_LEADERBOARD);
		return request;
	}

	public static NetworkResponse Parse(MemoryStream dataStream) {

		//same deal as ClashSpeciesListProtocol
		ResponseClashLeaderboard response = new ResponseClashLeaderboard();
		int count = DataReader.ReadInt(dataStream);
		Debug.Log ("Leaderboard: "+ count);
		for(int i = 0; i < count; i++){
			string playerName = DataReader.ReadString(dataStream);
			int playerWins = DataReader.ReadInt(dataStream);
			int playerLoses = DataReader.ReadInt(dataStream);
			response.addMatch (i+1, playerName, playerWins.ToString(), playerLoses.ToString());
		}

		return response;
	}
}

public class ResponseClashLeaderboard : NetworkResponse {
	/// <summary>
	/// Gets or sets the players.
	/// </summary>
	/// <value>The players dictionary with player ids as keys and names as values.</value>
	public Dictionary<int, string> playerNames {get; set;}
	public Dictionary<int, string> playerWinsNo {get; set;}
	public Dictionary<int, string> playerLosesNo {get; set;}


	/// <summary>
	/// Adds a player to the dictioanary
	/// </summary>
	/// <param name="player_id">Player id.</param>
	/// <param name="player_name">Player name.</param>
	public void addMatch(int ranking, string playerName, string playerWins, string playerLoses){
		playerNames.Add(ranking, playerName);
		playerWinsNo.Add(ranking, playerWins);
		playerLosesNo.Add(ranking, playerLoses);	
	}

	public ResponseClashLeaderboard() {
		protocol_id = NetworkCode.CLASH_LEADERBOARD;
		playerNames = new Dictionary<int, string>();
		playerWinsNo = new Dictionary<int, string>();
		playerLosesNo = new Dictionary<int, string>();

	}
}