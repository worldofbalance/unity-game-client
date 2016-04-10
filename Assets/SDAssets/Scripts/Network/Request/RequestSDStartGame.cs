using System;

namespace SD
{
    public class RequestSDStartGame : NetworkRequest
    {
        public RequestSDStartGame()
        {
            packet = new GamePacket (Constants.USER_ID);
        }

        public void Send(int playerId)
        {
            packet.addInt32 (playerId);
        }
    }
}

