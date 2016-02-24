using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace CW{
public class DealCardAction : TurnAction {
		


	public DealCardAction(int intCount, int stringCount, List<int> intList, List<string> stringList):
	base(intCount, stringCount, intList, stringList){ 	
	}

	override public void readData(){
		// Nothing do do here -- just deal a card to player2 in execute
	}

	 override public void execute(){
		Debug.Log ("Executing DealCard");
		// Deal one card .. no other information should be needed


		//GameManager.player2.dealDummyCard (1);
	}
}
}