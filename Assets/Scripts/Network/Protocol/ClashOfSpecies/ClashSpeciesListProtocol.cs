using System;
using System.Collections.Generic;
using System.IO;
using SpeciesType = ClashSpecies.SpeciesType;
using UnityEngine;

/// <summary>
/// Get the list of species available for the Clash of Specis game
/// </summary>
public class ClashSpeciesListProtocol{

	
	public static NetworkRequest Prepare() {
		NetworkRequest request = new NetworkRequest(NetworkCode.CLASH_SPECIES_LIST);
		return request;
	}

	/// <summary>
	/// Fill the Response object with data from the server
	/// </summary>
	/// <param name="dataStream">The input stream</param>
	public static NetworkResponse Parse(MemoryStream dataStream) {
		ResponseClashSpeciesList response = new ResponseClashSpeciesList();

		int count = DataReader.ReadInt(dataStream);
//		Debug.Log("received " + count + " species");
		for(int i = 0; i < count; i++) {
            ClashSpecies s = new ClashSpecies();
            s.id = DataReader.ReadInt(dataStream);
            s.name = DataReader.ReadString(dataStream);
			s.cost = DataReader.ReadInt(dataStream);
			s.type = (SpeciesType)DataReader.ReadInt(dataStream);
			s.description = DataReader.ReadString(dataStream);
			s.attack = DataReader.ReadInt(dataStream);
			s.hp = DataReader.ReadInt(dataStream);
			s.moveSpeed = DataReader.ReadInt(dataStream);
			s.attackSpeed = DataReader.ReadInt(dataStream);

			response.speciesList.Add(s);
		}
		return response;
	}
}

/// <summary>
/// Stores species list sent from server
/// </summary>
public class ResponseClashSpeciesList : NetworkResponse {
	/// <summary>
	/// The species list.
	/// </summary>
	public List<ClashSpecies> speciesList = new List<ClashSpecies>();

	public ResponseClashSpeciesList() {
	    protocol_id = NetworkCode.CLASH_SPECIES_LIST;
	}
}
