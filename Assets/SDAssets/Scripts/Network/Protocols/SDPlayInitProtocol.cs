using UnityEngine;
using System.Collections;
using System;
using System.IO;

namespace SD
{
    public class SDPlayInitProtocol {
        public static NetworkRequest Prepare (int playerID, int roomID)
        {
            NetworkRequest request = new NetworkRequest (NetworkCode.SD_PLAY_INIT);
            request.AddInt32 (playerID);
            request.AddInt32 (roomID);
            return request;
        }

        public static NetworkResponse Parse (MemoryStream dataStream) {
            ResponseSDPlayInit response = new ResponseSDPlayInit ();
            response.status = DataReader.ReadShort (dataStream);
            response.playerId = DataReader.ReadInt (dataStream);
            response.playerNumber = DataReader.ReadInt (dataStream);
            response.playerName = DataReader.ReadString (dataStream);
            return response;
        }
    }

    public class ResponseSDPlayInit : NetworkResponse
    {

        public short status { get; set; }
        public int playerId { get; set; }
        public int playerNumber { get; set; }
        public string playerName { get; set; }

        public ResponseSDPlayInit ()
        {
            protocol_id = NetworkCode.SD_PLAY_INIT;
        }
    }
}