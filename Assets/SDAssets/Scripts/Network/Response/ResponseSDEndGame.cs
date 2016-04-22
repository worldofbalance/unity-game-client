using UnityEngine;
using System.Collections;

namespace SD {

    public class ResponseSDEndGameEventArgs : ExtendedEventArgs {
        public bool isWinner { get; set;}
        public float finalScore { get; set; }
        public string winningPlayerId { get; set; }

        public ResponseSDEndGameEventArgs() {
            event_id = Constants.SMSG_SDEND_GAME;
        }
    }
    public class ResponseSDEndGame : NetworkResponse {

        private bool isWinner;
        private float finalScore;
        private string winningPlayerId;

        public ResponseSDEndGame() {
        }

        public override void parse() {
            isWinner = DataReader.ReadBool (dataStream);
            finalScore = DataReader.ReadFloat (dataStream);
            winningPlayerId = DataReader.ReadString (dataStream);
        }

        public override ExtendedEventArgs process() {
            ResponseSDEndGameEventArgs args = new ResponseSDEndGameEventArgs ();
            args.isWinner = this.isWinner;
            args.finalScore = this.finalScore;
            args.winningPlayerId = this.winningPlayerId;
            return args;
        }
    }
}
