using UnityEngine;
using System.Collections;
namespace CW{
public class EndGame : TreesHandler {

	
	public EndGame(Trees tree, BattlePlayer player) : base(tree, player) {
		
	}
	
	override public void clicked(){
		
	}
	
	public override void affect ()
	{
		player.createGameover ();
	}
}
}