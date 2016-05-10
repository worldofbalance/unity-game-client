using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

namespace CW{
	public class MatchActionProtocol {
		
		public static NetworkRequest Prepare(int playerID) {
			NetworkRequest request = new NetworkRequest(NetworkCode.MATCH_ACTION);
			request.AddInt32(playerID);
			return request;
		}
		
		public static NetworkResponse Parse(MemoryStream dataStream) {
			ResponseMatchAction response = new ResponseMatchAction();
			response.actionID =  DataReader.ReadShort(dataStream);
			response.intCount = DataReader.ReadInt (dataStream);
			response.stringCount = DataReader.ReadInt (dataStream);
			//Debug.Log("Action Protocol: length of ints " + response.intCount + " stringCount :" + response.stringCount);
			for (int i = 0; i < response.intCount; i++){
				response.intList.Add(DataReader.ReadInt(dataStream));
			}
			for (int i = 0; i < response.stringCount; i++){
				response.stringList.Add(DataReader.ReadString(dataStream));
				//Debug.Log("Action Protocol : read string " + response.stringList[i] + " actionID: " + response.actionID);
			}
			
			// This could be done in a better way
			// Status no match indicates failure to successfully enter a valid match
			if (response.actionID == Constants.STATUS_NO_MATCH) {
				response.action = new InvalidMatchAction (response.intCount, response.stringCount, 
				                                          response.intList, response.stringList);
			} else if (response.actionID == NetworkCode.DEAL_CARD) {
				response.action = new DealCardAction (response.intCount, response.stringCount, 
				                                      response.intList, response.stringList);
			} else if (response.actionID == NetworkCode.SUMMON_CARD) {
				response.action = new SummonCardAction (response.intCount, response.stringCount, 
				                                        response.intList, response.stringList);
			} else if (response.actionID == NetworkCode.END_TURN) {
				response.action = new EndTurnAction (response.intCount, response.stringCount, 
				                                     response.intList, response.stringList);
			} else if (response.actionID == NetworkCode.CARD_ATTACK) {
				response.action = new CardAttackAction (response.intCount, response.stringCount, 
				                                        response.intList, response.stringList);
			} else if (response.actionID == NetworkCode.TREE_ATTACK) {
				response.action = new TreeAttackAction (response.intCount, response.stringCount, 
				                                        response.intList, response.stringList);
			} else if (response.actionID == NetworkCode.QUIT_MATCH) {
				response.action = new QuitMatchAction (response.intCount, response.stringCount, 
				                                       response.intList, response.stringList);
			} else if (response.actionID == NetworkCode.MATCH_OVER) {
				response.action = new MatchOverAction (response.intCount, response.stringCount, 
				                                       response.intList, response.stringList);
			} else if (response.actionID == NetworkCode.MATCH_STATUS) {
				response.action = new MatchStatusAction (response.intCount, response.stringCount, 
				                                         response.intList, response.stringList);
			} else if (response.actionID == NetworkCode.APPLY_FOOD) {
				response.action = new FoodCardAction(response.intCount, response.stringCount,
				                                     response.intList, response.stringList);
            }else if (response.actionID == NetworkCode.APPLY_WEATHER) {
                response.action = new WeatherCardAction (response.intCount, response.stringCount,
                                                     response.intList, response.stringList);
            }
			return response;
		}
		
	}
	
	
	
	public class ResponseMatchAction: NetworkResponse {
		// update numFields if data type added to Cards
		public short actionID {get;set;}
		public int intCount {get; set;}
		public int stringCount {get; set;}
		public List<int> intList{get; set;}
		public List<string> stringList{get; set;}
		public TurnAction action;
		
		public ResponseMatchAction() {
			intList = new List<int>();
			stringList = new List<string>();
			protocol_id = NetworkCode.MATCH_ACTION;
		}
	}
}