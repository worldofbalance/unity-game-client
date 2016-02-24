using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

public class ConvergeHintProtocol
{

	public static NetworkRequest Prepare(int hintOffset) {
		NetworkRequest request = new NetworkRequest(NetworkCode.CONVERGE_HINT);
		request.AddInt32(hintOffset);
		
		return request;
	}
	
	public static NetworkResponse Parse(MemoryStream dataStream) {
		ResponseConvergeHint response = new ResponseConvergeHint();
		
		using (BinaryReader br = new BinaryReader(dataStream, Encoding.UTF8)) {
			int hintId = br.ReadInt32 ();
			short fldSize = br.ReadInt16 ();
			String text = System.Text.Encoding.UTF8.GetString (br.ReadBytes (fldSize));

			ConvergeHint hint = null;
			if (hintId != Constants.ID_NOT_SET) {
				hint = new ConvergeHint (hintId, text);
			}
			
			response.hint = hint;
		}
		
		return response;
	}
}

public class ResponseConvergeHint : NetworkResponse {
	
	public ConvergeHint hint { get; set; }
	
	public ResponseConvergeHint() {
		protocol_id = NetworkCode.CONVERGE_HINT;
	}

}

