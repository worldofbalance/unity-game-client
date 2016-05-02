using UnityEngine;
using System.Collections;

namespace SD {

    public class RequestSDKeyboard : NetworkRequest {

        public RequestSDKeyboard() {
            request_id = Constants.CMSG_KEYBOARD;
        }

        public void Send(int keyCode, int keyCombination) {
            packet = new GamePacket (request_id);
            packet.addInt32 (keyCode);
            packet.addInt32 (keyCombination);
        }
    }
}
