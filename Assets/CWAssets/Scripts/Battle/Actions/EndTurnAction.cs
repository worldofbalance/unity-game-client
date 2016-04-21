using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace CW{
public class EndTurnAction : TurnAction {

	public EndTurnAction(int intCount, int stringCount, List<int> intList, List<string> stringList):
	base(intCount, stringCount, intList, stringList){ 	
	
	}
	
	override public void readData(){
		// Nothing to do 

	}
	
	override public void execute(){
		//Debug.Log ("Executing EndTurnA");
		// This means the opponents turn as ended so start player1's turn
		GameManager.manager.startTurn();
	}
}
}