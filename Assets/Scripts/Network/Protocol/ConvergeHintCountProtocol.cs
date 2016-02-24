using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

public class ConvergeHintCountProtocol
{
	public static NetworkRequest Prepare ()
	{
		NetworkRequest request = new NetworkRequest (NetworkCode.CONVERGE_HINT_COUNT);

		return request;
	}
	
	public static NetworkResponse Parse (MemoryStream dataStream)
	{
		ResponseConvergeHintCount response = new ResponseConvergeHintCount ();
		
		response.count = DataReader.ReadInt (dataStream);
		
		return response;
	}
}

public class ResponseConvergeHintCount : NetworkResponse
{
	
	public int count { get; set; }
	
	public ResponseConvergeHintCount ()
	{
		protocol_id = NetworkCode.CONVERGE_HINT_COUNT;
	}
}

