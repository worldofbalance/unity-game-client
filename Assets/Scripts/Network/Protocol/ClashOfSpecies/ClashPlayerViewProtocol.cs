using System;
using System.Collections.Generic;
using System.IO;

using UnityEngine;

/// <summary>
/// Gets detailed data about a specific player relevant to Clash of Species
/// </summary>
public class ClashPlayerViewProtocol {

	/// <summary>
	/// Creates a request for the player data
	/// </summary>
	/// <param name="playerId">The id of the player requested.</param>
	public static NetworkRequest Prepare(int playerId) {

		NetworkRequest request = new NetworkRequest(NetworkCode.CLASH_PLAYER_VIEW);
		request.AddInt32(playerId);
		return request;
	}

	/// <summary>
	/// Fills the response object with data about the player
	/// </summary>
	/// <param name="dataStream">The input stream</param>
	public static NetworkResponse Parse(MemoryStream dataStream) {
		ResponseClashPlayerView response = new ResponseClashPlayerView();

		var defenseId = DataReader.ReadInt(dataStream);
		response.terrain = DataReader.ReadString(dataStream);
        response.playerId = DataReader.ReadInt(dataStream);

		string timeString = DataReader.ReadString(dataStream);
		long timeLong = 0;
		long.TryParse(timeString, out timeLong);
        TimeSpan ts = TimeSpan.FromMilliseconds(timeLong);
		DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
		response.createdAt = epoch.Add(ts).ToUniversalTime();

		int count = DataReader.ReadInt(dataStream);
		for(int i = 0; i < count; i++){
			int species = DataReader.ReadInt(dataStream);
			int instanceCount = DataReader.ReadInt(dataStream);
			List<Vector2> positions = new List<Vector2>();
			for(int j = 0; j < instanceCount; j++){
				float x = DataReader.ReadFloat(dataStream);
				float z = DataReader.ReadFloat(dataStream);
				positions.Add(new Vector2(x,z));
			}
            response.layout.Add(species, positions);
		}

		return response;
	}
}

/// <summary>
/// Stores data about the requested player's defense
/// </summary>
public class ResponseClashPlayerView : NetworkResponse {
	/// <summary>
	/// Terrain
	/// </summary>
	public string terrain {get; set;}

	/// <summary>
	/// Player ID
	/// </summary>
	public int playerId {get; set;}

	/// <summary>
	/// When the defense was created
	/// </summary>
	public DateTime createdAt {get; set;}

	/// <summary>
	/// Species in defense config
	/// </summary>
	public Dictionary<int, List<Vector2>> layout = new Dictionary<int,List<Vector2>>();

	public ResponseClashPlayerView() {
		protocol_id = NetworkCode.CLASH_PLAYER_VIEW;
	}
}
