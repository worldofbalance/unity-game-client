using UnityEngine;

using System;
namespace RR{
public class ResponseLoginEventArgs : ExtendedEventArgs {
	public short status { get; set; }
	public int user_id { get; set; }
	public string username { get; set; }
	public string last_logout { get; set; }
	public short player_level { get; set; }
	public int player_money { get; set; }
	
	public ResponseLoginEventArgs() {
		event_id = Constants.SMSG_AUTH;
	}
}
}

namespace RR {
public class ResponseLogin : NetworkResponse {
	
	private short status;
	private int user_id;
	private string username;
	private string last_logout;
	private short player_level;
	private int player_money;

	public ResponseLogin() {
	}
	
	public override void parse() {
		status = DataReader.ReadShort(dataStream);
		
		if (status == 0) {
			user_id = DataReader.ReadInt(dataStream);
			username = DataReader.ReadString(dataStream);
			last_logout = DataReader.ReadString(dataStream);
			player_money = DataReader.ReadInt(dataStream);
			player_level = DataReader.ReadShort(dataStream);
		}
	}
	
	public override ExtendedEventArgs process() {
		ResponseLoginEventArgs args = null;

		if (status == 0) {
			args = new ResponseLoginEventArgs();
			args.status = status;
			args.user_id = user_id;
			args.username = username;
			args.player_money = player_money;
			args.player_level = player_level;
			//args.last_logout = last_logout;
		}

		return args;
	}
}
}