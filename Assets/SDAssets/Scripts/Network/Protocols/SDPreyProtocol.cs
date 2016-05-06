using UnityEngine;
using System.Collections;
using System;
using System.IO;

namespace SD
{
    public class SDPreyProtocol {
        public static NetworkRequest Prepare (int preyId)
        {
            NetworkRequest request = new NetworkRequest (NetworkCode.SD_PREY);
            request.AddInt32 (preyId);
            return request;
        }

        public static NetworkResponse Parse (MemoryStream dataStream) {
            ResponseSDPrey response = new ResponseSDPrey ();
            response.preyId = DataReader.ReadInt(dataStream);
            response.xPosition = DataReader.ReadFloat(dataStream);
            response.yPosition = DataReader.ReadFloat(dataStream);
            response.isAlive = DataReader.ReadBool(dataStream);
            return response;
        }
    }

    public class ResponseSDPrey : NetworkResponse
    {
        public int preyId { get; set; }
        public float xPosition { get; set; }
        public float yPosition { get; set; }
        public bool isAlive { get; set; }

        public ResponseSDPrey ()
        {
            protocol_id = NetworkCode.SD_PREY;
        }
    }
}

