using UnityEngine;
using System.Collections;

public class HintBoard : MonoBehaviour
{
	// the menu button sprite
	public Texture Lion;
	public Texture LionHerbivore;
	public Texture LionPlant;

	public Texture DwarfSandSnake;
	public Texture DwarfSandSnakeHerbivore;
	public Texture DwarfSandSnakePlant;

	public Texture Leopard;
	public Texture LeopardHerbivore;
	public Texture LeopardPlant;

	public Texture AfricanWildDog;
	public Texture AfricanWildDogHerbivore;
	public Texture AfricanWildDogPlant;

	public Texture ServalCat;
	public Texture ServalCatHerbivore;
	public Texture ServalCatPlant;

	public Texture Arrow;

	public Texture Empty;

	private Texture leftSquare;
	private Texture middleSquare;
	private Texture rightSquare;

	// Use this for initialization
	void Start ()
	{
		leftSquare = Empty;
		middleSquare = Empty;
		rightSquare = Empty;

	}

	void OnGUI() {
		// draw the plant menu
		GUILayout.BeginArea (new Rect (150, 510, 1000, 540));
		GUILayout.BeginHorizontal ("box");

		GUI.enabled = false;
		GUILayout.Button (new GUIContent (leftSquare));
		GUILayout.Button (new GUIContent (Arrow));
		GUILayout.Button (new GUIContent (middleSquare));
		GUILayout.Button (new GUIContent (Arrow));
		GUILayout.Button (new GUIContent (rightSquare));
		
		// End GUI for plant menu
		GUILayout.EndVertical();
		GUILayout.EndArea ();
	}

	public void setNextPredator(int nextPredator) {

		switch (nextPredator)
		{
		case 1:
			//Lion
			leftSquare = Lion;
			middleSquare = LionHerbivore;
			rightSquare = LionPlant;
			break;
		case 2:
			//DwarfSandSnake
			leftSquare = DwarfSandSnake;
			middleSquare = DwarfSandSnakeHerbivore;
			rightSquare = DwarfSandSnakePlant;		
			break;
		case 3:
			//Leopard
			leftSquare = Leopard;
			middleSquare = LeopardHerbivore;
			rightSquare = LeopardPlant;		
			break;
		case 4:
			//AfricanWildDog
			leftSquare = AfricanWildDog;
			middleSquare = AfricanWildDogHerbivore;
			rightSquare = AfricanWildDogPlant;
			break;
		case 5:
			//ServalCat
			leftSquare = ServalCat;
			middleSquare = ServalCatHerbivore;
			rightSquare = ServalCatPlant;
			break;
		default:
			//Lion
			leftSquare = Lion;
			middleSquare = LionHerbivore;
			rightSquare = LionPlant;			
			break;
		}

	}

	// Update is called once per frame
	void Update ()
	{
	
	}
}

