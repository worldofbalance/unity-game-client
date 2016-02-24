using UnityEngine;
using System;
using System.IO;

public class GetRoomsProtocol {
	
	public static NetworkRequest Prepare() {
		NetworkRequest request = new NetworkRequest(NetworkCode.GET_ROOMS);
		return request;
	}
	
	public static NetworkResponse Parse(MemoryStream dataStream) {
		short numRooms = DataReader.ReadShort(dataStream);

		Debug.Log ("Getting room infomations...");

		RoomManager.getInstance ().clear ();
		for (short i = 0; i < numRooms; ++i) {
			short id = DataReader.ReadShort(dataStream);
			short gameid = DataReader.ReadShort(dataStream);
			string host = DataReader.ReadString(dataStream);

			var room = RoomManager.getInstance ().addRoom(id, gameid);
			room.host = host;

			short numClients = DataReader.ReadShort(dataStream);
			for(short c = 0; c < numClients; ++c) {
				room.addPlayer(DataReader.ReadShort(dataStream));
			}
		}

		return null;
	}
}