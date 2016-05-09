using UnityEngine;
using System.Collections;
using System;
using System.IO;

namespace SD
{
    public class SDNpcPositionProtocol {
        public static NetworkRequest Prepare (int numNpc)
        {
            NetworkRequest request = new NetworkRequest (NetworkCode.SD_NPCPOSITION);
            request.AddInt32 (numNpc);
            return request;
        }

        public static NetworkResponse Parse (MemoryStream dataStream) {
            ResponseSDNpcPosition response = new ResponseSDNpcPosition ();
            response.numFish = DataReader.ReadInt (dataStream);
            for (int i = 0; i < response.numFish; i++) {
                response.preyId = DataReader.ReadInt (dataStream);
                response.speciesId = DataReader.ReadInt (dataStream);
                response.xPosition = DataReader.ReadFloat (dataStream);
                response.yPosition = DataReader.ReadFloat (dataStream);
                response.rotation = DataReader.ReadFloat (dataStream);
            }
            return response;
        }
    }

    public class ResponseSDNpcPosition : NetworkResponse
    {
        public int numFish {get; set;}
        public int preyId { get; set; }
        public int speciesId {get; set;}
        public float xPosition { get; set; }
        public float yPosition { get; set; }
        public float rotation { get; set; }

        public ResponseSDNpcPosition ()
        {
            protocol_id = NetworkCode.SD_NPCPOSITION;
        }
    }
}
