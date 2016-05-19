using UnityEngine;

using System;

namespace SD
{
    public class RequestLogin : NetworkRequest
    {
        public RequestLogin()
        {
            request_id = Constants.CMSG_AUTH;
        }

        public RequestLogin send(string username, string password)
        {
            Debug.Log("sending packet");
            packet = new GamePacket(request_id);
            packet.addString(Constants.CLIENT_VERSION);
            packet.addString(username);
            packet.addString(password);
            return this;
        }
    }
}