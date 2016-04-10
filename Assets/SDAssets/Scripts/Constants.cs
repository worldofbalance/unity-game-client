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

        public static readonly short CMSG_SDSTART_GAME = 301;
        public static readonly short SMSG_SDSTART_GAME = 401;

        public static readonly short CMSG_SDEND_GAME = 701;
        public static readonly short SMSG_SDEND_GAME = 801;

        public static int USER_ID = -1;
    }
}

