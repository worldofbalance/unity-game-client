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
        // commented by Rujoota
		//Debug.Log ("Getting room infomations...");

		RoomManager.getInstance ().clear ();
		for (short i = 0; i < numRooms; ++i) {
			short id = DataReader.ReadShort(dataStream);
			short gameid = DataReader.ReadShort(dataStream);
			string host = DataReader.ReadString(dataStream);

			// DH   Only relevant for Multiplayer Convergence 
			short totalPlayers = DataReader.ReadShort(dataStream);
			short numRounds = DataReader.ReadShort(dataStream);
			short secPerRound = DataReader.ReadShort(dataStream);
			short betAmt = DataReader.ReadShort(dataStream);
			short ecoNum = DataReader.ReadShort(dataStream);
			short helps = DataReader.ReadShort(dataStream);

			var room = RoomManager.getInstance ().addRoom(id, gameid);
			room.host = host;

			// DH   Only relevant for Multiplayer Convergence 
			room.totalPlayers = totalPlayers;
			room.numRounds = numRounds;
			room.secPerRound = secPerRound;
			room.betAmt = betAmt;
			room.ecoNum = ecoNum;
			room.helps = helps;

			short numClients = DataReader.ReadShort(dataStream);
			for(short c = 0; c < numClients; ++c) {
				room.addPlayer(DataReader.ReadShort(dataStream));
			}
		}

		return null;
	}
}