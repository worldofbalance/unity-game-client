using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace CW{
public class MatchStatusAction : TurnAction{
	// our primitive system does not have bools so 0 = false, 1=true
	private string playerName;

	public MatchStatusAction(int intCount, int stringCount, List<int> intList, List<string> stringList):
	base(intCount, stringCount, intList, stringList){}	
	
	override public void readData(){
		// Player probably lost if receiving a MatchOver response
	}
	
	override public void execute(){
		playerName = stringList[0];
		Debug.Log ("MatchStatusAction: Player2's Name " + playerName);
		Debug.Log ("Executing MatchStatusAction");
		// set player2 name
		GameManager.player2.playerName = playerName;
	}
}
}