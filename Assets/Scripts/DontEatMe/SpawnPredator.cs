using UnityEngine;
using System.Collections;

/**
	This class is responsible for spawning predators.
	TODO: tweak, upgrade, and add some proper comments
*/
public class SpawnPredator : MonoBehaviour
{
	// DECLARATIONS //
	//
	int currIndex;
	int maxIndex;
	int nextSpawn;

	int spawnOne;
	int spawnTwo;
	int spawnThree;

	// Spawn points for each row
	Vector3 spawnPointOne;
	Vector3 spawnPointTwo;
	Vector3 spawnPointThree;

	GUIStyle largeFont;

	// INITIALIZATIONS //
	//
	float time; // Game timer
	bool gameOver; // Denotes game over status

	bool spawnedLastPredator; // If true, all predators have been spawned
	bool drawYouWon; // If true, draws the winning screen

	BuildMenu buildMenu; // The BuildMenu script object from the MainCamera

	// Not sure what hintBoard does at the moment...
	/*
	 * hintBoard = GameObject.Find ("HintBoard");
		hintBoardScript = hintBoard.GetComponent<HintBoard>();
		Acquire object like this.
	 * */

	/**
		Updates once per frame
	*/
	void Update()
	{
		// Game over check
		if (!gameOver) {
			// Advance timer
			// TODO: find where the Time class originates
			time += Time.deltaTime;
			//Debug.Log (time);

			// Perform final steps after last predator has spawned
			if (spawnedLastPredator) {

				GameObject[] arr = GameObject.FindGameObjectsWithTag("Carnivore");

				if (arr.Length == 0) {
					// TODO: not sure why the MainCamera object is called "buildMenu", and the actual
					// BuildMenu object is called "buildMenuScript"...
					// FIXME: buildMenu = GameObject.Find("MainCamera").GetComponent<BuildMenu>();
					buildMenu = GameObject.Find ("MainCamera").GetComponent<BuildMenu>();

					// Set winning bools to true
					drawYouWon = true;
					gameOver = true;

					// Trigger end of game
					buildMenu.endGame();
				}
			}
		}
	}

	void OnGUI() {
		// TODO: improve upon the "You Won" action; currently, predators continue to spawn once the game is won.
		if (drawYouWon) {
			// Draw resource menu
			GUILayout.BeginArea(new Rect (350, 350, 600, 600));
			GUILayout.BeginHorizontal("box");

			// Draw "You Won!" label
			GUILayout.Label("You Won!", largeFont);
			
			// End GUI for resource menu
			GUILayout.EndHorizontal();
			GUILayout.EndArea();
		}
	}

	/**
		TODO: figure out what this does
	*/
	public float getRunTime() {
		return time;
	}

	/**
		TODO: figure out what this does
	*/
	public void stopRunTime() {
		gameOver = true;
	}

	/**
		Called once the script is activated.
	*/
	void Start ()
	{
		// Initializations
		time = 0; // Init time to 0
		gameOver = false; // Init game over status to false
		spawnedLastPredator = false; // Init to false
		drawYouWon = false; // You just started, you haven't won yet

		// Create a large font for display
		largeFont = new GUIStyle();
		largeFont.fontSize = 32;
		largeFont.normal.textColor = Color.green;

		// TODO: figure out what "MyEvent" is... and why it's called something so 'tutorial-ly'
		StartCoroutine("MyEvent");
	}

