using UnityEngine;
using System.Collections;
using System;
using System.IO;

namespace CW
{
	public class MatchInitProtocol
	{
			
		public static NetworkRequest Prepare (int playerID, int roomID)
		{
			NetworkRequest request = new NetworkRequest (NetworkCode.MATCH_INIT);
			request.AddInt32 (playerID);
			request.AddInt32 (roomID);
		
			return request;
		}
	
		public static NetworkResponse Parse (MemoryStream dataStream)
		{
			ResponseMatchInit response = new ResponseMatchInit ();
			response.status = DataReader.ReadShort (dataStream);
		
			if (response.status == 0) {
				response.matchID = DataReader.ReadInt (dataStream);
			}
			return response;
		}
	}
	
	public class ResponseMatchInit : NetworkResponse
	{
		
		public short status { get; set; }

		public int matchID { get; set; }
		
		public ResponseMatchInit ()
		{
			protocol_id = NetworkCode.MATCH_INIT;
		}
	}
}