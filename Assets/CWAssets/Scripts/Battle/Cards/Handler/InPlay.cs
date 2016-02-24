using System;
using UnityEngine;

namespace CW{
public class InPlay : AbstractCardHandler
{
	public InPlay(AbstractCard card, BattlePlayer player) : base(card, player) {
	
	}

	public override void affect ()
	{
		BattlePlayer currentPlayer = GameManager.curPlayer;
		
		if(currentPlayer.clickedCard == null && player.isActive && card.canAttack()){
			player.clickedCard = card;
			if(player.player1){
				Debug.Log("Player 1 is ready to attack");
			} else {
				Debug.Log("Player 2 is ready to attack");
			}		
			
		}else if(currentPlayer != player && currentPlayer.clickedCard != null && currentPlayer.clickedCard.diet != AbstractCard.DIET.HERBIVORE){
			currentPlayer.targetCard = card;	
			bool attackback = false;
			if(currentPlayer.targetCard.diet != AbstractCard.DIET.HERBIVORE){
				attackback = true;
			}
			currentPlayer.clickedCard.attack(currentPlayer.clickedCard, currentPlayer.targetCard, attackback);

			currentPlayer.getProtocolManager().sendCardAttack(currentPlayer.playerID, currentPlayer.clickedCard.fieldIndex, currentPlayer.targetCard.fieldIndex);
			
			currentPlayer.clickedCard = null;
			currentPlayer.targetCard = null;
			
		}
		
		if(currentPlayer.clickedCard != null && currentPlayer == player && card != currentPlayer.clickedCard){
			currentPlayer.clickedCard = null;	
		}
		
	}
	public override void clicked ()
	{
		
		affect();
	}
}
}

