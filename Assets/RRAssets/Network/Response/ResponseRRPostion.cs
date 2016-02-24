using UnityEngine;
using System.Collections;



//<<<<<<< HEAD:WoB_Client_Basic/Assets/Network/Response/RRResponsePostion.cs
//public class RRResponsePositionEventArgs : ExtendedEventArgs {
//<<<<<<< HEAD
//=======
namespace RR{
public class ResponseRRPositionEventArgs : ExtendedEventArgs {
//>>>>>>> start:WoB_Client_Basic/Assets/Network/Response/ResponseRRPostion.cs
//	public int  x { get; set; }
//	public int  y { get; set; }
//=======
	public float  x { get; set; }
	public float  y { get; set; }
//>>>>>>> Dong
	
	public ResponseRRPositionEventArgs() {
		event_id = Constants.SMSG_RRPOSITION;
	}
}
}

namespace RR {
public class ResponseRRPostion : NetworkResponse {

//<<<<<<< HEAD
//		private int x;
//		private int y;
//=======
		private float x;
		private float y;
//>>>>>>> Dong

		private GameObject g;
		private Running[] p2;

		public ResponseRRPostion() { }

		public override void parse() {
//<<<<<<< HEAD
//			x = DataReader.ReadInt(dataStream);
//			y = DataReader.ReadInt(dataStream);
//=======
			x = DataReader.ReadFloat(dataStream);
			y = DataReader.ReadFloat(dataStream);
//>>>>>>> Dong
		}



		public override ExtendedEventArgs process() {
		Debug.Log ("loationResponse");

		ResponseRRPositionEventArgs args = new ResponseRRPositionEventArgs ();
		g = GameObject.Find ("GameLogic");
		p2 = g.GetComponents<Running> ();
//<<<<<<< HEAD
		Debug.Log ("x = "+ x + "\ny = " + y );
//		p2[0].player2.transform.position = new Vector3((float)x,(float)y,0f);
//=======
		//Debug.Log ("response:    x = "+ x + "\ny = " + y );
		p2[0].player2.transform.position = new Vector3(x,y,0f);
//>>>>>>> Dong
	




			args.x = x;
			args.y = y;
			return args;

		}







}
}
