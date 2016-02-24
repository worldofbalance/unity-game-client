using UnityEngine;

using System;
using System.Collections.Generic;
using System.Reflection;
namespace CW{
public class NetworkProtocolTable {

	private static Dictionary<short, Type> table = new Dictionary<short, Type>();

	private NetworkProtocolTable() {}

	public static void Init() {
		Add(NetworkCode.CLIENT, "Client");
		Add(NetworkCode.LOGIN, "Login");
		Add(NetworkCode.LOGOUT, "Logout");
		Add(NetworkCode.REGISTER, "Register");
		Add(NetworkCode.MESSAGE, "Message");
		Add(NetworkCode.SHOP, "Shop");
		Add(NetworkCode.SPECIES_LIST, "SpeciesList");
		Add(NetworkCode.SPECIES_CREATE, "SpeciesCreate");
		Add(NetworkCode.SHOP_ACTION, "ShopAction");
		Add(NetworkCode.UPDATE_RESOURCES, "UpdateResources");
		Add(NetworkCode.SPECIES_ACTION, "SpeciesAction");
		Add(NetworkCode.PREDICTION, "Prediction");
		Add(NetworkCode.UPDATE_TIME, "UpdateTime");
		Add(NetworkCode.UPDATE_ENV_SCORE, "UpdateEnvScore");
		Add(NetworkCode.WORLD,"World");
		Add(NetworkCode.PLAYERS, "Players");
		Add(NetworkCode.PLAYER_SELECT, "PlayerSelect");
		Add(NetworkCode.MATCH_INIT, "MatchInit");
		Add (NetworkCode.MATCH_STATUS, "MatchStatus");
		Add (NetworkCode.MATCH_OVER, "MatchOver");
		Add (NetworkCode.SUMMON_CARD, "SummonCard");
		Add (NetworkCode.CARD_ATTACK, "CardAttack");
		Add (NetworkCode.QUIT_MATCH, "QuitMatch");
		Add (NetworkCode.DEAL_CARD, "DealCard");
		Add (NetworkCode.END_TURN, "EndTurn");
		Add (NetworkCode.TREE_ATTACK, "TreeAttack");
		Add (NetworkCode.GET_DECK, "GetDeck");
		Add (NetworkCode.MATCH_ACTION, "MatchAction");
		Add (NetworkCode.MATCH_START, "MatchStart");
		Add (NetworkCode.RETURN_LOBBY, "ReturnLobby");

	}	

	
	public static void Add(short protocol_id, string name) {
		Type type = Type.GetType("CW." + name + "Protocol");

		if (type != null) {
			if (!table.ContainsKey(protocol_id)) {
				table.Add(protocol_id, type);
			} else {
				Debug.LogError("Protocol ID " + protocol_id + " already exists! Ignored " + name);
			}
		} else {
			Debug.LogError(name + " not found");
		}
	}
	
	public static Type Get(short protocol_id) {
		Type type = null;
		
		if (table.ContainsKey(protocol_id)) {
			type = table[protocol_id];
		} else {
			Debug.LogError("Protocol [" + protocol_id + "] Not Found");
		}
		
		return type;
	}
}
}