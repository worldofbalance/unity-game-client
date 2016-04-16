using UnityEngine;

using System;
using System.Collections.Generic;

namespace SD
{
    public class RequestPlayInit : NetworkRequest
    {

        public RequestPlayInit()
        {
            packet = new GamePacket(request_id = Constants.CMSG_RACE_INIT);
        }

        public void Send(int playerid, int roomid)
        {
            packet.addInt32(playerid);
            packet.addInt32(roomid);
        }
    }
}