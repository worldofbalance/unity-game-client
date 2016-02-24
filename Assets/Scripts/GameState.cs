using UnityEngine;

using System.Collections.Generic;

public class GameState : MonoBehaviour
{
	public static Account account { get; set; }

	public static Player player { get; set; }

	public static World world { get; set; }

	public static Ecosystem ecosystem { get; set; }

	private int month;

	public Dictionary<int, Species> speciesList { get; set; }

	public static CSVObject csvList { get; set; }

	public static int matchID { get; set; }
	
	// Use this for initialization
	void Awake ()
	{
		speciesList = new Dictionary<int, Species> ();

//
//		NetworkManager.Send(
//			NetworkCode.CHART,
//			ProcessChart
//		);

		NetworkManager.Listen (
			NetworkCode.ECOSYSTEM,
			ProcessEcosystem
		);

		NetworkManager.Listen (
			NetworkCode.SPECIES_CREATE,
			ProcessSpeciesCreate
		);

		string csv = ",1,2,3,4,5,6,7,8,9,10,11,12\nTree Mouse,5000.0,4348.049640655518,3498.2125759124756,2587.291717529297,1743.8188791275024,1097.4494218826294,643.5683965682983,354.99313473701477,176.78679525852203,91.19854867458344,40.83816707134247,17.714429646730423\nFlies,5000.0,3011.394739151001,1221.3971614837646,257.5061619281769,1.336263376288116,1.0892114987726131E-6,1.3877543862325648E-10,7.128640401399624E-13,2.956730960663441E-14,7.831051207937521E-15,0,\nDwarf Puddle Frog,5000.0,5539.774417877197,5473.539352416992,4501.90544128418,3035.2282524108887,1794.481635093689,979.7311425209045,500.2158284187317,226.70242190361023,109.04326289892197,44.129449874162674,17.338458448648453";
//		csv = ",1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25\nEcosystem Score,100,200,300,400,500,400,300,200,300,400,500,600,700,800,900,1000,2000,1200,600,500,400,700,900,1000,1100";
		
		csv = ",1,2,3,4,5\nSpecies A,5000,4300,3500,2600,1700\nSpecies B,2500,4000,4100,5000,1000\nSpecies C,1000,900,850,600,100";
		GameState.csvList = Functions.ParseCSV (csv);
	}
	
	// Update is called once per frame
	void Update ()
	{

	}

	void OnDestroy ()
	{
		NetworkManager.Ignore (
			NetworkCode.ECOSYSTEM,
			ProcessEcosystem
		);

		NetworkManager.Ignore (
			NetworkCode.SPECIES_CREATE,
			ProcessSpeciesCreate
		);
	}
	
	public void ProcessEcosystem (NetworkResponse response)
	{
		ResponseEcosystem args = response as ResponseEcosystem;
		
		if (args.status == 0) {
			GameState.ecosystem = args.ecosystem;
		}
	}
	
	public void ProcessSpeciesCreate (NetworkResponse response)
	{
		ResponseSpeciesCreate args = response as ResponseSpeciesCreate;

		SpeciesData species = null;

		if (SpeciesTable.speciesList.ContainsKey (args.species_id)) {
			species = SpeciesTable.speciesList [args.species_id];
		}

		if (species == null) {
			Debug.LogError ("Failed to create Species #" + args.species_id);
			return;
		}

		CreateSpecies (args.group_id, args.biomass, args.name, species);
	}

	public void CreateSpecies (int group_id, int biomass, string name, SpeciesData sdata)
	{
//		if (speciesList.ContainsKey(species_id)) {
//			UpdateSpecies(species_id, biomass);
//		} else {
		Species species = gameObject.AddComponent<Species> ();
		species.species_id = sdata.species_id;
		species.name = name;
		species.organism_type = sdata.organism_type;
		species.biomass = biomass;
	
		GameObject organism = species.CreateAnimal ();

		Dictionary<int, GameObject> zoneList = null;
		if (GameObject.Find ("Local Object")) {
			if (GameObject.Find ("Local Object").GetComponent<EcosystemController> ()) {
				zoneList = GameObject.Find ("Local Object").GetComponent<EcosystemController> ().zoneList;
			}
		}
		if (zoneList != null) {
			int zone_id = new List<int> (zoneList.Keys) [Random.Range (0, zoneList.Count)];

			organism.transform.position = zoneList [zone_id].transform.position + new Vector3 (Random.Range (-10f, 10f), 0, Random.Range (-10f, 10f));
		}
		speciesList [species.species_id] = species;

		if (zoneList != null) {
			GameObject.Find ("Global Object").GetComponent<EcosystemScore> ().Calculate ();
		}
//			NetworkManager.Send(
//				ShopActionProtocol.Prepare(),
//			);
//		}
	}
	
	public void UpdateSpecies (int species_id, int size)
	{
		Species species = speciesList [species_id];
		species.UpdateSize (size);
	}
	
	public void ProcessChart (NetworkResponse response)
	{
		ResponseChart args = response as ResponseChart;
	}
	
	public Species GetSpeciesGroup (int group_id)
	{
		return speciesList.ContainsKey (group_id) ? speciesList [group_id] : null;
	}
}
