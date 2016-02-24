using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;

/// <summary>
/// Used on first entering the Clash of Species game from lobby
/// </summary>

public class ClashEntryProtocol {
	/// <summary>
	/// Prepares the request to send
	/// </summary>
	public static NetworkRequest Prepare() {
		NetworkRequest request = new NetworkRequest(NetworkCode.CLASH_ENTRY);
		return request;
	}

	/// <summary>
	/// Creates a response object containg the data from the server
	/// </summary>
	/// <param name="dataStream">The input stream</param>
	public static NetworkResponse Parse(MemoryStream dataStream) {
		ResponseClashEntry response = new ResponseClashEntry();

		response.isNew = DataReader.ReadBool(dataStream);
		if (!response.isNew) {
            //read in data on own defense setup
			response.terrain = DataReader.ReadString(dataStream);
            response.config = new Dictionary<int, List<Vector2>>();

			int count = DataReader.ReadInt(dataStream);
//            Debug.Log(count);
			for (int i = 0; i < count; i++) {
				int id = DataReader.ReadInt(dataStream);
				int instanceCount = DataReader.ReadInt(dataStream);
				List<Vector2> positions = new List<Vector2>();
				for(int j = 0; j < instanceCount; j++){
					float x = DataReader.ReadFloat(dataStream);
					float y = DataReader.ReadFloat(dataStream);
					positions.Add(new Vector2(x, y));
				}
                response.config.Add(id, positions);
			}
		}

		return response;
	}
}

/// <summary>
/// Container for data sent by the server
/// </summary>
public class ResponseClashEntry : NetworkResponse {

	/// <summary>
	/// Whether the player has a defense set up for Clash of Species
	/// </summary>
	/// <value><c>true</c> if no defense; otherwise, <c>false</c>.</value>
	public bool isNew {get; set;}

	/// <summary>
	/// Gets and sets the terrain
	/// </summary>
	/// <value>the terrain in the defense setup, if one exists</value>
	public string terrain {get; set;}

	/// <summary>
	/// Gets and sets the list of species
	/// </summary>
	/// <value>The list of species in the defense setup, if one exists</value>
	public Dictionary<int, List<Vector2>> config = null;

	public ResponseClashEntry() {
		protocol_id = NetworkCode.CLASH_ENTRY;
	}
}
