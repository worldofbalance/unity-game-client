using UnityEngine;
using System.Collections;

namespace SD
{

    public class ResponseSDPreyEventArgs : ExtendedEventArgs
    {
        public int prey_id { get; set; }
        public float xPosition { get; set; }
        public float yPosition { get; set; }
        public bool isAlive { get; set; }

        public ResponseSDPreyEventArgs()
        {
            event_id = Constants.SMSG_PREY;
        }

    }

    public class ResponseSDPrey : NetworkResponse
    {
        private int prey_id;
        private float xPosition;
        private float yPosition;
        private bool isAlive;

        public override void parse()
        {
            prey_id = DataReader.ReadInt(dataStream);
            xPosition = DataReader.ReadFloat(dataStream);
            yPosition = DataReader.ReadFloat(dataStream);
            isAlive = DataReader.ReadBool(dataStream);
        }

        public override ExtendedEventArgs process()
        {

            ResponseSDPreyEventArgs args = new ResponseSDPreyEventArgs();
            args.prey_id = prey_id;
            args.xPosition = xPosition;
            args.yPosition = yPosition;
            args.isAlive = isAlive;
            return args;
        }
    }
}