using UnityEngine;
using System.Collections;

namespace SD {

    public class RequestSDPosition : NetworkRequest {

        public RequestSDPosition() {
            request_id = Constants.CMSG_POSITION;
        }

        public void Send(string x, string y) {
            packet = new GamePacket (request_id);
            packet.addString (x);
            packet.addString (y);
        }
    }
}
