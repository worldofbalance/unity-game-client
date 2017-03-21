using UnityEngine;
using System.Collections;
[ExecuteInEditMode]
public class EcosystemScore : MonoBehaviour {

	public GUISkin skin;
	
	public int score { get; set; }
	public int highScore { get; set; }

	// Use this for initialization
	void Start() {
		Game.networkManager.Listen(
			NetworkCode.UPDATE_ENV_SCORE,
			ProcessUpdateEcoScore
		);

		skin = Resources.Load("Skins/DefaultSkin") as GUISkin;
	}
	
	// Update is called once per frame
	void Update() {
		// Calculate();   // We're getting the Environment score from the server
	}

	void OnDestroy() {
		Game.networkManager.Ignore(
			NetworkCode.UPDATE_ENV_SCORE,
			ProcessUpdateEcoScore
		);
	}
	
	void OnGUI() {
        GUIStyle scoreStyle = new GUIStyle ();
        scoreStyle.fontSize = 28;
        scoreStyle.alignment= TextAnchor.LowerCenter;
		if (score == 0) {
			score = GameState.envScore;
			highScore = GameState.envHighScore;
		}
        GUIExtended.Label (new Rect(250, 650, 200, 50), "Environment Score: " + score , scoreStyle, Color.black, Color.black);


		GUI.BeginGroup(new Rect(Screen.width / 2 - 200, 500, 300, 100));
			GUIStyle style = new GUIStyle();
			style.font = skin.font;
			style.fontSize = 20;
			//style.alignment = TextAnchor.UpperCenter;
			
			Color color = new Color(1.0f, 0.93f, 0.73f, 1.0f);
	
			style.fontSize = 24;
			style.alignment = TextAnchor.UpperCenter;
	
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
		highScore = args.highScore;

		GameState.envScore = args.score;
		GameState.envHighScore = args.highScore;
	}
}
