using UnityEngine;
using System.Collections;

public class CWGame : MonoBehaviour {

	public string scene;
	private static float time = 1f;
	private static Texture2D texture;
	private static int isFading = 0;
	private static float alphaFadeValue;
	private static string nextScene;
	private NetworkManager networkManager;

	void Awake() {
		networkManager = new NetworkManager(
			this,
			new ConnectionManager(Config.REMOTE_HOST, CW.Constants.REMOTE_PORT)
		);

		DontDestroyOnLoad(gameObject);

		//SpeciesTable.Initialize();
		texture = Functions.CreateTexture2D(Color.black);
	}
	
	// Use this for initialization
	void Start() {
//		NetworkManager.Send(
//			SpeciesListProtocol.Prepare(),
//			ProcessSpeciesList
//		);

		if (scene != "") {
	//		Application.LoadLevel(scene);
			Game.SwitchScene(scene);
		} else {
			Debug.Log("Missing Scene");
		}
	}
	
	// Update is called once per frame
	void Update() {
		networkManager.Update();
		if (isFading != 0) {
			PerformTransition();
		}
	}

	void OnGUI() {
		GUI.color = new Color(0, 0, 0, alphaFadeValue);
		GUI.depth = 0;
		GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), texture);
	}

	public static bool SwitchScene(string scene) {
		if (scene != nextScene) {
			if (nextScene == null) {
				Game.LoadScene(scene);
				Debug.Log("Next scene is null");
			} else {
				StartLeaveTransition();
			}
			nextScene = scene;
			return true;
		}
		return false;
	}


	public static void LoadScene(string name) {
		Application.LoadLevel(name);

		GameObject gObject = GameObject.Find("Global Object");

		if (gObject != null) {
			switch (name) {
				case "Login":
					Destroy(gObject.GetComponent<EcosystemScore>());
					Destroy(gObject.GetComponent<GameResources>());
					Destroy(gObject.GetComponent<Clock>());
					Destroy(gObject.GetComponent<Chat>());
					//on non-initial login, need to reset isFading 1->0
					StartEnterTransition();
					break;
				case "CWBattleMainMenu":
					break;
				case "CWBattle":
					break;
			}
		}
	}

	public static void StartEnterTransition() {
		isFading = 1;
	}

	public static void StartLeaveTransition() {
		isFading = -1;
	}

	private void PerformTransition() {
		if (isFading > 0) {
			alphaFadeValue = Mathf.Max(0, alphaFadeValue - Time.deltaTime / time);
			
			if (Mathf.Approximately(alphaFadeValue, 0)) {
				isFading = 0;
				alphaFadeValue = 0;
			}
		} else if (isFading < 0) {
			alphaFadeValue = Mathf.Min(1, alphaFadeValue + Time.deltaTime / time);
			
			if (Mathf.Approximately(alphaFadeValue, 1)) {
				isFading = 0;
				alphaFadeValue = 1;

				if (nextScene != null) {
					Game.LoadScene(nextScene);
				}
			}
		}
	}
	
	public void ProcessSpeciesList(NetworkResponse response) {
		ResponseSpeciesList args = response as ResponseSpeciesList;
//		SpeciesTable.Update(args.speciesList);
//		SpeciesTable.speciesList = args.speciesList;
	}
	
	public GameObject CreateMessageBox(string message) {
		GameObject messageBox = Instantiate(Resources.Load(Constants.PREFAB_RESOURCES_PATH + "MessageBox")) as GameObject;
		messageBox.GetComponent<MessageBox>().setMessage(message);
		
		return messageBox;
	}
}