using UnityEngine;

using System;

namespace RR
{
    /// <summary>
    /// Request login.
    /// </summary>
    public class RequestLogin : NetworkRequest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RR.RequestLogin"/> class.
        /// </summary>
        public RequestLogin ()
        {
            request_id = Constants.CMSG_AUTH;
        }
    
        /// <summary>
        /// Builds a login request packet
        /// </summary>
        /// <param name="username">Username.</param>
        /// <param name="password">Password.</param>
        public RequestLogin send (string username, string password)
        {
            Debug.Log ("Building Packet");

            packet = new GamePacket (request_id);
            packet.addString (Constants.CLIENT_VERSION);    
            packet.addString (username);
            packet.addString (password);

            return this;
        }
    }
}