using UnityEngine;

using System;
using System.IO;

public class PlayerSelectProtocol {
	
	public static NetworkRequest Prepare(int player_id) {
		NetworkRequest request = new NetworkRequest(NetworkCode.PLAYER_SELECT);
		request.AddInt32(player_id);
		
		return request;
	}
	
	public static NetworkResponse Parse(MemoryStream dataStream) {
		ResponsePlayerSelect response = new ResponsePlayerSelect();
		response.status = DataReader.ReadShort(dataStream);
		
		if (response.status == 0) {
			int player_id = DataReader.ReadInt(dataStream);

			Player player = new Player(player_id);
			player.name = DataReader.ReadString(dataStream);
			player.level = DataReader.ReadShort(dataStream);
			player.xp = DataReader.ReadInt(dataStream);
			player.credits = DataReader.ReadInt(dataStream);

			string[] rgb = DataReader.ReadString(dataStream).Split(',');
			player.color = new Color32(byte.Parse(rgb[0]), byte.Parse(rgb[1]), byte.Parse(rgb[2]), 255);

			player.last_played = DataReader.ReadString(dataStream);

			response.player = player;
		}
		
		return response;
	}
}

public class ResponsePlayerSelect : NetworkResponse {
	
	public short status { get; set; }
	public Player player { get; set; }
	
	public ResponsePlayerSelect() {
		protocol_id = NetworkCode.PLAYER_SELECT;
	}
}
