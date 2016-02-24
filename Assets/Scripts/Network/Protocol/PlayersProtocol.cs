using UnityEngine;

using System;
using System.Collections.Generic;
using System.IO;

public class PlayersProtocol {

	public static NetworkRequest Prepare() {
		NetworkRequest request = new NetworkRequest(NetworkCode.PLAYERS);
		
		return request;
	}
	
	public static NetworkResponse Parse(MemoryStream dataStream) {
		ResponsePlayers response = new ResponsePlayers();

		Dictionary<int, Player> playerList = new Dictionary<int, Player>();

		int size = DataReader.ReadShort(dataStream);
		for (int i = 0; i < size; i++) {
			int player_id = DataReader.ReadInt(dataStream);

			Player player = new Player(player_id);
			player.name = DataReader.ReadString(dataStream);

			string[] rgb = DataReader.ReadString(dataStream).Split(',');
			player.color = new Color32(byte.Parse(rgb[0]), byte.Parse(rgb[1]), byte.Parse(rgb[2]), 255);

			playerList.Add(player.GetID(), player);
		}

		response.playerList = playerList;

		return response;
	}
}

public class ResponsePlayers : NetworkResponse {

	public Dictionary<int, Player> playerList { get; set; }
	
	public ResponsePlayers() {
		protocol_id = NetworkCode.PLAYERS;
	}
}
