using UnityEngine;

using System;
using System.Collections.Generic;
using System.Reflection;

namespace RR {

    /// <summary>
    /// Network response table.
    /// </summary>
    public class NetworkResponseTable {

        // A map of responses and their codes
        public static Dictionary<short, Type> responseTable { get; set; }

        /// <summary>
        /// Setup the table.
        /// </summary>
        public static void init() {

            // TODO this static initiating method is not very clever as it is
            // relying that it is going to be called and specificaly for one time only.

            responseTable = new Dictionary<short, Type>();
            
            add(RR.Constants.SMSG_AUTH, "ResponseLogin");
            add(RR.Constants.SMSG_HEARTBEAT, "ResponseHeartbeat");
            add(RR.Constants.SMSG_GAME_STATE, "ResponseGameState");
            add(RR.Constants.SMSG_RACE_INIT, "ResponseRaceInit");
            add(RR.Constants.SMSG_KEYBOARD, "ResponseKeyboard");
    //<<<<<<< HEAD
    //        add(Constants.SMSG_RRPOSITION, "RRResponsePostion");
    //<<<<<<< HEAD
    //=======
    //        add(Constants.SMSG_RRSPECIES, "RRResponseSpecies");

    //>>>>>>> Dong
    //=======
            add(RR.Constants.SMSG_RRPOSITION, "ResponseRRPostion");
            add(RR.Constants.SMSG_RRSPECIES, "ResponseRRSpecies");
            add(RR.Constants.SMSG_RRSTARTGAME, "ResponseRRStartGame");

            add(RR.Constants.SMSG_RRENDGAME, "ResponseRREndGame");
            add(RR.Constants.SMSG_RRGETMAP, "ResponseRRGetMap");
    //>>>>>>> start
            
    //        Debug.Log("dictionary check: " + responseTable.TryGetValue);
    //        Debug.Log("Response table: " + (responseTable[Constants.SMSG_AUTH]).ToString());
        }

        /// <summary>
        /// Add the specified response_id and name.
        /// </summary>
        /// <param name="response_id">Response identifier.</param>
        /// <param name="name">Name.</param>
        public static void add(short response_id, string name) {
            responseTable.Add(response_id, Type.GetType("RR."+name));
        }

        /// <summary>
        /// Get the specified response_id.
        /// </summary>
        /// <param name="response_id">Response identifier.</param>
        /// <returns>NetworkResponse</returns>
        public static NetworkResponse get(short response_id) {
            NetworkResponse response = null;
            
            if (responseTable.ContainsKey(response_id)) {
                response = (NetworkResponse) Activator.CreateInstance(responseTable[response_id]);
                response.response_id = response_id;
            } else {
                Debug.Log("Response [" + response_id + "] Not Found");
                Debug.Log (response.ToString());
            }
            
            return response;
        }
    }
}