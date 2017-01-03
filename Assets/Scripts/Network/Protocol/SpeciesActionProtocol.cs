using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class SpeciesActionProtocol {

	// action = 2
	// Obtains the current species_id and biomass for all the species that in the ecosystem
	// This is the data found in eco_species, not the data in the tiles
	public static NetworkRequest Prepare(short action) {
		NetworkRequest request = new NetworkRequest(NetworkCode.SPECIES_ACTION);
		request.AddShort16(action);

		return request;
	}
	
	public static NetworkRequest Prepare(short action, short type) {
		NetworkRequest request = new NetworkRequest(NetworkCode.SPECIES_ACTION);
		request.AddShort16(action);
		request.AddShort16(type);
		
		return request;
	}
	
	public static NetworkRequest Prepare(short action, Dictionary<int, int> speciesList) {
		NetworkRequest request = new NetworkRequest(NetworkCode.SPECIES_ACTION);
		request.AddShort16(action);
		request.AddShort16((short) speciesList.Count);
		
		foreach (KeyValuePair<int, int> entry in speciesList) {
			request.AddInt32(entry.Key);
			request.AddInt32(entry.Value);
		}
		
		return request;
	}
	
	public static NetworkResponse Parse(MemoryStream dataStream) {
		ResponseSpeciesAction response = new ResponseSpeciesAction();
		response.action = DataReader.ReadShort(dataStream);
		response.status = DataReader.ReadShort(dataStream);
		
		if (response.action == 0) {
			response.type = DataReader.ReadShort (dataStream);
			response.selectionList = DataReader.ReadString (dataStream);
		} else if (response.action == 2) {			
			response.speciesList = new Dictionary<int,int> ();
			int count = DataReader.ReadShort (dataStream);
			int species_id, biomass;
			for (int i = 0; i < count; i++) {
				species_id = DataReader.ReadInt (dataStream);
				biomass = DataReader.ReadInt (dataStream);
				response.speciesList.Add (species_id, biomass);
			}
		}

		return response;
	}
}

public class ResponseSpeciesAction : NetworkResponse {

	public short action { get; set; }
	public short status { get; set; }
	public short type { get; set; }
	public string selectionList { get; set; }
	public Dictionary<int, int> speciesList;
	
	public ResponseSpeciesAction() {
		protocol_id = NetworkCode.SPECIES_ACTION;
	}
}
