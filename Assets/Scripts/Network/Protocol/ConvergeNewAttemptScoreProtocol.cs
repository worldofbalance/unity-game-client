using UnityEngine;
using System;
using System.Collections;
using System.IO;
using System.Text;

public class ConvergeNewAttemptScoreProtocol
{
	
	public static NetworkRequest Prepare (
		int playerId, 
		int ecosystemId, 
		int attemptId, 
		int score
		)
	{
		NetworkRequest request = new NetworkRequest (NetworkCode.CONVERGE_NEW_ATTEMPT_SCORE);
		request.AddInt32 (playerId);
		request.AddInt32 (ecosystemId);
		request.AddInt32 (attemptId);
		request.AddInt32 (score);

		return request;
	}
	
	public static NetworkResponse Parse (MemoryStream dataStream)
	{
		ResponseConvergeNewAttemptScore response = new ResponseConvergeNewAttemptScore ();
		int status = 0;

		using (BinaryReader br = new BinaryReader(dataStream, Encoding.UTF8)) {
			status = br.ReadInt32 ();
		}
		response.status = status;
		
		return response;
	}
}

public class ResponseConvergeNewAttemptScore : NetworkResponse
{
	
	public int status { get; set; }
	
	public ResponseConvergeNewAttemptScore ()
	{
		protocol_id = NetworkCode.CONVERGE_NEW_ATTEMPT_SCORE;
	}
}


