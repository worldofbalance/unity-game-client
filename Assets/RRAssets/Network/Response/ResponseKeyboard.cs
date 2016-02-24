using UnityEngine;
using System.Collections;

namespace RR{
public class ResponseKeyboardEventArgs : ExtendedEventArgs {
	public int keytype { get; set; }
	public int key { get; set; }
	
	
	public ResponseKeyboardEventArgs() {
		event_id = Constants.SMSG_KEYBOARD;
	}
}
}

namespace RR {
public class ResponseKeyboard : NetworkResponse {

	private int keytype,key;
	private GameObject g;
	PlayerController2[] p2;


	public override void parse() {

		keytype = DataReader.ReadInt (dataStream);
		key = DataReader.ReadInt (dataStream);
	}

	public override ExtendedEventArgs process() {
		Debug.Log ("response process--");

		g = GameObject.Find("Player_sprite_2(Clone)");
		p2 = g.GetComponents<PlayerController2> ();

		p2[0].keytype = keytype;
		p2 [0].key = key;
		ResponseKeyboardEventArgs args = new ResponseKeyboardEventArgs ();
		args.keytype = keytype;
		args.key = key;

		return args;

	}







}
}
