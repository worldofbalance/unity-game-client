using UnityEngine;
using System.Collections;

/**
	This class is responsible for spawning predators.
	TODO: tweak, upgrade, and add some proper comments
	(in progress... almost done)
*/
public class SpawnPredator : MonoBehaviour
{
	// DECLARATIONS //
	//
	public const int MIN_ROWS = 3; // Minimum number of rows
	public const int MAX_ROWS = 5; // Maximum number of rows

	Vector3[] spawnPoints; // Spawn points for each row
	int numRows; // Number of rows

	string[] carnivoreNames; // Holds the names of carnivores

	GUIStyle largeFont; // GUI font

	float time; // Game timer
	bool gameOver; // Denotes game over status

	bool spawnedLastPredator; // If true, all predators have been spawned
	bool drawYouWon; // If true, draws the winning screen

	// METHODS //
	//
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

		// Initialize number of rows to MIN_ROWS; update with 'setNumRows'
		numRows = MIN_ROWS;

		// Define GUI font
		largeFont = new GUIStyle();
		largeFont.fontSize = 32;
		largeFont.normal.textColor = Color.green;

		// Initialize constant spawn points (x, y, z = 0)
		// TODO: determine points 4 and 5 after more rows are implemented in the game.
		spawnPoints = new Vector3[MAX_ROWS]
		{
			new Vector3(8.7f, 1.4f),
			new Vector3(8.7f, -0.45f),
			new Vector3(8.7f, -1.9f),
			new Vector3(),
			new Vector3()
		};

		// Initialize carnivore names
		carnivoreNames = new string[MAX_ROWS]
		{
			"Lion",
			"DwarfSandSnake",
			"Leopard",
			"AfricanWildDog",
			"ServalCat"
		};

		// Start spawn event
		StartCoroutine("SpawnEvent");
	}

	/**
		TODO: documentation...
	*/
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
		Updates once per frame
	*/
	void Update()
	{

	}

	/**
		TODO: documentation...
	*/
	void createCreature(int spawnLevel, int creatureId) {


	}

	/**
		TODO: documentation...
	*/
	public IEnumerator SpawnEvent()
	{
		// Determines the wait time and the type of predator to be spawned at each of the three original rows.
		// The format for each array row is:
		// {wait time in seconds, row 1 creatureId, row 2 creatureId, row 3 creatureId}
		// Problem with this: it's the same pattern every time.
		// Solution? Randomize some of these numbers.
		// TODO: randomize the array values within a realistic range to make each round unique.
		int[,] spawnArray = new int[,]
		{
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

		// Initialize iterables
		int currIndex = 0;
		int maxIndex = spawnArray.GetLength(0);
		int spawnLag = 0;

		// Initialize hint board
		// TODO: eliminiate entirely; apparently the HintBoard.cs script was removed without my knowledge.
		// It was an eye sore anyway.
		//HintBoard hintBoard = GameObject.Find ("HintBoard").GetComponent<HintBoard>();

		// Initialize spawns array (used for the hint board, at least for now)
		// TODO: eliminate since the HintBoard.cs script is now gone
		//int[] spawns = new int[MAX_ROWS];

		// Loop until all predators have been spawned
		while(currIndex < maxIndex && !gameOver)
		{
			// Parse wait time for next spawn
			spawnLag = spawnArray[currIndex,0];

			// Parse creatureId values for each row
			// TODO: eliminate since the HintBoard.cs script is now gone
			/*
			for (int i = 1; i < numRows; i++) {
				spawns[i] = spawnArray[currIndex, i];

				//Set hint screen to image

				if (spawns[i] != 0)
					hintBoard.setNextPredator(spawns[i]);
				
			}
			*/

			// Wait for next spawn
			yield return new WaitForSeconds(spawnLag);

			// If game is not over, spawn the next predator
			if (!gameOver) {
				for (int i = 1; i < 4; i++)
					// Create the next predator
					createCreature(i, spawnArray[currIndex,i]);

				// Advance to next predator
				currIndex++;
			}
		}
		// Spawn array has been exhausted
		spawnedLastPredator = true;
	}

	/**
		Returns the current number of rows
	*/
	public int getNumRows () {
		return numRows;
	}

	/**
		Sets the number of rows.
		Discards new values that are out of bounds.
	*/
	public void setNumRows (int rows) {
		numRows = rows >= MIN_ROWS && rows <= MAX_ROWS ? rows : numRows;
	}

	/**
		TODO: figure out what this does
	*/
	public float getRunTime() {
		return time;
	}

	/**
		Stops gameplay
	*/
	public void stopRunTime() {
		gameOver = true;
	}
}