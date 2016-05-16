using UnityEngine;
using System.Collections;
using System;
using System.IO;

namespace SD
{
    public class SDLoginProtocol {
        public static NetworkRequest Prepare (string username, string password)
        {
            NetworkRequest request = new NetworkRequest (NetworkCode.SD_GAME_LOGIN);
            request.AddString (Constants.CLIENT_VERSION);
            request.AddString (username);
            request.AddString (password);
            return request;
        }

        public static NetworkResponse Parse (MemoryStream dataStream) {
            ResponseSDLogin response = new ResponseSDLogin ();
            response.status = DataReader.ReadShort(dataStream);
            if (response.status == 0) {
                response.userId = DataReader.ReadInt (dataStream);
                response.username = DataReader.ReadString (dataStream);
            }

            //response.lastLogout = DataReader.ReadString (dataStream);  TODO 
            //response.playerMoney = DataReader.ReadInt (dataStream);
            //response.playerLevel = DataReader.ReadShort (dataStream);
            return response;
        }
    }

    public class ResponseSDLogin : NetworkResponse
    {

        public short status { get; set; }
        public int userId { get; set; }
        public string username { get; set; }
        //public string lastLogout { get; set; }
        //public short playerLevel { get; set; }
        //public int playerMoney { get; set; }

        public ResponseSDLogin ()
        {
            protocol_id = NetworkCode.SD_GAME_LOGIN;
        }
    }
}
