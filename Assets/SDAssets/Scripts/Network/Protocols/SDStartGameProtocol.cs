using UnityEngine;
using System.Collections;
using System;
using System.IO;

namespace SD
{
    public class SDStartGameProtocol {
        public static NetworkRequest Prepare (int playerID)
        {
            NetworkRequest request = new NetworkRequest (NetworkCode.SD_START_GAME);
            request.AddInt32 (playerID);
            return request;
        }

        public static NetworkResponse Parse (MemoryStream dataStream) {
            ResponseSDStartGame response = new ResponseSDStartGame ();
            response.status = DataReader.ReadShort(dataStream);
            return response;
        }
    }

    public class ResponseSDStartGame : NetworkResponse
    {
        public short status { get; set; }

        public ResponseSDStartGame ()
        {
            protocol_id = NetworkCode.SD_START_GAME;
        }
    }
}