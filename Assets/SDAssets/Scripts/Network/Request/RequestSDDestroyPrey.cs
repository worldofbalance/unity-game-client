using System;
using System.Collections;

namespace SD
{
    public class RequestSDDestroyPrey : NetworkRequest
    {
        public RequestSDDestroyPrey ()
        {
            request_id = Constants.CMSG_EAT_PREY;
        }

        public void Send(int i) {
            packet = new GamePacket (request_id);
            packet.addInt32 (i);
        }
    }
}

