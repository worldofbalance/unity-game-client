using UnityEngine;
using System.Collections;

namespace RR{
public class ResponseRRPositionEventArgs : ExtendedEventArgs {
	public float  x { get; set; }
	public float  y { get; set; }
	public ResponseRRPositionEventArgs() {
		event_id = Constants.SMSG_RRPOSITION;
	}
}
}

namespace RR {
public class ResponseRRPostion : NetworkResponse {
		private float x;
		private float y;
		private GameObject g;
		private Running[] p2;

		public ResponseRRPostion() { }

		public override void parse() {
			x = DataReader.ReadFloat(dataStream);
			y = DataReader.ReadFloat(dataStream);
		}

		public override ExtendedEventArgs process() {
		Debug.Log ("loationResponse");

		ResponseRRPositionEventArgs args = new ResponseRRPositionEventArgs ();
		g = GameObject.Find ("GameLogic");
		p2 = g.GetComponents<Running> ();
		Debug.Log ("x = "+ x + "\ny = " + y );
		p2[0].player2.transform.position = new Vector3(x,y,0f);
			args.x = x;
			args.y = y;
			return args;
		}
	}
}
