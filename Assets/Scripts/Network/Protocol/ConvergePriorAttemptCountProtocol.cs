using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

public class ConvergePriorAttemptCountProtocol
{
		
	public static NetworkRequest Prepare (int player_id, int ecosystem_id)
	{
		NetworkRequest request = new NetworkRequest (NetworkCode.CONVERGE_PRIOR_ATTEMPT_COUNT);
		request.AddInt32 (player_id);
		request.AddInt32 (ecosystem_id);

		return request;
	}
		
	public static NetworkResponse Parse (MemoryStream dataStream)
	{
		ResponseConvergePriorAttemptCount response = new ResponseConvergePriorAttemptCount ();
			
		response.player_id = DataReader.ReadInt (dataStream);
		response.ecosystem_id = DataReader.ReadInt (dataStream);
		response.count = DataReader.ReadInt (dataStream);

		return response;
	}
}
		
public class ResponseConvergePriorAttemptCount : NetworkResponse
{
			
	public int player_id { get; set; }
	public int ecosystem_id { get; set; }
	public int count { get; set; }

	public ResponseConvergePriorAttemptCount ()
	{
		protocol_id = NetworkCode.CONVERGE_PRIOR_ATTEMPT_COUNT;
	}
}
	