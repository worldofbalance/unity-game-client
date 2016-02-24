using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace CW{
public class QuitMatchAction : TurnAction {

	public QuitMatchAction(int intCount, int stringCount, List<int> intList, List<string> stringList):
	base(intCount, stringCount, intList, stringList){

	}	
	
	override public void readData(){
		// Nothing to do 
		
	}
	
	override public void execute(){
		Debug.Log ("Executing QuitMatchAction");
		Debug.Log ("Returning player to Lobby -- no transition");

		// display game over and calls returnToLobby
		GameManager.player1.createGameover();
	}
}
}