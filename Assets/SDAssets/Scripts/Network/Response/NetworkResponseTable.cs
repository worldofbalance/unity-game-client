using UnityEngine;

using System;
using System.Collections.Generic;
using System.Reflection;

namespace SD
{
    public class NetworkResponseTable
    {

        public static Dictionary<short, Type> responseTable { get; set; }

        public static void init()
        {
            responseTable = new Dictionary<short, Type>();

            add(Constants.SMSG_AUTH, "ResponseLogin");
            add(Constants.SMSG_RACE_INIT, "ResponsePlayInit");
            add (Constants.SMSG_SDSTART_GAME, "ResponseSDStartGame");
            add (Constants.SMSG_SDEND_GAME, "ResponseSDEndGame");
            add (Constants.SMSG_KEYBOARD, "ResponseSDKeyboard");
            add (Constants.SMSG_POSITION, "ResponseSDPosition");
        }

        public static void add(short response_id, string name)
        {
            responseTable.Add(response_id, Type.GetType("SD." + name));
        }

        public static NetworkResponse get(short response_id)
        {
            NetworkResponse response = null;

            		Debug.Log("Response table: " + responseTable[response_id].ToString());

            if (responseTable.ContainsKey(response_id))
            {
                response = (NetworkResponse)Activator.CreateInstance(responseTable[response_id]);
                response.response_id = response_id;
            }
            else {
                Debug.Log("Response [" + response_id + "] Not Found");
                Debug.Log(response.ToString());
            }

            return response;
        }
    }
}