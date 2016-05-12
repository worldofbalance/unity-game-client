using UnityEngine;
using System.Collections;
using System;
using System.IO;

namespace SD
{
    public class SDPlayerPositionProtocol {
        public static NetworkRequest Prepare (string x, string y, string rotation)
        {
            NetworkRequest request = new NetworkRequest (NetworkCode.SD_PLAYER_POSITION);
            request.AddString (x); // Change these to float at some point
            request.AddString (y);
            request.AddString (rotation);
            return request;
        }

        public static NetworkResponse Parse (MemoryStream dataStream) {
            ResponseSDPlayerPosition response = new ResponseSDPlayerPosition ();
            response.xPosition = DataReader.ReadFloat (dataStream);
            response.yPosition = DataReader.ReadFloat (dataStream);
            response.rotation = DataReader.ReadFloat (dataStream);
            return response;
        }
    }

    public class ResponseSDPlayerPosition : NetworkResponse
    {
        public float xPosition { get; set; }
        public float yPosition { get; set; }
        public float rotation { get; set; }

        public ResponseSDPlayerPosition ()
        {
            protocol_id = NetworkCode.SD_PLAYER_POSITION;
        }
    }
}

