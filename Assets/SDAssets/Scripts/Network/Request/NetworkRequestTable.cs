using UnityEngine;

using System;
using System.Collections.Generic;
using System.Reflection;

namespace SD
{
    public class NetworkRequestTable
    {

        public static Dictionary<short, Type> requestTable { get; set; }

        public static void init()
        {
            requestTable = new Dictionary<short, Type>();
            add(Constants.CMSG_AUTH, "RequestLogin");
            add(Constants.CMSG_RACE_INIT, "RequestPlayInit");
            add (Constants.CMSG_SDSTART_GAME, "RequestSDStartGame");
            add (Constants.CMSG_SDEND_GAME, "RequestSDEndGame");
            add (Constants.CMSG_KEYBOARD, "RequestSDKeyboard");
            add (Constants.CMSG_POSITION, "RequestSDPosition");
            add(Constants.CMSG_PREY, "RequestSDPrey");
            add (Constants.CMSG_EAT_PREY, "RequestSDDestroyPrey");
            add (Constants.CMSG_SCORE, "RequestSDScore");
        }

        public static void add(short request_id, string name)
        {
            requestTable.Add(request_id, Type.GetType(name));
        }

        public static NetworkRequest get(short request_id)
        {
            NetworkRequest request = null;

            if (requestTable.ContainsKey(request_id))
            {
                request = (NetworkRequest)Activator.CreateInstance(requestTable[request_id]);
                request.request_id = request_id;
            }
            else {
                Debug.Log("Request [" + request_id + "] Not Found");
            }

            return request;
        }
    }
}