	/**
		TODO: figure out what this does.
		Based on the name of the method, I'm assuming it creates a creature.
		Most likely a predator...
	*/
	void createCreature(int spawnLevel, int creatureId) {
		spawnPointOne.x = 8.7f;
		spawnPointOne.y = 1.4f;
		spawnPointOne.z = 0f;

		spawnPointTwo.x = 8.7f;
		spawnPointTwo.y = -0.45f;
		spawnPointTwo.z = 0f;

		spawnPointThree.x = 8.7f;
		spawnPointThree.y = -1.9f;
		spawnPointThree.z = 0f;

		GameObject spawnedPredator;

		Vector3 currSpawnPoint;
		if (spawnLevel == 1) {
			currSpawnPoint = spawnPointOne;
		} else if (spawnLevel == 2) {
			currSpawnPoint = spawnPointTwo;
		} else {
			currSpawnPoint = spawnPointThree;
		}

		// TODO: just make a string with the load path and make one call to Instantiate
		string carnivoreName; // Name of carnivore to load on Instantiate
		switch (creatureId) {
			case 1:
				carnivoreName = "Lion";
				break;
			case 2:
				carnivoreName = "DwarfSandSnake";
				break;
			case 3:
				carnivoreName = "Leopard";
				break;
			case 4:
				carnivoreName = "AfricanWildDog";
				break;
			case 5:
				carnivoreName = "ServalCat";
				break;
			default:
				carnivoreName = "Lion";
				break;
		}
		spawnedPredator = (GameObject) Instantiate(Resources.Load("Prefabs/Carnivore/" + carnivoreName), currSpawnPoint,  Quaternion.identity);

		/*
		switch (creatureId)
		{
		case 1:
			spawnedPredator = (GameObject) Instantiate(Resources.Load("Prefabs/Carnivore/Lion"), currSpawnPoint,  Quaternion.identity);
			break;
		case 2:
			spawnedPredator = (GameObject) Instantiate(Resources.Load("Prefabs/Carnivore/DwarfSandSnake"), currSpawnPoint,  Quaternion.identity);
			break;
		case 3:
			spawnedPredator = (GameObject) Instantiate(Resources.Load("Prefabs/Carnivore/Leopard"), currSpawnPoint,  Quaternion.identity);
			break;
		case 4:
			spawnedPredator = (GameObject) Instantiate(Resources.Load("Prefabs/Carnivore/AfricanWildDog"), currSpawnPoint,  Quaternion.identity);
			break;
		case 5:
			spawnedPredator = (GameObject) Instantiate(Resources.Load("Prefabs/Carnivore/ServalCat"), currSpawnPoint,  Quaternion.identity);
			break;
		default:
			spawnedPredator = (GameObject) Instantiate(Resources.Load("Prefabs/Carnivore/Lion"), currSpawnPoint,  Quaternion.identity);
			break;
		}
		*/

	}

	/**
		TODO: figure out what this does, and rename it something that actually sounds somewhat descriptive.
	*/
	public IEnumerator MyEvent()
	{
		int[,] spawnArray = new int[,] {
			{2, 0, 1, 0}, 
			{11, 2, 0, 0},
			{15, 0, 0, 3},
			{10, 0, 1, 0},
			{13, 3, 0, 0},
			{27, 0, 0, 2},
			{18, 0, 1, 0},
			{19, 3, 0, 0},
			{21, 0, 2, 0},
			{25, 3, 0, 0},
			{10, 0, 0, 2},
			{22, 3, 0, 0},
			{25, 0, 4, 0},
			{28, 2, 0, 0},
			{19, 0, 0, 1},
			{21, 0, 3, 0},
			{23, 5, 0, 0},
			{24, 0, 3, 0},
			{17, 0, 4, 0},
			{32, 5, 0, 0},
			{45, 0, 0, 5}
		};
										


		
		currIndex = 0;
		nextSpawn = 0;
		maxIndex = spawnArray.GetLength (0);



		

		while(currIndex < maxIndex && !gameOver)
		{
			nextSpawn = spawnArray[currIndex,0];
			spawnOne = spawnArray[currIndex,1];
			spawnTwo = spawnArray[currIndex,2];
			spawnThree = spawnArray[currIndex,3];


			//Debug.Log ("Sleeping");
			yield return new WaitForSeconds(nextSpawn); // wait x seconds
			//Debug.Log ("Waited " + nextSpawn + " seconds!" + gameOver);
			if (!gameOver) {
				//Debug.Log ("Spawning Something!" + gameOver);
				for (int i = 1; i < 4; i++) {
					if (spawnArray[currIndex,i] != 0) {
						createCreature(i, spawnArray[currIndex,i]);
					}
				}
				currIndex++; //Move onto next predator
			}
		}

		spawnedLastPredator = true;

	}


}

