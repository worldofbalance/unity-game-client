using System;
using UnityEngine;

namespace CW{
public class RemoveFromPlay : AbstractCardHandler
{
	public RemoveFromPlay (AbstractCard card, BattlePlayer player) : base(card, player)
	{
		
	}
	
	public override void affect ()
	{
		player.cardsInPlay.Remove(card.gameObject);
		player.cardsInPlay.TrimToSize();
		
		player.GraveYard.Add(card.gameObject);
		GameManager.manager.repositionField();
		

		GameObject.Destroy(card.gameObject);

	}
	
	public override void clicked ()
	{
	}
	
	
}
}

