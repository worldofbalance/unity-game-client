using UnityEngine;
using System.Collections;
using System;
using System.IO;

namespace SD
{
    public class SDKeyboardProtocol {
        public static NetworkRequest Prepare (int keyCode, int keyCombination)
        {
            NetworkRequest request = new NetworkRequest (NetworkCode.SD_KEYBOARD);
            request.AddInt32 (keyCode);
            request.AddInt32 (keyCombination);
            return request;
        }

        public static NetworkResponse Parse (MemoryStream dataStream) {
            ResponseSDKeyboard response = new ResponseSDKeyboard ();
            response.keyCode = DataReader.ReadInt (dataStream);
            response.keyCombination = DataReader.ReadInt (dataStream);
            return response;
        }
    }

    public class ResponseSDKeyboard : NetworkResponse
    {
        public int keyCode { get; set; }
        public int keyCombination { get; set;}

        public ResponseSDKeyboard ()
        {
            protocol_id = NetworkCode.SD_KEYBOARD;
        }
    }
}

