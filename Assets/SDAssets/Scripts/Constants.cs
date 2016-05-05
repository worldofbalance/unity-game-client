using UnityEngine;
using System.Collections;

namespace SD
{
    public class Constants
    {
        // Constants
        public static readonly string CLIENT_VERSION = "1.00";

        public static readonly int REMOTE_PORT = 9258; // 20040

        public static int USER_ID = -1; // Overwritten by the response. (Player_id)
        public static int PLAYER_NUMBER = -1;

        public static readonly int TEMP_ROOM_ID = 101;

        public static readonly int PLAYER_WIN = 1;
        public static readonly int PLAYER_LOSE = 2;
        public static readonly int PLAYER_DRAW = 3;
    }
}

