using System;
using UnityEngine;
namespace CW{
public class LivingTreeClick : TreesHandler {
	
	public LivingTreeClick (Trees tree, BattlePlayer player) : base(tree, player)
	{
		
	}
	
	
	//Has many conditions that allow the card to attack this tree
	override public void clicked(){
		
		if(!player.isActive 				//The tree belongs to opposite team of attacker, 
			&& GameManager.curPlayer.clickedCard != null 	//There is an attker card
			&& GameManager.curPlayer.clickedCard.canAttack() //If the card hasn't attacked this turn
			&& GameManager.curPlayer.clickedCard.diet != AbstractCard.DIET.CARNIVORE){ //if the clicked card is not a carnivore
			
			//Conditions met, allow through
			affect ();
		}
	}
	
	//Clicked card attacks the tree
	override public void affect(){

		GameManager.curPlayer.getProtocolManager().sendTreeAttack(GameManager.curPlayer.playerID, GameManager.curPlayer.clickedCard.fieldIndex);

		GameManager.curPlayer.clickedCard.attackTree(tree);

	}
}
}

