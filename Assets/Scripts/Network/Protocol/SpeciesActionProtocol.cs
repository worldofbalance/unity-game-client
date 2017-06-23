using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class SpeciesActionProtocol {

	// action = 2 or 5 or 6
	// action 2: Obtains the current species_id and biomass for all the species that in the ecosystem
	// This is the data found in eco_species, not the data in the tiles
	// action 5: Just returns the count
	// action 6: Returns 3 day counts; current day, first day for player, last simulation day for player
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

	// action = 3
	// Obtains the cost and server biomass (float) for a given species id
	public static NetworkRequest Prepare(short action, short index, int species_id) {
		NetworkRequest request = new NetworkRequest(NetworkCode.SPECIES_ACTION);
		request.AddShort16(action);
		request.AddInt32(species_id);
		request.AddShort16(index);
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

	// action = 4
	// Obtains the history of changes to the biomass value
	public static NetworkRequest Prepare(short action, int species_id) {
		NetworkRequest request = new NetworkRequest(NetworkCode.SPECIES_ACTION);
		request.AddShort16(action);
		request.AddInt32(species_id);
		return request;
	}


	// action = 7
	// Obtains the history of changes to the biomass value, starting from a day
	public static NetworkRequest Prepare(short action, int species_id, int startDay) {
		NetworkRequest request = new NetworkRequest(NetworkCode.SPECIES_ACTION);
		request.AddShort16(action);
		request.AddInt32(species_id);
		request.AddInt32(startDay);
		return request;
	}


	// action = 8
	// Used to generate Ben's food web graph. configStr has the parameter settings and 
	// spStr is the species string. This routine returns the count of bytes
	public static NetworkRequest Prepare(short action, string configStr, string speciesStr) {
		NetworkRequest request = new NetworkRequest(NetworkCode.SPECIES_ACTION);
		request.AddShort16(action);
		request.AddString(configStr);
		request.AddString(speciesStr);
		return request;
	}

	// action = 9
	// Used to generate Ben's food web graph. configStr has the parameter settings and 
	// spStr is the species string. The byte to start the transfer is specified. The transfer
	// is done in segments to stay under the 32K limit
	public static NetworkRequest Prepare(short action, string configStr, string speciesStr, 
				int startByte) {
		NetworkRequest request = new NetworkRequest(NetworkCode.SPECIES_ACTION);
		request.AddShort16(action);
		request.AddString(configStr);
		request.AddString(speciesStr);
		request.AddInt32(startByte);
		return request;
	}
		

	public static NetworkResponse Parse(MemoryStream dataStream) {
		ResponseSpeciesAction response = new ResponseSpeciesAction();
		response.action = DataReader.ReadShort(dataStream);
		response.status = DataReader.ReadShort(dataStream);
		
		if (response.action == 0) {
			response.type = DataReader.ReadShort (dataStream);
			response.selectionList = DataReader.ReadString (dataStream);
			// Debug.Log ("SpeciesAction Response, action = 0, selectionList = " + response.selectionList);
		} else if (response.action == 2) {		
			response.speciesList = new Dictionary<int,int> ();
			int count = DataReader.ReadShort (dataStream);
			// Debug.Log ("SpeciesAction Response, action = 2, count = " + count);	
			int species_id, biomass;
			for (int i = 0; i < count; i++) {
				species_id = DataReader.ReadInt (dataStream);
				biomass = DataReader.ReadInt (dataStream);
				// Debug.Log ("id, biomass = " + species_id + " " + biomass);
				response.speciesList.Add (species_id, biomass);
			}
		} else if (response.action == 3) {
			response.species_id = DataReader.ReadInt (dataStream);
			response.cost = DataReader.ReadInt (dataStream);
			response.biomassServer = DataReader.ReadFloat (dataStream);
			response.index = DataReader.ReadShort (dataStream);
		} else if ((response.action == 4) || (response.action == 7)) {
			response.species_id = DataReader.ReadInt (dataStream);
			int count = DataReader.ReadShort (dataStream);
			response.speciesHistoryList = new Dictionary<int,int> ();
			int day, biomass;
			for (int i = 0; i < count; i++) {
				day = DataReader.ReadInt (dataStream);
				biomass = DataReader.ReadInt (dataStream);
				// Debug.Log ("day, biomass = " + day + " " + biomass);
				response.speciesHistoryList.Add (day, biomass);
			}
		} else if (response.action == 5) {
			response.count = DataReader.ReadShort (dataStream);
			Debug.Log ("SpeciesAction Response, action = 5, count = " + response.count);	
		} else if (response.action == 6) {
			response.cDay = DataReader.ReadInt (dataStream);
			response.fDay = DataReader.ReadInt (dataStream);
			response.lDay = DataReader.ReadInt (dataStream);
			Debug.Log ("SpeciesAction Response, action = 6, the 3 days are = " +
			response.cDay + " " + response.fDay + " " + response.lDay);	
		} else if (response.action == 8) {
			response.byteCount = DataReader.ReadInt (dataStream);
			Debug.Log ("SpeciesAction Response, action = 8, byteCount = " + response.byteCount);
		} else if (response.action == 9) {
			response.startByte = DataReader.ReadInt (dataStream);
			response.fileContents = DataReader.ReadBytes (dataStream);
			response.byteCount = response.fileContents.Length;
			Debug.Log ("SpeciesAction Response, action = 9, byteCount, startByte = " 
				+ response.byteCount + " " + response.startByte);
		}
			
		return response;
	}
}

public class ResponseSpeciesAction : NetworkResponse {

	public short action { get; set; }
	public short status { get; set; }
	public short type { get; set; }
	public short count { get; set; }
	public int species_id { get; set; }
	public short index { get; set; }
	public int cost { get; set; }
	public float biomassServer { get; set; }
	public int cDay { get; set; }
	public int fDay { get; set; }
	public int lDay { get; set; }
	public string selectionList { get; set; }
	public Dictionary<int, int> speciesList;
	public Dictionary<int, int> speciesHistoryList;
	public int byteCount { get; set; }
	public int startByte { get; set; }
	public byte[] fileContents;
	
	public ResponseSpeciesAction() {
		protocol_id = NetworkCode.SPECIES_ACTION;
	}
}
