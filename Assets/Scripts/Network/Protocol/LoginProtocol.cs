using UnityEngine;
using System;
using System.IO;

public class LoginProtocol {

	public static NetworkRequest Prepare(string user_id, string password) {
		NetworkRequest request = new NetworkRequest(NetworkCode.LOGIN);
		request.AddString(user_id);
		request.AddString(password);
		
		return request;
	}
	
	public static NetworkResponse Parse(MemoryStream dataStream) {
		ResponseLogin response = new ResponseLogin();
		response.status = DataReader.ReadShort(dataStream);
		
		if (response.status == 0) {
			int account_id = DataReader.ReadInt(dataStream);

			Account account = new Account(account_id);
			account.username = DataReader.ReadString(dataStream);
			account.last_logout = DataReader.ReadString(dataStream);

			response.account = account;
		} else {
			Debug.Log ("Login failed, server message = " + response.status);
		}
		
		return response;
	}
}

public class ResponseLogin : NetworkResponse {
	
	public short status { get; set; }
	public Account account { get; set; }
	
	public ResponseLogin() {
		protocol_id = NetworkCode.LOGIN;
	}
}
