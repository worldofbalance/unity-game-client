using UnityEngine;
using System.Collections;

public class BuildMenu : MonoBehaviour
{

	// Currently building...
	public static BuildInfo currentlyBuilding;

	// Currently about to delete?
	public static bool currentlyDeleting = false;

	// Player's current resource amount
	public static int currentResources = 250;

	//Player's current score amount
	public static int score = 0;

	// Plant prefabs
  public GameObject[] plants;

	// Prey prefabs
	public BuildInfo[] prey;

	int coins = 0;


	void OnGUI ()
	{




		// draw resource menu
		GUILayout.BeginArea(new Rect(0, 0, 155, 200));
		GUILayout.BeginHorizontal("box");

		// draw resource counter
		GUILayout.Button(new GUIContent("Resources: " + currentResources.ToString()), GUILayout.Height(70));

		// end GUI for resource menu
		GUILayout.EndHorizontal();
		GUILayout.EndArea();

		// draw score menu
		GUILayout.BeginArea(new Rect(800, 0, 155, 200));
		GUILayout.BeginHorizontal("box");

		// draw score counter
		GUILayout.Button(new GUIContent("Coins: " + coins.ToString()), GUILayout.Height(70));

		// end GUI for score menu
		GUILayout.EndHorizontal();
		GUILayout.EndArea();

		// draw the plant menu
		GUILayout.BeginArea(new Rect(0, 80, 100, 400));
    GUILayout.BeginVertical();

		// Draw each plant's build info


    foreach (GameObject plant in plants) {
     
      BuildInfo info = plant.GetComponent<BuildInfo> ();

			GUI.enabled = currentResources >= info.price;
			// if button is clicked, then set currentlyBuilding to the info of the button you clicked
      if (GUILayout.Button(new GUIContent(info.previewImage) , GUILayout.Width(40), GUILayout.Height(40)) ){

				// If a selection is currently in progress...
				if (currentlyBuilding && DemMain.currentSelection) {
					// Ignore button click if for the same species
					if (currentlyBuilding.name == info.name)
						continue;
					// Otherwise, destroy the current selection before continuing
					else
						Destroy(DemMain.currentSelection);
				}
				// Set / reset currentlyBuilding

				currentlyBuilding = info;
        currentlyBuilding.isPlant = true;

				// Create the current prey object
        GameObject currentPlant = DemAnimalFactory.Create(currentlyBuilding.name , 0 ,0) as GameObject;

				// Define the current prey's initial location relative to the world (i.e. NOT on a screen pixel basis)
				Vector3 init_pos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y));
				init_pos.z = -1.5f;

				// Instantiate the current prey
        DemMain.currentSelection = (GameObject)Instantiate
				(
					currentPlant,
					init_pos,
					Quaternion.identity
				);
  

				// Set DemMain's preyOrigin as the center of the button
				DemMain.setBuildOrigin(Camera.main.ScreenToWorldPoint(Input.mousePosition));

        		DemMain.boardController.SetAvailableTiles();

				// DEBUG MESSAGE
				Debug.Log("currentPlant set to " + currentPlant.name);
			}

		}

		// End GUI for plant menu
    GUILayout.EndVertical();
		GUILayout.EndArea();

		// Now, draw prey menu
		GUILayout.BeginArea(new Rect(300, 0, 500, 220));
		GUILayout.BeginHorizontal("box");

		// draw each prey's build info
		foreach (BuildInfo info in prey) {
			GUI.enabled = currentResources >= info.price;
			// if button is clicked, then set currentlyBuilding to the info of the button you clicked

      if (GUILayout.Button(new GUIContent(info.price.ToString(), info.previewImage)  )) {
				// If a selection is currently in progress...
				if (currentlyBuilding && DemMain.currentSelection) {
					// Ignore button click if for the same species
					if (currentlyBuilding.name == info.name)
						continue;
					// Otherwise, destroy the current selection before continuing
					else
						Destroy(DemMain.currentSelection);
				}
				// Set / reset currentlyBuilding

				currentlyBuilding = info;

				// Create the current prey object
				GameObject currentPrey = (GameObject)Resources.Load("Prefabs/Herbivores/" + currentlyBuilding.name);

				// Define the current prey's initial location relative to the world (i.e. NOT on a screen pixel basis)
				Vector3 init_pos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y));
				init_pos.z = -1.5f;


				// Instantiate the current prey
				DemMain.currentSelection = (GameObject)Instantiate
				(
					currentPrey,
					init_pos,
					Quaternion.identity
				);
        
				// Set DemMain's preyOrigin as the center of the button
				DemMain.setBuildOrigin(Camera.main.ScreenToWorldPoint(Input.mousePosition));

        		DemMain.boardController.SetAvailableTiles();

				// DEBUG MESSAGE
				Debug.Log("currentPrey set to " + currentPrey.name);
			}
		}

		// End GUI for prey menu
		GUILayout.EndVertical();
		GUILayout.EndArea();
	}

	// this method increases score every 2s
	void increaseResources ()
	{
		currentResources += 50;
	}

	// Use this for initialization
	void Start ()
	{
		// set resources to grow over time
		InvokeRepeating("increaseResources", 2, 3.0F);

    plants = new GameObject[7];
    plants [0] = DemAnimalFactory.Create("Acacia" , 0 , 0);
    plants [1] = DemAnimalFactory.Create("Baobab" , 0 , 0);
    plants [2] = DemAnimalFactory.Create("Big Tree" , 0 , 0);
    plants [3] = DemAnimalFactory.Create("Fruits And Nectar" , 0 , 0);
    plants [4] = DemAnimalFactory.Create("Grains And Seeds" , 0 , 0);
    plants [5] = DemAnimalFactory.Create("Grass And Herbs" , 0 , 0);
    plants [6] = DemAnimalFactory.Create("Trees And Shrubs" , 0 , 0);



	}

	public void CalculateCoins ()
	{
		int newCoins = (score / 100);

		int numCoinsDrop = (newCoins - coins);

		if (newCoins > 0) {
			dropCoins(newCoins);
		}

	}

	void dropCoins (int numCoins)
	{
		//Drop numCoins from screen
		//Debug.Log ("DropCoins: " + numCoins);
		coins += numCoins;

		Vector3 spawnPoint = new Vector3(8f, 3.3f, 0f);
		Instantiate(Resources.Load("Prefabs/SpinCoin"), spawnPoint, Quaternion.identity);
	}


	public void endGame ()
	{
		Debug.Log("Game ended with X coins: " + coins);

		//LOBBY TEAM, PUT YOUR RETURN CODE HERE, PASS BACK
		//coins variable
		NetworkManager.Send(
			EndGameProtocol.Prepare(1, coins),
			ProcessEndGame
		);
	}

	// Updates player's credits
	public void ProcessEndGame (NetworkResponse response)
	{
		ResponsePlayGame args = response as ResponsePlayGame;

		if (args.status == 1) {

			GameState.player.credits = args.creditDiff;
			Debug.Log(args.creditDiff);
		}
	}

	// Update is called once per frame
	void Update ()
	{
	}


}
