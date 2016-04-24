using System;

namespace SD
{
    public class ResponseSDDestroyPreyEventArgs: ExtendedEventArgs {
        public int prey_id { get; set; }

        public ResponseSDDestroyPreyEventArgs() {
            event_id = Constants.SMSG_EAT_PREY;
        }
    }

    public class ResponseSDDestroyPrey : NetworkResponse
    {
        private int prey_id;
        public override void parse ()
        {
            prey_id = DataReader.ReadInt (dataStream);
        }

        public override ExtendedEventArgs process ()
        {
            ResponseSDDestroyPreyEventArgs args = new ResponseSDDestroyPreyEventArgs ();
            args.prey_id = prey_id;
            return args;
        }
    }
}

