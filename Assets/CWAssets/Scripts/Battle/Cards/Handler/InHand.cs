using UnityEngine;
using System;

namespace CW{
public class InHand : AbstractCardHandler
{
	public InHand(AbstractCard card, BattlePlayer player) : base(card, player) {

	}
	

	override public void affect(){



	//set Max card in field.
	if (player.cardsInPlay.Count < 6 && player.isActive && player.currentMana >= card.getManaCost() && player == GameManager.player1) {
		
		GameObject removeCard = (GameObject)player.hand[player.hand.IndexOf(card.gameObject)];
		
		int temp = 0, temp2 = 0, count = 0;
		string cardName, cardName2, newCardName, newCardName2;
		
		card.isInHand = false;
		player.hand.Remove (removeCard);
		player.currentMana -= card.getManaCost();

		card.isInPlay = true;
		player.cardsInPlay.Add (removeCard.gameObject);

	
		card.setCanAttack(false);
		card.handler = new InPlay (card, player);
			Vector3 position = new Vector3(card.transform.position.x, card.transform.position.y, card.transform.position.z);
			player.reposition();
			Vector3 destination =  new Vector3(card.transform.position.x, card.transform.position.y, card.transform.position.z);
			card.transform.position = position;

			card.calculateDirection(destination, false);

		GameManager.player1.getProtocolManager().sendSummon (player.playerID, card.cardID,  card.dietChar, 
			                                        card.level, card.dmg,  card.maxHP, 
			                                        card.name,  card.type, 
			                                        card.description);

		}
	
	}
	override public void clicked(){
	
		if(player.isActive)
			affect();
			
		
	}
}

}
	

