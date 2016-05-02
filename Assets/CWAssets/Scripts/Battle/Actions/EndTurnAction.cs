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
	
	override public void execute ()
        {
            DebugConsole.Log("end turn action, p2frozen="+GameManager.player2.playerFrozen+",p1frozen="+GameManager.player1.playerFrozen);
            // means player2 played frozed card
            if (GameManager.player2.playerFrozen && GameManager.player1.playerFrozen==true) 
            {
                GameManager.player1.playerFrozen = true;
                GameManager.player2.playerFrozen = false;
                for (int i = 0; i < GameManager.player1.cardsInPlay.Count; i++) {
                    AbstractCard card = ((GameObject)GameManager.player1.cardsInPlay [i]).GetComponent<AbstractCard> ();
                    card.freeze ();
                }

                for (int i = 0; i < GameManager.player2.cardsInPlay.Count; i++) {
                    AbstractCard card = ((GameObject)GameManager.player2.cardsInPlay [i]).GetComponent<AbstractCard> ();
                    card.unfreeze ();
                }
               
            }

		GameManager.manager.startTurn();
	}
}
}