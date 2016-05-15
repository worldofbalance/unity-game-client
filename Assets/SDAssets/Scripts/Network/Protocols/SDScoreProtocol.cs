using UnityEngine;
using System.Collections;
using System;
using System.IO;

namespace SD
{
    public class SDScoreProtocol {
        public static NetworkRequest Prepare (float score)
        {
            NetworkRequest request = new NetworkRequest (NetworkCode.SD_SCORE);
            request.AddFloat (score);
            return request;
        }

        public static NetworkResponse Parse (MemoryStream dataStream) {
            ResponseSDScore response = new ResponseSDScore ();
            response.score = DataReader.ReadInt (dataStream);
            return response;
        }
    }

    public class ResponseSDScore : NetworkResponse
    {
        public int score { get; set; }

        public ResponseSDScore ()
        {
            protocol_id = NetworkCode.SD_SCORE;
        }
    }
}

