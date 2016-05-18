using UnityEngine;
using System.Collections;
using System;
using System.IO;

namespace SD
{
    public class SDEndGameProtocol {
        public static NetworkRequest Prepare (bool gameCompleted, float score)
        {
            NetworkRequest request = new NetworkRequest (NetworkCode.SD_END_GAME);
            request.AddBool (gameCompleted);
            request.AddFloat (score);
            return request;
        }

        public static NetworkResponse Parse (MemoryStream dataStream) {
            ResponseSDEndGame response = new ResponseSDEndGame ();
            response.status = DataReader.ReadInt (dataStream);
            response.winningScore = DataReader.ReadFloat (dataStream);
            response.winningPlayerId = DataReader.ReadString (dataStream);
            return response;
        }
    }

    public class ResponseSDEndGame : NetworkResponse
    {
        public int status { get; set; }
        public float winningScore { get; set; }
        public string winningPlayerId { get; set; }

        public ResponseSDEndGame ()
        {
            protocol_id = NetworkCode.SD_END_GAME;
        }
    }
}

