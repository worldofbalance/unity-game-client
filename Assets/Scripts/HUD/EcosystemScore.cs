using UnityEngine;
using System.Collections;

public class EcosystemScore : MonoBehaviour {

	public GUISkin skin;
	
	public int score { get; set; }

	// Use this for initialization
	void Start() {
		NetworkManager.Listen(
			NetworkCode.UPDATE_ENV_SCORE,
			ProcessUpdateEcoScore
		);

		skin = Resources.Load("Skins/DefaultSkin") as GUISkin;
	}
	
	// Update is called once per frame
	void Update() {
		Calculate();
	}

	void OnDestroy() {
		NetworkManager.Ignore(
			NetworkCode.UPDATE_ENV_SCORE,
			ProcessUpdateEcoScore
		);
	}
	
	void OnGUI() {
		//GUI.Label (new Rect(300, 0, 200, 50), "Environment Score: " + score );
		GUI.BeginGroup(new Rect(Screen.width / 2 - 100, 60, 200, 100));
			GUIStyle style = new GUIStyle();
			style.font = skin.font;
			style.fontSize = 20;
			style.alignment = TextAnchor.UpperCenter;
			
			Color color = new Color(1.0f, 0.93f, 0.73f, 1.0f);
	
			GUIExtended.Label(new Rect(0, 0, 200, 50), "Ecosystem Score", style, Color.black, color);
	
			style.fontSize = 24;
			style.alignment = TextAnchor.UpperCenter;
	
			GUIExtended.Label(new Rect(0, 25, 200, 50), score.ToString("n0"), style, Color.black, Color.white);
		GUI.EndGroup();
	}

	public void Calculate() {
		float biomass = 0;

		GameState gs = GameObject.Find("Global Object").GetComponent<GameState>();

		foreach (Species species in gs.speciesList.Values) {
			SpeciesData type = SpeciesTable.speciesList[species.species_id];
			type.biomass = 100;
			biomass += type.biomass * Mathf.Pow(3000 / type.biomass, type.trophic_level);
		}
		
		if (biomass > 0) {
			biomass = Mathf.Round(Mathf.Log(biomass) / Mathf.Log(2)) * 5;
		}

		SetScore((int) Mathf.Round(Mathf.Pow(biomass, 2) + Mathf.Pow(gs.speciesList.Count, 2)));
	}
	
	public void SetScore(int score) {
		this.score = score;
	}
	
	public void ProcessUpdateEcoScore(NetworkResponse response) {
		ResponseUpdateEnvScore args = response as ResponseUpdateEnvScore;

		SetScore(args.score);
	}
}
