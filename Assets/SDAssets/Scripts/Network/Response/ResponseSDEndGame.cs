using UnityEngine;
using System.Collections;

namespace SD {

    public class ResponseSDEndGameEventArgs : ExtendedEventArgs {
        public int status { get; set; }
        public float winningScore { get; set; }
        public string winningPlayerId { get; set; }

        public ResponseSDEndGameEventArgs() {
            event_id = Constants.SMSG_SDEND_GAME;
        }
    }
    public class ResponseSDEndGame : NetworkResponse {

        private int status;
        private float winningScore;
        private string winningPlayerId;

        public ResponseSDEndGame() {
        }

        public override void parse() {
            status = DataReader.ReadInt (dataStream);
            winningScore = DataReader.ReadFloat (dataStream);
            winningPlayerId = DataReader.ReadString (dataStream);
        }

        public override ExtendedEventArgs process() {
            ResponseSDEndGameEventArgs args = new ResponseSDEndGameEventArgs ();
            args.status = this.status;
            args.winningScore = this.winningScore;
            args.winningPlayerId = this.winningPlayerId;
            return args;
        }
    }
}
