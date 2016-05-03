using UnityEngine;
using System.Collections;

namespace SD {
    public class RequestSDEndGame : NetworkRequest {
        public RequestSDEndGame() {
            packet = new GamePacket (request_id = Constants.CMSG_SDEND_GAME);
        }

        public void Send(bool gameCompleted, float score) {
            packet.addBool (gameCompleted);
            packet.addFloat32 (score);
        }
    }
}
