using UnityEngine;
using System.Collections;
using System;
using System.IO;

namespace SD
{
    public class SDDestroyPreyProtocol {
        public static NetworkRequest Prepare (int preyId, int speciesId)
        {
            NetworkRequest request = new NetworkRequest (NetworkCode.SD_EAT_PREY);
            request.AddInt32 (speciesId);
            request.AddInt32 (preyId);
            return request;
        }

        public static NetworkResponse Parse (MemoryStream dataStream) {
            ResponseSDDestroyPrey response = new ResponseSDDestroyPrey ();
            response.status = DataReader.ReadShort (dataStream);
            response.preyId = DataReader.ReadInt (dataStream);
            return response;
        }
    }

    public class ResponseSDDestroyPrey : NetworkResponse
    {

        public short status { get; set; }
        public int preyId { get; set; }

        public ResponseSDDestroyPrey ()
        {
            protocol_id = NetworkCode.SD_EAT_PREY;
        }
    }
}
