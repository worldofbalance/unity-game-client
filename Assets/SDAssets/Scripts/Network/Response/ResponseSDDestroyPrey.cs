using UnityEngine;
using System.Collections;

namespace SD
{
    public class ResponseSDDestroyPreyEventArgs: ExtendedEventArgs {
        public short status { get; set; }
        public int prey_id { get; set; }

        public ResponseSDDestroyPreyEventArgs() {
            event_id = Constants.SMSG_EAT_PREY;
        }
    }

    public class ResponseSDDestroyPrey : NetworkResponse
    {
        private short status;
        private int prey_id;
        public override void parse ()
        {
            status = DataReader.ReadShort (dataStream);
            prey_id = DataReader.ReadInt (dataStream);
        }

        public override ExtendedEventArgs process ()
        {
            ResponseSDDestroyPreyEventArgs args = new ResponseSDDestroyPreyEventArgs ();
            args.status = status;
            args.prey_id = prey_id;
            return args;
        }
    }
}

