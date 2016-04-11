using UnityEngine;
using System.Collections;

namespace SD {

    public class RequestSDPosition : NetworkRequest {

        public RequestSDPosition() {
            request_id = Constants.CMSG_POSITION;
        }

        public void send(float x, float y) {
            packet = new GamePacket (request_id);
            packet.addFloat32 (x);
            packet.addFloat32 (y);
        }
    }
}
