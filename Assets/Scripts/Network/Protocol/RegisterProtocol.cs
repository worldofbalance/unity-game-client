using UnityEngine;
using System;
using System.IO;

public class RegisterProtocol {

	public static NetworkRequest Prepare(string fname, string lname, string email, string password, string name, short color) {
		NetworkRequest request = new NetworkRequest(NetworkCode.REGISTER);
		request.AddString(fname);
		request.AddString(lname);
		request.AddString(email);
		request.AddString(password);		
		request.AddString(name);
		
		return request;
	}
	
	public static NetworkResponse Parse(MemoryStream dataStream) {
		ResponseRegister response = new ResponseRegister();
		response.status = DataReader.ReadShort(dataStream);
		if (response.status != 0) {
			Debug.LogWarning ("Server error during registration, status = " + response.status);
		}
		return response;
	}
}

public class ResponseRegister : NetworkResponse {
	
	public short status { get; set; }
	
	public ResponseRegister() {
		protocol_id = NetworkCode.REGISTER;
	}
}
