using UnityEngine;

using System;
using System.Collections.Generic;
using System.IO;

public class ZoneListProtocol {
	
	public static NetworkRequest Prepare() {
		NetworkRequest request = new NetworkRequest(NetworkCode.ZONE_LIST);
		
		return request;
	}
	
	public static NetworkResponse Parse(MemoryStream dataStream) {
		ResponseZoneList response = new ResponseZoneList();
		response.status = DataReader.ReadShort(dataStream);

		Dictionary<int, Player> players = new Dictionary<int, Player>();
		List<Zone> zones = new List<Zone>();

		if (response.status == 0) {
			short size = DataReader.ReadShort(dataStream);
			for (int i = 0; i < size; i++) {
				int player_id = DataReader.ReadInt(dataStream);

				Player player = new Player(player_id);
				player.name = DataReader.ReadString(dataStream);

				string[] rgb = DataReader.ReadString(dataStream).Split(',');
				player.color = new Color32(byte.Parse(rgb[0]), byte.Parse(rgb[1]), byte.Parse(rgb[2]), 255);

				players.Add(player_id, player);
			}
			response.players = players;

			response.height = DataReader.ReadShort(dataStream);
			response.width = DataReader.ReadShort(dataStream);

			string zone_str = DataReader.ReadString(dataStream);
			foreach (string item in zone_str.Split(';')) {
				if (item == "") {
					continue;
				}

				string[] temp = item.Split(',');

				Zone zone = new Zone(int.Parse(temp[0]));
				zone.row = short.Parse(temp[1]);
				zone.column = short.Parse(temp[2]);
				zone.terrain_type = short.Parse(temp[3]);
				zone.v_capacity = int.Parse(temp[4]);
				zone.player_id = int.Parse(temp[5]);

				zones.Add(zone);
			}
			response.zones = zones;
		}

		return response;
	}
}

public class ResponseZoneList : NetworkResponse {

	public short status { get; set; }
	public Dictionary<int, Player> players { get; set; }
	public short height { get; set; }
	public short width { get; set; }
	public List<Zone> zones { get; set; }
	
	public ResponseZoneList() {
		protocol_id = NetworkCode.ZONE_LIST;
	}
}
