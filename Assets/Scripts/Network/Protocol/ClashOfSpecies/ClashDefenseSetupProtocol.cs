using System;
using UnityEngine;
using System.Collections.Generic;
using System.IO;

/// <summary>
/// Sends a defense setup the player would like to use, gets back whether it is
/// valid
/// </summary>
public class ClashDefenseSetupProtocol {
	/// <summary>
	/// Creates the network request in the format
	/// 	id of this protocol (short)
	/// 	terrain name (string)
	/// 	# of species in config (int)
	/// 	for each species in config
	/// 		species id (int)
	/// 		instance count (int)
	/// 		for each instance
	/// 			x-coordinate (float)
	/// 			y-coordinate (float)
	/// </summary>
	/// <param name="terrainName">the terain name</param>
	/// <param name="config">The species in the config, with species id's as keys
	/// and Vector2's of x- & y-coordinates as values.</param>
	
	public static NetworkRequest Prepare(string terrainName, Dictionary<int, List<Vector2>> config) {
		NetworkRequest request = new NetworkRequest(NetworkCode.CLASH_DEFENSE_SETUP);
		request.AddString(terrainName);
		request.AddInt32(config.Count);
		foreach(var pair in config){
			request.AddInt32(pair.Key);
			List<Vector2> positions = pair.Value;
			request.AddInt32(positions.Count);
			foreach(Vector2 v in positions){
				request.AddFloat(v.x);
				request.AddFloat(v.y);
			}
		}
		return request;
	}

	/// <summary>
	/// Reads in whether the configuration sent was valid
	/// </summary>
	/// <param name="dataStream">The input stream.</param>
	public static NetworkResponse Parse(MemoryStream dataStream) {
		ResponseClashDefenseSetup response = new ResponseClashDefenseSetup();
		response.valid = DataReader.ReadBool(dataStream);
		return response;
	}
}

/// <summary>
/// Stores whether the requested config was valid
/// </summary>
public class ResponseClashDefenseSetup : NetworkResponse {
	/// <summary>
	/// Gets or sets the validity flag
	/// </summary>
	/// <value><c>true</c> if valid; otherwise, <c>false</c>.</value>
	public bool valid {get; set;}

	public ResponseClashDefenseSetup() {
		protocol_id = NetworkCode.CLASH_DEFENSE_SETUP;
	}
}
