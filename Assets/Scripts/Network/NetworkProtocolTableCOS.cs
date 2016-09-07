using UnityEngine;

using System;
using System.Collections.Generic;
using System.Reflection;

public class NetworkProtocolTableCOS : NetworkProtocolTableBase
{
    //    public NetworkProtocolTableCOS()
    //    {
    //
    //    }

    protected override void Init()
    {
        //Clash of Species
        Add(NetworkCode.CLASH_ENTRY, "ClashEntry");
        Add(NetworkCode.CLASH_SPECIES_LIST, "ClashSpeciesList");
        Add(NetworkCode.CLASH_DEFENSE_SETUP, "ClashDefenseSetup");
        Add(NetworkCode.CLASH_PLAYER_LIST, "ClashPlayerList");
        Add(NetworkCode.CLASH_PLAYER_VIEW, "ClashPlayerView");
        Add(NetworkCode.CLASH_INITIATE_BATTLE, "ClashInitiateBattle");
        Add(NetworkCode.CLASH_END_BATTLE, "ClashEndBattle");
		Add(NetworkCode.CLASH_PLAYER_HISTORY, "ClashPlayerHistory");
		Add(NetworkCode.CLASH_LEADERBOARD, "ClashLeaderboard");
		Add(NetworkCode.CLASH_NOTIFICATION, "ClashNotification");

        Add(NetworkCode.CLIENT, "Client");
        Add(NetworkCode.LOGIN, "Login");
    }

}
