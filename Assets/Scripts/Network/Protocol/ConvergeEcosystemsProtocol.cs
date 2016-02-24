using UnityEngine;

using System;
using System.Collections.Generic;
using System.IO;

//1/28/15 - this protocol is not active.  Data read in from text file in ConvergeGame.cs
public class ConvergeEcosystemsProtocol
{
	
	public static NetworkRequest Prepare ()
	{
		NetworkRequest request = new NetworkRequest (NetworkCode.CONVERGE_ECOSYSTEMS);

		return request;
	}
	
	public static NetworkResponse Parse (MemoryStream dataStream)
	{
		ResponseConvergeEcosystems response = new ResponseConvergeEcosystems ();
		
		List<ConvergeEcosystem> ecosystemList = new List<ConvergeEcosystem> ();
		
		int size = DataReader.ReadShort (dataStream);

		//TODO: Read converge-ecosystems data

		response.ecosystemList = ecosystemList;
		
		return response;
	}
}

public class ResponseConvergeEcosystems : NetworkResponse
{
		
	public List<ConvergeEcosystem> ecosystemList { get; set; }
		
	public ResponseConvergeEcosystems ()
	{
		protocol_id = NetworkCode.CONVERGE_ECOSYSTEMS;
	}
}
	