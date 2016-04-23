using UnityEngine;
using System.Collections;

namespace SD
{
    public class Constants
    {
        // Constants
        public static readonly string CLIENT_VERSION = "1.00";
        //public static readonly string REMOTE_HOST = "localhost";
        public static readonly int REMOTE_PORT = 20040;

        public static readonly short CMSG_AUTH = 101;
        public static readonly short SMSG_AUTH = 201;

        public static readonly short CMSG_RACE_INIT = 501;
        public static readonly short SMSG_RACE_INIT = 601;

        public static readonly short CMSG_SDSTART_GAME = 502;
        public static readonly short SMSG_SDSTART_GAME = 602;

        public static readonly short CMSG_SDEND_GAME = 503;
        public static readonly short SMSG_SDEND_GAME = 603;

        public static readonly short CMSG_KEYBOARD = 108;
        public static readonly short SMSG_KEYBOARD = 208;

        public static readonly short CMSG_POSITION = 110;
        public static readonly short SMSG_POSITION = 210;

        public static readonly short CMSG_PREY = 504;
        public static readonly short SMSG_PREY = 604;

        public static int USER_ID = -1; // Overwritten by the response. (Player_id)
        public static readonly int TEMP_ROOM_ID = 101;

        public static readonly int PLAYER_WIN = 1;
        public static readonly int PLAYER_LOSE = 2;
        public static readonly int PLAYER_DRAW = 3;
    }
}

