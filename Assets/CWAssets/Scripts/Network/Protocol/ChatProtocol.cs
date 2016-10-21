using UnityEngine;
using System;
using System.IO;
public class ChatProtocol {
  public static NetworkRequest Prepare(string username, string message) {
    NetworkRequest request = new NetworkRequest(NetworkCode.CHAT);
    request.AddString(username);
    request.AddString(message);
    return request;
  }
  public static NetworkResponse Parse(MemoryStream dataStream) {
    ResponseChat response = new ResponseChat();
    response.status = DataReader.ReadShort(dataStream);
    
    if (response.status == 0) {
      response.username =  DataReader.ReadString(dataStream);
      response.message = DataReader.ReadString(dataStream);
    } else {
      Debug.LogError ("CHAT failed = " + response.status);
    }
    
    return response;
  }
}

public class ResponseChat : NetworkResponse {
  
  public short status { get; set; }
  public string message { get; set; }
  public string username { get; set; }

  public ResponseChat() {
    protocol_id = NetworkCode.CHAT;
  }
}
