using UnityEngine;
using System.Collections;

namespace SD {

    public class ResponseSDKeyboardEventArgs : ExtendedEventArgs {

        public int keyCode { get; set; }
        public int keyCombination { get; set;}

        public ResponseSDKeyboardEventArgs() {
            event_id = Constants.SMSG_KEYBOARD;
        }
    }

    public class ResponseSDKeyboard : NetworkResponse {

        private int keyCode;
        private int keyCombination;

        public override void parse() {
            keyCode = DataReader.ReadInt (dataStream);
            keyCombination = DataReader.ReadInt (dataStream);
        }

        public override ExtendedEventArgs process() {
            ResponseSDKeyboardEventArgs args = new ResponseSDKeyboardEventArgs ();
            args.keyCode = keyCode;
            args.keyCombination = keyCombination;
            return args;
        }
    }
}
