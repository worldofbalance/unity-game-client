using UnityEngine;
using System;
using System.IO;

public class TopListProtocol {
	
	public static NetworkRequest Prepare() {
		NetworkRequest request = new NetworkRequest(NetworkCode.TOPLIST);
		
		return request;
	}
	
	public static NetworkResponse Parse(MemoryStream dataStream) {
		ResponseTopList response = new ResponseTopList();
		response.name1 = DataReader.ReadString(dataStream);
		response.score1 = DataReader.ReadInt(dataStream);
		response.name2 = DataReader.ReadString(dataStream);
		response.score2 = DataReader.ReadInt(dataStream);
		response.name3 = DataReader.ReadString(dataStream);
		response.score3 = DataReader.ReadInt(dataStream);
		
		return response;
	}
}

public class ResponseTopList : NetworkResponse {
	
	public string name1 { get; set; }
	public string name2 { get; set; }
	public string name3 { get; set; }
	public int score1 { get; set; }
	public int score2 { get; set; }
	public int score3 { get; set; }
	
	public ResponseTopList() {
		protocol_id = NetworkCode.TOPLIST;
	}
}
