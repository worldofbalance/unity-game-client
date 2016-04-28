using UnityEngine;
using System.Collections;

namespace SD {

    public class ResponseSDPositionEventArgs : ExtendedEventArgs {

        public float xPosition { get; set; }
        public float yPosition { get; set; }
        public float rotation { get; set; }

        public ResponseSDPositionEventArgs() {
            event_id = Constants.SMSG_POSITION;
        }

    }

    public class ResponseSDPosition : NetworkResponse {

        private float xPosition;
        private float yPosition;
        private float rotation;

        public override void parse() {
            xPosition = DataReader.ReadFloat (dataStream);
            yPosition = DataReader.ReadFloat (dataStream);
            rotation = DataReader.ReadFloat (dataStream);
        }

        public override ExtendedEventArgs process ()
        {
            ResponseSDPositionEventArgs args = new ResponseSDPositionEventArgs ();
            args.xPosition = xPosition;
            args.yPosition = yPosition;
            args.rotation = rotation;
            //Debug.Log ("The values are " + args.xPosition + " and " + args.yPosition);
            return args;
        }
    }
}
