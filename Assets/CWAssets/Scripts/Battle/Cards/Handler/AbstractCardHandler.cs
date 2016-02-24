using System;

namespace CW{
public abstract class AbstractCardHandler
{
	protected AbstractCard card;
	protected BattlePlayer player;
	
	public AbstractCardHandler(AbstractCard card, BattlePlayer player)
	{
		this.card = card;
		this.player = player;
	}
	public abstract void clicked();
	public abstract void affect();
}

}
