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
		ResponseConvergeEcosystems response = new ResponseConvergeEcosystems ();
		
		List<ConvergeEcosystem> ecosystemList = new List<ConvergeEcosystem> ();

	    using (BinaryReader br = new BinaryReader(dataStream, Encoding.UTF8)) {
            int size = br.ReadInt16 ();
            int responseId = br.ReadInt16 ();
            int ecosystemCnt = br.ReadInt16 ();

            for (int i = 0; i < ecosystemCnt; i++) {
                int ecosystem_id = br.ReadInt32 ();

                ConvergeEcosystem ecosystem = new ConvergeEcosystem (ecosystem_id);
                int fldSize = br.ReadInt16 ();
                ecosystem.description = System.Text.Encoding.UTF8.GetString (br.ReadBytes (fldSize));
                ecosystem.timesteps = br.ReadInt32 ();
                fldSize = br.ReadInt16 ();
                ecosystem.config_default = System.Text.Encoding.UTF8.GetString (br.ReadBytes (fldSize));
                fldSize = br.ReadInt16 ();
                ecosystem.config_target = System.Text.Encoding.UTF8.GetString (br.ReadBytes (fldSize));
                fldSize = br.ReadInt16 ();
                ecosystem.csv_default_string = System.Text.Encoding.UTF8.GetString (br.ReadBytes (fldSize));
                fldSize = br.ReadInt16 ();
                ecosystem.csv_target_string = System.Text.Encoding.UTF8.GetString (br.ReadBytes (fldSize));

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
	