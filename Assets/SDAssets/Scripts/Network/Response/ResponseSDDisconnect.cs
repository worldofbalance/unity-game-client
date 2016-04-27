using UnityEngine;
using System;

namespace SD
{
    public class ResponseSDDisconnectEventArgs : ExtendedEventArgs {
        public short status { get; set; }

        public ResponseSDDisconnectEventArgs() {
            event_id = Constants.SMSG_DISCONNECT;
        }
    }

    public class ResponseSDDisconnect : NetworkResponse {

        private short status;

        public override void parse() {
            status = DataReader.ReadShort(dataStream);
        }

        public override ExtendedEventArgs process() {
            ResponseSDDisconnectEventArgs args = new ResponseSDDisconnectEventArgs();
            args.status = status;
            return args;
        }
    }
}