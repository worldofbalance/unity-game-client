using UnityEngine;
using System.Collections;

namespace SD
{

    public class RequestSDPrey : NetworkRequest
    {

        public RequestSDPrey()
        {
            request_id = Constants.CMSG_PREY;
        }

        public void send(int i)
        {
            packet = new GamePacket(request_id);
            packet.addInt32(i);
        }
    }
}
