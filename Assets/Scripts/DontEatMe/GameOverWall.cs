using UnityEngine;
using System.Collections;

public class GameOverWall : MonoBehaviour
{
	bool GameIsOver = false;

	GUIStyle largeFont;

	
	GameObject spawnPredator;
	SpawnPredator spawnPredatorScript;

	GameObject buildMenu;
	BuildMenu buildMenuScript;

	bool endedGame = false;

	void OnCollisionStay2D(Collision2D collision) {
		LostGame ();
	
	}

	public void LostGame() {
		
		GameIsOver = true;
		
		GameObject predatorSpawner = GameObject.Find ("PredatorSpawner");
		spawnPredatorScript = predatorSpawner.GetComponent<SpawnPredator>();
		
		spawnPredatorScript.stopRunTime ();

	}

	void Start () 
	{

		largeFont = new GUIStyle();
	
		largeFont.fontSize = 32;
		largeFont.normal.textColor = Color.red;

	}

	void OnGUI() {

		if (GameIsOver) {

	  		spawnPredator = GameObject.Find ("PredatorSpawner");
			spawnPredatorScript = spawnPredator.GetComponent<SpawnPredator>();
	
			spawnPredatorScript.stopRunTime();

			// draw resource menu
			GUILayout.BeginArea (new Rect (350, 350, 600, 600));
			GUILayout.BeginHorizontal ("box");
		
			GUILayout.Label("You Lost!", largeFont);
		
			// end GUI for resource menu
			GUILayout.EndHorizontal ();
			GUILayout.EndArea ();

			if (GUI.Button (new Rect (360, 420, 140, 30), "Return to Lobby")) {
				Destroy (this);
				Game.SwitchScene("World");
			}

		}

		if (GameIsOver && !endedGame) {
			endedGame = true;

			buildMenu = GameObject.Find ("MainCamera");
			buildMenuScript = buildMenu.GetComponent<BuildMenu>();

			buildMenuScript.endGame();

		}
	}

}

