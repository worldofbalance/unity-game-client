using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

//1/28/15 - this protocol is not active.  Data read in from text file in ConvergeGame.cs
// 5/24/2017 - this protocol has been made active. The text (really binary) file is not used anymore
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
		ConvergeEcosystem convergeEcosystem;
		Debug.Log ("Inside ConvergeEcosystemsProtocol, parse");

		using (BinaryReader br = new BinaryReader (dataStream, Encoding.UTF8)) {
			int size = (int) br.ReadInt16 ();
			Debug.Log ("size = " + size);
			int ecoId;
			for (int i = 0; i < size; i++) {
				ecoId = br.ReadInt32 ();
				Debug.Log ("ecoId = " + ecoId);
				convergeEcosystem = new ConvergeEcosystem (ecoId);
				short fldSize = br.ReadInt16 ();
				Debug.Log ("fldSize = " + fldSize);
				convergeEcosystem.description = System.Text.Encoding.UTF8.GetString (br.ReadBytes (fldSize));
				Debug.Log ("description = " + convergeEcosystem.description);
				convergeEcosystem.timesteps = br.ReadInt32 ();
				Debug.Log ("timesteps = " + convergeEcosystem.timesteps);
				fldSize = br.ReadInt16 ();
				Debug.Log ("fldSize = " + fldSize);
				convergeEcosystem.config_default = System.Text.Encoding.UTF8.GetString (br.ReadBytes (fldSize));
				Debug.Log ("convergeEcosystem.config_default = " + convergeEcosystem.config_default);
				fldSize = br.ReadInt16 ();
				convergeEcosystem.config_target = System.Text.Encoding.UTF8.GetString (br.ReadBytes (fldSize));
				ecosystemList.Add (convergeEcosystem);
			}
		}

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
	