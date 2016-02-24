using UnityEngine;

using System;
using System.Collections.Generic;
using System.Reflection;

public class NetworkProtocolTable {

	private static Dictionary<short, Type> table = new Dictionary<short, Type>();

	private NetworkProtocolTable() {}

	public static void Init() {
		Add(NetworkCode.CLIENT, "Client");
		Add(NetworkCode.HEARTBEAT, "Heartbeat");
		Add(NetworkCode.LOGIN, "Login");
		Add(NetworkCode.LOGOUT, "Logout");
		Add(NetworkCode.REGISTER, "Register");
		Add(NetworkCode.MESSAGE, "Message");
		Add(NetworkCode.SHOP, "Shop");
		Add(NetworkCode.SPECIES_LIST, "SpeciesList");
		Add(NetworkCode.SPECIES_CREATE, "SpeciesCreate");
		Add(NetworkCode.ECOSYSTEM, "Ecosystem");
		Add(NetworkCode.SHOP_ACTION, "ShopAction");
		Add(NetworkCode.UPDATE_RESOURCES, "UpdateResources");
		Add(NetworkCode.SPECIES_ACTION, "SpeciesAction");
		Add(NetworkCode.PREDICTION, "Prediction");
		Add(NetworkCode.UPDATE_TIME, "UpdateTime");
		Add(NetworkCode.UPDATE_ENV_SCORE, "UpdateEnvScore");
		Add(NetworkCode.ZONE_LIST,"ZoneList");
		Add(NetworkCode.ZONE,"Zone");
		Add(NetworkCode.WORLD,"World");
		Add(NetworkCode.ZONE_UPDATE, "ZoneUpdate");
		Add(NetworkCode.PLAYERS, "Players");
		Add(NetworkCode.PLAYER_SELECT, "PlayerSelect");
		Add(NetworkCode.CONVERGE_ECOSYSTEMS, "ConvergeEcosystems");
		Add(NetworkCode.CONVERGE_NEW_ATTEMPT, "ConvergeNewAttempt");
		Add(NetworkCode.CONVERGE_PRIOR_ATTEMPT, "ConvergePriorAttempt");
		Add(NetworkCode.CONVERGE_PRIOR_ATTEMPT_COUNT, "ConvergePriorAttemptCount");
		Add(NetworkCode.CONVERGE_HINT, "ConvergeHint");
		Add(NetworkCode.CONVERGE_HINT_COUNT, "ConvergeHintCount");
		Add(NetworkCode.CONVERGE_NEW_ATTEMPT_SCORE, "ConvergeNewAttemptScore");
		Add(NetworkCode.TOPLIST, "TopList");
		Add(NetworkCode.PAIR, "Pair");
		Add(NetworkCode.QUIT_ROOM, "QuitRoom");
		Add(NetworkCode.GET_ROOMS, "GetRooms");
		Add(NetworkCode.BACK_TO_LOBBY, "BackToLobby");
		Add(NetworkCode.PLAY_GAME, "PlayGame");
		Add(NetworkCode.END_GAME, "EndGame");

		//Clash of Species
		Add(NetworkCode.CLASH_ENTRY, "ClashEntry");
		Add(NetworkCode.CLASH_SPECIES_LIST, "ClashSpeciesList");
		Add(NetworkCode.CLASH_DEFENSE_SETUP, "ClashDefenseSetup");
		Add(NetworkCode.CLASH_PLAYER_LIST, "ClashPlayerList");
		Add(NetworkCode.CLASH_PLAYER_VIEW, "ClashPlayerView");
		Add(NetworkCode.CLASH_INITIATE_BATTLE, "ClashInitiateBattle");
		Add(NetworkCode.CLASH_END_BATTLE, "ClashEndBattle");
	}
	
	public static void Add(short protocol_id, string name) {
		Type type = Type.GetType(name + "Protocol");

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
