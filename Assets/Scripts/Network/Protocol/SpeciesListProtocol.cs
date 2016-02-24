using System;
using System.Collections.Generic;
using System.IO;

public class SpeciesListProtocol {
	
	public static NetworkRequest Prepare() {
		NetworkRequest request = new NetworkRequest(NetworkCode.SPECIES_LIST);
		
		return request;
	}
	
	public static NetworkResponse Parse(MemoryStream dataStream) {
		ResponseSpeciesList response = new ResponseSpeciesList();

		Dictionary<int, SpeciesData> speciesList = new Dictionary<int, SpeciesData>();
		
		int size = DataReader.ReadShort(dataStream);
		//Debug.Log ("species list size is: " + size);
		for (int i = 0; i < size; i++) {
			SpeciesData species = new SpeciesData(DataReader.ReadInt(dataStream));
			species.level = DataReader.ReadShort(dataStream);
			//Debug.Log ("species list level is: " + species.level);
			species.name = DataReader.ReadString(dataStream);
			species.description = DataReader.ReadString(dataStream);
			
			short numArgs = DataReader.ReadShort(dataStream);
			string[] extraArgs = new string[numArgs];
			
			for (int j = 0; j < numArgs; j++) {
				string arg = DataReader.ReadString(dataStream);
				extraArgs[j] = arg;
			}
			
			species.biomass = int.Parse(extraArgs[0]);
			species.diet_type = short.Parse(extraArgs[1]);
			species.trophic_level = float.Parse(extraArgs[2]);
			
			Dictionary<int, string> predatorList = species.predatorList;
			foreach (string predator_id in extraArgs[3].Split(new string[]{","}, StringSplitOptions.None)) {
				if (predator_id != "") {
					predatorList.Add(int.Parse(predator_id), "");
				}
			}
			
			Dictionary<int, string> preyList = species.preyList;
			foreach (string prey_id in extraArgs[4].Split(new string[]{","}, StringSplitOptions.None)) {
				if (prey_id != "") {
					preyList.Add(int.Parse(prey_id), "");
				}
			}
			
			species.categoryList = DataReader.ReadString(dataStream).Split(new string[]{", "}, StringSplitOptions.None);
			
			speciesList.Add(species.species_id, species);
			
			//Constants.speciesList = speciesList;
		}
		//Debug.Log("Reached here!!----------" + Constants.speciesList.Count);
		response.speciesList = speciesList;

		return response;
	}
}

public class ResponseSpeciesList : NetworkResponse {

	public Dictionary<int, SpeciesData> speciesList { get; set; }
	
	public ResponseSpeciesList() {
		protocol_id = NetworkCode.SPECIES_LIST;
	}
}
