using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace CW{
	public class FoodCardAction : TurnAction {
		
		private int attackersPosition, attackedPosition;
		
		public FoodCardAction(int intCount, int stringCount, List<int> intList, List<string> stringList):
		base(intCount, stringCount, intList, stringList){ }
		
		
		override public void readData(){
			attackersPosition = intList[0];
			attackedPosition =  intList[1];
		}
		
		override public void execute(){
			readData ();
            

			GameObject obj = (GameObject)GameManager.player2.cardsInPlay[attackersPosition];
            //GameObject obj = (GameObject)GameManager.player2.cardsInPlay[attackedPosition];
			AbstractCard target = obj.GetComponent<AbstractCard> ();
			GameManager.player2.applyFoodBuff(target, 1, 1);

			GameObject cardUsed = (GameObject)GameManager.player2.hand [0];
			GameManager.player2.hand.Remove(cardUsed);
			GameObject.Destroy (cardUsed);

			// initiate attack
			
		}
	}
}