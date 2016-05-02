using UnityEngine;
using System;

namespace SD
{
    public class ResponseSDStartGameEventArgs : ExtendedEventArgs {
        public short status { get; set; }

        public ResponseSDStartGameEventArgs() {
            event_id = Constants.SMSG_SDSTART_GAME;
        }
    }

    public class ResponseSDStartGame : NetworkResponse
    {
        private short status;
        public ResponseSDStartGame ()
        {
        }

        public override void parse() {
            status = DataReader.ReadShort(dataStream);
            Debug.Log("Parsed ResponseSDStartGame");
            Debug.Log ("The status is " + status);
        }

        public override ExtendedEventArgs process() {
            ResponseSDStartGameEventArgs args = new ResponseSDStartGameEventArgs();
            args.status = status;
            // Process the status once the server returns a number other than 0
            return args;
        }
    }
}
