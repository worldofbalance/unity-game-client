using UnityEngine;
using System.Collections;
using System;
using System.IO;

namespace SD
{
    public class SDHeartbeatProtocol {
        public static NetworkRequest Prepare ()
        {
            NetworkRequest request = new NetworkRequest (NetworkCode.SD_HEARTBEAT);
            return request;
        }
    }
}

