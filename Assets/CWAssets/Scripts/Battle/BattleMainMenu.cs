
using System;
using UnityEngine;
using Random = UnityEngine.Random;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
namespace CW{
public class BattleMainMenu : MonoBehaviour
{
	private int playerID = 108;

	public static readonly string SceneNameMainMenu = "CWBattleMainMenu";
    public static readonly string SceneNameGame = "CWBattle";
	//public static readonly string SceneNameEditDeck = "EditDeckScene";


	public static string playerMessage;
	public static string opponentMessage;

	public static string buttonTxt;
	public static bool playerIsReady;
	public static bool opponentIsReady;


    public void Awake()
    {
	//	CardTemplate instance = CardTemplate.Instance; 
	//		DontDestroyOnLoad (gameObject.GetComponent ("Login"));
	//		player_id = GameState.player.GetID ();
    }


	public void Start()
	{
		//Game.StartEnterTransition ();

		playerIsReady = false;
		opponentIsReady = false;
	}

	public void Update()
	{
		//Find opponent ID and if they are ready
		//RequestMatchStatus (int  playerID);
		//ResponseMatchStatus: (Boolean isActive, Boolean opponentIsReady)

		if (!playerIsReady) {
			playerMessage = "You are NOT Ready\n";
			buttonTxt = "Click here if you're ready";
		} 
		else if (playerIsReady)
		{
			playerMessage = "You're Ready!\nWaiting for Opponent";
			buttonTxt = "Click here if you're NOT ready";
		}
		if (!opponentIsReady) {
			opponentMessage = "Opponent is NOT Ready\n";
		} 
		else if (opponentIsReady)
		{
			opponentMessage = "Opponent is Ready!\nWaiting for You";
		}
		if(playerIsReady && opponentIsReady){
			Application.LoadLevel(SceneNameGame);
		}
	}
	//Toggles when player is ready after editing deck
	public void toggleReady()
	{
		//if (!playerIsReady) {
	//		playerIsReady=true;
	//	} 
	//	else if (playerIsReady)
	//	{
	//		playerIsReady=false;
	//	}

	}



	//For debugging  
	//Toggles when opponent is ready
	public void toggleOpponentReady()
	{
		if (!opponentIsReady) {
			opponentIsReady=true;
		} 
		else if (opponentIsReady)
		{
			opponentIsReady=false;
		}
	}

    public void OnGUI()
    {

//		GUI.Label(new Rect(Screen.width / 2 -30, ((Screen.height - 350) / 2)+300, 600, 150), Currency.messagecurrency);
        GUI.skin.box.fontStyle = FontStyle.Bold;
        GUI.Box(new Rect((Screen.width - 400) / 2, (Screen.height - 350) / 2, 400, 300), "Cards of the Wild");
        GUILayout.BeginArea(new Rect((Screen.width - 400) / 2, (Screen.height - 350) / 2, 400, 250));

        GUILayout.Space(50);

		GUILayout.BeginHorizontal();

		if (playerMessage!="" ) GUILayout.Box(playerMessage );
		if (opponentMessage!="") GUILayout.Box(opponentMessage);
		GUILayout.EndHorizontal();

		GUILayout.Space(50);
		

		//Ready buttons
		//Player
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
		if (GUILayout.Button(buttonTxt, GUILayout.Width(200)))
			{
				toggleReady();
			}

			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();

		//Opponent (for debugging)
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		if (GUILayout.Button("Toggle Opponent Ready", GUILayout.Width(200)))
		{
			toggleOpponentReady(); //Activates when created for some reason
		}
		
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			if (GUILayout.Button("Change deck", GUILayout.Width(200)))
			{
				playerIsReady=false;//If they are editing their deck, they aren't ready
//				Application.LoadLevel(EditDeckScene);
				
				
			}
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
		

        GUILayout.EndArea();
    }


}
}