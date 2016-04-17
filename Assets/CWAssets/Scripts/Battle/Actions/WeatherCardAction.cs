using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace CW{
    public class WeatherCardAction : TurnAction {
        
        private int card_id;
        private int player_id;
        
        public WeatherCardAction(int intCount, int stringCount, List<int> intList, List<string> stringList):
        base(intCount, stringCount, intList, stringList){ }
        
        
        override public void readData(){
            player_id = intList [0];
            card_id = intList[1];
        }
        
        override public void execute(){
            readData ();
            GameManager.player1.applyWeather (card_id);
            GameManager.player2.applyWeather (card_id);

            //removes card from opponents hand
            GameObject cardUsed = (GameObject)GameManager.player2.hand [0];
            GameManager.player2.hand.RemoveAt (0);
            GameObject.Destroy (cardUsed);
            Debug.Log ("Executing WeatherCardAction: " + card_id);
            // initiate attack
            
        }

    }
}