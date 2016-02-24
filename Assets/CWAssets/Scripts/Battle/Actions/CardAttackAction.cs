using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace CW{
public class CardAttackAction : TurnAction {

	private int attackersPosition, attackedPosition;

	public CardAttackAction(int intCount, int stringCount, List<int> intList, List<string> stringList):
	base(intCount, stringCount, intList, stringList){ }
		

	override public void readData(){
		attackersPosition = intList[0];
		attackedPosition =  intList[1];
	}
	
	override public void execute(){
		readData ();

		GameManager.player2.attackWith(attackersPosition, attackedPosition);
		Debug.Log ("Executing CardAttackAction, attackedPosition:" + attackedPosition);
		// initiate attack

	}
}
}