using UnityEngine;

using System;
using System.Collections.Generic;
using System.IO;

public class EcosystemProtocol {
	
	public static NetworkRequest Prepare(int world_id, int player_id) {
		NetworkRequest request = new NetworkRequest(NetworkCode.ECOSYSTEM);
		request.AddInt32(world_id);
		request.AddInt32(player_id);
		
		return request;
	}
	
	public static NetworkResponse Parse(MemoryStream dataStream) {
		ResponseEcosystem response = new ResponseEcosystem();
		response.status = DataReader.ReadShort(dataStream);

		List<Zone> zones = new List<Zone>();

		if (response.status == 0) {
			int eco_id = DataReader.ReadInt(dataStream);
	
			Ecosystem ecosystem = new Ecosystem(eco_id);
			ecosystem.type = DataReader.ReadShort(dataStream);
			ecosystem.score = DataReader.ReadInt(dataStream);

			int player_id = DataReader.ReadInt(dataStream);
			
			Player player = new Player(player_id);
			player.name = DataReader.ReadString(dataStream);
			
			string[] rgb = DataReader.ReadString(dataStream).Split(',');
			player.color = new Color32(byte.Parse(rgb[0]), byte.Parse(rgb[1]), byte.Parse(rgb[2]), 255);

			ecosystem.player = player;

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
				
				zones.Add(zone);
			}
			ecosystem.zones = zones;
			
			response.ecosystem = ecosystem;
		}

		return response;
	}
}

public class ResponseEcosystem : NetworkResponse {

	public short status { get; set; }
	public Ecosystem ecosystem { get; set; }
	
	public ResponseEcosystem() {
		protocol_id = NetworkCode.ECOSYSTEM;
	}
}
