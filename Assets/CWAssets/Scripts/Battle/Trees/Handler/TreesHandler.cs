using UnityEngine;
using System.Collections;
namespace CW{
public abstract class TreesHandler {

	protected Trees tree;
	protected BattlePlayer player;
	
	public TreesHandler(Trees tree, BattlePlayer player)
	{
		this.tree = tree;
		this.player = player;
	}
	public abstract void clicked();
	public abstract void affect();
}
}