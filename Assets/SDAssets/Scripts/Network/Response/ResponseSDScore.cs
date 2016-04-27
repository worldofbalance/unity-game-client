using UnityEngine;
using System.Collections;

namespace SD {

    public class ResponseSDScoreEventArgs : ExtendedEventArgs {

        public int score { get; set; }

        public ResponseSDScoreEventArgs() {
            event_id = Constants.SMSG_SCORE;
        }
    }

    public class ResponseSDScore : NetworkResponse {

        private int score;

        public override void parse() {
            score = DataReader.ReadInt (dataStream);
        }

        public override ExtendedEventArgs process() {
            ResponseSDScoreEventArgs args = new ResponseSDScoreEventArgs ();
            args.score = score;
            return args;
        }
    }
}
