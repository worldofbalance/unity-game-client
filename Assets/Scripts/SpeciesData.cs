using UnityEngine;

using System.Collections.Generic;

public class SpeciesData {
	
	public int species_id { get; set; }
	public string name { get; set; }
	public short organism_type { get; set; }
	public string description { get; set; }
	public int biomass { get; set; }
	public short diet_type { get; set; }
	public float trophic_level { get; set; }
	public Dictionary<int, string> predatorList { get; set; }
	public Dictionary<int, string> preyList { get; set; }
	public string[] categoryList { get; set; }
	public Texture image { get; set; }

	public short level { get; set; }
	
	public SpeciesData(int species_id) {
		this.species_id = species_id;

		predatorList = new Dictionary<int, string>();
		preyList = new Dictionary<int, string>();
		categoryList = new string[0];
	}
	
	public SpeciesData(SpeciesData species) {
		species_id = species.species_id;
		name = species.name;
		organism_type = species.organism_type;
		description = species.description;
		biomass = species.biomass;
		diet_type = species.diet_type;
		trophic_level = species.trophic_level;

		level = species.level;

		predatorList = new Dictionary<int, string>();
		foreach (KeyValuePair<int, string> predator in species.predatorList) {
			predatorList.Add(predator.Key, predator.Value);
		}

		preyList = new Dictionary<int, string>();
		foreach (KeyValuePair<int, string> prey in species.preyList) {
			preyList.Add(prey.Key, prey.Value);
		}

		categoryList = new string[0];

		image = species.image;
	}
}