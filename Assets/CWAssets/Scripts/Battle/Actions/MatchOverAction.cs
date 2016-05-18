using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace CW{
public class MatchOverAction : TurnAction{
	// our primitive system does not have bools so 0 = false, 1=true
	private int wonGame;

	public MatchOverAction(int intCount, int stringCount, List<int> intList, List<string> stringList):
	base(intCount, stringCount, intList, stringList){}	
		
	override public void readData(){
        if (intCount > 0)
        {
            wonGame = intList[0];
        }
    }

    override public void execute()
    {
        readData();
        Debug.Log("Executing MatchOverAction");
        // Do stuff
        //GameManager.protocols.sendMatchOver(GameManager.player1.playerID, wonGame);
    }
}
}