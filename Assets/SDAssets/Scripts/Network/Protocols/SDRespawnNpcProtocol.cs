using UnityEngine;
using System.Collections;
using System;
using System.IO;

namespace SD
{
    public class SDRespawnNpcProtocol {
        public static NetworkRequest Prepare ()
        {
            NetworkRequest request = new NetworkRequest (NetworkCode.SD_RESPAWN);
            return request;
        }

        public static NetworkResponse Parse (MemoryStream dataStream) {
            ResponseSDRespawnNpc response = new ResponseSDRespawnNpc ();
            response.speciesId = DataReader.ReadInt (dataStream);
            response.numNpc = DataReader.ReadInt (dataStream);
            return response;
        }
    }

    public class ResponseSDRespawnNpc : NetworkResponse
    {

        public int speciesId { get; set; }  // The species to spawn
        public int numNpc { get; set;}      // The number of that species to spawn

        public ResponseSDRespawnNpc ()
        {
            protocol_id = NetworkCode.SD_RESPAWN;
        }
    }
}
