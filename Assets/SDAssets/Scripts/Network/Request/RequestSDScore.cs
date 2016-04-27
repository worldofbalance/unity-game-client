using System;

namespace SD
{
    public class RequestSDScore : NetworkRequest
    {
        public RequestSDScore()
        {
            packet = new GamePacket (request_id = Constants.CMSG_SCORE);
        }

        public void Send(float score)
        {
            packet.addFloat32 (score);
        }
    }
}