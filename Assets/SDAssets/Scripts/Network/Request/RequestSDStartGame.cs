using System;

namespace SD
{
    public class RequestSDStartGame : NetworkRequest
    {
        public RequestSDStartGame()
        {
            packet = new GamePacket (request_id = Constants.CMSG_SDSTART_GAME);
        }

        public void Send(int playerId)
        {
            packet.addInt32 (playerId);
        }
    }
}