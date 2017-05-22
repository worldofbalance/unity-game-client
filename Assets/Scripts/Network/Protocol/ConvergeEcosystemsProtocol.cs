using UnityEngine;

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

public class ConvergeEcosystemsProtocol
{
	
	public static NetworkRequest Prepare ()
	{
		NetworkRequest request = new NetworkRequest (NetworkCode.CONVERGE_ECOSYSTEMS);

		return request;
	}
	
	public static NetworkResponse Parse (MemoryStream dataStream)
	{
		var dataStreamLength = dataStream.Length;
		long bytesRead = 0;
		
		ResponseConvergeEcosystems response = new ResponseConvergeEcosystems ();
		
		List<ConvergeEcosystem> ecosystemList = new List<ConvergeEcosystem> ();

	    using (BinaryReader br = new BinaryReader(dataStream, Encoding.UTF8)) {
            int ecosystemCnt = br.ReadInt16 ();
		    bytesRead += 2;

            for (int i = 0; i < ecosystemCnt; i++) {
                int ecosystem_id = br.ReadInt32 ();
	            bytesRead += 4;

                ConvergeEcosystem ecosystem = new ConvergeEcosystem (ecosystem_id);
	            
                int fldSize = br.ReadInt16 ();
	            bytesRead += 2;
	            var bytes = br.ReadBytes(fldSize);
	            bytesRead += bytes.Length;
                ecosystem.description = System.Text.Encoding.UTF8.GetString (bytes);
	            
                ecosystem.timesteps = br.ReadInt32 ();
	            bytesRead += 4;
	            
                fldSize = br.ReadInt16 ();
	            bytesRead += 2;
	            bytes = br.ReadBytes(fldSize);
	            bytesRead += bytes.Length;
                ecosystem.config_default = System.Text.Encoding.UTF8.GetString (bytes);
	            
                fldSize = br.ReadInt16 ();
	            bytesRead += 2;
	            bytes = br.ReadBytes(fldSize);
	            bytesRead += bytes.Length;
                ecosystem.config_target = System.Text.Encoding.UTF8.GetString (bytes);
	            
                ecosystemList.Add (ecosystem);
            }
	    }

        if (ecosystemList.Count == 0) {
            Debug.LogError ("No converge ecosystems found.");
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
	